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
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using TraceLab.UI.WPF.EventArgs;
using TraceLab.UI.WPF.ViewModels;

namespace TraceLab.UI.WPF.Controls
{
    /// <summary>
    /// Represents a rich text box control with intellisense assist, and decision code statements with comboboxes.
    /// </summary>
    public class RichTextBoxWithIntellisense : RichTextBox
    {
        #region Contructing and loading RichTextBox

        static RichTextBoxWithIntellisense()
        {
            BindableDocumentProperty.OverrideMetadata(typeof(RichTextBoxWithIntellisense), new FrameworkPropertyMetadata(new PropertyChangedCallback(DocumentPropertyChanged)));
        }

        public RichTextBoxWithIntellisense() : base()
        {
            Loaded += new RoutedEventHandler(RichTextBoxWithIntellisense_Loaded);
        }

        void RichTextBoxWithIntellisense_Loaded(object sender, RoutedEventArgs e)
        {
            m_assistListPopup.MaxHeight = 100;
            m_assistListPopup.MinWidth = 100;
            m_assistListPopup.SelectedStatement += OnSelectedStatement;
            m_assistListPopup.Closed += OnPopupClosed;
            IsTextDirty = false;
        }

        #endregion

        /// <summary>
        /// Represents a bindable rich editing control which operates on System.Windows.Documents.FlowDocument
        /// objects.    
        /// </summary>
        #region Bindable RichTextBox 

        /// <summary>
        /// Identifies the <see cref="Document"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BindableDocumentProperty = DependencyProperty.Register("BindableDocument", typeof(FlowDocument), typeof(RichTextBoxWithIntellisense));

        /// <summary>
        /// Raises the <see cref="E:System.Windows.FrameworkElement.Initialized"></see> event. This method is invoked whenever <see cref="P:System.Windows.FrameworkElement.IsInitialized"></see> is set to true internally.
        /// </summary>
        /// <param title="e">The <see cref="T:System.Windows.RoutedEventArgs"></see> that contains the event data.</param>
        protected override void OnInitialized(System.EventArgs e)
        {
            // Hook up to get notified when DocumentProperty changes.
            DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(BindableDocumentProperty, typeof(RichTextBoxWithIntellisense));
            descriptor.AddValueChanged(this, delegate
            {
                // If the underlying value of the dependency property changes,
                // update the underlying document, also.
                base.Document = (FlowDocument)GetValue(BindableDocumentProperty);
            });

            // By default, we support updates to the source when focus is lost (or, if the LostFocus
            // trigger is specified explicity.  We don't support the PropertyChanged trigger right now.
            this.LostFocus += new RoutedEventHandler(RichTextBoxWithIntellisense_LostFocus);

            base.OnInitialized(e);
        }

