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
using System.Linq;
using System.Text;
using TraceLab.Core.Experiments;
using System.ComponentModel;
using System.Windows.Input;
using TraceLab.UI.WPF.Commands;
using System.Collections.ObjectModel;
using System.Windows;

namespace TraceLab.UI.WPF.ViewModels
{
    class DefiningCompositeComponentSetupViewModel : INotifyPropertyChanged
    {
        #region Constructor

        public DefiningCompositeComponentSetupViewModel(ExperimentViewModel experiment, ObservableCollection<SettingsPathViewModel> componentsPaths, 
                                                        Action<TraceLab.Core.Components.CompositeComponentMetadataDefinition> howToAddToComponentsLibrary)
        {
            m_wrappedSetup = new DefiningCompositeComponentSetup((Experiment)experiment.TopLevel, howToAddToComponentsLibrary);
            CompositeComponentGraph = new ExperimentViewModel(m_wrappedSetup.CompositeComponentGraph);

            //propagate notify on wrapped properties changes
            m_wrappedSetup.PropertyChanged += WrappedPropertyChanged;

            ComponentPaths = componentsPaths;
            SelectedComponentsDirectory = ComponentPaths.First<SettingsPathViewModel>();

            AdvanceState = new DelegateCommand(DoAdvanceState, CanAdvanceState);
            BacktrackState = new DelegateCommand(DoBacktrackState, CanBacktrackState);
            DefineComponent = new DelegateCommand(ExecuteDefineComponent, CanExecuteDefineComponent);
        }
        
        #endregion

        #region Define Component command

        public ICommand DefineComponent
        {
            get;
            private set;
        }

        private void ExecuteDefineComponent(object param)
        {
            //ask for confirmation if file exists
            bool shouldContinue = ShouldOverwriteFile();

            if (shouldContinue)
            {
                try
                {
                    m_wrappedSetup.DefineComponent();
                    ConfirmationMessage = String.Format("Composite component '{0}' has been created successfully!", Name);
                }
                catch (System.UnauthorizedAccessException ex) //if access to the specified path is denied
                {
                    ErrorMessage = ex.Message + "\n\n You may go back and try again.";
                }
                catch (System.IO.PathTooLongException ex)
                {
                    ErrorMessage = ex.Message + "\n\n You may go back and try again.";
                }
                catch (System.IO.IOException ex)
                {
                    ErrorMessage = ex.Message + "\n\n You may go back and try again.";
                }
                catch (System.Security.SecurityException ex)
                {
                    ErrorMessage = ex.Message + "\n\n You may go back and try again.";
                }
                finally
                {
                    DoAdvanceState(null);
                }
            }
        }

