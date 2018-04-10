using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraceLab;
using System.Diagnostics;
using System.Data;

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

        public string GetLogUntouched()
        {
            return WebConsoleUI.getLogUntouched();
        }

        public bool IsExperimentRunning()
        {
            return WebConsoleUI.isExperimentRunning();
        }

        public void ClearLog()
        {
             WebConsoleUI.clearLog ();
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

        public DataTable GetComponentListForDropDown()
        {
            return WebConsoleUI.GetComponentsForDropDown ();
        }



        public string GetNodes()
        {
            return WebConsoleUI.GetNodes (); // TODO WebConsoleUI.GetNodes
        }

        public void OpenExperiment(string path)
        {
            //"C:\\Program Files (x86)\\COEST\\TraceLab\\Tutorials\\First experiment\\VectorSpaceStandardExperiment.teml"
            WebConsoleUI.OpenExperiment(path);
        }

        public void SaveExperiment(string path)
        {
            //"C:\\Program Files (x86)\\COEST\\TraceLab\\Tutorials\\First experiment\\VectorSpaceStandardExperiment.teml"
            WebConsoleUI.SaveExperiment (path);
        }

        public void AddEdge(string edgeName,string sourceName,string targetName)
        {
            //"C:\\Program Files (x86)\\COEST\\TraceLab\\Tutorials\\First experiment\\VectorSpaceStandardExperiment.teml"
            WebConsoleUI.AddEdge(edgeName,sourceName,targetName );
        }

        public void AddNode(string componentType,int xloc,int yloc)
        {
     
                    WebConsoleUI.AddComponenetNode (componentType ,xloc,yloc);
                    
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
                    string[] args = { CommandLineProcessor.SwitchCharacter + "base:C:\\Users\\Andrew\\Documents\\Git\\TraceLab\\Main\\WebService\\Bin" };
                    INSTANCE = new TraceLabApplicationWebConsole();
                    INSTANCE.Run(args);
                }
                return INSTANCE;
            }
        }
    }
}
