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
using TraceLab.Core.Settings;
using System.Collections.Generic;


namespace TraceLab.UI.GTK
{
    public partial class SettingsDialog : Gtk.Dialog
    {
        private Gtk.ListStore componentDirectoryModel;
        private Gtk.ListStore typeDirectoryModel;

        /**
         * will store the temporary component directories, for reatachment with the main settings list
         */
        private List<SettingsPath> temporaryComponentDirectories= new List<SettingsPath>();
        /**
         * will store the temporary types directories, for reatachment with the main settings list
         */
        private List<SettingsPath> temporaryTypesDirectories = new List<SettingsPath>();

        private TraceLab.Core.Settings.Settings settingsRef = TraceLab.Core.Settings.Settings.GetSettings();

        private void init ()
        {
            //---- Create the model and mappings for Component directories:
            trv_componentDirectories.AppendColumn("User defined Component directories", new Gtk.CellRendererText (),"text",0);
            // Create the model 
            componentDirectoryModel = new Gtk.ListStore (typeof (string));
            // Assign the model to the TreeView
            trv_componentDirectories.Model = componentDirectoryModel;


            //------ Create the model and mappings for the Type Directories:
            trv_typeDirectories.AppendColumn("User defined Type directories", new Gtk.CellRendererText(), "text",0);
            typeDirectoryModel = new Gtk.ListStore(typeof(string));
            trv_typeDirectories.Model = typeDirectoryModel;

            //typeDirectoryModel
        }


        public SettingsDialog ()
        {
            this.Build ();
            this.init ();

            this.ShowAll ();

            //fill in the components paths
            String bulletPt="<b>></b> ";

            String temporaryPaths="";
            foreach (SettingsPath path in settingsRef.ComponentPaths) 
            {
                if(path.IsTemporary)
                {
                    temporaryPaths +=bulletPt+path.Path+"\n";
                    temporaryComponentDirectories.Add(path);
                }
                else
                {
                     addComponentDirectoryToTree (path.Path);
                }
            }
            temporaryPaths.TrimEnd('\n');
            lbl_temporaryComponentDirectories.Markup =temporaryPaths;


            //fill in the types paths view
            temporaryPaths ="";
            foreach (SettingsPath path in settingsRef.TypePaths) 
            {
                if(path.IsTemporary)
                {
                    temporaryPaths +=bulletPt +path.Path+"\n";
                    temporaryTypesDirectories.Add(path);
                }
                else
                {
                    addTypeDirectoryToTree(path.Path);
                }
            }
            lbl_temporaryTypesDirectories.Markup = temporaryPaths;

            //set the default experiment:
            tbx_defaultExperimentPath.Text = settingsRef.DefaultExperiment;
            // set the experiments directory:
            tbx_defaultExperimentDirectory.Text= settingsRef.DefaultExperimentsDirectory;
        }

        private void addComponentDirectoryToTree(string dir)
        {
            componentDirectoryModel.AppendValues(dir);
        }


        private void addTypeDirectoryToTree (string dir)
        {
            typeDirectoryModel.AppendValues(dir);
        }


        protected void componentDirectoryDeleteClickedHandler (object sender, EventArgs e)
        {
            removeSelectedElementInTreeview(trv_componentDirectories, componentDirectoryModel);
        }



        protected void addComponentDirectoryClickedHandler(object sender, EventArgs e)
        {
            string directoryPath;

            if (FileDialogs.SelectCatalogDialog (this, out directoryPath)) 
            {
                addComponentDirectoryToTree(directoryPath);
            }
        }



        protected void componentDirectoryMoveUpClickedHandler (object sender, EventArgs e)
        {
            moveUpSelectedElementInTreeview(trv_componentDirectories, componentDirectoryModel);
        }

        protected void componentDirectoryMoveDownClickedHandler(object sender, EventArgs e)
        {
            moveDownSelectedElementInTreeview(trv_componentDirectories, componentDirectoryModel);
        }

     

        protected void typesDirectoryMoveUpClickedHandler (object sender, EventArgs e)
        {
            moveUpSelectedElementInTreeview(trv_typeDirectories, typeDirectoryModel);
        }

        protected void typesDirectoryMoveDownClickedHandler (object sender, EventArgs e)
        {
            moveDownSelectedElementInTreeview(trv_typeDirectories, typeDirectoryModel);
        }

        protected void addTypesDirectoryClickedHandler (object sender, EventArgs e)
        {
            string directoryPath;

            if (FileDialogs.SelectCatalogDialog (this, out directoryPath))
            {
                addTypeDirectoryToTree(directoryPath);
            }
        }



        protected void typesDirectoryDeleteClickedHandler (object sender, EventArgs e)
        {
            removeSelectedElementInTreeview(trv_typeDirectories, typeDirectoryModel);
        }

        protected void defaultExperimentSelectClickedHandler (object sender, EventArgs e)
        {
            string directoryPath;

            if (FileDialogs.SelectFileDialog (this, out directoryPath)) 
            {
                tbx_defaultExperimentPath.Text= directoryPath;
            }
        }