        private bool ShouldOverwriteFile()
        {
            bool shouldContinue = true;

            if (System.IO.File.Exists(CompositeComponentLocationFilePath))
            {
                //show dialog
                string messageBoxText = String.Format(TraceLab.Core.Messages.ShouldOverwriteFileQuestion, CompositeComponentLocationFilePath);
                // Display message box
                MessageBoxResult result = MessageBox.Show(messageBoxText, TraceLab.Core.Messages.DoYouWantToContinue, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                shouldContinue = (result == MessageBoxResult.Yes);
            }
            return shouldContinue;
        }

        private bool CanExecuteDefineComponent(object param)
        {
            return ValidateComponentInfoForm();
        }

        private bool ValidateComponentInfoForm()
        {
            bool isValid = true;
            if (String.IsNullOrWhiteSpace(Name))
                isValid = false;

            return isValid;
        }

        #endregion 

        #region Helper Properties

        private string m_errorMessage;

        /// <summary>
        /// Gets or sets the error, if composite component creation was unsuccessful
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        public string ErrorMessage
        {
            get { return m_errorMessage; }
            set 
            {
                if (m_errorMessage != value)
                {
                    m_errorMessage = value;
                    NotifyPropertyChanged("ErrorMessage");
                }
            }
        }

        private string m_confirmationMessage;

        /// <summary>
        /// Gets or sets the error, if composite component creation was unsuccessful
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        public string ConfirmationMessage
        {
            get { return m_confirmationMessage; }
            set
            {
                if (m_confirmationMessage != value)
                {
                    m_confirmationMessage = value;
                    NotifyPropertyChanged("ConfirmationMessage");
                }
            }
        }

        private ObservableCollection<SettingsPathViewModel> m_componentsPaths;

        /// <summary>
        /// Gets the component paths settings from main application
        /// </summary>
        public ObservableCollection<SettingsPathViewModel> ComponentPaths
        {
            get { return m_componentsPaths; }
            private set
            {
                if (value != m_componentsPaths)
                {
                    m_componentsPaths = value;
                    NotifyPropertyChanged("ComponentPaths");
                }
            }
        }

        private SettingsPathViewModel m_selectedComponentsDirectory;
        public SettingsPathViewModel SelectedComponentsDirectory
        {
            get { return m_selectedComponentsDirectory; }
            set
            {
                if (value != m_selectedComponentsDirectory)
                {
                    m_selectedComponentsDirectory = value;
                    UpdateFinalComponentPath();
                    NotifyPropertyChanged("SelectedComponentsDirectory");
                }
            }
        }

        private bool m_isInfoStep;
        /// <summary>
        /// Gets a value indicating whether the wizard is in its Info step
        /// Allows determining the visibility of define button
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this wizard is in info step; otherwise, <c>false</c>.
        /// </value>
        public bool IsInfoStep
        {
            get { return m_isInfoStep; }
            private set
            {
                if (m_isInfoStep != value)
                {
                    m_isInfoStep = value;
                    NotifyPropertyChanged("IsInfoStep");
                }
            }
        }

        private bool m_isConfirmationStep;
        /// <summary>
        /// Gets a value indicating whether the wizard is on the confirmation screen.
        /// Allows determining the visibility of buttons
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the wizard is in the confirmation step; otherwise, <c>false</c>.
        /// </value>
        public bool IsConfirmationStep
        {
            get { return m_isConfirmationStep; }
            private set 
            {
                if (m_isConfirmationStep != value)
                {
                    m_isConfirmationStep = value;
                    NotifyPropertyChanged("IsConfirmationStep");
                }
            }
        }

        private bool m_isNextStep = true;
        /// <summary>
        /// Gets a value indicating whether the wizard is on the confirmation screen.
        /// Allows determining the visibility of Next button
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the wizard is in the confirmation step; otherwise, <c>false</c>.
        /// </value>
        public bool IsNextStep
        {
            get { return m_isNextStep; }
            private set
            {
                if (m_isNextStep != value)
                {
                    m_isNextStep = value;
                    NotifyPropertyChanged("IsNextStep");
                }
            }
        }

        #endregion

        #region WrappedProperties

        /// <summary>
        /// Field with reference to the wrapped setup.
        /// </summary>
        private DefiningCompositeComponentSetup m_wrappedSetup;

        private ExperimentViewModel m_compositeComponentGraph;
        /// <summary>
        /// Gets the view of wrapped composite component graph.
        /// </summary>
        public ExperimentViewModel CompositeComponentGraph
        {
            get { return m_compositeComponentGraph; }
            private set
            {
                if (value != m_compositeComponentGraph)
                {
                    m_compositeComponentGraph = value;
                    NotifyPropertyChanged("CompositeComponentGraph");
                }
            }
        }

        public SortedDictionary<string, ItemSetting> InputSettings
        {
            get { return m_wrappedSetup.InputSettings; }
        }

        public SortedDictionary<string, ItemSetting> OutputSettings
        {
            get { return m_wrappedSetup.OutputSettings; }
        }

        public SortedDictionary<string, ConfigItemSetting> ConfigSettings
        {
            get { return m_wrappedSetup.ConfigSettings; }
        }

        public string Name
        {
            get { return m_wrappedSetup.Name; }
            set 
            {
                if (value != m_wrappedSetup.Name)
                {
                    m_wrappedSetup.Name = value; //it notifies property change already
                    UpdateFinalComponentPath();
                }
            }
        }

        public string Author
        {
            get { return m_wrappedSetup.Author; }
            set { m_wrappedSetup.Author = value; }
        }

        public string Description
        {
            get { return m_wrappedSetup.Description; }
            set { m_wrappedSetup.Description = value; }
        }

        public string Version
        {
            get { return m_wrappedSetup.Version; }
            set { m_wrappedSetup.Version = value; }
        }

        public string CompositeComponentLocationFilePath
        {
            get { return m_wrappedSetup.CompositeComponentLocationFilePath; }
            set { m_wrappedSetup.CompositeComponentLocationFilePath = value; }
        }

        #endregion

        #region Helper Private Methods

        private void WrappedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(e.PropertyName);
        }

        private void UpdateFinalComponentPath()
        {
            string filename = Name.ToLower() + ".tcml";
            CompositeComponentLocationFilePath = System.IO.Path.Combine(SelectedComponentsDirectory, filename);
        }

        #endregion

        #region Wizard State with advance/back commands

        private DefiningCompositeComponentWizardState m_currentState;
        public DefiningCompositeComponentWizardState CurrentState
        {
            get
            {
                return m_currentState;
            }
            private set
            {
                if (m_currentState != value)
                {
                    m_currentState = value;
                    IsInfoStep = (m_currentState == DefiningCompositeComponentWizardState.Info);
                    IsConfirmationStep = (m_currentState == DefiningCompositeComponentWizardState.Confirmation);
                    IsNextStep = (m_currentState != DefiningCompositeComponentWizardState.Info && m_currentState != DefiningCompositeComponentWizardState.Confirmation);
                    NotifyPropertyChanged("CurrentState");                    
                }
            }
        }

        public ICommand AdvanceState
        {
            get;
            private set;
        }

        public ICommand BacktrackState
        {
            get;
            private set;
        }

        private bool CanAdvanceState(object param)
        {
            return CurrentState != DefiningCompositeComponentWizardState.Confirmation;
        }

        private void DoAdvanceState(object param)
        {
            var state = CurrentState;

            if (state != DefiningCompositeComponentWizardState.Confirmation)
            {
                CurrentState = ++state;
            }
        }

        private bool CanBacktrackState(object param)
        {
            return CurrentState != DefiningCompositeComponentWizardState.IOSpec;
        }

        private void DoBacktrackState(object param)
        {
            var state = CurrentState;
            if (state == DefiningCompositeComponentWizardState.Confirmation)
            {
                //clear the messages
                ErrorMessage = null;
                ConfirmationMessage = null;
            }

            if (state != DefiningCompositeComponentWizardState.IOSpec)
            {
                CurrentState = --state;
            }
        }

        #endregion

        #region INotifyPropertyChanged

        [NonSerialized]
        private PropertyChangedEventHandler m_propertyChanged;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                m_propertyChanged += value;
            }
            remove
            {
                m_propertyChanged -= value;
            }
        }
        protected void NotifyPropertyChanged(string prop)
        {
            if (m_propertyChanged != null)
                m_propertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }

    public enum DefiningCompositeComponentWizardState : int
    {
        IOSpec,
        Configuration,
        Info,
        Confirmation
    }
}
