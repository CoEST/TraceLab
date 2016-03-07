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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TraceLab
{
    /// <summary>
    /// TraceLabApplication with Graphical User Interface.
    /// The main interface is implemented with WPF. 
    /// It has a fallback to non-wpf interface, currently not implemented yet.
    /// </summary>
    class TraceLabApplicationGUI : TraceLabApplication
    {
        /// <summary>
        /// Runs the UI.
        /// The method detects whether TraceLab application runs in Mono runtime.
        /// If so, it will run Gtk Ui, otherwise Wpf Ui.
        /// </summary>
        protected override void RunUI()
        {
            bool shouldFallBack = true;
            if (!TraceLabSDK.RuntimeInfo.IsRunInMono)
            {
                // Fallback if we fail to run our WPF layer.
                shouldFallBack = !RunWpfUI(MainViewModel);
            }

            // If we're running Mono, or we are unable to load our WPF UI layer, fall back to the default one.
            if (shouldFallBack)
            {
                RunGtkUI(MainViewModel);
            }
        }
        
        #region Wpf

        /// <summary>
        /// Runs the WPF UI.
        /// </summary>
        /// <param name="mainViewModel">Main application view model.</param>
        /// <returns></returns>
        private bool RunWpfUI(object mainViewModel)
        {
            return InvokeMethodInWpfMain("Run", new object[] { mainViewModel });
        }

        /// <summary>
        /// Opens the new experiment
        /// </summary>
        /// <param name="vm">The vm.</param>
        /// <param name="experimentFilename">The experiment filename.</param>
        /// <returns></returns>
        private static bool OpenNewExperiment(object vm, string experimentFilename)
        {
            return InvokeMethodInWpfMain("OpenNewExperiment", new object[] { vm, experimentFilename });
        }

        /// <summary>
        /// Invokes the method in TraceLab.Ui.WPF assembly and WpfMain class
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="methodArguments">The method arguments.</param>
        /// <returns></returns>
        private static bool InvokeMethodInWpfMain(string methodName, object[] methodArguments)
        {
            // We load the WPF UI via reflection to get around the fact that MonoDevelop 
            // cannot compile anything the in the WPF namespace
            // if it happens to be targetting the Mono platform, so Wpf.Views will be our bastion of WPF
            string assembly = "TraceLab.UI.WPF, Version=0.1.0.0, Culture=neutral, PulicKeyToken=null";
            string mainClassName = "TraceLab.UI.WPF.WpfMain";
            return InvokeUIMainMethod(assembly, mainClassName, methodName, methodArguments);
        }

        #region Single Instance TraceLab

        /// <summary>
        /// Signals the external first application instance.
        /// It is being executed if TraceLab is already running, and user attempts to 
        /// open TraceLab again. See SingleInstance.InitializeAsFirstInstance
        /// </summary>
        /// <param name="args">The args.</param>
        public static void SignalExternalFirstApplicationInstance(IList<string> args)
        {
            // Detect if we're running in Mono - if not, then we can use WPF
            Type t = Type.GetType("Mono.Runtime");
            if (t == null)
            {
                CommandLineProcessor processor = new CommandLineProcessor();

                string experimentFilename = null;
                processor.Commands["o"] = delegate(string value) { experimentFilename = value; };
                processor.Commands["open"] = delegate(string value) { experimentFilename = value; };

                processor.Parse(args.ToArray());

                if (String.IsNullOrEmpty(experimentFilename) == false)
                {
                    OpenNewExperiment(MainViewModel, experimentFilename);
                }
            }
            else
            {
                System.Diagnostics.Trace.Write("Application already running! Currently sending the command args to currently running non-wpf application is not supported");
                throw new NotImplementedException("Application already running! Currently sending the command args to currently running non-wpf application is not supported");
            }
        }

        #endregion Single Instance TraceLab

        #endregion Wpf

        #region Gtk

        /// <summary>
        /// Runs the GTK UI.
        /// Method attempts to load TraceLab.UI.GTK and execute Run method in GtkMain class.
        /// </summary>
        /// <returns>
        /// <c>true</c>, if GTK UI was run, <c>false</c> otherwise.
        /// </returns>
        /// <param name="mainViewModel">main application view model</param>
        private bool RunGtkUI(object mainViewModel)
        {
            string methodName = "Run";
            object[] methodArguments = new object[] { mainViewModel };
            return InvokeUIMainMethod(GtkAssembly, GtkMainClassName, methodName, methodArguments);
        }

        /// <summary>
        /// Displays error in the gtk dialog using the call via reflection to TraceLabGTK.
        /// If it fails it prints out message to the console. 
        /// TraceLab.csproj project cannot reference gtksharp libraries, because then even Windows WPF verions
        /// would require installation of Mono, which we do not want. 
        /// We cannot also rely on displaying windows forms - because on Mac user would be required to install
        /// XQuartz only to see the message. 
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="messageType">Message type.</param>
        internal static void DisplayGtkErrorDialog(string message, string messageType)
        {
            string methodName = "ShowErrorDialog";
            object[] methodArguments = new object[] { message, messageType };
            bool success = InvokeUIMainMethod(GtkAssembly, GtkMainClassName, methodName, methodArguments);
            if(!success)
            {
                System.Console.WriteLine(message);
            }
        }

        private static string GtkAssembly = "TraceLab.UI.GTK";
        private static string GtkMainClassName = "TraceLab.UI.GTK.GtkMain";

        #endregion Gtk
        
        /// <summary>
        /// Invokes the specified method from specified assembly and class with provided
        /// method arguments. Assembly is dynamically loaded using reflection.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="mainClassName">Name of the main class.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="methodArguments">The method arguments.</param>
        /// <returns></returns>
        private static bool InvokeUIMainMethod(string assembly, string mainClassName, string methodName, object[] methodArguments)
        {
            bool success = false;
            System.Reflection.Assembly uiAssembly = null;

            try
            {
                uiAssembly = System.Reflection.Assembly.Load(assembly);
            }
            catch (System.Exception)
            {

            }

            if (uiAssembly != null)
            {
                // Assuming we successfully loaded our library, then attempt to get the type info for the Main class
                // and use it's static Run() to run the actual application.
                Type type = uiAssembly.GetType(mainClassName);
                if (type == null)
                {
                    System.Windows.Forms.MessageBox.Show("TraceLab UI assembly is invalid: Does not contain required application class.");
                }
                else
                {
                    System.Reflection.MethodInfo info = type.GetMethod(methodName);
                    if (info == null)
                    {
                        System.Windows.Forms.MessageBox.Show("TraceLab UI assembly is invalid: Does not contain required application class.");
                    }
                    else
                    {
                        info.Invoke(null, methodArguments);
                        success = true;
                    }
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Unable to load UI assembly.");
            }

            return success;
        }
    }
}