        /// <summary>
        /// Handles the LostFocus event of the BindableRichTextBox control.
        /// </summary>
        /// <param title="sender">The source of the event.</param>
        /// <param title="e">The <see cref="System.Windows.RoutedEventArgs"/> s_instance containing the event data.</param>
        void RichTextBoxWithIntellisense_LostFocus(object sender, RoutedEventArgs e)
        {
            // If we have a binding that is set for LostFocus or Default (which we are specifying as default)
            // then update the source.
            Binding binding = BindingOperations.GetBinding(this, BindableDocumentProperty);
            if (binding.UpdateSourceTrigger == UpdateSourceTrigger.Default ||
                binding.UpdateSourceTrigger == UpdateSourceTrigger.LostFocus)
            {
                BindingOperations.GetBindingExpression(this, BindableDocumentProperty).UpdateSource();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="T:System.Windows.Documents.FlowDocument"></see> that represents the contents of the <see cref="T:System.Windows.Controls.BindableRichTextBox"></see>.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Windows.Documents.FlowDocument"></see> object that represents the contents of the <see cref="T:System.Windows.Controls.BindableRichTextBox"></see>.By default, this property is set to an empty <see cref="T:System.Windows.Documents.FlowDocument"></see>.  Specifically, the empty <see cref="T:System.Windows.Documents.FlowDocument"></see> contains a single <see cref="T:System.Windows.Documents.Paragraph"></see>, which contains a single <see cref="T:System.Windows.Documents.Run"></see> which contains no text.</returns>
        /// <exception cref="T:System.ArgumentException">Raised if an attempt is made to set this property to a <see cref="T:System.Windows.Documents.FlowDocument"></see> that represents the contents of another <see cref="T:System.Windows.Controls.RichTextBox"></see>.</exception>
        /// <exception cref="T:System.ArgumentNullException">Raised if an attempt is made to set this property to null.</exception>
        /// <exception cref="T:System.InvalidOperationException">Raised if this property is set while a change block has been activated.</exception>
        public FlowDocument BindableDocument
        {
            get { return (FlowDocument)GetValue(BindableDocumentProperty); }
            set { SetValue(BindableDocumentProperty, value); }
        }

        #endregion

        #region DocumentPropertyChanged - reattached event handlers to all child comboboxes in the new document

        /// <summary>
        /// When Document Property Changed, the new Document needs to attached the event handlers of all comboboxes
        /// withing then new Document, and dettach handlers in old document.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void DocumentPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            RichTextBoxWithIntellisense textBox = sender as RichTextBoxWithIntellisense;

            if (textBox != null)
            {
                FlowDocument oldDocument = args.OldValue as FlowDocument;
                if (oldDocument != null)
                {
                    textBox.DettachEventHandlersFromAllComboBoxes(oldDocument);
                }
                FlowDocument newDocument = args.NewValue as FlowDocument;
                if (newDocument != null)
                {
                    textBox.AttachEventHandlersToAllComboBoxes(newDocument);
                }

                textBox.IsTextDirty = false;
            }
        }

        /// <summary>
        /// Searches for all chidren comboboxes and attaches event handlers to them. 
        /// </summary>
        private void AttachEventHandlersToAllComboBoxes(FlowDocument document)
        {
            System.Diagnostics.Debug.WriteLine("Attaching eventhandlers to combobox statements");

            for (TextPointer position = document.ContentStart;
                  position != null && position.CompareTo(document.ContentEnd) <= 0;
                  position = position.GetNextContextPosition(LogicalDirection.Forward))
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd)
                {
                    // process only UiElements. all other elements are ignored
                    InlineUIContainer uiContainer;
                    if ((uiContainer = position.Parent as InlineUIContainer) != null)
                    {
                        ComboBoxStatement statementBox = uiContainer.Child as ComboBoxStatement;
                        statementBox.ComboBoxDropDownOpened += OnComboBoxStatementDropDownOpened;
                        statementBox.ComboBoxDropDownClosed += OnComboBoxStatementDropDownClosed;
                    }

                }
            } 
        }