        protected void defaultExperimentDirectorySelectFolderClickedHandler (object sender, EventArgs e)
        {
            string directoryPath;

            if (FileDialogs.SelectCatalogDialog (this, out directoryPath)) 
            {
                tbx_defaultExperimentDirectory.Text= directoryPath;
            }
        }

        /*
         * Returns the value of the removed element, or null- if no element removed
         */
        private String removeSelectedElementInTreeview(Gtk.TreeView target, Gtk.ListStore model)
        {
            Gtk.TreeIter iter; 
            Gtk.TreePath[] selectedRows=  target.Selection.GetSelectedRows();

            if(selectedRows.Length >0 )
            {
                model.GetIter(out iter, selectedRows[0]);
                string value = (string)model.GetValue(iter, 0);

                if(model.Remove(ref iter))
                    return value;
                else
                    return null;
            }
            return null;
        }

        
        /**
         * returnes true if changed and false if not
         * */
        private bool moveUpSelectedElementInTreeview (Gtk.TreeView target, Gtk.ListStore model)
        {
            Gtk.TreeIter selectedIter; 
            Gtk.TreeIter prevIter;

            Gtk.TreePath[] selectedRows=  target.Selection.GetSelectedRows();

            if(selectedRows.Length >0 && getPrevIterFromSelection(out selectedIter, out prevIter, selectedRows[0],model))
            {
                model.MoveBefore(selectedIter,prevIter);
                return true;
            }
            return false;
        }


        /**
         * returnes true if changed and false if not
         * */
        private bool moveDownSelectedElementInTreeview (Gtk.TreeView target, Gtk.ListStore model)
        {
            Gtk.TreeIter selectedIter; 
            Gtk.TreeIter nextIter;

            Gtk.TreePath[] selectedRows=  target.Selection.GetSelectedRows();

            if(selectedRows.Length >0 && getNextIterFromSelection(out selectedIter, out nextIter, selectedRows[0],model))
            {
              model.MoveAfter(selectedIter,nextIter);
              return true;
            }
            else
                return false;
        }

        private List<String> getEntryListForModel(Gtk.ListStore model)
        {
            List<String> outList = new List<string>();

            Gtk.TreeIter iter;
            model.GetIterFirst(out iter);

            if(iter.Equals(null))
                return null;

            do
            {
                outList.Add(model.GetValue (iter, 0).ToString());

            }while(model.IterNext(ref iter));

            return outList;
        }

        /**
         * Gets both iters from the provided ListStore obj based on the passed TreePath object(the selected row)
         */

        private bool getNextIterFromSelection(out Gtk.TreeIter selectedIter, out Gtk.TreeIter nextIter, 
                                                Gtk.TreePath selectedRow,  Gtk.ListStore listModel)
        {
            
            listModel.GetIter(out selectedIter, selectedRow);
            selectedRow.Next();
            listModel.GetIter(out nextIter, selectedRow);

            return (nextIter.Stamp!=0  && !nextIter.Equals(selectedIter));
        }


        /**
         * Gets both iters from the provided ListStore obj based on the passed TreePath object(the selected row)
         */
        private bool getPrevIterFromSelection(out Gtk.TreeIter selectedIter, out Gtk.TreeIter prevIter, 
                                                Gtk.TreePath selectedRow,  Gtk.ListStore listModel)
        {
            
            listModel.GetIter(out selectedIter, selectedRow);
            selectedRow.Prev();
            listModel.GetIter(out prevIter, selectedRow);

            return  (prevIter.Stamp!=0 && !prevIter.Equals(selectedIter));
        }


        protected void saveChangesClickedHandler (object sender, EventArgs ev)
        {
            //this could use some validation, but really- someone would have to be very silly to screw this up!

            settingsRef.DefaultExperiment = tbx_defaultExperimentPath.Text;
            settingsRef.DefaultExperimentsDirectory = tbx_defaultExperimentDirectory.Text;

            // get and store the components directories:
            settingsRef.ComponentPaths.Clear();
            foreach(SettingsPath sp in temporaryComponentDirectories)
                settingsRef.ComponentPaths.Add(sp);

            Gtk.TreeIter iter;
            componentDirectoryModel.GetIterFirst(out iter);
            if(iter.Stamp!=0)
            do
            {
                SettingsPath sp = new SettingsPath(false,componentDirectoryModel.GetValue (iter, 0).ToString());
                settingsRef.ComponentPaths.Add(sp);
            }while(componentDirectoryModel.IterNext(ref iter));

            // get and store the type directories
            settingsRef.TypePaths.Clear();
            foreach(SettingsPath sp in temporaryTypesDirectories)
                settingsRef.TypePaths.Add(sp);

            typeDirectoryModel.GetIterFirst(out iter);
            if(iter.Stamp!=0)
            do
            {
                SettingsPath sp = new SettingsPath(false,typeDirectoryModel.GetValue(iter,0).ToString());
                settingsRef.TypePaths.Add(sp);
            }while(typeDirectoryModel.IterNext(ref iter));

            TraceLab.Core.Settings.Settings.SaveSettings(this.settingsRef);

            this.Destroy();
        }


        protected void cancelClickedHandler(object sender, EventArgs ev)
        {
            this.Destroy();
        }
    }
}

