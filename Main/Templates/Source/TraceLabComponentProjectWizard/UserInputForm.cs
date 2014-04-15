using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TraceLabComponentProjectWizard
{
    public partial class UserInputForm : Form
    {
        public UserInputForm()
        {
            InitializeComponent();
            folderBrowserDialog.ShowNewFolderButton = true;
        }

        private void TLSDKBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = openTraceLabSDK.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = openTraceLabSDK.FileName;
                TBTraceLabSDK.Text = file;
            }
        }

        private void TLSDKTypesBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = openTraceLabSDK.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = openTraceLabSDK.FileName;
                TBTLTypes.Text = file;
            }
        }

        private void OutputDirectoryBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string path = folderBrowserDialog.SelectedPath;
                TBOutputDirectory.Text = path;
            }
        }
    }
}
