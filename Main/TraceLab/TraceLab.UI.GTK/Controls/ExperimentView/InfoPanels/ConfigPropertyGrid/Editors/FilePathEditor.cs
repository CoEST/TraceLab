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
using System.Linq;
using Gtk;
using Mono.Unix;
using TraceLabSDK.Component.Config;
using MonoDevelop.Components.PropertyGrid;

namespace TraceLab.UI.GTK.PropertyGridEditors
{
    /// <summary>
    /// FilePathEditor
    /// Note, that this is not original MonoDevelop editor.
    /// This editor is specific for TraceLab File Path purpose.
    /// </summary>
    [PropertyEditorType (typeof (TraceLabSDK.Component.Config.FilePath))]
    public class FilePathEditor : PropertyEditorCell
    {
        public override bool DialogueEdit {
            get { return true; }
        }
        
        public override void LaunchDialogue ()
        {
            string newFilenameLocation = SelectFileDialog(null);
            if(newFilenameLocation != null) 
            {
                FilePath path = Property.GetValue(Instance) as FilePath;
                if(path != null) 
                {
                    path.Absolute = newFilenameLocation;
                }
                else 
                {
                    //construct new filepath
                    path = new FilePath();
                    //string experimentPath = TODO - get experiment path
                    //string dataRoot = System.IO.Path.GetDirectoryName(experimentPath);
                    string dataRoot = null;

                    path.Init(newFilenameLocation, dataRoot);
                }

                Property.SetValue(Instance, path);
            }
        }

        protected override string GetValueText ()
        {
            FilePath filepath = Property.GetValue(Instance) as FilePath;
            if(filepath != null) 
            {
                return filepath.Absolute;
            }

            return null;
        }

        private string SelectFileDialog(Window parentWindow) 
        {
            var fileChooserDialog = new Gtk.FileChooserDialog(Mono.Unix.Catalog.GetString ("Select file..."), 
                                                              parentWindow,
                                                              FileChooserAction.Open, 
                                                              Gtk.Stock.Cancel, 
                                                              Gtk.ResponseType.Cancel,
                                                              Gtk.Stock.Open, Gtk.ResponseType.Ok);
            
            fileChooserDialog.AlternativeButtonOrder = new int[] { (int)ResponseType.Ok, (int)ResponseType.Cancel };
            fileChooserDialog.SelectMultiple = false;

            //add filter for all files
            FileFilter allFilesFilter = new FileFilter();
            allFilesFilter.Name = Mono.Unix.Catalog.GetString("All files");
            allFilesFilter.AddPattern("*.*");
            fileChooserDialog.AddFilter(allFilesFilter);

            int response = fileChooserDialog.Run();
            
            string filename = null;
            if(response == (int)Gtk.ResponseType.Ok) 
            {
                filename = fileChooserDialog.Filename;
            }
            
            fileChooserDialog.Destroy();
            
            return filename;
        }
    }
}