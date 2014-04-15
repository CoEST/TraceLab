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


        private static void AddFilters(FileChooserDialog dialog) 
        {
            // Add experiment files filter
            FileFilter fileFilter = new FileFilter();
            fileFilter.AddPattern("*.teml");
            fileFilter.Name = Mono.Unix.Catalog.GetString("Experiment files (.teml)");
            dialog.AddFilter(fileFilter);
            
            //add another option of All files
            FileFilter allFilesFilter = new FileFilter();
            allFilesFilter.Name = Mono.Unix.Catalog.GetString("All files");
            allFilesFilter.AddPattern("*.*");
            dialog.AddFilter(allFilesFilter);
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


        internal static bool SelectCatalogDialog (Window parentWindow, out string selectedDirectory)
        {

            var fileChooserDialog = new Gtk.FileChooserDialog (Mono.Unix.Catalog.GetString ("Select Directory..."), 
                                                              parentWindow,
                                                              FileChooserAction.SelectFolder, 
                                                              Gtk.Stock.Cancel, 
                                                              Gtk.ResponseType.Cancel,
                                                              Gtk.Stock.Open, Gtk.ResponseType.Ok);
            
            fileChooserDialog.AlternativeButtonOrder = new int[] { (int)ResponseType.Ok, (int)ResponseType.Cancel };
            fileChooserDialog.SelectMultiple = false;

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
    }
}

