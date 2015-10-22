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
    static class FileDialogs
    {
        internal static string OpenExperimentDialog(Window parentWindow) 
        {
            var fileChooserDialog = new Gtk.FileChooserDialog(Mono.Unix.Catalog.GetString ("Open Experiment File"), 
                                                               parentWindow,
                                                               FileChooserAction.Open, 
                                                               Gtk.Stock.Cancel, 
                                                               Gtk.ResponseType.Cancel,
                                                               Gtk.Stock.Open, Gtk.ResponseType.Ok);

            fileChooserDialog.AlternativeButtonOrder = new int[] { (int)ResponseType.Ok, (int)ResponseType.Cancel };
            fileChooserDialog.SelectMultiple = false;

            // HERZUM SPRINT 4 TLAB-214
            fileChooserDialog.SetCurrentFolder(TraceLab.Core.Settings.Settings.GetSettings().DefaultExperimentsDirectory);
            // END SPRINT HERZUM 4 HERZUM 4: TLAB-214

            AddFilters(fileChooserDialog);

            int response = fileChooserDialog.Run();

            string filename = null;
            if(response == (int)Gtk.ResponseType.Ok) 
            {
                filename = fileChooserDialog.Filename;
            }
            
            fileChooserDialog.Destroy();

            return filename;
        }


        internal static bool NewExperimantDialog (Window parentWindow, ref Experiment experiment)
        {
            var newExperimentDialog = new NewExperimentDialog(ref experiment);
            newExperimentDialog.Run();
           
            return newExperimentDialog.Results;
        }


        /// <summary>
        /// Adds the filters. Returns default extension
        /// </summary>
        /// <returns>The filters.</returns>
        /// <param name="dialog">Dialog.</param>
        private static string AddFilters(FileChooserDialog dialog) 
        {
            string defaultExtension = "teml";
            string cryptedFileExtension = "temlx";
            // Add experiment files filter
            FileFilter fileFilter = new FileFilter();
            fileFilter.AddPattern(string.Format("*.{0}", defaultExtension));
            fileFilter.Name = Mono.Unix.Catalog.GetString(string.Format("Experiment files (.{0})", defaultExtension));
            dialog.AddFilter(fileFilter);

            // Add experiment files filter
            //TLAB-67
        //here: we check if the file is crypted, if it's so we ask the user to insert a password. then we decrypt the file and check the 
            /// password. if the pwd is the same we contninue with the standard process, otherwise we raise and error
            FileFilter fileFilterCryptedFile = new FileFilter();
            fileFilterCryptedFile.AddPattern(string.Format("*.{0}", cryptedFileExtension));
            fileFilterCryptedFile.Name = Mono.Unix.Catalog.GetString(string.Format("Experiment files (.{0})", cryptedFileExtension));
            dialog.AddFilter(fileFilterCryptedFile);
            
            //add another option of All files
            FileFilter allFilesFilter = new FileFilter();
            allFilesFilter.Name = Mono.Unix.Catalog.GetString("All files");
            allFilesFilter.AddPattern("*.*");
            dialog.AddFilter(allFilesFilter);

            return defaultExtension;
        }

        internal static string ShowSaveAsDialog(Window parentWindow, string currentFilename = null) 
        {
            var fileChooserDialog = new FileChooserDialog (Mono.Unix.Catalog.GetString ("Save Experiment File"),
                                             parentWindow,
                                             FileChooserAction.Save,
                                             Gtk.Stock.Cancel,
                                             Gtk.ResponseType.Cancel,
                                             Gtk.Stock.Save, Gtk.ResponseType.Ok);
            
            fileChooserDialog.DoOverwriteConfirmation = true;
            fileChooserDialog.AlternativeButtonOrder = new int[] { (int)ResponseType.Ok, (int)ResponseType.Cancel };
            fileChooserDialog.SelectMultiple = false;

            if (String.IsNullOrEmpty(currentFilename) == false)
            {
                fileChooserDialog.SetFilename(currentFilename);
            }

            string defaultExtension = AddFilters(fileChooserDialog);

            while (fileChooserDialog.Run() == (int)Gtk.ResponseType.Ok) 
            {
                string filename = fileChooserDialog.Filename;
                if (string.IsNullOrEmpty(Path.GetExtension(filename))) 
                {
                    // No extension; add one from the format descriptor.
                    filename = string.Format ("{0}.{1}", filename, defaultExtension);
                    fileChooserDialog.CurrentName = Path.GetFileName (filename);

                    // We also need to display an overwrite confirmation message manually,
                    // because MessageDialog won't do this for us in this case.
                    if (File.Exists (filename) && !ConfirmOverwrite (fileChooserDialog, filename))
                    {
                        continue;
                    }
                }

                fileChooserDialog.Destroy();
                return filename;
            }

            //if file was not selected return null
            fileChooserDialog.Destroy();
            return null;
        }

        internal static void ShowSaveErrorDialog(Window parentWindow, string message, string file)
        {
            NLog.LogManager.GetCurrentClassLogger().Error(String.Format("Failed to Save File {0}. {1}", file, message));
            Gtk.MessageDialog dlg = new Gtk.MessageDialog(parentWindow, 
                                                           Gtk.DialogFlags.Modal, 
                                                           Gtk.MessageType.Error, 
                                                           Gtk.ButtonsType.Ok, message);
            dlg.Title = "Failed to save experiment!";
            dlg.Run();
            dlg.Destroy();
        }

        // HERZUM SPRINT 4: TLAB-214
        internal static bool SelectCatalogDialog (Window parentWindow, out string selectedDirectory, string defaultDirectory)
        // END HERZUM SPRINT 4: TLAB-214
        {

            var fileChooserDialog = new Gtk.FileChooserDialog (Mono.Unix.Catalog.GetString ("Select Directory..."), 
                                                              parentWindow,
                                                              FileChooserAction.SelectFolder, 
                                                              Gtk.Stock.Cancel, 
                                                              Gtk.ResponseType.Cancel,
                                                              Gtk.Stock.Open, Gtk.ResponseType.Ok);
            
            fileChooserDialog.AlternativeButtonOrder = new int[] { (int)ResponseType.Ok, (int)ResponseType.Cancel };
            fileChooserDialog.SelectMultiple = false;

            // HERZUM SPRINT 4 TLAB-214
            if (defaultDirectory != null)
                fileChooserDialog.SetCurrentFolder(defaultDirectory);
            // END SPRINT HERZUM 4 HERZUM 4: TLAB-214

            int response = fileChooserDialog.Run ();

            if (response == (int)Gtk.ResponseType.Ok) 
            {
                selectedDirectory = fileChooserDialog.Filename;
                fileChooserDialog.Destroy();
                return true;
            } 
            else 
            {
                selectedDirectory = "";
                fileChooserDialog.Destroy();
                return false;
            }
        }

        internal static bool SelectFileDialog (Window parentWindow, out string selectedFile)
        {
            var fileChooserDialog = new Gtk.FileChooserDialog (Mono.Unix.Catalog.GetString ("Select File..."), 
                                                              parentWindow,
                                                              FileChooserAction.Open, 
                                                              Gtk.Stock.Cancel, 
                                                              Gtk.ResponseType.Cancel,
                                                              Gtk.Stock.Open, Gtk.ResponseType.Ok);

            AddFilters(fileChooserDialog);

            fileChooserDialog.AlternativeButtonOrder = new int[] { (int)ResponseType.Ok, (int)ResponseType.Cancel };
            fileChooserDialog.SelectMultiple = false;

            int response = fileChooserDialog.Run ();

            if (response == (int)Gtk.ResponseType.Ok)
            {
                selectedFile = fileChooserDialog.Filename;
                fileChooserDialog.Destroy();
                return true;
            } 
            else
            {
                selectedFile = "";
                fileChooserDialog.Destroy();
                return false;
            }
        }

        internal static string SelectPackageFileDialog(Window parentWindow)
        {
            var fileChooserDialog = new FileChooserDialog (Mono.Unix.Catalog.GetString ("Save Package As"),
                                                           parentWindow,
                                                           FileChooserAction.Save,
                                                           Gtk.Stock.Cancel,
                                                           Gtk.ResponseType.Cancel,
                                                           Gtk.Stock.Save, Gtk.ResponseType.Ok);

            fileChooserDialog.DoOverwriteConfirmation = true;
            fileChooserDialog.AlternativeButtonOrder = new int[] { (int)ResponseType.Ok, (int)ResponseType.Cancel };
            fileChooserDialog.SelectMultiple = false;

            // Add experiment files filter
            string defaultExtension = "tpkg";
            FileFilter fileFilter = new FileFilter();
            fileFilter.AddPattern(string.Format("*.{0}", defaultExtension));
            fileFilter.Name = Mono.Unix.Catalog.GetString(string.Format("TraceLab Package Files (.{0})", defaultExtension));
            fileChooserDialog.AddFilter(fileFilter);

            while (fileChooserDialog.Run() == (int)Gtk.ResponseType.Ok) 
            {
                string filename = fileChooserDialog.Filename;
                if (string.IsNullOrEmpty(Path.GetExtension(filename))) 
                {
                    // No extension; add one from the format descriptor.
                    filename = string.Format ("{0}.{1}", filename, defaultExtension);
                    fileChooserDialog.CurrentName = Path.GetFileName (filename);

                    // We also need to display an overwrite confirmation message manually,
                    // because MessageDialog won't do this for us in this case.
                    if (File.Exists (filename) && !ConfirmOverwrite (fileChooserDialog, filename))
                    {
                        continue;
                    }
                }

                fileChooserDialog.Destroy();
                return filename;
            }

            fileChooserDialog.Destroy();
            return null;
        }

        private static bool ConfirmOverwrite (FileChooserDialog fcd, string file)
        {
            string primary = Mono.Unix.Catalog.GetString ("A file named \"{0}\" already exists. Do you want to replace it?");
            string secondary = Mono.Unix.Catalog.GetString ("The file already exists in \"{1}\". Replacing it will overwrite its contents.");
            string message = string.Format ("<span weight=\"bold\">{0}</span>\n\n{1}", primary, secondary);

            MessageDialog md = new MessageDialog (fcd, DialogFlags.Modal | DialogFlags.DestroyWithParent,
                                                  MessageType.Question, ButtonsType.None,
                                                  true, message, System.IO.Path.GetFileName (file), fcd.CurrentFolder);

            md.AddButton (Stock.Cancel, ResponseType.Cancel);
            md.AddButton (Stock.Save, ResponseType.Ok);
            md.DefaultResponse = ResponseType.Cancel;
            md.AlternativeButtonOrder = new int[] { (int)ResponseType.Ok, (int)ResponseType.Cancel };

            int response = md.Run ();
            md.Destroy ();

            return response == (int)ResponseType.Ok;
        }
    }
}

