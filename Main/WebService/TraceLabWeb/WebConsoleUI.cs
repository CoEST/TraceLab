using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using TraceLab.Core.ViewModels;
using System.Collections.Specialized;
using TraceLab.Core.Components;
using TraceLab.Core.Workspaces;
using TraceLab.Core.Experiments;
using TraceLab.Core.Utilities;
using System.Data;

namespace TraceLabWeb
{
    public class WebConsoleUI
    {
        public static string log = "";
        public static string prevStatus = "";
        public static ExperimentProgress progress = new ExperimentProgress();
        public DataTable ComponentConfigDatatable = new DataTable();

        public static string getLog()
        {
            log += progress.GetLog;
            return log;
        }


        public static string getLogUntouched()
        {
            return log;
        }
    
        public static bool isExperimentRunning()
        {
            return ConsoleInstance.Application.Experiment.IsExperimentRunning ;
        }

        public static void clearLog()
        {
            log = "";
            progress.CurrentStatus = "";
            prevStatus = "";
        }

        public static void progLog(object sender, EventArgs e)
        {
            log += progress.CurrentStatus;            
        }

        public static void ExperimentCompleted(object sender, EventArgs e)
        {
            //log += progress.CurrentStatus;
                        ConsoleInstance.Application.Experiment.ExperimentCompleted -= ExperimentCompleted;
            progress.ProgressChanged -= progLog;
        }

        public static void UpdateLog( string inLog)
        {
            log += inLog;
        }

        private WebConsoleUI(ApplicationViewModel application)
        {
            Application = application;
            ComponentLibraryScannningWaiter = new System.Threading.ManualResetEventSlim();
            if (Application.ComponentLibraryViewModel.IsRescanning == false)
            {
                ComponentLibraryScannningWaiter.Set();
            }
        }

        internal static string GetWorkspace()
        {
            string components = "<ul>";
            if (ConsoleInstance.Application.Workspace.Units.Count == 0)
            {
                components += ("<li>Workspace is empty</li>");
            }
            else
            {
                foreach (WorkspaceUnit unitData in ConsoleInstance.Application.Workspace.Units)
                {
                    components += string.Format("<li>{0}</li>", unitData.FriendlyUnitName);
                }
            }
            components += "</ul>";
            return components;
        }

        internal static string GetComponents()
        {
            string components = "<ul>";
            foreach (MetadataDefinition definition in ConsoleInstance.Application.ComponentLibraryViewModel.ComponentsCollection)
            {
                components += string.Format("<li>{0}</li>", definition.Label);
            }

            components += "</ul>";
            return components;
        }
           
        internal static DataTable  GetComponentsForDropDown()
        {
            DataTable dt = new DataTable ();
            dt.Columns.Add("Label");
            dt.Columns.Add("ID");
            foreach (MetadataDefinition definition in ConsoleInstance.Application.ComponentLibraryViewModel.ComponentsCollection)
            {
                DataRow dr = dt.NewRow();
                dr["Label"] = definition.Label;
                dr["ID"] = definition.ID;
                dt.Rows.Add(dr);
            }
            
            return dt;
        }


