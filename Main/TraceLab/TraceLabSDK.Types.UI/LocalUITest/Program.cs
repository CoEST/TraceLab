using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TraceLabSDK.Types;

namespace LocalUITest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var editor = new TraceLabSDK.Types.TLArtifactsCollectionEditor();

            editor.Data = PrepareMockData();

            Application.Run(editor);
        }

        private static object PrepareMockData()
        {
            TLArtifactsCollection artifacts = new TLArtifactsCollection();
            int i = 0;
            foreach (string text in new string[] { "artifact text 1", "artifact text 2", "artifact text 3" })
            {
                artifacts.Add(new TLArtifact(i.ToString(), text));
                i++;
            }
            return artifacts;
        }
    }
}
