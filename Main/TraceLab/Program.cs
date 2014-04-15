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

using Microsoft.Shell;
using System;
using System.Threading;
using TraceLab.Core.Utilities;

namespace TraceLab
{
    public class Program
    {
        /// <summary>
        /// Unique name defines unique identifier for single instance application.
        /// </summary>
        private const string Unique = "{3d31b46c-d18a-48b8-afab-bee69a27e4a9-TRACELAB}";

        [STAThread]
        public static void Main(string[] args)
        {
            bool isRunOnMono = TraceLabSDK.RuntimeInfo.IsRunInMono;
            bool startInstance;
            if (!isRunOnMono)
            {
                //in windows TraceLab runs as single instance application
                //preventing TraceLab being open as multiple processes
                startInstance = (SingleInstance.InitializeAsFirstInstance(Unique));
            }
            else
            {
                //in mono don't use single instance startup
                //single instance must be developed in another way than on windows
                startInstance = true;
            }

            if(startInstance) 
            {
                try
                {
                    try
                    {
                        Thread.CurrentThread.CurrentCulture = ThreadFactory.DefaultCulture;
                        var app = new TraceLabApplicationGUI();
                        app.Run(args);
                    }
                    catch (InvalidKeyException e)
                    {
                        if (!isRunOnMono)
                        {
                            System.Windows.Forms.MessageBox.Show(e.Message, @"Invalid key!",
                                                          System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        }
                        else
                        {
                            //attempt to display missing key warning in Gtk 
                            TraceLabApplicationGUI.DisplayGtkErrorDialog(e.Message, "Warning");
                        }
                    }
                    catch (Exception e)
                    {
                        if (!isRunOnMono)
                        {
                            System.Windows.Forms.MessageBox.Show(e.Message, @"An Exception occurred.",
                                                             System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        }
                        else
                        {
                            //attempt to display error in Gtk (this is absolutely CRITICAL error, Gtk Main catches it's own unhandled exceptions) 
                            TraceLabApplicationGUI.DisplayGtkErrorDialog(e.Message, "Error");
                        }
                    }
                }
                finally
                {
                    if (!isRunOnMono)
                    {
                        // Allow single instance code to perform cleanup operations
                        SingleInstance.Cleanup();
                    }
                }
            }
        }
    }
}