        /// <summary>
        /// Searches for all chidren comboboxes and deattaches event handlers from them. 
        /// </summary>
        private void DettachEventHandlersFromAllComboBoxes(FlowDocument document)
        {
            System.Diagnostics.Debug.WriteLine("Detaching eventhandlers from combobox statements");

            for (TextPointer position = document.ContentStart;
              position != null && position.CompareTo(document.ContentEnd) <= 0;
              position = position.GetNextContextPosition(LogicalDirection.Forward))
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd)
                {
                    // process only UiElements. all other elements are ignored
                    InlineUIContainer uiContainer;
                    if ((uiContainer = position.Parent as InlineUIContainer) != null)
                    {
                        ComboBoxStatement statementBox = uiContainer.Child as ComboBoxStatement;
                        statementBox.ComboBoxDropDownOpened -= OnComboBoxStatementDropDownOpened;
                        statementBox.ComboBoxDropDownClosed -= OnComboBoxStatementDropDownClosed;
                    }

                }
            } 
        }

        #endregion

        #region FillItemsEvent on combo box dropdown opened

        /// <summary>
        /// EventHandler that is being called if any of the comboboxes are opened
        /// </summary>
        public event EventHandler<FillItemsNeededEventArgs> FillItemsNeeded;

        /// <summary>
        /// If any of child comboboxes is opened fire the FillItemsNeeded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnComboBoxStatementDropDownOpened(object sender, FillItemsNeededEventArgs args)
        {
            IsComboOpen = true;
            if (FillItemsNeeded != null)
            {
                FillItemsNeeded(sender, args);
            }
        }

        private bool IsComboOpen;

        #endregion

        #region Explicit Update Binding events

        void OnComboBoxStatementDropDownClosed(object sender, FillItemsNeededEventArgs e)
        {
            IsComboOpen = false;

            UpdateDocumentBinding();
        }

        protected override void OnIsKeyboardFocusedChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsKeyboardFocusedChanged(e);
            if ((bool)e.NewValue == false)
            {
                if (m_assistListPopup.IsOpen == false && IsComboOpen == false)
                {
                    UpdateDocumentBinding();
                }
            }
        }
        
        private void OnPopupClosed(object sender, System.EventArgs e)
        {
            if (IsKeyboardFocusWithin == false)
            {
                UpdateDocumentBinding();
            }
        }

        public void UpdateDocumentBinding()
        {
            BindingExpression be = GetBindingExpression(BindableDocumentProperty);
            if (be != null)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!Update document binding to source."));
                be.UpdateSource();
                IsTextDirty = false;
            }
        }


        public readonly static DependencyProperty IsTextDirtyProperty = DependencyProperty.Register("IsTextDirty", typeof(bool), typeof(RichTextBoxWithIntellisense));
        public bool IsTextDirty
        {
            get { return (bool)GetValue(IsTextDirtyProperty); }
            set {
                System.Diagnostics.Debug.WriteLine(String.Format("Text dirty set to value {0}", value));
                SetValue(IsTextDirtyProperty, value); 
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            if (e.Changes.Count != 0)
            {
                IsTextDirty = true;
            }
        }

        #endregion

        #region Assist Intellisense Popup

        /// <summary>
        /// list of assist triggers for the assist intellisense list popup/
        /// </summary>
        public List<String> ContentAssistTriggers
        {
            get { return (List<String>)GetValue(ContentAssistTriggersProperty); }
            set { SetValue(ContentAssistTriggersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentAssistSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentAssistTriggersProperty =
            DependencyProperty.Register("ContentAssistTriggers", typeof(List<String>), typeof(RichTextBoxWithIntellisense), new UIPropertyMetadata(new List<String>()));

        /// <summary>
        /// Represents assist intelisense popup
        /// </summary>
        private AssistListPopup m_assistListPopup = new AssistListPopup();

        /// <summary>
        /// when value in assist popup has been selected insert new statement with combobox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnSelectedStatement(object sender, SelectedStatementEventArgs args)
        {
            if (args != null)
            {
                Focus();
                //first remove last typed word
                RemoveLastWord();
                InsertStatementWithComboBox(args.SelectedStatement);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            // Get the last word in the text
            string lastWord = GetLastWord();

            bool wordIsValid = false;

            // Check if the last word equal to the any item from triggers
            if (!string.IsNullOrWhiteSpace(lastWord) && ContentAssistTriggers.Any(item => item.StartsWith(lastWord, StringComparison.CurrentCultureIgnoreCase)))
            {
                wordIsValid = true;
            }
            else
            {
                // All non-valid words should close the popup.
                ClosePopup();
            }

            e.Handled = true;

            if ((e.Key >= Key.A && e.Key <= Key.Z) || e.Key == Key.Back || e.Key == Key.Delete)
            {
                if (wordIsValid)
                {
                    ShowPopup(lastWord);
                }
            }
        }

        /// <summary>
        /// Shows popup window, if last letter is part of the any of the existing content assist triggers
        /// </summary>
        /// <param name="e"></param>
        protected override void  OnPreviewKeyDown(KeyEventArgs e)
        {
            if (m_assistListPopup.IsOpen)
            {
                if (e.Key == Key.Down)
                {
                    m_assistListPopup.MoveToNext();
                    e.Handled = true;
                }
                else if (e.Key == Key.Up)
                {
                    m_assistListPopup.MoveToPrevious();
                    e.Handled = true;
                }
                else if (e.Key == Key.Enter || e.Key == Key.Return)
                {
                    m_assistListPopup.SelectCurrent();
                    m_assistListPopup.IsOpen = false;
                    e.Handled = true;
                }
                else if (e.Key == Key.Escape)
                {
                    m_assistListPopup.IsOpen = false;
                    e.Handled = true;
                }
            }

            if (!e.Handled)
            {
                base.OnPreviewKeyDown(e);
            }
        }

        private void ClosePopup()
        {
            m_assistListPopup.IsOpen = false;
        }

        /// <summary>
        /// Shows popup, filters assist list based on the last word. Place popup just at the current CaretPosition within RichTextBox
        /// </summary>
        /// <param name="lastWord"></param>
        private void ShowPopup(string lastWord)
        {
            m_assistListPopup.DataContext = ContentAssistTriggers.FindAll(item => item.StartsWith(lastWord, StringComparison.CurrentCultureIgnoreCase));
            m_assistListPopup.PlacementTarget = this;
            m_assistListPopup.PlacementRectangle = CaretPosition.GetCharacterRect(LogicalDirection.Forward);
            m_assistListPopup.IsOpen = true;
        }

        /// <summary>
        /// Insert statement with a combobox into current document within richtextbox at current caret position.
        /// </summary>
        /// <param name="precedeStatement"></param>
        public void InsertStatementWithComboBox(Statement precedeStatement)
        {
            //get position where element should be added
            TextPointer insertionPosition = CaretPosition.GetInsertionPosition(LogicalDirection.Forward);

            ComboBoxStatement statementBox = new ComboBoxStatement(precedeStatement);

            //attach event
            statementBox.ComboBoxDropDownOpened += OnComboBoxStatementDropDownOpened;
            statementBox.ComboBoxDropDownClosed += OnComboBoxStatementDropDownClosed;
            
            InlineUIContainer myInlineUIContainer = new InlineUIContainer(statementBox, insertionPosition);

            //move caret position
            TextPointer newCaretPosition = insertionPosition.GetNextInsertionPosition(LogicalDirection.Forward);
            if (newCaretPosition != null)
                CaretPosition = newCaretPosition;
        }

        private void RemoveLastWord()
        {
            int lastWordLength = GetLastWord().Length;
            TextPointer deletePosition = CaretPosition.GetPositionAtOffset(-lastWordLength, LogicalDirection.Forward);
            deletePosition.DeleteTextInRun(lastWordLength);
        }

        private static readonly char[] SplitTokens = new char[] { ' ', '{', '(', ')', '=', '\n', '\r' };

        /// <summary>
        /// determine last word typed in. Looks for list word until reaches space ' ', or any of the characters: '{', '}', '(', ')', '='
        /// </summary>
        /// <returns></returns>
        private string GetLastWord()
        {
            //Get the last word in the text from current Caret position
            string txtBefore = CaretPosition.GetTextInRun(LogicalDirection.Backward);
            string txtAfter = CaretPosition.GetTextInRun(LogicalDirection.Forward);

            string[] tokens = txtBefore.Split(SplitTokens);
            string[] tokensAfter = txtAfter.Split(SplitTokens);
            string lastToken = tokens[tokens.Length - 1];

            if (tokensAfter.Length > 0 && string.IsNullOrWhiteSpace(tokensAfter[0]) == false)
            {
                lastToken = lastToken + tokensAfter[0];
            }

            // Make sure that the previous character isn't whitespace - eg. we haven't moved past the last word.
            if (txtBefore.Length > 0)
            {
                string previousString = new string(txtBefore[txtBefore.Length - 1], 1);
                if (string.IsNullOrWhiteSpace(previousString))
                {
                    lastToken = string.Empty;
                }
            }

            return lastToken;
        }

        #endregion
    }

}
