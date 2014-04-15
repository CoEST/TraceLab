// TraceLab - Software Traceability Instrument to Facilitate and Empower Traceability Research
// Copyright (C) 2012-2013 CoEST - National Science Foundation MRI-R2 Grant # CNS: 0959924
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see<http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using TraceLab.Core.Components;
using TraceLab.Core.ExperimentExecution;
using TraceLab.Core.Experiments;
using TraceLab.UI.WPF.Converters;
using TraceLab.UI.WPF.EventArgs;
using TraceLab.UI.WPF.ViewModels;
using TraceLab.UI.WPF.Views;
using TraceLab.UI.WPF.Commands;
using TraceLab.UI.WPF.Utilities;
using TraceLab.UI.WPF.ViewModels.Nodes;
namespace TraceLab.UI.WPF.Views.Nodes
{
    /// <summary>
    /// Represents base class for contols for Decisions and LoopScopeDecisions control panels
    /// </summary>
    public abstract class DecisionInfoControlBase : UserControl
    {
        protected abstract TraceLab.UI.WPF.Controls.RichTextBoxWithIntellisense DecisionCodeRichTextBox { get; }

        #region Constructing and loading

        /// <summary>
        /// Initializes a new instance of the <see cref="DecisionInfoControlBase"/> class.
        /// </summary>
        public DecisionInfoControlBase()
        {
            InitRichTextBoxIntellisenseTrigger();
            
            AppendCodeTokenCommand = new DelegateCommand(AppendCodeTokenFunc, CanAppendCodeTokenFunc);
            CompileDecisionModuleCommand = new DelegateCommand(CompileDecisionModuleFunc, CanCompileDecisionModuleFunc);

            DataContextChanged += DecisionNodeInfoControl_DataContextChanged;
        }

        /// <summary>
        /// Handles the DataContextChanged event of the DecisionNodeInfoControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void DecisionNodeInfoControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ExperimentNodeInfo newInfo = e.NewValue as ExperimentNodeInfo;
            if (newInfo != null && (newInfo is DecisionNodeInfo || newInfo is LoopDecisionNodeInfo))
            {
                IDecision metadata = (IDecision)newInfo.Node.Data.Metadata;
                m_experiment = newInfo.Node.Owner;
                metadata.RequestLatestCode += metadata_RequestLatestCode;
            }

            ExperimentNodeInfo oldInfo = e.NewValue as ExperimentNodeInfo;
            if (oldInfo != null && (newInfo is DecisionNodeInfo || newInfo is LoopDecisionNodeInfo))
            {
                IDecision metadata = (IDecision)oldInfo.Node.Data.Metadata;
                metadata.RequestLatestCode -= metadata_RequestLatestCode;
            }
        }

