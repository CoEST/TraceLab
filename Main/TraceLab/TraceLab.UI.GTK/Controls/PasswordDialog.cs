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
using TraceLab.Core.Experiments;
using System.Collections.Generic;
using System.IO;

namespace TraceLab.UI.GTK
{
    static class PasswordDialog
    {
        internal static string TakePwd (Window parentWindow, string challengePwd, string experimentPwd)
        {              
            var passwordPickerDialog = new InsertPassword ();       
            string passwordFromUser = null;
            if (passwordPickerDialog.Run () == (int)Gtk.ResponseType.Ok) {
                //check pwds here
                passwordFromUser = passwordPickerDialog.userEnteredPassword;
                if (string.IsNullOrEmpty (passwordFromUser)) {
                    //show alert dialog
                    Dialog dialog = new Dialog (
                        "Warning",
                        parentWindow,
                        DialogFlags.Modal,
                        "Close", ResponseType.Ok
                        );

                    dialog.VBox.Add (new Label ("Please insert a valid password")); 
                    dialog.ShowAll ();
                    dialog.Run ();
                    dialog.Destroy ();

                    return null;

                } else if (!checkPasswords (passwordFromUser, challengePwd, experimentPwd)) {
                    //show alert
                    Dialog dialog = new Dialog (
                        "Warning",
                        parentWindow,
                        DialogFlags.Modal,
                        "Close", ResponseType.Ok
                        );

                    dialog.VBox.Add (new Label ("Entered password is not valid")); 
                    dialog.ShowAll ();
                    dialog.Run ();
                    dialog.Destroy ();

                    return null;
                }

                return passwordFromUser;
            } 

            passwordPickerDialog.Destroy ();
            return null;  
        }

         static bool checkPasswords(string password, string challengePwd, string expPwd){
            if (!string.IsNullOrEmpty (challengePwd)) {
                if (password.Equals (challengePwd))
                    return true;
            }

            if (!string.IsNullOrEmpty (expPwd)) {
                if (password.Equals (expPwd))
                    return true;
            }
           
            return false;
        } 
    }
}

