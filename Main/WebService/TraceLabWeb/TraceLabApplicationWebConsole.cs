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

        public DataTable  GetComponentConfigInfo(string comLabel)
        {
            DataTable   CompConf = WebConsoleUI.GetComponentInfo (comLabel );
            
            return CompConf;
        }

        public int UpdateComponentConfigInfo(string comID,DataTable  comUpdate)
        {
            return WebConsoleUI.updateComponent(comID, comUpdate);
        }


        public string GetNodes()
        {
            return WebConsoleUI.GetNodes(); // TODO WebConsoleUI.GetNodes
        }

        public DataTable GetEdges()
        {
            return WebConsoleUI.GetLinks();
        }

        public DataTable  GetNodesForDropdown()
        {
            return WebConsoleUI.GetNodesForDropdown(); // TODO WebConsoleUI.GetNodes
        }

        public Dictionary<string, object> OpenExperiment(string path)
        {
            //"C:\\Program Files (x86)\\COEST\\TraceLab\\Tutorials\\First experiment\\VectorSpaceStandardExperiment.teml"
            return WebConsoleUI.OpenExperiment(path);
        }

        public void SaveExperiment(string path)
        {
            //"C:\\Program Files (x86)\\COEST\\TraceLab\\Tutorials\\First experiment\\VectorSpaceStandardExperiment.teml"
            WebConsoleUI.SaveExperiment (path);
        }

        public void SaveExperimentInfo(string name, string authors, string contributors, string description)
        {
            //"C:\\Program Files (x86)\\COEST\\TraceLab\\Tutorials\\First experiment\\VectorSpaceStandardExperiment.teml"
            WebConsoleUI.SaveExperimentInfo(name, authors, contributors, description);
        }

        public void AddEdge(string edgeName,string sourceName,string targetName)
        {
            //"C:\\Program Files (x86)\\COEST\\TraceLab\\Tutorials\\First experiment\\VectorSpaceStandardExperiment.teml"
            WebConsoleUI.AddEdge(edgeName,sourceName,targetName );
        }

        public void Delete_Edge(string edgeName, string sourceName, string targetName)
        {
            //"C:\\Program Files (x86)\\COEST\\TraceLab\\Tutorials\\First experiment\\VectorSpaceStandardExperiment.teml"
            WebConsoleUI.Delete_Edge(edgeName, sourceName, targetName);
        }

        public void AddNode(string componentType,int xloc,int yloc)
        {
     
                    WebConsoleUI.AddComponenetNode (componentType ,xloc,yloc);
                    
        }

        public void Delete_Node(string nodeID)
        {
            //"C:\\Program Files (x86)\\COEST\\TraceLab\\Tutorials\\First experiment\\VectorSpaceStandardExperiment.teml"
            WebConsoleUI.Delete_Node(nodeID);
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
                    string[] args = { CommandLineProcessor.SwitchCharacter + "base:C:\\Users\\Owner\\Documents\\W&M\\cs635\\TraceLab\\Main\\WebService\\Bin" };
                    INSTANCE = new TraceLabApplicationWebConsole();
                    INSTANCE.Run(args);
                }
                return INSTANCE;
            }
        }
    }
}