        /// <summary>
        /// Handles the RequestLatestCode event of the metadata control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void metadata_RequestLatestCode(object sender, System.EventArgs e)
        {
            //the update of document binding must be invoked on main GUI thread - the same thread that has constructed the decision node info control
            //if it was not invoked on main ui thread, it would crash tracelab if info control was open, decision code was changed, and experiment was run. 
            //Experiment running happens in seperate thread, and one of action that is done is the recompilaton of (dirty changed) decision code, 
            //which then fires the RequestLatestCode event.
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                DecisionCodeRichTextBox.UpdateDocumentBinding();
            }));
        }

        #endregion

        #region Members

        protected IExperiment m_experiment;

        #endregion

        #region ContentAssistTriggers Property

        /// <summary>
        /// Gets or sets the content assist triggers.
        /// </summary>
        /// <value>
        /// The content assist triggers.
        /// </value>
        public List<String> ContentAssistTriggers
        {
            get { return (List<String>)GetValue(ContentAssistTriggersProperty); }
            set { SetValue(ContentAssistTriggersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentAssistTriggers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentAssistTriggersProperty =
            DependencyProperty.Register("ContentAssistTriggers", typeof(List<String>), typeof(DecisionNodeInfoControl), new UIPropertyMetadata(new List<String>()));

        /// <summary>
        /// Inits the rich text box intellisense trigger.
        /// </summary>
        protected virtual void InitRichTextBoxIntellisenseTrigger()
        {
            ContentAssistTriggers.Clear();
            ContentAssistTriggers.Add("Select");
            ContentAssistTriggers.Add("Load");
        }

        #endregion

        #region OnFillItemsNeeded

        /// <summary>
        /// when requested it fills the comboboxes with items from the graph
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void OnFillItemsNeeded(object sender, FillItemsNeededEventArgs args)
        {
            ComboBox comboBox = sender as ComboBox;

            if (comboBox != null && args != null)
            {
                object selectedItem = comboBox.SelectedItem;
                 
                switch (args.Statement)
                {
                    case Statement.Select:
                        FillItemsWithNextNodesLabels(comboBox);
                        break;

                    case Statement.Load:

                        comboBox.Items.Clear();
                        FillItemsWithIncomingOutputsFromPreviousNodes(comboBox);
                        break;
                }

                if (comboBox.Items.Contains(selectedItem))
                {
                    comboBox.SelectedItem = selectedItem;
                }
            }
        }

        /// <summary>
        /// Fills the items with next nodes labels.
        /// </summary>
        /// <param name="comboBox">The combo box.</param>
        private void FillItemsWithNextNodesLabels(ComboBox comboBox)
        {
            comboBox.Items.Clear();
            ExperimentNodeInfo decisionNodeInfo = DataContext as ExperimentNodeInfo;
            ExperimentNode currentNode = decisionNodeInfo.Node;

            foreach (ExperimentNodeConnection outEdge in m_experiment.OutEdges(currentNode))
            {
                comboBox.Items.Add(outEdge.Target.Data.Metadata.Label);
            }
        }

        /// <summary>
        /// Fills the items with incoming outputs from previous nodes.
        /// </summary>
        /// <param name="comboBox">The combo box.</param>
        private void FillItemsWithIncomingOutputsFromPreviousNodes(ComboBox comboBox)
        {
            comboBox.Items.Clear();
            ExperimentNodeInfo decisionNodeInfo = DataContext as ExperimentNodeInfo;
            ExperimentNode currentNode = decisionNodeInfo.Node;

            Dictionary<string, string> predeccessorsOutputsNameTypeLookup;

            var availableInputMappingsPerNode = new TraceLab.Core.Utilities.InputMappings(m_experiment);

            if (availableInputMappingsPerNode.TryGetValue(currentNode, out predeccessorsOutputsNameTypeLookup) == false)
            {
                predeccessorsOutputsNameTypeLookup = new Dictionary<string, string>(); //return empty - there is not path from start node to decision
            }

            foreach (string workspaceUnitName in predeccessorsOutputsNameTypeLookup.Keys)
            {
                comboBox.Items.Add(workspaceUnitName);
            }

        }

        #endregion
        
        #region Insert Code Token command

        public static readonly DependencyProperty AppendCodeTokenCommandProperty = DependencyProperty.Register("AppendCodeTokenCommand", typeof(ICommand), typeof(DockableGraph));

        /// <summary>
        /// Gets or sets the append code token command.
        /// </summary>
        /// <value>
        /// The append code token command.
        /// </value>
        public ICommand AppendCodeTokenCommand
        {
            get
            {
                return (ICommand)GetValue(AppendCodeTokenCommandProperty);
            }
            set
            {
                SetValue(AppendCodeTokenCommandProperty, value);
            }
        }

        /// <summary>
        /// Executes the append code token.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        protected void ExecuteAppendCodeToken(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppendCodeTokenCommand != null)
            {
                AppendCodeTokenCommand.Execute(e.Parameter);
            }
        }

        /// <summary>
        /// Determines whether this instance [can execute append code token] the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.CanExecuteRoutedEventArgs"/> instance containing the event data.</param>
        protected void CanExecuteAppendCodeToken(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppendCodeTokenCommand != null)
            {
                e.CanExecute = AppendCodeTokenCommand.CanExecute(e.Parameter);
            }
            else
            {
                e.CanExecute = false;
            }
        }

        /// <summary>
        /// Appends the code token
        /// </summary>
        /// <param name="param">The param.</param>
        private void AppendCodeTokenFunc(object param)
        {
            string statement = param as string;
            if (statement != null) 
            {
                string selectStatement = @"^Select\(""(.*)""\);$";
                string loadStatement = @"^Load\(""(.*)""\)$";
                if (Regex.IsMatch(statement, selectStatement))
                {
                    DecisionCodeRichTextBox.InsertStatementWithComboBox(Statement.Select);
                } 
                else if(Regex.IsMatch(statement, loadStatement)) 
                {
                    DecisionCodeRichTextBox.InsertStatementWithComboBox(Statement.Load);
                }
            }
        }

        /// <summary>
        /// Determines whether code token can be appended to the current code
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns>
        ///   <c>true</c> if code token can be appended to the current code; otherwise, <c>false</c>.
        /// </returns>
        private bool CanAppendCodeTokenFunc(object param)
        {
            bool canAppendCodeToken = false;
            string statement = param as string;
            if (statement != null)
            {
                canAppendCodeToken = true;
            }
            return canAppendCodeToken;
        }

        #endregion

        #region Decision Node Compile Code

        public static readonly DependencyProperty CompileDecisionModuleCommandProperty = DependencyProperty.Register("CompileDecisionModuleCommand", typeof(ICommand), typeof(DockableGraph));

        /// <summary>
        /// Gets or sets the compile decision module command.
        /// </summary>
        /// <value>
        /// The compile decision module command.
        /// </value>
        public ICommand CompileDecisionModuleCommand
        {
            get
            {
                return (ICommand)GetValue(CompileDecisionModuleCommandProperty);
            }
            set
            {
                SetValue(CompileDecisionModuleCommandProperty, value);
            }
        }

        ///// <summary>
        ///// Executes the compile decision module.
        ///// </summary>
        ///// <param name="sender">The sender.</param>
        ///// <param name="e">The <see cref="System.Windows.Input.ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        protected void ExecuteCompileDecisionModule(object sender, ExecutedRoutedEventArgs e)
        {
            if (CompileDecisionModuleCommand != null)
            {
                CompileDecisionModuleCommand.Execute(e.Parameter);
            }
        }

        /// <summary>
        /// Determines whether this instance [can execute compile decision module] the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.CanExecuteRoutedEventArgs"/> instance containing the event data.</param>
        protected void CanExecuteCompileDecisionModule(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CompileDecisionModuleCommand != null)
            {
                e.CanExecute = CompileDecisionModuleCommand.CanExecute(e.Parameter);
            }
            else
            {
                e.CanExecute = false;
            }
        }

        /// <summary>
        /// Compiles the decision module func.
        /// </summary>
        /// <param name="param">The param.</param>
        private void CompileDecisionModuleFunc(object param)
        {
            var args = param as List<object>;
            if (args != null && args.Count == 2)
            {
                //validate args
                var node = args[0] as ExperimentNode;
                var appVM = args[1] as ApplicationViewModelWrapper;

                if (node != null && appVM != null)
                {
                    List<string> workspaceTypeDirectories = appVM.WorkspaceViewModel.WorkspaceTypeDirectories;
                    string topExperimentId = appVM.ExperimentViewModel.TopLevel.ExperimentInfo.Id;
                    LoggerNameRoot loggerNameRoot = new LoggerNameRoot(topExperimentId);

                    TraceLab.Core.Decisions.DecisionCompilationRunner.CompileDecision(node, m_experiment, workspaceTypeDirectories, loggerNameRoot);
                }
            }
        }
        
        /// <summary>
        /// Determines whether this instance [can compile decision module func] the specified param.
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns>
        ///   <c>true</c> if this instance [can compile decision module func] the specified param; otherwise, <c>false</c>.
        /// </returns>
        private bool CanCompileDecisionModuleFunc(object param)
        {
            bool canCompile = false;
            var args = param as List<object>;
            if (args != null && args.Count == 2)
            {
                //validate args
                bool isDecision = args[0] is ExperimentDecisionNode || args[0] is LoopScopeNode;
                bool isApplicationViewModelWrapper = args[1] is ApplicationViewModelWrapper;

                if (isDecision && isApplicationViewModelWrapper)
                {
                    canCompile = true;
                }
            }
            return canCompile;
        }

        #endregion

        #region Intercept Paste

        /// <summary>
        /// Adds the paste handler.
        /// </summary>
        protected void AddPasteHandler()
        {
            DataObject.AddPastingHandler(DecisionCodeRichTextBox, new DataObjectPastingEventHandler(OnPaste));
        }

        /// <summary>
        /// Called when [paste].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.DataObjectPastingEventArgs"/> instance containing the event data.</param>
        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            RichTextBox richTextBox = sender as RichTextBox;
            if (richTextBox == null) { return; }

            if (e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true))
            {
                string text = e.SourceDataObject.GetData(DataFormats.UnicodeText, true) as string;
                if (text != null)
                {
                    InsertTextToRichTextBoxAtPosition(richTextBox, text);
                }

            }

            e.Handled = true;
            e.CancelCommand();
        }

        /// <summary>
        /// Inserts the text to rich text box at position.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="text">The text.</param>
        public void InsertTextToRichTextBoxAtPosition(RichTextBox richTextBox, string text)
        {
            StringToFlowDocumentConverter converter = new StringToFlowDocumentConverter();

            //first determine if richtextbox document is empty... if so add first paragraph
            if (richTextBox.Document.Blocks.Count == 0)
            {
                richTextBox.Document.Blocks.Add(new Paragraph());
            }
            TextPointer insertionPosition = richTextBox.CaretPosition.GetInsertionPosition(LogicalDirection.Forward);
            Paragraph currentParagraph = insertionPosition.Paragraph;

            using (StringReader reader = new StringReader(text))
            {
                string newLine;
                bool firstLine = true;
                while ((newLine = reader.ReadLine()) != null)
                {
                    //if this is first line - append it to the existing paragraph in document
                    if (firstLine)
                    {
                        converter.AppendTextToParagraph(currentParagraph, newLine);
                        firstLine = false;
                    }
                    else
                    {
                        //all other lines append as new paragraphs to text after first paragraph
                        Paragraph newParagraph = converter.ConvertStringLineToParagraph(newLine);
                        richTextBox.Document.Blocks.InsertAfter(currentParagraph, newParagraph);
                        currentParagraph = newParagraph;
                    }
                }
            }
        }

        #endregion

        #region Intercept Copy

        /// <summary>
        /// Adds the copy data event handler.
        /// </summary>
        protected void AddCopyDataEventHandler()
        {
            DataObject.AddSettingDataHandler(DecisionCodeRichTextBox, new DataObjectSettingDataEventHandler(OnDataSetting));
        }

        /// <summary>
        /// Called when [data setting].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.DataObjectSettingDataEventArgs"/> instance containing the event data.</param>
        private void OnDataSetting(object sender, DataObjectSettingDataEventArgs e)
        {
            RichTextBox richTextBox = sender as RichTextBox;
            if (richTextBox == null) { return; }

            StringToFlowDocumentConverter converter = new StringToFlowDocumentConverter();

            TextSelection selection = richTextBox.Selection;
            string text = converter.ConvertFlowDocumentDataToStringWithinSelection(selection.Start, selection.End);

            e.DataObject.SetData(System.Windows.DataFormats.UnicodeText, text);
        }

        #endregion 
    }
}
