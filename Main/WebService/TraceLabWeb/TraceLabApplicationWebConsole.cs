using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraceLab;
using System.Diagnostics;

namespace TraceLabWeb
{
    public class TraceLabApplicationWebConsole : TraceLab.TraceLabApplication
    {

        private static TraceLabApplicationWebConsole INSTANCE;

        private TraceLabApplicationWebConsole() { }

        protected override void RunUI()
        {
            WebConsoleUI.Run(MainViewModel);
        }

        public string GetLog()
        {
            return WebConsoleUI.getLog();
        }

        public void DisplayHelp()
        {
            WebConsoleUI.DisplayHelp();
        }

        public string GetWorkspace()
        {
            return WebConsoleUI.GetWorkspace();
        }

        public string GetComponents()
        {
            return WebConsoleUI.GetComponents();
        }

        public void OpenExperiment(string path)
        {
            //"C:\\Program Files (x86)\\COEST\\TraceLab\\Tutorials\\First experiment\\VectorSpaceStandardExperiment.teml"
            WebConsoleUI.OpenExperiment(path);
        }

        public void RunExperiment()
        {
            //"C:\\Program Files (x86)\\COEST\\TraceLab\\Tutorials\\First experiment\\VectorSpaceStandardExperiment.teml"
            WebConsoleUI.RunExperiment();
        }

        public static TraceLabApplicationWebConsole Instance
        {
            get
            {
                if (INSTANCE == null)
                {
                    string[] args = { };
                    INSTANCE = new TraceLabApplicationWebConsole();
                    INSTANCE.Run(args);
                    INSTANCE.RunUI();
                }
                return INSTANCE;
            }
        }
    }
}
