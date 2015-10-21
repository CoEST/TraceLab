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
//
using System;
using TraceLab.Core.Experiments;
using Gtk;

// HERZUM SPRINT 3.0: TLAB-176
using TraceLab.Core.Components;
// END HERZUM SPRINT 3.0: TLAB-176

namespace TraceLab.UI.GTK
{
    public partial class DefineCompositeComponentWizard : Gtk.Window
    {
        public DefineCompositeComponentWizard () : 
                base(Gtk.WindowType.Toplevel)
        {
            this.Build ();
        }

        // HERZUM SPRINT 3.0: TLAB-176
        private bool IncludeChallenge(CompositeComponentGraph experiment) {
            if (experiment == null)
                return false;
            foreach (ExperimentNode node in experiment.Vertices){
                ChallengeMetadata ch_meta = node.Data.Metadata as ChallengeMetadata;
                if (ch_meta != null){

                    // HERZUM SPRINT 3.0: TLAB-176
                    ExperimentNodeConnection firstInEdge = experiment.InEdge (node,0);
                    ExperimentNodeConnection firstOutEdge = experiment.OutEdge (node,0);
                    if (firstInEdge != null && firstInEdge.Source!= null && firstOutEdge != null && firstOutEdge.Target!= null)
                        experiment.AddEdge(new ExperimentNodeConnection(Guid.NewGuid().ToString(), firstInEdge.Source, firstOutEdge.Target));
                    // END HERZUM SPRINT 3.0: TLAB-176
                    experiment.RemoveVertex (node);
                    return true;
                }
                CompositeComponentBaseMetadata meta = node.Data.Metadata as CompositeComponentBaseMetadata;
                if (meta != null)
                    return IncludeChallenge(meta.ComponentGraph);
            }
            return false;
        }

        internal void ShowMessageDialog(string message, string title, Gtk.ButtonsType buttonsType, Gtk.MessageType messageType)
        {
            Gtk.MessageDialog dlg = new Gtk.MessageDialog(new Gtk.Window("Message"), 
                                                          Gtk.DialogFlags.Modal, 
                                                          messageType, 
                                                          buttonsType, message);
            dlg.Title = title;
            dlg.Run();
            dlg.Destroy();
        }
        // END HERZUM SPRINT 3.0: TLAB-176

        public DefineCompositeComponentWizard(ApplicationContext context) : this() 
        {

            //setup extracts selected nodes from current experiment and constructs composite component
            m_setup = new DefiningCompositeComponentSetup(context.Application.Experiment);

            // HERZUM SPRINT 3.0: TLAB-176
            if (IncludeChallenge(m_setup.CompositeComponentGraph))
            {
                ShowMessageDialog("Challenge Component Removed",
                                  "Define Composite", Gtk.ButtonsType.Ok, Gtk.MessageType.Warning);
            }
            // END HERZUM SPRINT 3.0: TLAB-176

            // HERZUM SPRINT 2.4 TLAB-157
            //create new experiment drawer with own factories
            /*
            ExperimentDrawer drawer = new ExperimentDrawer(this.experimentcanvaswidget1,
                                                           new NodeControlFactory(context),
                                                           new NodeConnectionControlFactory(context));
            //draw composite component that is being defined into canvas
            drawer.DrawExperiment(m_setup.CompositeComponentGraph, false);
            */

            ExperimentCanvasPadFactory.CreateCompositeExperimentCanvasPad(context, this.experimentcanvaswidget1, m_setup.CompositeComponentGraph) ;
            // END HERZUM SPRINT 2.4 TLAB-157

            //zoom to fit moves view to display entire graph in the visible canvas
            //rather part that is not visible
            this.experimentcanvaswidget1.ZoomToFit();

            //if experiment is smaller than than the view scale it to original 
            if(this.experimentcanvaswidget1.ZoomScale > 1)
            {
                this.experimentcanvaswidget1.ZoomToOriginal();
            }
            
            m_ioSpecSetupPage = new IOSpecSetupPage(m_setup);
            m_configPage = new ConfigSetupPage(m_setup);
            m_componentInfoPage = new ComponentInfoPage(m_setup, context.Application.Settings.ComponentPaths);
            m_confirmationPage = new ConfirmationPage();
            
            this.mainVBox.Add(m_ioSpecSetupPage);
            this.mainVBox.Add(m_configPage);
            this.mainVBox.Add(m_componentInfoPage);
            this.mainVBox.Add(m_confirmationPage);

            m_componentInfoPage.ComponentNameChanged += delegate {
                RefreshDefineButton();
            };

            m_currentState = WizardState.IOSpec;
            
            DisplayCurrentPage();
        }
        
        private void AdvancePage()
        {
            if (m_currentState != WizardState.Confirmation) //last step is confirmation
            {
                m_currentState++;
            }
            
            RefreshAllButtons();
            DisplayCurrentPage();
        }
        
        private void BacktracePage()
        {
            if (m_currentState > 0)
            {
                m_currentState--;
            }
            
            RefreshAllButtons(); 
            DisplayCurrentPage();
        }

        #region Refresh Buttons Visibility

