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
using System.Reflection;
using TraceLabSDK;
using System.Text;
using System.Collections.Generic;
using TraceLab.Core.Exceptions;

namespace TraceLab.Core.Workspaces
{
    public static class WorkspaceViewerLoader
    {
        /// <summary>
        /// Loads the viewer.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <param name="windowTitle">Window title.</param>
        /// <param name="error">Error.</param>
        /// <returns>
        ///   <c>true</c>, if viewer was loaded, <c>false</c> otherwise.
        /// </returns>
        public static bool LoadViewer(object data, string windowTitle, out string error)
        {
            return LoadViewer(data, windowTitle, WorkspaceUIAssemblyExtensions.Extensions, 
                                                 new DisplayEditor[] { DisplayWindowsFormEditor }, 
                                                 out error);
        }

        /// <summary>
        /// Loads the viewer.
        /// Method searches for assemblies constructed with name of data type assembly + extension.
        /// For example, if provided extensions are '.UI.WPF.dll' and '.UI.dll'
        /// and data is of type TLArtifactsCollection from assembly TraceLabSDK.Types.dll,
        /// method will search for TraceLabSDK.Types.WPF.UI.dll and TraceLabSDK.Types.UI.dll in that order
        /// for type TLArtifactsCollectionEditor (namespace of both must be the same)
        /// 
        /// The provided display functions will try to display windows based on their types.
        /// Typically, one will check for Windows Form and the other for Gui specific editor.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="windowTitle">The window title.</param>
        /// <param name="assemblyExtensions">The list of assembly extensions to check; </param>
        /// <param name="displayUnitEditors">The display unit editors.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public static bool LoadViewer(object data, string windowTitle, string[] assemblyExtensions, DisplayEditor[] displayUnitEditors, out string error)
        {
            bool success = false;
            Type editorType;
            Type dataType = data.GetType();
            error = String.Empty;

            try
            {
                if(GetEditorType(dataType, assemblyExtensions, out editorType)) 
                {
                    ConstructorInfo constructor = editorType.GetConstructor (Type.EmptyTypes);
                    if (constructor != null) 
                    {
                        IWorkspaceUnitEditor createdEditor = (IWorkspaceUnitEditor)constructor.Invoke (null); 
                        createdEditor.Data = data;
                        
                        //try all display functions
                        foreach (DisplayEditor displayFunc in displayUnitEditors)
                        {
                            success = displayFunc(createdEditor, windowTitle);
                            if (success == true)
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    success = false;
                    error = "Editor has not been found for type: " + dataType.FullName;
                }
            }
            catch(Exception ex)
            {
                //format errors if loading was not successful
                StringBuilder builder = new StringBuilder();
                
                builder.AppendLine("Unable to create editor for type: " + dataType.FullName);
                builder.AppendLine(ex.Message);
                
                error = builder.ToString();
            }

            return success;
        }

        /// <summary>
        /// Checks if editor exists.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <returns></returns>
        public static bool CheckIfEditorExists(Type dataType) 
        {
            return CheckIfEditorExists(dataType, WorkspaceUIAssemblyExtensions.Extensions);
        }

        /// <summary>
        /// Checks if editor exists.
        /// Method searches for assemblies constructed with name of data type assembly + extensions.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="assemblyExtensions">The assembly extensions.</param>
        /// <returns></returns>
        public static bool CheckIfEditorExists(Type dataType, string[] assemblyExtensions)
        {
            Type editorType;
            return GetEditorType(dataType, assemblyExtensions, out editorType);
        }

        public static bool GetEditorType(Type dataType, string[] assemblyExtensions, out Type editorType) 
        {
            editorType = null; 
            bool found = editorLookup.TryGetValue(dataType, out editorType);

            if(!found) 
            {
                found = FindEditor(dataType, assemblyExtensions, out editorType);
                editorLookup.Add(dataType, editorType);
            }

            bool success = (editorType != null);

            return success;
        }

        private static bool FindEditor(Type dataType, string[] assemblyExtensions, out Type editorType)
        {
            string editorTypeName = dataType.FullName + EDITOR_POSTFIX;
            string assemblyLocation = dataType.Assembly.Location;
            string dir = System.IO.Path.GetDirectoryName(assemblyLocation);
            string name = System.IO.Path.GetFileNameWithoutExtension(assemblyLocation);
            
            bool success = false;
            editorType = null;

            if(dir != null && name != null)
            {
                foreach (string assemblyExtension in assemblyExtensions)
                {
                    string editorAssemblyName = name + assemblyExtension;
                    string editorPath = System.IO.Path.Combine(dir, editorAssemblyName);

                    if (System.IO.File.Exists(editorPath))
                    {
                        Assembly editorAssembly = System.Reflection.Assembly.LoadFrom(editorPath);
                        Type editor = editorAssembly.GetType(editorTypeName);

                        // If the type implements IWorkspaceUnitEditor, then we want to try to instantiate it.
                        if (editor != null && editor.GetInterface("IWorkspaceUnitEditor") != null)
                        {
                            success = true;
                            editorType = editor;
                            break;
                        }
                    }
                }
            }

            return success;
        }

        /// <summary>
        /// Displays the windows form editor - the default fallback, if GUI specific editor is not provided
        /// </summary>
        /// <returns><c>true</c>, if windows form editor was displayed, <c>false</c> otherwise.</returns>
        /// <param name="editor">Editor.</param>
        /// <param name="windowTitle">Title of the displayed window.</param>
        public static bool DisplayWindowsFormEditor(IWorkspaceUnitEditor editor, String windowTitle) 
        {
            System.Windows.Forms.Form winFormWindow = editor as System.Windows.Forms.Form;
            if (winFormWindow != null) 
            {
                winFormWindow.Text = windowTitle;

                if (RuntimeInfo.OperatingSystem == RuntimeInfo.OS.Mac
                    || RuntimeInfo.OperatingSystem == RuntimeInfo.OS.Linux)
                {
                    //Application.Run begins running a standard application message loop on the current thread,
                    //and makes the specified form visible.
                    //It seems to be taking over the GTK+ message loop, thus the main GTK Window is not responding until 
                    //the form is closed. 
                    System.Windows.Forms.Application.Run(winFormWindow);
                    //winFormWindow.Close();
                    //winFormWindow.Dispose();
                    winFormWindow.Hide();
                }
                else
                {
                    winFormWindow.Show();
                }
                return true;
            }
            return false;
        }

        private const string EDITOR_POSTFIX = "Editor";

        private static Dictionary<Type, Type> editorLookup = new Dictionary<Type, Type>();

        public delegate bool DisplayEditor(IWorkspaceUnitEditor editor, String windowTitle);
    }
}

