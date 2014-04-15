// Copyright (c) Sven Groot (Ookii.org) 2009
// See license.txt for details
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf.Interop;
using System.Windows;

namespace Ookii.Dialogs.Wpf
{
    /// <summary>
    /// Prompts the user to select a location for saving a file.
    /// </summary>
    /// <remarks>
    /// This class will use the Vista style save file dialog if possible, and automatically fall back to the old-style 
    /// dialog on versions of Windows older than Vista.
    /// </remarks>
    /// <remarks>
    /// <para>
    ///   Windows Vista provides a new style of common file dialog, with several new features (both from
    ///   the user's and the programmers perspective).
    /// </para>
    /// <para>
    ///   This class will use the Vista-style file dialogs if possible, and automatically fall back to the old-style 
    ///   dialog on versions of Windows older than Vista. This class is aimed at applications that
    ///   target both Windows Vista and older versions of Windows, and therefore does not provide any
    ///   of the new APIs provided by Vista's file dialogs.
    /// </para>
    /// <para>
    ///   This class precisely duplicates the public interface of <see cref="SaveFileDialog"/> so you can just replace
    ///   any instances of <see cref="SaveFileDialog"/> with the <see cref="VistaSaveFileDialog"/> without any further changes
    ///   to your code.
    /// </para>
    /// </remarks>
    /// <threadsafety instance="false" static="true" />
    [Designer("System.Windows.Forms.Design.SaveFileDialogDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"), Description("Prompts the user to open a file.")]
    public class TraceLabSaveAsFileDialog : VistaSaveFileDialog
    {
        /// <summary>
        /// Creates a new instance of <see cref="VistaSaveFileDialog" /> class.
        /// </summary>
        public TraceLabSaveAsFileDialog(string root) : base()
        {
            m_rootDrive = root;
        }

        private string m_rootDrive;

        #region Internal Methods

        internal override Ookii.Dialogs.Wpf.Interop.IFileDialog CreateFileDialog()
        {
            return new Ookii.Dialogs.Wpf.Interop.NativeFileSaveDialog();
        }

        /// <summary>
        /// Raises the <see cref="VistaFileDialog.FileOk" /> event.
        /// </summary>
        /// <param name="e">A <see cref="System.ComponentModel.CancelEventArgs" /> that contains the event data.</param>        
        protected override void OnFileOk(CancelEventArgs e)
        {
            string newRoot = System.IO.Path.GetPathRoot(FileName);
            if (newRoot.Equals(m_rootDrive) == false && m_referencedFilesProcessing == ReferencedFiles.KEEPREFERENCE)
            {
                MessageBox.Show(cannotDetermineRelativePathsError, cannotDetermineRelativePathsTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                e.Cancel = true;
                return;
            }
        }

        private const int _configVisualGroupId = 0x4002;
        private const int _comboBoxId = 0x4003;
        private const int _ignoreId = 0x4004;
        private const int _copyId = 0x4005;
        private const int _copyOverwriteId = 0x4006;
        private const int _keepReferenceId = 0x4007;
        private const int _helpTextId = 0x5000;

        private const string ignoreLabel = "Ignore";
        private const string copyLabel = "Copy without override";
        private const string copyOverwriteLabel = "Copy with overwrite";
        private const string determinePathsLabel = "Keep reference";
        private const string ignoreDescription = "References are updated to new locations, but files are not copied.";
        private const string copyDescription = "References are updated to new locations, and files are going to be copied, but will not overwrite existing files. Files in referenced directories are not copied!";
        private const string copyOverwriteDescription = "References are updated to new locations, files are going to be copied, and will overwrite existing files. Files in referenced directories are not copied!";
        private const string determinePathsDescription = "References to old files are going to be sustained.";
        private const string cannotDetermineRelativePathsTitle = "Keep reference to files error";
        private const string cannotDetermineRelativePathsError = "Cannot determine new relative paths, if the experiment is saved to different drive.";

        internal override void SetDialogProperties(IFileDialog dialog)
        {
            base.SetDialogProperties(dialog);

            uint cookie;
            var events = new TraceLabSaveAsDialogEvents(this);
            dialog.Advise(events, out cookie);
            
            Ookii.Dialogs.Wpf.Interop.IFileDialogCustomize customize = (Ookii.Dialogs.Wpf.Interop.IFileDialogCustomize)dialog;
            
            customize.StartVisualGroup(_configVisualGroupId, "Referenced files:");
            customize.AddComboBox(_comboBoxId);
            customize.AddControlItem(_comboBoxId, _ignoreId, ignoreLabel);
            customize.AddControlItem(_comboBoxId, _copyId, copyLabel);
            customize.AddControlItem(_comboBoxId, _copyOverwriteId, copyOverwriteLabel);
            customize.AddControlItem(_comboBoxId, _keepReferenceId, determinePathsLabel);
            customize.SetSelectedControlItem(_comboBoxId, _ignoreId); //select default
            customize.EndVisualGroup();

            customize.AddText(_helpTextId, ignoreDescription);
        }

        internal void OnItemSelected(IFileDialogCustomize customize, int dwIDCtl, int dwIDItem)
        {
            if (dwIDCtl == _comboBoxId)
            {
                if (dwIDItem == _ignoreId) customize.SetControlLabel(_helpTextId, ignoreDescription);
                if (dwIDItem == _copyId) customize.SetControlLabel(_helpTextId, copyDescription);
                if (dwIDItem == _copyOverwriteId) customize.SetControlLabel(_helpTextId, copyOverwriteDescription);
                if (dwIDItem == _keepReferenceId) customize.SetControlLabel(_helpTextId, determinePathsDescription);
            }
        }

        internal override void GetResult(IFileDialog dialog)
        {
            Ookii.Dialogs.Wpf.Interop.IFileDialogCustomize customize = (Ookii.Dialogs.Wpf.Interop.IFileDialogCustomize)dialog;
            int selected;
            customize.GetSelectedControlItem(_comboBoxId, out selected);

            SetRelativePathsProcessing(selected);

            base.GetResult(dialog);
        }

        #endregion

        private ReferencedFiles m_referencedFilesProcessing = ReferencedFiles.IGNORE;
        /// <summary>
        /// Determines what to do with files that are referenced in the experiment.
        /// </summary>
        public ReferencedFiles ReferencedFilesProcessing
        {
            get
            {
                return m_referencedFilesProcessing;
            }
        }

        private void SetRelativePathsProcessing(int selected)
        {
            if (selected == _copyId)
            {
                m_referencedFilesProcessing = ReferencedFiles.COPY;
            }
            else if (selected == _copyOverwriteId)
            {
                m_referencedFilesProcessing = ReferencedFiles.COPYOVERWRITE;
            }
            else if (selected == _keepReferenceId)
            {
                m_referencedFilesProcessing = ReferencedFiles.KEEPREFERENCE;
            }
            else
            {
                m_referencedFilesProcessing = ReferencedFiles.IGNORE;
            }
        }

        /// <summary>
        /// Determines what to do with files that are referenced in the experiment.
        /// </summary>
        public enum ReferencedFiles
        {
            IGNORE,
            COPY,
            COPYOVERWRITE,
            KEEPREFERENCE
        }
    }
}
