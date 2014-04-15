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
using TraceLab.Core.Experiments;

namespace TraceLab.UI.WPF.ViewModels
{
    /// <summary>
    /// FileDialogs contains static methods that shows various open/save dialogs used in TraceLab
    /// </summary>
    static class FileDialogs
    {
        #region Open/Save Dialog

        internal static string GetExperimentFilename<T>(string dialogTitle) where T : Ookii.Dialogs.Wpf.VistaFileDialog, new()
        {
            return GetFilename<T>(dialogTitle, ".teml", "Experimental Graph Files|*.teml");
        }

        internal static string GetExperimentFilenameSaveAsDialog(string rootDrive, out TraceLab.Core.Experiments.ReferencedFiles referencedFilesProcessing)
        {
            Ookii.Dialogs.Wpf.TraceLabSaveAsFileDialog fileDialog = new Ookii.Dialogs.Wpf.TraceLabSaveAsFileDialog(rootDrive);
            fileDialog.Title = "Save as";
            fileDialog.CheckFileExists = false;
            fileDialog.AddExtension = true;
            fileDialog.DefaultExt = ".teml";
            fileDialog.Filter = "Experimental Graph Files|*.teml";

            string filename = null;
            bool? success = fileDialog.ShowDialog();
            if (success.HasValue && success.Value)
            {
                filename = fileDialog.FileName;
            }

            referencedFilesProcessing = TranslateReferencedFilesEnum(fileDialog.ReferencedFilesProcessing);
            return filename;
        }

        internal static string GetExperimentFilename<T>() where T : Ookii.Dialogs.Wpf.VistaFileDialog, new()
        {
            return GetFilename<T>("Open experiment", ".teml", "Experimental Graph Files|*.teml|All Files|*.*");
        }

        internal static string GetCompositeComponentFilename()
        {
            return GetFilename<Ookii.Dialogs.Wpf.VistaSaveFileDialog>("Save component as", ".tcml", "Tracelab Composite Components Files|*.tcml");
        }

        private static string GetFilename<T>(string title, string defaultExt, string filter) where T : Ookii.Dialogs.Wpf.VistaFileDialog, new()
        {
            T fileDialog = new T();
            fileDialog.Title = title;
            fileDialog.CheckFileExists = false;
            fileDialog.AddExtension = true;
            fileDialog.DefaultExt = defaultExt;
            fileDialog.Filter = filter;

            bool? success = fileDialog.ShowDialog();
            if (success.HasValue && success.Value)
            {
                return fileDialog.FileName;
            }

            return null;
        }

        /// <summary>
        /// Translates the referenced files enum from the type froo Oooki.Dialogs to the TraceLab.Core.ReferencedFiles.
        /// It is better this way rather having all projects referencing Oooki.Dialogs. 
        /// It may be a design flaw, but for now it's fine.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static ReferencedFiles TranslateReferencedFilesEnum(Ookii.Dialogs.Wpf.TraceLabSaveAsFileDialog.ReferencedFiles value)
        {
            switch (value)
            {
                case Ookii.Dialogs.Wpf.TraceLabSaveAsFileDialog.ReferencedFiles.IGNORE:
                    return ReferencedFiles.IGNORE;

                case Ookii.Dialogs.Wpf.TraceLabSaveAsFileDialog.ReferencedFiles.COPY:
                    return ReferencedFiles.COPY;

                case Ookii.Dialogs.Wpf.TraceLabSaveAsFileDialog.ReferencedFiles.COPYOVERWRITE:
                    return ReferencedFiles.COPYOVERWRITE;

                case Ookii.Dialogs.Wpf.TraceLabSaveAsFileDialog.ReferencedFiles.KEEPREFERENCE:
                    return ReferencedFiles.KEEPREFERENCE;

                default: return ReferencedFiles.IGNORE;
            }
        }

        #endregion
    }
}
