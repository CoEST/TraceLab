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
using System.IO;
using TraceLab.Core.Experiments;
using TraceLab.Core.ViewModels;
using System.Text.RegularExpressions;
using TraceLab.Core.Components;
using System.Collections.Generic;
using TraceLab.Core.Experiments;
using Gtk;

namespace TraceLab.UI.GTK
{
    public partial class AboutExperimentDialog : Gtk.Dialog
    {
        private ApplicationViewModel m_applicationViewModel;
        private string dateFormat = "MM/dd/yyyy";
        private string ABOUT_THE_EXPERIMENT = "About the experiment";
        private string ABOUT_THE_CHALLENGE = "About the challenge";
        private string EXPERIMENT_NAME = "Experiment name:";
        private string CHALLENGE_NAME = "Challenge name:";

        protected AboutExperimentDialog ()
        {
            this.Build ();
        }

        public AboutExperimentDialog (ApplicationViewModel appVM) : this()
        {
            m_applicationViewModel = appVM;

            tbx_experimentName.Text = m_applicationViewModel.Experiment.ExperimentInfo.Name;
            tbx_author.Text = m_applicationViewModel.Experiment.ExperimentInfo.Author;
            tbx_contributors.Text = m_applicationViewModel.Experiment.ExperimentInfo.Contributors;
            tbx_description.Buffer.Text = m_applicationViewModel.Experiment.ExperimentInfo.Description;

            string IsChallenge = m_applicationViewModel.Experiment.ExperimentInfo.IsChallenge;
        
            //check if it's challenge or not
            if (!string.IsNullOrEmpty (IsChallenge) && IsChallenge.Equals ("True")) {
                showOrHydeHiddenFields (true);
                tbx_tags.Text = m_applicationViewModel.Experiment.ExperimentInfo.Tags;
          
                //fill the metric dropdown with output variables and set the default value (if any)
                fillMetricVariable ();

                String metricValue = m_applicationViewModel.Experiment.ExperimentInfo.ExperimentResultsUnitname;
                if (!string.IsNullOrEmpty (metricValue)) {
                    setActiveComboBoxValue (metricCombobox, metricValue);
                }

                //sets the DeadLine
                if (!string.IsNullOrEmpty (m_applicationViewModel.Experiment.ExperimentInfo.Deadline)) {
                    this.deadLineTextArea.Text = m_applicationViewModel.Experiment.ExperimentInfo.Deadline;
                }

                //change label for window title and Label name
                this.label1.Text = CHALLENGE_NAME;
                this.Title = ABOUT_THE_CHALLENGE;
               
            } else {
                //change label for window title and Label name
                this.label1.Text = EXPERIMENT_NAME;
                this.Title = ABOUT_THE_EXPERIMENT;

                showOrHydeHiddenFields (false);
            }            
        }

        protected void buttonOKClickedHandler (object sender, EventArgs e)
        {
            m_applicationViewModel.Experiment.ExperimentInfo.Name = this.tbx_experimentName.Text;
            m_applicationViewModel.Experiment.ExperimentInfo.Author = this.tbx_author.Text;
            m_applicationViewModel.Experiment.ExperimentInfo.Contributors = this.tbx_contributors.Text;
            m_applicationViewModel.Experiment.ExperimentInfo.Description = this.tbx_description.Buffer.Text;
            m_applicationViewModel.Experiment.ExperimentInfo.Tags = formatTags (this.tbx_tags.Text);
            m_applicationViewModel.Experiment.ExperimentInfo.Deadline = this.deadLineTextArea.Text;

            TreeIter tree;
            if (metricCombobox.GetActiveIter (out tree)) {
                m_applicationViewModel.Experiment.ExperimentInfo.ExperimentResultsUnitname = ((String)metricCombobox.Model.GetValue (tree, 0));
            }

            this.Destroy ();
        }

        protected void buttonCancelClickedHandler (object sender, EventArgs e)
        {
            if (pwdPicker != null)
                pwdPicker.Destroy ();
            if (datePicker != null)
                datePicker.Destroy ();

            this.Destroy ();
        }

