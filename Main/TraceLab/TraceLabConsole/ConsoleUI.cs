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
using TraceLab.Core.ViewModels;
using System.Collections.Specialized;
using TraceLab.Core.Components;
using TraceLab.Core.Workspaces;

namespace TraceLabConsole
{
    public class ConsoleUI
    {
        private ConsoleUI(ApplicationViewModel application)
        {
            Application = application;

            ComponentLibraryScannningWaiter = new System.Threading.ManualResetEventSlim();
            if (Application.ComponentLibraryViewModel.IsRescanning == false)
            {
                ComponentLibraryScannningWaiter.Set();
            }

            //attach event to the rescanned event of components library, so that console prompt is shown after rescan is done
            Application.ComponentLibraryViewModel.Rescanned += new EventHandler(ComponentLibraryViewModel_Rescanned);
        }

        public static void Run(ApplicationViewModel application)
        {
            if (ConsoleInstance != null)
            {
                throw new InvalidOperationException("Console UI is already running!");
            }

            ConsoleInstance = new ConsoleUI(application);

            ConsoleInstance.ComponentLibraryScannningWaiter.Wait();

            ConsoleInstance.DisplayExistingLogs();
            ConsoleInstance.StartListenToLogEvents();

            ConsoleInstance.Exit = false;

            
            ConsoleInstance.ParseInput("open: C:\\Program Files (x86)\\COEST\\TraceLab\\Tutorials\\First experiment\\VectorSpaceStandardExperiment.teml");
            ConsoleInstance.ParseInput("run");

            while (true) {
                Console.Write("#> ");
                string input = Console.ReadLine();

            };
            
        }

        public static void RunCommand(string input)
        {
            if (ConsoleInstance.ParseInput(input) == false)
            {
                Console.WriteLine("Command is incorrect. Display commands using ?");
            }
        }
        /*    
            public static void Run(ApplicationViewModel application)
            {
                if (ConsoleInstance != null)
                {
                    throw new InvalidOperationException("Console UI is already running!");
                }

                ConsoleInstance = new ConsoleUI(application);

                ConsoleInstance.ComponentLibraryScannningWaiter.Wait();

                ConsoleInstance.DisplayExistingLogs();
                ConsoleInstance.StartListenToLogEvents();

                ConsoleInstance.Exit = false;

                while (!ConsoleInstance.Exit)
                {
                    Console.Write("#> ");
                    string input = Console.ReadLine();
                    if (ConsoleInstance.ParseInput(input) == false)
                    {
                        Console.WriteLine("Command is incorrect. Display commands using ?");
                    }
                }

                //cleanup
                LogViewModel.DestroyLogTargets();
            }
            */

        #region Private Properties

        private static ConsoleUI ConsoleInstance
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

        #endregion

        #region Console Commands

        private static string stopcommand = "stop";

        static ConsoleUI()
        {
            Commands = new Dictionary<string, Action<string>>();
            Commands["run"] = new Action<string>(RunExperiment);
            Commands["stop"] = new Action<string>(StopExperiment);
            Commands["open"] = new Action<string>(OpenExperiment);
            Commands["o"] = new Action<string>(OpenExperiment);
            Commands["experiment"] = new Action<string>(DisplayExperimentInfo);
            Commands["e"] = new Action<string>(DisplayExperimentInfo);
            Commands["workspace"] = new Action<string>(DisplayWorkspace);
            Commands["w"] = new Action<string>(DisplayWorkspace);
            Commands["components"] = new Action<string>(DisplayComponents);
            Commands["c"] = new Action<string>(DisplayComponents);
            Commands["exit"] = new Action<string>(ExitProgram);
            Commands["?"] = new Action<string>(DisplayHelp);
            
        }
        
        private static void DisplayHelp(string value)
        {
            Console.WriteLine("COEST TraceLab");
            Console.WriteLine();
            Console.WriteLine("Available commands:");
            Console.WriteLine("\topen [filepath]\t- Opens the experiment file. Abbreviated as: o");
            Console.WriteLine("\trun \t- Runs the experiment");
            Console.WriteLine("\tstop \t- Stops the running experiment");
            Console.WriteLine("\texperiment \t- Displays info about currently opened experiment. Abbreviated as: e");
            Console.WriteLine("\tcomponents\t- Displays loaded components.  Abbreviated as: c");
            Console.WriteLine("\tworkspace\t- Displays data in the workspace.  Abbreviated as: w");
            Console.WriteLine("\texit\t- Exits tracelab");
            Console.WriteLine("\t?\t- This help message.");
            Console.WriteLine();
        }

        private static void DisplayExperimentInfo(string value)
        {
            if (ConsoleInstance.Application.Experiment == null)
            {
                Console.WriteLine("\tExperiment has not been opened yet.");
                Console.WriteLine("\tOpen experiment first: open:[filepath]");
            }
            else
            {
                Console.WriteLine("Experiment information");
                Console.WriteLine("\tName:\t {0}",ConsoleInstance.Application.Experiment.ExperimentInfo.Name);
                Console.WriteLine("\tFilePath:\t {0}",ConsoleInstance.Application.Experiment.ExperimentInfo.FilePath);
                Console.WriteLine("\tAuthor:\t {0}",ConsoleInstance.Application.Experiment.ExperimentInfo.Author);
                Console.WriteLine("\tContributors:\t {0}",ConsoleInstance.Application.Experiment.ExperimentInfo.Contributors);
                Console.WriteLine("\tDescription:\t {0}",ConsoleInstance.Application.Experiment.ExperimentInfo.Description);
                Console.WriteLine("\tVertices count:\t {0}", ConsoleInstance.Application.Experiment.VertexCount);
            }
        }