        internal static DataTable  GetComponentInfo(string selectedID)
        {
            string compConfInfo = "";
            DataTable dt = new DataTable() ;
            dt.Columns.Add("Type");
            dt.Columns.Add("Value");
            dt.Columns.Add("Name");
            dt.Columns.Add("Section");

            ComponentNode node =(ComponentNode) ConsoleInstance.Application.Experiment.GetNode(selectedID);
            ComponentMetadata meta = (ComponentMetadata)node.Data.Metadata;

            foreach (IOItem inputItem in meta.IOSpec.Input .Values)
            {
              compConfInfo += inputItem.IOItemDefinition.Type + ","+ inputItem.MappedTo+",In,"+ inputItem.IOItemDefinition.Name  +";";
                DataRow dr = dt.NewRow();
                dr["Type"] =inputItem.IOItemDefinition.Type;
                dr["Value"] = inputItem.MappedTo ;
                dr["Name"] = inputItem.IOItemDefinition.Name ;
                dr["Section"] = "In";
                dt.Rows.Add(dr);
            }

            foreach (IOItem outputItem in meta.IOSpec.Output.Values)
            {
                compConfInfo +=  outputItem.IOItemDefinition.Type + "," + outputItem.MappedTo+",Out,"+outputItem.IOItemDefinition.Name  +  ";";

                DataRow dr = dt.NewRow();
                dr["Type"] = outputItem.IOItemDefinition.Type ;
                dr["Value"] = outputItem.MappedTo ;
                dr["Name"] = outputItem.IOItemDefinition.Name ;
                dr["Section"] = "Out";
                dt.Rows.Add(dr);
            }

            foreach (ConfigPropertyObject  confwrap in meta.ConfigWrapper.ConfigValues.Values  )
            {
                if (confwrap.Type.ToString().Equals("TraceLabSDK.Component.Config.FilePath"))
                {
                    TraceLabSDK.Component.Config.FilePath fpath = (TraceLabSDK.Component.Config.FilePath)confwrap.Value;
                    compConfInfo += confwrap.Type + "," + fpath.Absolute  + ",Conf," + confwrap.Name + ";";
                    DataRow dr = dt.NewRow();
                    dr["Type"] = confwrap.Type ;
                    dr["Value"] = fpath.Absolute ;
                    dr["Name"]= confwrap.Name ;
                    dr["Section"] = "Out";
                    dt.Rows.Add(dr);
                }
                else
                {
                compConfInfo += confwrap.Type + "," + confwrap.Value + ",Conf,"+confwrap.Name +";";
                    DataRow dr = dt.NewRow();
                    dr["Type"] = confwrap.Type ;
                    dr["Value"] = confwrap.Value ;
                    dr["Name"] = confwrap.Name ;
                    dr["Section"] = "Conf";
                    dt.Rows.Add(dr);
                }
            }

            return dt;// compConfInfo.Substring (0,compConfInfo.Length -1) ;
        }