        void RefreshAllButtons() 
        {
            RefreshBackButton();
            RefreshForwardButton();
            RefreshDefineButton();
            RefreshCancelButton();
            RefreshOkButton();
        }
        
        void RefreshBackButton()
        {
            //allow going back if it is not the first step
            if (m_currentState == 0) 
            {
                backButton.Sensitive = false;
            }
            else
            {
                backButton.Sensitive = true;
            }
        }
        
        void RefreshForwardButton()
        {
            if (m_currentState >= 0 && m_currentState < WizardState.Info) 
            {
                forwardButton.Visible = true;
            } 
            else
            {
                forwardButton.Visible = false;
            }
        }

        void RefreshDefineButton()
        {
            if (m_currentState == WizardState.Info) 
            {
                defineButton.Visible = true;
                if(ValidateComponentInfoForm()) 
                {
                    defineButton.Sensitive = true;
                } 
                else
                {
                    defineButton.Sensitive = false;
                }
            } 
            else
            {
                defineButton.Visible = false;
            }
        }

        void RefreshCancelButton()
        {
            if (m_currentState == WizardState.Confirmation) 
            {
                cancelButton.Visible = (HasError) ? true : false;
            } 
            else
            {
                cancelButton.Visible = true;
            }
        }

        void RefreshOkButton()
        {
            if (m_currentState == WizardState.Confirmation) 
            {
                okButton.Visible = (HasError) ? false : true;
            } 
            else
            {
                okButton.Visible = false;
            }
        }

        #endregion Refresh Buttons Visibility

        private bool HasError
        {
            get;
            set;
        }
        
        private void DisplayCurrentPage() 
        {
            //hide all 
            m_ioSpecSetupPage.Hide();
            m_configPage.Hide();
            m_componentInfoPage.Hide();
            m_confirmationPage.Hide();
            
            switch(m_currentState)
            {
            case WizardState.IOSpec: 
                m_ioSpecSetupPage.Show();
                break;
            case WizardState.Configuration:
                m_configPage.Show();
                break;
            case WizardState.Info:
                m_componentInfoPage.Show();
                break;
            case WizardState.Confirmation:
                m_confirmationPage.Show();
                break;
            default:
                throw new InvalidOperationException();
            }
        }
        
        protected void forwardButtonClicked(object sender, EventArgs e)
        {
            AdvancePage();
        }
        
        protected void backButtonClicked(object sender, EventArgs e)
        {
            BacktracePage();
        }
        
        protected void defineButtonClicked(object sender, EventArgs e)
        {
            //ask for confirmation if file exists
            bool shouldContinue = ShouldOverwriteFile();

            if (shouldContinue && ValidateComponentInfoForm())
            {
                try
                {
                    m_setup.DefineComponent();
                    m_confirmationPage.SetConfirmationMessage(
                        String.Format("Composite component '{0}' has been created successfully!\n\n Refresh components library to find the component.", m_setup.Name));
                }
                catch (System.UnauthorizedAccessException ex) //if access to the specified path is denied
                {
                    m_confirmationPage.SetErrorMessage(ex.Message + "\n\n You may go back and try again.");
                }
                catch (System.IO.PathTooLongException ex)
                {
                    m_confirmationPage.SetErrorMessage(ex.Message + "\n\n You may go back and try again.");
                }
                catch (System.IO.IOException ex)
                {
                    m_confirmationPage.SetErrorMessage(ex.Message + "\n\n You may go back and try again.");
                }
                catch (System.Security.SecurityException ex)
                {
                    m_confirmationPage.SetErrorMessage(ex.Message + "\n\n You may go back and try again.");
                }
                finally
                {
                    AdvancePage();
                }
            }
        }

        private bool ShouldOverwriteFile()
        {
            bool shouldContinue = true;
            
            if (System.IO.File.Exists(m_setup.CompositeComponentLocationFilePath))
            {
                //show dialog
                string messageBoxText = String.Format(TraceLab.Core.Messages.ShouldOverwriteFileQuestion, 
                                                      m_setup.CompositeComponentLocationFilePath);
                // Display message box
                Gtk.MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, 
                                                             ButtonsType.YesNo,
                                                             messageBoxText);
                ResponseType response = (ResponseType)dialog.Run();

                shouldContinue = (response == ResponseType.Yes);

                dialog.Destroy();
            }

            return shouldContinue;
        }

        private bool ValidateComponentInfoForm()
        {
            bool isValid = true;
            if (String.IsNullOrWhiteSpace(m_setup.Name))
                isValid = false;
            
            return isValid;
        }

        /// <summary>
        /// Closes the window when either Ok, or Cancel button is clicked
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected void closeWindow(object sender, EventArgs e)
        {
            Destroy();
        }

        private DefiningCompositeComponentSetup m_setup;
        private WizardState m_currentState;
        private IOSpecSetupPage m_ioSpecSetupPage;
        private ConfigSetupPage m_configPage;
        private ComponentInfoPage m_componentInfoPage;
        private ConfirmationPage m_confirmationPage;

        private enum WizardState : int
        {
            IOSpec,
            Configuration,
            Info,
            Confirmation
        }
    }
}

