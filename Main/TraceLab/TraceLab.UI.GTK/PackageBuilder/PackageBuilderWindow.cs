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
using TraceLab.Core.PackageBuilder;

namespace TraceLab.UI.GTK
{
    public partial class PackageBuilderWindow : Gtk.Window
    {
        public PackageBuilderWindow () : 
                base(Gtk.WindowType.Toplevel)
        {
            this.Build ();
        }

        public PackageBuilderWindow(PackageBuilderViewModel viewModel) : this()
        {
            m_viewModel = viewModel;

            DisplayCurrentPage();
        }

        private void DisplayCurrentPage() 
        {
            if(m_viewModel.CurrentState == PackageBuilderWizardPage.Config)
            {
                m_settingsPage = new PackageBuilderSettingsPage(m_viewModel);
                this.mainVBox.Add(m_settingsPage);
                this.mainVBox.ShowAll();
                this.generateButton.Visible = true;
            } 
            else
            {
                if(m_settingsPage != null) 
                {
                    this.mainVBox.Remove(m_settingsPage);
                    this.generateButton.Visible = false;
                }
                m_mainPage = new PackageBuilderMainPage(m_viewModel);
                this.mainVBox.Add(m_mainPage);
                this.mainVBox.ShowAll();
                this.buildPackageButton.Visible = true;
            }
        }

        private PackageBuilderViewModel m_viewModel;
        private Gtk.Widget m_settingsPage;
        private Gtk.Widget m_mainPage;

        protected void generateButtonClicked(object sender, EventArgs e)
        {
            // END HERZUM SPRINT 4.1: TLAB-215
            try {
            // END HERZUM SPRINT 4.1: TLAB-215

            m_viewModel.GeneratePackageContent(); //it sets the root package source info
            m_viewModel.CurrentState = PackageBuilderWizardPage.FileViewer;
            DisplayCurrentPage();
         // HERZUM SPRINT 4.1: TLAB-215
         }
         catch (Exception ex)
            {
                ShowMessageDialog( ex.Message,
                                  "Package Creation Error", Gtk.ButtonsType.Ok, Gtk.MessageType.Error);
                return;
            }
            // END HERZUM SPRINT 4.1: TLAB-215
        }

        /// <summary>
        /// Opens a file dialog for selecting the package file destination.
        /// </summary>
        /// <param name="pFileName">Name of the package file.</param>
        /// <returns>Package file path.</returns>
        private string GetFilePath(string pFileName)
        {
            return FileDialogs.SelectPackageFileDialog(this);
        }

        protected void buildButtonClicked(object sender, EventArgs e)
        {
            PackageSourceInfo info = m_viewModel.PackageSourceInfo;
            bool noError = true;
            TraceLab.Core.PackageSystem.Package pkg = null;

            string pkgFilePath = GetFilePath(info.Name);
            if (pkgFilePath != null)
            {
                string pkgFileName = System.IO.Path.GetFileNameWithoutExtension(pkgFilePath);
                string pkgTempDirectory = pkgFilePath + "~temp";

                try
                {
                    System.IO.Directory.CreateDirectory(pkgTempDirectory);

                    // HERZUM SPRINT 4.2: TLAB-215
                    if (info.Name.Contains(" "))
                    {
                        ShowMessageDialog("Package name:" +info.Name+ ", can not contain space characters: ",
                                          "Package Creation Error", Gtk.ButtonsType.Ok, Gtk.MessageType.Error);
                        noError = false;
                    }
                    // END HERZUM SPRINT 4.2: TLAB-215

                    try
                    {
                        pkg = new TraceLab.Core.PackageSystem.Package(info.Name, pkgTempDirectory, false);
                    }
                    catch (TraceLab.Core.PackageSystem.PackageAlreadyExistsException)
                    {
                        ShowMessageDialog("Package already exists in: " + pkgTempDirectory,
                                        "Package Creation Error", Gtk.ButtonsType.Ok, Gtk.MessageType.Error);
                        noError = false;
                    }
                    catch (TraceLab.Core.PackageSystem.PackageException ex)
                    {
                        ShowMessageDialog("Error creating package: " + ex.Message,
                                          "Package Creation Error", Gtk.ButtonsType.Ok, Gtk.MessageType.Error);
                        noError = false;
                    }

                    if (pkg != null && noError)
                    {
                        foreach (PackageFileSourceInfo item in info.Files)
                        {
                            try
                            {
                                noError = noError && PackageCreator.AddItemToPackage(pkg, item, m_viewModel.IsExperimentPackage);
                            }
                            catch (TraceLab.Core.Exceptions.PackageCreationFailureException ex)
                            {
                                ShowMessageDialog("Error creating package: " + ex.Message,
                                                  "Package Creation Error", Gtk.ButtonsType.Ok, Gtk.MessageType.Error);
                                noError = false;
                            }
                        }

                        pkg.SaveManifest();

                        using (System.IO.FileStream stream = new System.IO.FileStream(pkgFilePath, System.IO.FileMode.Create))
                        {
                            pkg.Pack(stream);
                        }
                    }
                }
                catch (System.IO.IOException error)
                {
                    ShowMessageDialog("Unable to create package. Error: " + error.Message,
                                      "Package Creation Error", Gtk.ButtonsType.Ok, Gtk.MessageType.Error);
                    noError = false;
                }
                catch (System.UnauthorizedAccessException error)
                {
                    ShowMessageDialog("Unable to create package - Unauthorized access: " + error.Message,
                                      "Package Creation Error", Gtk.ButtonsType.Ok, Gtk.MessageType.Error);
                    noError = false;
                }

                try
                {
                    if (System.IO.Directory.Exists(pkgTempDirectory))
                    {
                        System.IO.Directory.Delete(pkgTempDirectory, true);
                    }
                }
                catch (System.IO.IOException error)
                {
                    ShowMessageDialog("Unable to cleanup after package creation. Error: " + error.Message,
                                      "After Package Cleanup Failure", Gtk.ButtonsType.Ok, Gtk.MessageType.Warning);
                }
                catch (System.UnauthorizedAccessException error)
                {
                    ShowMessageDialog("Unable to cleanup after package creation. Unauthorized access: " + error.Message,
                                      "After Package Cleanup Failure", Gtk.ButtonsType.Ok, Gtk.MessageType.Warning);
                }
            }

            if (noError && pkg != null)
            {
                ShowMessageDialog("Package \"" + info.Name + "\" was built successfully.", "Package Created",
                                  Gtk.ButtonsType.Ok, Gtk.MessageType.Info);
            }
        }

        internal void ShowMessageDialog(string message, string title, Gtk.ButtonsType buttonsType, Gtk.MessageType messageType)
        {
            Gtk.MessageDialog dlg = new Gtk.MessageDialog(this, 
                                                          Gtk.DialogFlags.Modal, 
                                                          messageType, 
                                                          buttonsType, message);
            dlg.Title = title;
            dlg.Run();
            dlg.Destroy();
        }
    }
}