        private void fillMetricVariable (){
            clearComboBox (metricCombobox);

            ExperimentNode currentNode = m_applicationViewModel.Experiment.StartNode as ExperimentNode;

            //ExperimentNode currentNode = DecisionControl.ExperimentNode;
            Dictionary<string, string> predeccessorsOutputsNameTypeLookup;
            var availableInputMappingsPerNode = new TraceLab.Core.Utilities.InputMappings (m_applicationViewModel.Experiment);

            if (availableInputMappingsPerNode.TryGetValue (currentNode, out predeccessorsOutputsNameTypeLookup) == false) {
                predeccessorsOutputsNameTypeLookup = new Dictionary<string, string> (); 
            }

            foreach (string workspaceUnitName in predeccessorsOutputsNameTypeLookup.Keys) {
                this.metricCombobox.AppendText ( workspaceUnitName );
            }     

           // setActiveComboBoxValue (metricCombobox, SELECT_A_VALUE);  
        }

        private void clearComboBox (Gtk.ComboBox combo)
        {
            Gtk.ComboBox cb = combo;

            cb.Clear ();
            CellRendererText cell = new CellRendererText ();
            cb.PackStart (cell, false);
            cb.AddAttribute (cell, "text", 0);
            ListStore store = new ListStore (typeof(string));
            cb.Model = store;
        }

        protected void onChangeValidateHandler (object sender, EventArgs e)
        {
            bool isDataValid = true;

            isDataValid = isDataValid && !String.IsNullOrWhiteSpace (tbx_experimentName.Text);

            btn_ok.Sensitive = isDataValid;
        }

        /// TLAB-66
        private void showOrHydeHiddenFields (bool isChallenge)
        {
            //DO NOT show the "challenge" related fields
            if (!isChallenge) {
               
                this.tbx_tags.Visible = isChallenge;
                this.tagsLabel.Visible = isChallenge;
                this.metricLabel.Visible = isChallenge;
                this.metricCombobox.Visible = isChallenge;
                this.deadlineLabel.Visible = isChallenge;
                this.deadLineTextArea.Visible = isChallenge;
                this.datePickerButton.Visible = isChallenge;
                this.lockUnlock.Visible = isChallenge;

                this.Resize (671, 200);
               
            } else {
                //if the user unlocked the challenge with a password challenge he can't 
                //access all the fields
                if (hasChallengePasswordBeenUsed()) {
                    this.tbx_tags.IsEditable = false;
                    this.tbx_experimentName.IsEditable = false;
                    this.deadLineTextArea.IsEditable = false;
                    this.lockUnlock.Visible = false;
                    this.datePickerButton.Visible = false;
                    this.metricCombobox.Sensitive = false;
                }

                this.Resize(671, 502);
            }        
          
        }

        ///TLAB-66
        private bool checkTags ()
        {
            string separators = @", | ,|,";  
            string[] words = Regex.Split (tbx_tags.Text, separators);
            foreach (string s in words) {
                string[] twoWords = Regex.Split (s.Trim (), @" ");
                if (twoWords.Length > 1) {
                    return false;
                }   
            }
            return true;
        }

        ///TLAB-66
        private string formatTags (string tags)
        {
            string toReturn = "";
            string separators = @", | ,|,";  
            string[] words = Regex.Split (tbx_tags.Text, separators);
            foreach (string s in words) {
                string trimmedString = s.Trim ();
                if (!String.IsNullOrEmpty (trimmedString))
                    toReturn += trimmedString + ", ";
            }

            //remove last char that is a ","
            if(toReturn.Length > 0)
                toReturn = toReturn.Remove (toReturn.Length-2); 

            return toReturn;
        }

        private CalendarDatePickerDialog datePicker;
        private PasswordPicker pwdPicker;

        public void datePickerDestroyed (object sender, EventArgs e)
        { 
            this.Modal = true;
            this.deadLineTextArea.Text = datePicker.DatePicked.ToString (dateFormat);

            datePicker = null;
        }

