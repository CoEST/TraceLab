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
using TraceLabSDK.Component.Config;
using Gtk;
using MonoDevelop.Components.PropertyGrid;

namespace TraceLab.UI.GTK.PropertyGridEditors
{
    [PropertyEditorType (typeof (TraceLabSDK.Component.Config.DirectoryPath))]
    public class DirectoryPathEditor : PropertyEditorCell
    {
        public override bool DialogueEdit {
            get { return true; }
        }

        // HERZUM SPRINT 4.3: TLAB-238 TLAB-243
        public override void LaunchDialogue ()
        {
            string newDirectoryLocation = SelectFileDialog(null);
            SetDirectory (newDirectoryLocation);
        }
        // END HERZUM SPRINT 4.3: TLAB-238 TLAB-243

        // HERZUM SPRINT 4.2: TLAB-202
        public override void SetDirectory (String newDirectoryLocation)
        {
            if(newDirectoryLocation != null) 
            {
                DirectoryPath path = Property.GetValue(Instance) as DirectoryPath;
                if(path != null) 
                {
                    path.Absolute = newDirectoryLocation;
                }
                else   
                {
                    //construct new filepath
                    path = new DirectoryPath();
                    //string experimentPath = TODO - get experiment path
                    //string dataRoot = System.IO.Path.GetDirectoryName(experimentPath);
                    string dataRoot = null;
                  
                    // HERZUM SPRINT 5.0: TLAB-238 TLAB-243
                    dataRoot = DataRoot;
                    // END HERZUM SPRINT 5.0: TLAB-238 TLAB-243

                    path.Init(newDirectoryLocation, dataRoot);
                }

                Property.SetValue(Instance, path);
            }
        }
        // END HERZUM SPRINT 4.2: TLAB-202

        protected override string GetValueText ()
        {
            DirectoryPath filepath = Property.GetValue(Instance) as DirectoryPath;
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
                                                              FileChooserAction.SelectFolder, 
                                                              Gtk.Stock.Cancel, 
                                                              Gtk.ResponseType.Cancel,
                                                              Gtk.Stock.Open, Gtk.ResponseType.Ok);
            
            fileChooserDialog.AlternativeButtonOrder = new int[] { (int)ResponseType.Ok, (int)ResponseType.Cancel };
            fileChooserDialog.SelectMultiple = false;

            int response = fileChooserDialog.Run();
            
            string directory = null;
            if(response == (int)Gtk.ResponseType.Ok) 
            {
                directory = fileChooserDialog.Filename;
            }
            
            fileChooserDialog.Destroy();
            
            return directory;
        }
    }
}

