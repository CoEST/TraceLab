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

using System.Windows;
using TraceLab.UI.WPF.ViewModels;
using TraceLab.UI.WPF.Views;
using TraceLab.Core.ViewModels;
using System;

namespace TraceLab.UI.WPF
{
    internal sealed class WpfMain
    {
        public static void Run(TraceLab.Core.ViewModels.ApplicationViewModel context)
        {
            var wrapper = new ApplicationViewModelWrapper(context);
            var app = new Application();
            Window wind = new MainWindow(wrapper);

            app.Run(wind);
        }

        public static void OpenNewExperiment(TraceLab.Core.ViewModels.ApplicationViewModel context, string experimentFilepath)
        {
            //find any MainWindow
            Application.Current.Dispatcher.Invoke(new Action(delegate()
            {
                //first search through opened MainWindows to see if any window already have the given experimentFilename opened
                bool found = TryActivateExistingWindow(experimentFilepath);

                if (found == false)
                {
                    OpenExperimentInNewWindow(experimentFilepath);
                }
            }));
        }

        /// <summary>
        /// Opens the experiment in new window.
        /// </summary>
        /// <param name="experimentFilepath">The experiment filepath.</param>
        private static void OpenExperimentInNewWindow(string experimentFilepath)
        {
            foreach (Window wind in Application.Current.Windows)
            {
                MainWindow mainWindow = wind as MainWindow;
                if (mainWindow != null)
                {
                    var applicationViewModelWrapper = (ApplicationViewModelWrapper)mainWindow.DataContext;
                    applicationViewModelWrapper.OpenExperiment(experimentFilepath);
                    break;
                }
            }
        }

        /// <summary>
        /// Tries the find existing window that has the given experiment opened.
        /// If found, focus that window
        /// </summary>
        /// <param name="experimentFilepath">The experiment filepath.</param>
        /// <returns></returns>
        private static bool TryActivateExistingWindow(string experimentFilepath)
        {
            bool found = false;
            foreach (Window window in Application.Current.Windows)
            {
                MainWindow mainWindow = window as MainWindow;
                if (mainWindow != null)
                {
                    var applicationViewModelWrapper = (ApplicationViewModelWrapper)mainWindow.DataContext;

                    //check if window has experiment with same filepath opened (in windows case does not matter - since we are already in WPF just ignore the case)
                    if (applicationViewModelWrapper.ExperimentViewModel != null &&
                        applicationViewModelWrapper.ExperimentViewModel.TopLevel.ExperimentInfo.FilePath.Equals(experimentFilepath, StringComparison.OrdinalIgnoreCase))
                    {
                        //if so activate the window (it assures that window is brought to foreground even if it is minimized)
                        if (!mainWindow.IsVisible)
                        {
                            mainWindow.Show();
                        }

                        if (mainWindow.WindowState == WindowState.Minimized)
                        {
                            mainWindow.WindowState = WindowState.Normal;
                        }

                        mainWindow.Activate();
                        mainWindow.Topmost = true;  
                        mainWindow.Topmost = false; 
                        mainWindow.Focus();         

                        found = true;
                        break;
                    }
                }
            }
            return found;
        }
    }
}