        public void passwordPickerDestroyed (object sender, EventArgs e)
        { 
            if (pwdPicker.passwordChanged) {
                this.Modal = true;
                //update passwords in the metadata
                this.m_applicationViewModel.Experiment.ExperimentInfo.ChallengePassword = pwdPicker.challengePwd;
                this.m_applicationViewModel.Experiment.ExperimentInfo.ExperimentPassword = pwdPicker.experimentPwd;
                string oldPath = m_applicationViewModel.Experiment.ExperimentInfo.FilePath;
                string extension = System.IO.Path.GetFileNameWithoutExtension (oldPath);

                if (!string.IsNullOrEmpty (pwdPicker.challengePwd) || !string.IsNullOrEmpty (pwdPicker.experimentPwd)) {
                    //there is AT LEAST one password 
                    //call the procedure to save a locked esxperiment in TEMLX format
                    string path;
                    if (!extension.Equals (".temlx")) {
                        path = addExt (m_applicationViewModel.Experiment.ExperimentInfo.FilePath, ".temlx");
                     } else {
                        //save an updated version of the experiment
                        path = m_applicationViewModel.Experiment.ExperimentInfo.FilePath;               
                    }
                    ExperimentManager.SaveToCrypt (this.m_applicationViewModel.Experiment, path);

                } else {
                    //there are no password so we should decrypt the file, if this is a TEMLX
                    //save the experiment decrypted and delete the .temlx file
                    string uniquePat = GetUniqueName (m_applicationViewModel.Experiment.ExperimentInfo.FilePath, ".teml");
                    ExperimentManager.Save (this.m_applicationViewModel.Experiment, uniquePat);
                }

                //delete old file TEML
                ExperimentManager.DeleteFile (oldPath);
            }
            pwdPicker = null;
        }

        protected void OnDatePickerButtonClicked (object sender, EventArgs e)
        {
            if (datePicker == null) {               
                if (string.IsNullOrEmpty (this.deadLineTextArea.Text))
                    datePicker = new CalendarDatePickerDialog (DateTime.Now);
                else
                    datePicker = new CalendarDatePickerDialog (DateTime.Parse (this.deadLineTextArea.Text));

                datePicker.Destroyed += new EventHandler (datePickerDestroyed); 
                this.Modal = false;
                datePicker.Modal = true;
                datePicker.ShowAll (); 
            }
        }

        protected void OnLockUnlockClicked (object sender, EventArgs e)
        {
            if (pwdPicker == null) {
                pwdPicker = new PasswordPicker (this.m_applicationViewModel.Experiment.ExperimentInfo.ChallengePassword, this.m_applicationViewModel.Experiment.ExperimentInfo.ExperimentPassword);
                pwdPicker.Destroyed += new EventHandler (passwordPickerDestroyed);
                this.Modal = false;
                pwdPicker.Modal = true;
                pwdPicker.ShowAll (); 

            }
        }

        private string addExt (string originalPath, string ext)
        {
            int delimitator = originalPath.LastIndexOf (".");                        
            string withoutExt = originalPath.Remove (originalPath.Length - (originalPath.Length - delimitator));
            return withoutExt + ext;
        }

        private string GetUniqueName(string path, string ext)
        {
            string pathAndFileName = path;
            string pathToFile = System.IO.Path.GetDirectoryName(path);
            string validatedName = System.IO.Path.GetFileNameWithoutExtension(path) + ext;
         //   string pathToFile = path.Replace (System.IO.Path.GetFileName(path), '');

            int count = 1;
            while(File.Exists(System.IO.Path.Combine(pathToFile, validatedName)))
            {
                validatedName = string.Format ("{0}{1}{2}",
                                              System.IO.Path.GetFileNameWithoutExtension (pathAndFileName),
                                              count++,
                                              ext);
                                              //System.IO.Path.GetExtension(pathAndFileName));
            }
            return System.IO.Path.Combine(pathToFile, validatedName);
        }

        protected void OnMetricComboboxChanged (object sender, EventArgs e)
        {
            TreeIter tree;
            if (metricCombobox.GetActiveIter (out tree)) {
                m_applicationViewModel.Experiment.ExperimentInfo.ExperimentResultsUnitname = ((String)metricCombobox.Model.GetValue (tree, 0));
            }
        }

        private void setActiveComboBoxValue (Gtk.ComboBox cb, string s)
        {                      
            Gtk.TreeIter iter;
            cb.Model.GetIterFirst (out iter);
            do {
                GLib.Value thisRow = new GLib.Value ();
                cb.Model.GetValue (iter, 0, ref thisRow);
                if ((thisRow.Val as string).Equals (s)) {
                    cb.SetActiveIter (iter);
                    break;
                }
            } while (cb.Model.IterNext (ref iter));          
        }

        protected void OnPublishButtonClicked (object sender, EventArgs e)
        {
            Console.WriteLine ("PublishButton Clicked");
        }

        private bool hasChallengePasswordBeenUsed(){
            if (!string.IsNullOrEmpty (m_applicationViewModel.Experiment.ExperimentInfo.ChallengePassword) &&
                m_applicationViewModel.Experiment.ExperimentInfo.ChallengePassword.Equals(m_applicationViewModel.Experiment.unlockingPassword)){
                return true;
            }
            return false;        
        }
    }
}

