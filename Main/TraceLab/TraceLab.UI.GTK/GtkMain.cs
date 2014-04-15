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
using Gtk;

namespace TraceLab.UI.GTK
{
    class GtkMain
    {
        public static void Run(TraceLab.Core.ViewModels.ApplicationViewModel applicationViewModel) 
        {
            GLib.ExceptionManager.UnhandledException += new GLib.UnhandledExceptionHandler(ExceptionManager_UnhandledException);

            Application.Init();
            ApplicationContext app = new ApplicationContext(applicationViewModel);
            app.InitializeWindow();
            Application.Run();
        }

        public static void ShowErrorDialog(string message, string messageType)
        {
            Gtk.Application.Init();

            Gtk.MessageType gtkMessageType;

            if(!Enum.TryParse<Gtk.MessageType>(messageType, out gtkMessageType))
            {
                gtkMessageType = Gtk.MessageType.Error;
            }

            Gtk.MessageDialog dialog = new Gtk.MessageDialog(null, Gtk.DialogFlags.Modal, gtkMessageType,
                                                             Gtk.ButtonsType.Ok, message);
            dialog.Run();
        }

        private static void ExceptionManager_UnhandledException(GLib.UnhandledExceptionArgs args)
        {
            ErrorDialog errorDialog = new ErrorDialog(null);
            
            Exception ex = (Exception)args.ExceptionObject;
            
            try 
            {
                errorDialog.Message = string.Format("{0}:\n{1}", "Unhandled exception", ex.Message);
                errorDialog.AddDetails(ex.ToString (), false);
                errorDialog.Run();
            }
            finally 
            {
                errorDialog.Destroy();
                TraceLab.Core.ViewModels.LogViewModel.DestroyLogTargets();
                Application.Quit();
            }
        }
    }
}