        private static void RunExperiment(string value)
        {
            var experiment = ConsoleInstance.Application.Experiment;
            if (experiment != null)
            {
                ExperimentProgress progress = new ExperimentProgress();
                experiment.RunExperiment(progress, ConsoleInstance.Application.Workspace, ComponentsLibrary.Instance);
            }
            else
            {
                Console.WriteLine("\tExperiment has not been opened yet.");
                Console.WriteLine("\tOpen experiment first: open:[filepath]");
            }
        }

        private static void StopExperiment(string value)
        {
            var experiment = ConsoleInstance.Application.Experiment;
            if (experiment != null && experiment.IsExperimentRunning == true)
            {
                Console.WriteLine("\tStop experiment signaled. Stops experiment after all currently running components have finished processing");
                experiment.StopRunningExperiment();
            }
            else
            {
                Console.WriteLine("\tExperiment is not running.");
            }
        }

        private static void OpenExperiment(string value)
        {
            try
            {
                value = "C:\\Program Files (x86)\\COEST\\TraceLab\\Tutorials\\First experiment\\VectorSpaceStandardExperiment.teml";
                var experiment = TraceLab.Core.Experiments.ExperimentManager.Load(value, ComponentsLibrary.Instance);
                if (experiment != null)
                {
                    ReloadApplicationViewModel(experiment);
                    Console.WriteLine("\tExperiment has been opened.");
                    DisplayExperimentInfo(null);
                }
            }
            catch (TraceLab.Core.Exceptions.ExperimentLoadException ex)
            {
                string msg = String.Format("Unable to open the file {0}. Error: {1}", value, ex.Message);
                NLog.LogManager.GetCurrentClassLogger().Warn(msg);
            }
            catch (Exception ex)
            {
                string msg = String.Format("Unable to open the file {0}. Error: {1}", value, ex.Message);
                NLog.LogManager.GetCurrentClassLogger().Warn(msg);
            }
        }

        private static void DisplayWorkspace(string value)
        {
            Console.WriteLine("Workspace:");
            if (ConsoleInstance.Application.Workspace.Units.Count == 0)
            {
                Console.WriteLine("\tWorkspace is empty");
            }
            else
            {
                foreach (WorkspaceUnit unitData in ConsoleInstance.Application.Workspace.Units)
                {
                    Console.WriteLine("\t{0}", unitData.FriendlyUnitName);
                }
            }
        }

        private static void DisplayComponents(string value)
        {
            Console.WriteLine("Components: ");
            foreach (MetadataDefinition definition in ConsoleInstance.Application.ComponentLibraryViewModel.ComponentsCollection)
            {
                Console.WriteLine("\t{0}", definition.Label);
            }
        }

        private static void ExitProgram(string value)
        {
            ConsoleInstance.Exit = true;
        }

        #endregion Console Commands

        #region Events Handlers
        
        private void ComponentLibraryViewModel_Rescanned(object sender, EventArgs e)
        {
            var experiment = ConsoleInstance.Application.Experiment;
            if (experiment != null)
            {
                var refreshedExperiment = TraceLab.Core.Experiments.ExperimentManager.ReloadExperiment(experiment, ComponentsLibrary.Instance);
                ReloadApplicationViewModel(refreshedExperiment);
            }
            NLog.LogManager.GetCurrentClassLogger().Info("Components Library scanning finished successfully!");
            ComponentLibraryScannningWaiter.Set();
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
        
        private void StartListenToLogEvents()
        {
            ((INotifyCollectionChanged)Application.LogViewModel.Events).CollectionChanged += LogEventsCollectionChanged;
        }

        private void StopListenToLogEvents()
        {
            ((INotifyCollectionChanged)Application.LogViewModel.Events).CollectionChanged -= LogEventsCollectionChanged;
        }

        #endregion Events Handlers

        #region Other Methods

        private bool ParseInput(string input)
        {
            bool validInput = false;

            if (input != null)
            {

                string command = input;
                string commandValue = null;

                int valuePosition = input.IndexOf(':');
                if (valuePosition != -1)
                {
                    command = input.Substring(0, valuePosition);
                    commandValue = input.Substring(command.Length + 1);
                }
                
                Action<string> func = null;
                if (Commands.TryGetValue(command, out func) && func != null)
                {
                    var currentExperiment = ConsoleInstance.Application.Experiment;
                    if (currentExperiment != null && currentExperiment.IsExperimentRunning && command.Equals(stopcommand) == false)
                    {
                        Console.WriteLine("\tCurrently there is running experiment, and this command cannot be executed.");
                    }
                    else
                    {
                        func(commandValue);
                        validInput = true;
                    }
                }
            }

            return validInput;
        }
        
        private void DisplayExistingLogs()
        {
            foreach (LogInfo logInfo in new System.Collections.ArrayList(Application.LogViewModel.Events))
            {
                PrintLog(logInfo);
            }
        }

        private static void PrintLog(LogInfo logInfo)
        {
            Console.WriteLine("{0} from '{1}': Message: {2}", logInfo.Level, logInfo.SourceName, logInfo.Message);
            if (logInfo.Exception != null)
            {
                Console.WriteLine(logInfo.Exception.ToString());
            }
        }

        private static void ReloadApplicationViewModel(TraceLab.Core.Experiments.Experiment experiment)
        {
            ConsoleInstance.StopListenToLogEvents();
            ConsoleInstance.Application = ApplicationViewModel.CreateNewApplicationViewModel(ConsoleInstance.Application, experiment);
            ConsoleInstance.StartListenToLogEvents();
        }

        #endregion
    }
}
