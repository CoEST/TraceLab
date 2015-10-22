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
using Gtk;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;

namespace TraceLab.UI.GTK
{
    public partial class PasswordPicker : Gtk.Dialog
    {
        private bool isChallengePwdOk, isExperimentPwdOk;
        private string error;
        public string challengePwd, experimentPwd;
        private string oldChallengePwd, oldExperimentPwd;
        public bool passwordChanged;

        private static string WARNING_MESSAGE_ONLY_CHALLENGE_PASSWORD = "  \nYou entered only the \"Challenge password\", you will not be able to modify the challenge any more. \nAre you sure you want to continue?  ";

        protected PasswordPicker ()
        {
            this.Build ();
        }

        public PasswordPicker (string challengePwd, string experimentPwd) : this()
        {
            this.reinsChallengePwdEntry.Visibility = false;
            this.challengePwdEntry.Visibility = false;
            this.experimentPwdEntry.Visibility = false;
            this.reinsExperimentPwdEntry.Visibility = false;

            if (!string.IsNullOrEmpty (challengePwd)) {
                this.challengePwdEntry.Text = challengePwd;
                this.reinsChallengePwdEntry.Text = challengePwd;
                oldChallengePwd = challengePwd;
            } else 
                oldChallengePwd = "";

            if (!string.IsNullOrEmpty (experimentPwd)) {
                this.experimentPwdEntry.Text = experimentPwd;
                this.reinsExperimentPwdEntry.Text = experimentPwd;
                oldExperimentPwd = experimentPwd;
            } else 
                oldExperimentPwd = "";

            this.passwordChanged = false;
        }

        protected void OnButtonOkClicked (object sender, EventArgs e)
        {
            this.error = "";
            this.challengePwd = oldChallengePwd;
            this.experimentPwd = oldExperimentPwd;

            //check challenge password
            if (!string.IsNullOrEmpty (challengePwdEntry.Text) || !string.IsNullOrEmpty (reinsChallengePwdEntry.Text)) {
                if (challengePwdEntry.Text.Length < 8) {
                    error += "Challenge password must be at least 8 characters long\n"; 
                } else if (string.IsNullOrEmpty (reinsChallengePwdEntry.Text)) {
                    error += "Please reinsert the challenge password\n";

                } else if (!challengePwdEntry.Text.Equals (reinsChallengePwdEntry.Text)) {
                    error += "Challenge passwords must be the same\n";

                } else {
                    isChallengePwdOk = true;
                    passwordChanged = true;
                    this.challengePwd = challengePwdEntry.Text;
                }
            } else {
                if (!challengePwdEntry.Text.Equals (oldChallengePwd)) {
                    this.challengePwd = challengePwdEntry.Text;
                    passwordChanged = true;
                }
            }

            //check experiment password
            if (!string.IsNullOrEmpty (experimentPwdEntry.Text) || !string.IsNullOrEmpty (this.reinsExperimentPwdEntry.Text)) {
                if (experimentPwdEntry.Text.Length < 8) {
                    error += "Experiment password must be at least 8 characters long\n"; 
                } else if (string.IsNullOrEmpty (reinsExperimentPwdEntry.Text)) {
                    this.error += "Please reinsert the experiment password\n";

                } else if (!experimentPwdEntry.Text.Equals (reinsExperimentPwdEntry.Text)) {
                    
                    this.error += "Experiment passwords must be the same\n";
                } else {
                    //pwds are the same
                    this.isExperimentPwdOk = true;
                    this.passwordChanged = true;
                    this.experimentPwd = experimentPwdEntry.Text;
                }
            } else {
                if (!experimentPwdEntry.Text.Equals (oldExperimentPwd)) {
                    this.experimentPwd = experimentPwdEntry.Text;
                    passwordChanged = true;
                }
            }

            if (this.challengePwdEntry.Text.Equals (this.experimentPwdEntry.Text) && !string.IsNullOrEmpty (this.challengePwdEntry.Text)) {
                this.error += "Challenge and Experiment password cannot be the same\n";
            }

            if (!string.IsNullOrEmpty (this.error)) {
                //in this there is an error so we show a Warning window to the user 
                //to let him know
                Dialog dialog = null;
                ResponseType response = ResponseType.None;

                try {
                    dialog = new Dialog (
                        "Warning",
                        this,
                        DialogFlags.DestroyWithParent | DialogFlags.Modal,
                        "Ok", ResponseType.Yes
                    );
                    dialog.VBox.Add (new Label ("\n\n  " + this.error + "  \n"));
                    dialog.ShowAll ();

                    response = (ResponseType)dialog.Run ();
                } finally {
                    if (dialog != null)
                        dialog.Destroy ();
                }
                return;

            } else if (string.IsNullOrEmpty (this.experimentPwdEntry.Text) && !string.IsNullOrEmpty (this.challengePwdEntry.Text)) {     
                //there is only a Challenge password so we warn the user he will not be able to modify the 
                //challenge experiment any more
                Dialog dialog = null;
                ResponseType response = ResponseType.None;

                try {
                    dialog = new Dialog (
                        "Warning",
                        this,
                        DialogFlags.DestroyWithParent | DialogFlags.Modal,
                        "Ok", ResponseType.Yes
                        , "Cancel", ResponseType.No
                    );
                    dialog.VBox.Add (new Label (WARNING_MESSAGE_ONLY_CHALLENGE_PASSWORD));
                    dialog.ShowAll ();

                    response = (ResponseType)dialog.Run ();
                } finally {
                    if (dialog != null)
                        dialog.Destroy ();
                }

                if (response == ResponseType.Yes)
                    this.Destroy ();
                else
                    return;
            } else {

                this.Destroy ();
            }
        }

        protected void OnButtonCancelClicked (object sender, EventArgs e)
        {
            this.passwordChanged = false;
            this.Destroy ();
        }
    }
}

