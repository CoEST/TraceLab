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

// HERZUM SPRINT 3.0: TLAB-82 CLASS

using System;
using TraceLab.Core.PackageBuilder;
using Gtk;

namespace TraceLab.UI.GTK
{
    public class ChallengePackageBuilder
    {
        private PackageBuilderViewModel m_viewModel;

        public ChallengePackageBuilder(PackageBuilderViewModel viewModel)
        {
            m_viewModel = viewModel;
        }

        public void Build(String filePath)
        {
            m_viewModel.GeneratePackageContent(); //it sets the root package source info
            m_viewModel.CurrentState = PackageBuilderWizardPage.FileViewer;

            PackageSourceInfo info = m_viewModel.PackageSourceInfo;
            bool noError = true;
            TraceLab.Core.PackageSystem.Package pkg = null;

            // string pkgFilePath = GetFilePath(info.Name);
            string pkgFilePath = filePath;
            // HERZUM SPRINT 3: TLAB-206 
            string pkgFileName = System.IO.Path.GetFileNameWithoutExtension(pkgFilePath);
            // END HERZUM SPRINT 3: TLAB-206
            if (pkgFilePath != null)
            {
               
                // string pkgFileName = System.IO.Path.GetFileNameWithoutExtension(pkgFilePath);
                string pkgTempDirectory = pkgFilePath + "~temp";

                try
                {
                    System.IO.Directory.CreateDirectory(pkgTempDirectory);

                    try
                    {
                        // HERZUM SPRINT 3: TLAB-206 
                        // pkg = new TraceLab.Core.PackageSystem.Package(info.Name, pkgTempDirectory, false);
                        pkg = new TraceLab.Core.PackageSystem.Package(pkgFileName, pkgTempDirectory, false);
                        // END HERZUM SPRINT 3: TLAB-206
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
                // HERZUM SPRINT 3: TLAB-206
                // ShowMessageDialog("Package \"" + info.Name + "\" was built successfully.", "Package Created",
                ShowMessageDialog("Package \"" + pkgFileName + "\" was built successfully.", "Package Created",
                // END HERZUM SPRINT 3: TLAB-206 
                                  Gtk.ButtonsType.Ok, Gtk.MessageType.Info);
            }

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
    }

}

