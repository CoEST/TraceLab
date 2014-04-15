using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ookii.Dialogs.Wpf.Interop;

namespace Ookii.Dialogs.Wpf
{
    class TraceLabSaveAsDialogEvents : IFileDialogEvents, IFileDialogControlEvents
    {
        public TraceLabSaveAsDialogEvents(TraceLabSaveAsFileDialog dialog)
        {
            if (dialog == null)
                throw new ArgumentNullException("dialog");

            _dialog = dialog;
            dialogEvents = new VistaFileDialogEvents(dialog);
        }

        private TraceLabSaveAsFileDialog _dialog;

        private VistaFileDialogEvents dialogEvents;

        #region IFileDialogControlEvents Members

        public void OnItemSelected(IFileDialogCustomize pfdc, int dwIDCtl, int dwIDItem)
        {
            _dialog.OnItemSelected(pfdc, dwIDCtl, dwIDItem);
        }

        public void OnButtonClicked(IFileDialogCustomize pfdc, int dwIDCtl)
        {

        }

        public void OnCheckButtonToggled(IFileDialogCustomize pfdc, int dwIDCtl, bool bChecked)
        {

        }

        public void OnControlActivating(IFileDialogCustomize pfdc, int dwIDCtl)
        {

        }

        #endregion

        #region IFileDialogEvents Members

        public HRESULT OnFileOk(IFileDialog pfd)
        {
            return dialogEvents.OnFileOk(pfd);
        }

        public HRESULT OnFolderChanging(IFileDialog pfd, IShellItem psiFolder)
        {
            return dialogEvents.OnFolderChanging(pfd, psiFolder);
        }

        public void OnFolderChange(IFileDialog pfd)
        {
            dialogEvents.OnFolderChange(pfd);
        }

        public void OnSelectionChange(IFileDialog pfd)
        {
            dialogEvents.OnSelectionChange(pfd);
        }

        public void OnShareViolation(IFileDialog pfd, IShellItem psi, out NativeMethods.FDE_SHAREVIOLATION_RESPONSE pResponse)
        {
            dialogEvents.OnShareViolation(pfd, psi, out pResponse);
        }

        public void OnTypeChange(IFileDialog pfd)
        {
            dialogEvents.OnTypeChange(pfd);
        }

        public void OnOverwrite(IFileDialog pfd, IShellItem psi, out NativeMethods.FDE_OVERWRITE_RESPONSE pResponse)
        {
            dialogEvents.OnOverwrite(pfd, psi, out pResponse);
        }

        #endregion
    }
}