        public static int updateComponent(string selectedID, DataTable  compUpdate)
        {//TODO there is definately a better way to do this. Look into later comparing WPF
            try
            {

                ComponentNode node = (ComponentNode)ConsoleInstance.Application.Experiment.GetNode(selectedID);
                ComponentMetadata meta = (ComponentMetadata)node.Data.Metadata;
                
                foreach(DataRow drow in compUpdate.Rows )
                {
                    if(drow["Section"].Equals ("In"))
                    {
                        foreach(IOItem inputItem in meta.IOSpec.Input.Values)
                        {
                            if (drow["Name"].Equals(inputItem.IOItemDefinition.Name))
                            {
                                inputItem.MappedTo = drow["Value"].ToString();
                            }
                        }
                    }
                    else if(drow["Section"].Equals ("Out"))
                    {
                        foreach (IOItem outputItem in meta.IOSpec.Output.Values )
                        {
                            if(drow["Name"].Equals (outputItem.IOItemDefinition.Name ))
                            {
                                outputItem.MappedTo = drow["Value"].ToString() ;
                            }
                        }
                    }
                    else
                    {
                        foreach (ConfigPropertyObject confwrap in meta.ConfigWrapper.ConfigValues.Values)
                        {
                            if(drow["Name"].Equals(confwrap.Name))
                            {
                                confwrap.Value = drow["Value"];
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }

        public static string GetNodes()
        {
            string components = "<ul><li>Nodes:</li>";
            foreach (ExperimentNode Node in ConsoleInstance.Application.Experiment.Vertices)
            {
                components += string.Format("<li>{0}</li>","ID: "+ Node.ID + " Label: " + Node.Data .Metadata.Label  );
            }

            components += "<li>Links:</li>";
            foreach (ExperimentNodeConnection Link in ConsoleInstance.Application.Experiment.Edges)
            {
                components += string.Format("<li>{0}</li>", "Source: " + Link.Source.Data.Metadata.Label + " Target: " + Link.Target.Data.Metadata.Label);

            }
                components += "</ul>";
            return components;
        }

        public static DataTable GetLinks()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("source");
            dt.Columns.Add("target");
            dt.Columns.Add("ID");
            foreach (ExperimentNodeConnection Link in ConsoleInstance.Application.Experiment.Edges )
            {
                DataRow drow = dt.NewRow();

                drow["source"]=Link.Source.ID;
                drow["target"] = Link.Target.ID;
                drow["ID"] = Link.ID;

                dt.Rows.Add(drow);
            }

            return dt;
        }


        public static DataTable   GetNodesForDropdown()
        {
            DataTable dt = new DataTable() ;
            dt.Columns.Add("ID");
            dt.Columns.Add("Label");
            dt.Columns.Add("X");
            dt.Columns.Add("Y");
            foreach (ExperimentNode Node in ConsoleInstance.Application.Experiment.Vertices)
            {
                DataRow drow = dt.NewRow();
                drow["ID"]=Node.ID;
                drow["Label"] = Node.Data.Metadata.Label;
                drow["X"] = Node.Data.X;
                drow["Y"] = Node.Data.Y;
                dt.Rows.Add(drow);
            }
            
            return dt;
        }


        public static void Run(ApplicationViewModel application)
        {
            ConsoleInstance = new WebConsoleUI(application);

            ConsoleInstance.ComponentLibraryScannningWaiter.Wait();

            ConsoleInstance.DisplayExistingLogs();
            ConsoleInstance.StartListenToLogEvents();

            log += "TraceLab is ready<br />";
                        
        }

        private void LogEventsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (LogInfo logInfo in e.NewItems)
                    {
                        PrintLog(logInfo);
                    }
                    break;
            }
        }

        public static void DisplayHelp()
        {
            log += ("<br />COEST TraceLab<br />");

            log += ("Available commands:<br />");
            log += ("\topen [filepath]\t- Opens the experiment file. Abbreviated as: o<br />");
            log += ("\trun \t- Runs the experiment<br />");
            log += ("\tstop \t- Stops the running experiment<br />");
            log += ("\texperiment \t- Displays info about currently opened experiment. Abbreviated as: e<br />");
            log += ("\tcomponents\t- Displays loaded components.  Abbreviated as: c<br />");
            log += ("\tworkspace\t- Displays data in the workspace.  Abbreviated as: w<br />");
            log += ("\texit\t- Exits tracelab<br />");
            log += ("\t?\t- This help message.<br />");
        }

        public static void OpenExperiment(string value)
        {
            //log = "";
            try
            {
                var experiment = TraceLab.Core.Experiments.ExperimentManager.Load(value, ComponentsLibrary.Instance);
                if (experiment != null)
                {
                    ReloadApplicationViewModel(experiment);
                    log += ("\tExperiment has been opened. <br/>");
                    DisplayExperimentInfo(null);
                }
            }
            catch (TraceLab.Core.Exceptions.ExperimentLoadException ex)
            {
                string msg = String.Format("Unable to open the file {0}. Error: {1}", value, ex.Message);
                log += msg;
            }
            catch (Exception ex)
            {
                string msg = String.Format("Unable to open the file {0}. Error: {1}", value, ex.Message);
                log += msg;
            }
        }

        public static void SaveExperiment(string value)
        {
            //log = "";
            try
            {

                TraceLab.Core.Experiments.ExperimentManager.SaveAs(ConsoleInstance.Application.Experiment , value, ReferencedFiles.IGNORE);

            }
            catch (TraceLab.Core.Exceptions.ExperimentLoadException ex)
            {
                string msg = String.Format("Unable to save the file {0}. Error: {1}", value, ex.Message);
                log += msg;
            }
            catch (Exception ex)
            {
                string msg = String.Format("Unable to save the file {0}. Error: {1}", value, ex.Message);
                log += msg;
            }
        }

        public static void AddEdge(string value, string sourceName,string targetName)
        {
            //log = "";
            try
            {
                var experiment = ConsoleInstance.Application.Experiment;

                string[] nodeNames = value.Split(',');
               
                    if (sourceName .Equals(targetName ))
                    {
                        log += "Same node detected twice";
                    }
                    else
                    {
                        ExperimentNode targ = ConsoleInstance.Application.Experiment.GetNode (targetName  );
                        ExperimentNode sour = ConsoleInstance.Application.Experiment.GetNode(sourceName );

                        if (sour != null && targ != null)
                        {
                           
                        ExperimentNodeConnection Edge = new ExperimentNodeConnection(value,sour,targ) ;
                            experiment.AddEdge(Edge);
                        }
                        else
                        {
                            log += "Non-existant Node Detected";
                        }

                    }
                
            }
            catch (Exception ex)
            {
                string msg = String.Format("Unable to add edge {0}. Error: {1}", value, ex.Message);
                log += msg;
            }
        }

        public static void Delete_Edge(string value, string sourceName, string targetName)
        {
            //log = "";
            try
            {
                var experiment = ConsoleInstance.Application.Experiment;

                if (sourceName.Equals(targetName))
                {
                    log += "Same node detected twice";
                }
                else
                {
                    ExperimentNode targ = ConsoleInstance.Application.Experiment.GetNode(targetName);
                    ExperimentNode sour = ConsoleInstance.Application.Experiment.GetNode(sourceName);

                    if (sour != null && targ != null)
                    {

                        //ExperimentNodeConnection Edge = new ExperimentNodeConnection(value, sour, targ);
                        bool found = false;
                        foreach (ExperimentNodeConnection Link in experiment.Edges)
                        {
                            if (Link.Source.ID.Equals(sourceName) && Link.Target.ID.Equals(targetName))
                            {
                                experiment.RemoveEdge(Link);
                                found = true;
                                break;
                            }

                        }

                        if (!found)
                        {
                            log += "Couldn't find that edge";
                        }
                    }
                    else
                    {
                        log += "Non-existant Node Detected";
                    }

                }

            }
            catch (Exception ex)
            {
                string msg = String.Format("Unable to delete edge {0}. Error: {1}", value, ex.Message);
                log += msg;
            }
        }

        public static void Delete_Node(string nodeID)
        {
            //log = "";
            try
            {
                var experiment = ConsoleInstance.Application.Experiment;
                ExperimentNode nodeToDel = ConsoleInstance.Application.Experiment.GetNode(nodeID);

                if (nodeToDel != null)
                {
                    ArrayList LinksToDel = new ArrayList();

                    foreach (ExperimentNodeConnection Link in experiment.Edges)
                    {
                        if (Link.Source.ID.Equals(nodeID) || Link.Target.ID.Equals(nodeID))
                        {
                            LinksToDel.Add(Link);
                        }

                    }

                    foreach (ExperimentNodeConnection Link in LinksToDel)
                    {
                        experiment.RemoveEdge(Link);
                    }

                    experiment.RemoveVertex(nodeToDel);
                }
                else
                {
                    log += "Non-existant Node Detected";
                }

            }
            catch (Exception ex)
            {
                string msg = String.Format("Unable to delete Node {0}. Error: {1}", nodeID, ex.Message);
                log += msg;
            }
        }

        public static void AddStartNode(string value) //TODO Uneccesary apparently
        {
            try
            {
                var experiment = ConsoleInstance.Application.Experiment;

                SerializedVertexData svd = new SerializedVertexData();
                svd.Metadata = new DecisionMetadata(value);
                ExperimentDecisionNode nodeToAdd = new ExperimentDecisionNode(Guid.NewGuid().ToString(), svd);

                nodeToAdd.Data.Metadata.Label = value;
                experiment.AddVertex(nodeToAdd);
            }

            catch
            {
                log += "Failed to create node";
            }

        }

        public static void AddEndNode(string value) //TODO Implement for Node Type
        {
            try
            {
                var experiment = ConsoleInstance.Application.Experiment;

                SerializedVertexData svd = new SerializedVertexData();
                svd.Metadata = new DecisionMetadata(value);
                ExperimentDecisionNode nodeToAdd = new ExperimentDecisionNode(Guid.NewGuid().ToString(), svd);

                nodeToAdd.Data.Metadata.Label = value;
                experiment.AddVertex(nodeToAdd);
            }

            catch
            {
                log += "Failed to create node";
            }

        }


        public static void AddComponenetNode(string value, int xloc,int yloc) //TODO Implement for Node Type
        {
            try
            {
                var experiment = ConsoleInstance.Application.Experiment;

                MetadataDefinition metadataDefinition =ConsoleInstance.Application.ComponentLibraryViewModel.GetComponentByID (value)  ;

                ExperimentNode nodeToAdd = experiment.AddComponentFromDefinition(metadataDefinition, xloc,yloc);


            }

            catch
            {
                log += "Failed to create node";
            }

        }

        public static void AddCompositeComponenetNode(string value) //TODO Implement for Node Type
        {
            try
            {
                var experiment = ConsoleInstance.Application.Experiment;

                SerializedVertexData svd = new SerializedVertexData();
                svd.Metadata = new DecisionMetadata(value);
                ExperimentDecisionNode nodeToAdd = new ExperimentDecisionNode(Guid.NewGuid().ToString(), svd);

                nodeToAdd.Data.Metadata.Label = value;
                experiment.AddVertex(nodeToAdd);
            }

            catch
            {
                log += "Failed to create node";
            }

        }

        /// <summary>
        /// Add Decision Node to Experiment
        /// </summary>
        /// <param name="value">Label of Decision Node</param>
        public static void AddDecisionNode(string value)
        {
            try
            {
                var experiment = ConsoleInstance.Application.Experiment;

                SerializedVertexData svd = new SerializedVertexData();
                svd.Metadata = new DecisionMetadata(value);
                ExperimentDecisionNode nodeToAdd = new ExperimentDecisionNode(Guid.NewGuid().ToString(), svd);

                nodeToAdd.Data.Metadata.Label = value;
                experiment.AddVertex(nodeToAdd);
            }

            catch
            {
                log += "Failed to create node";
            }

        }


        public static void AddExitDecisionNode(string value) //TODO Implement for Node Type
        {
            try
            {
                var experiment = ConsoleInstance.Application.Experiment;

                SerializedVertexData svd = new SerializedVertexData();
                svd.Metadata = new DecisionMetadata(value);
                ExperimentDecisionNode nodeToAdd = new ExperimentDecisionNode(Guid.NewGuid().ToString(), svd);

                nodeToAdd.Data.Metadata.Label = value;
                experiment.AddVertex(nodeToAdd);
            }

            catch
            {
                log += "Failed to create node";
            }

        }

        /// <summary>
        /// Add a scopeNode to the Experiment
        /// </summary>
        /// <param name="value">Name of the scope node</param>
        public static void AddScopeNode(string value)
        {
            try
            {
                var experiment = ConsoleInstance.Application.Experiment;
                var componentGraph = new CompositeComponentEditableGraph(true);

                if (componentGraph.References != null)
                {
                    componentGraph.References = experiment.References.CopyCollection();
                }
                SerializedVertexDataWithSize svd = new SerializedVertexDataWithSize();
                svd.Metadata = new ScopeMetadata (componentGraph,value, System.IO.Path.GetDirectoryName(experiment.ExperimentInfo.FilePath));
                ScopeNode  nodeToAdd = new ScopeNode(Guid.NewGuid().ToString(),svd);

                nodeToAdd.Data.Metadata.Label = value;
                experiment.AddVertex(nodeToAdd);
            }

            catch
            {
                log += "Failed to create node";
            }

        }

        /// <summary>
        /// Add a Loop Scope Node to the Experiment
        /// </summary>
        /// <param name="value">The name of the node</param>
        public static void AddLoopScopeNode(string value) 
        {
            try
            {
                var experiment = ConsoleInstance.Application.Experiment;
                var componentGraph = new CompositeComponentEditableGraph(true);

                if (componentGraph.References != null)
                {
                    componentGraph.References = experiment.References.CopyCollection();
                }

                SerializedVertexDataWithSize svd = new SerializedVertexDataWithSize();
                svd.Metadata = new LoopScopeMetadata ( componentGraph, value, System.IO.Path.GetDirectoryName(experiment.ExperimentInfo.FilePath));
                LoopScopeNode  nodeToAdd = new LoopScopeNode(Guid.NewGuid().ToString(), svd);

                nodeToAdd.Data.Metadata.Label = value;
                experiment.AddVertex(nodeToAdd);
            }

            catch
            {
                log += "Failed to create node";
            }

        }

        /// <summary>
        /// Add a challenge node to the Experiment
        /// </summary>
        /// <param name="value">the name of the challenge</param>
        public static void AddChallengeNode(string value)
        {
            try
            {
                var experiment = ConsoleInstance.Application.Experiment;

                SerializedVertexDataWithSize svd = new SerializedVertexDataWithSize();
                CompositeComponentEditableGraph compositeComponentGraph = new CompositeComponentEditableGraph (true);

                svd.Metadata = new ChallengeMetadata (compositeComponentGraph ,value, System.IO.Path.GetDirectoryName(experiment.ExperimentInfo.FilePath));
                ChallengeNode  nodeToAdd = new ChallengeNode(Guid.NewGuid().ToString(), svd);
             
               nodeToAdd.Data.Metadata.Label = value;
                experiment.AddVertex(nodeToAdd);
            }

            catch
            {
                log += "Failed to create node";
            }

        }


        public static void AddCommentNode(string value) //TODO Implement for Node Type
        {
            try
            {
                var experiment = ConsoleInstance.Application.Experiment;

                SerializedVertexData svd = new SerializedVertexData();
                svd.Metadata = new DecisionMetadata(value);
                ExperimentDecisionNode nodeToAdd = new ExperimentDecisionNode(Guid.NewGuid().ToString(), svd);

                nodeToAdd.Data.Metadata.Label = value;
                experiment.AddVertex(nodeToAdd);
            }

            catch
            {
                log += "Failed to create node";
            }

        }


        public static void RunExperiment()
        {
            var experiment = ConsoleInstance.Application.Experiment;
            if (experiment != null)
            {
                     experiment .ExperimentCompleted += ExperimentCompleted;
                progress.ProgressChanged += progLog;
      
                experiment.RunExperiment(progress, ConsoleInstance.Application.Workspace, ComponentsLibrary.Instance);
                log += progress.GetLog;
            }
            else
            {
                log += ("\tExperiment has not been opened yet.");
                log += ("\tOpen experiment first: open:[filepath]");
            }
        }

        private void StartListenToLogEvents()
        {
            ((INotifyCollectionChanged)Application.LogViewModel.Events).CollectionChanged += LogEventsCollectionChanged;
        }

        private void StopListenToLogEvents()
        {
            ((INotifyCollectionChanged)Application.LogViewModel.Events).CollectionChanged -= LogEventsCollectionChanged;
        }

        private static void DisplayExperimentInfo(string value)
        {
            if (ConsoleInstance.Application.Experiment == null)
            {
                log += ("\tExperiment has not been opened yet.");
                log += ("\tOpen experiment first: open:[filepath]");
            }
            else
            {
                log += string.Format("<br />Experiment information<br />");
                log += string.Format("\tName:\t {0}<br />", ConsoleInstance.Application.Experiment.ExperimentInfo.Name);
                log += string.Format("\tFilePath:\t {0}<br />", ConsoleInstance.Application.Experiment.ExperimentInfo.FilePath);
                log += string.Format("\tAuthor:\t {0}<br />", ConsoleInstance.Application.Experiment.ExperimentInfo.Author);
                log += string.Format("\tContributors:\t {0}<br />", ConsoleInstance.Application.Experiment.ExperimentInfo.Contributors);
                log += string.Format("\tDescription:\t {0}<br />", ConsoleInstance.Application.Experiment.ExperimentInfo.Description);
                log += string.Format("\tVertices count:\t {0}<br /><br/>", ConsoleInstance.Application.Experiment.VertexCount);
            }
        }

        private static void PrintLog(LogInfo logInfo)
        {
            //Console.WriteLine("{0} from '{1}': Message: {2}", logInfo.Level, logInfo.SourceName, logInfo.Message);
            if (logInfo.Exception != null)
            {
                //       Console.WriteLine(logInfo.Exception.ToString());
                UpdateLog(logInfo.Exception.ToString());
            }
        }

        private void DisplayExistingLogs()
        {
            foreach (LogInfo logInfo in new System.Collections.ArrayList(Application.LogViewModel.Events))
            {
                //UpdateLog(logInfo.Exception.ToString());
                PrintLog(logInfo);
            }
        }

        private static void ReloadApplicationViewModel(TraceLab.Core.Experiments.Experiment experiment)
        {
            ConsoleInstance.StopListenToLogEvents();
            ConsoleInstance.Application = ApplicationViewModel.CreateNewApplicationViewModel(ConsoleInstance.Application, experiment);
            ConsoleInstance.StartListenToLogEvents();
        }

        private static WebConsoleUI ConsoleInstance
        {
            get;
            set;
        }

        private static Dictionary<string, Action<string>> Commands
        {
            get;
            set;
        }

        private System.Threading.ManualResetEventSlim ComponentLibraryScannningWaiter
        {
            get;
            set;
        }

        private ApplicationViewModel Application
        {
            get;
            set;
        }

        private bool Exit
        {
            get;
            set;
        }
    }

}
