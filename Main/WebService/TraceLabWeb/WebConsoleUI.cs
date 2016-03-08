using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceLab.Core.ViewModels;
using System.Collections.Specialized;
using TraceLab.Core.Components;
using TraceLab.Core.Workspaces;


namespace TraceLabWeb
{
    public class WebConsoleUI
    {
        public static string log;

        public static void UpdateLog( string inLog)
        {
            log = inLog;
        }

        private WebConsoleUI(ApplicationViewModel application)
        {
            Application = application;
        }

        public static void Run(ApplicationViewModel application)
        {
            ConsoleInstance = new WebConsoleUI(application);

           // ConsoleInstance.ComponentLibraryScannningWaiter.Wait();

            ConsoleInstance.DisplayExistingLogs();

            /*
            if (ConsoleInstance != null)
            {
                throw new InvalidOperationException("Console UI is already running!");
            }

            ConsoleInstance = new WebConsoleUI(application);

            ConsoleInstance.ComponentLibraryScannningWaiter.Wait();

            ConsoleInstance.DisplayExistingLogs();



            ConsoleInstance.StartListenToLogEvents();

            ConsoleInstance.Exit = false;

            /*while (!ConsoleInstance.Exit)
            {
                Console.Write("#> ");
                string input = Console.ReadLine();
                if (ConsoleInstance.ParseInput(input) == false)
                {
                    Console.WriteLine("Command is incorrect. Display commands using ?");
                }
            }

            //cleanup
            LogViewModel.DestroyLogTargets();*/
            
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
            log += ("COEST TraceLab");

            log += ("Available commands:");
            log += ("\topen [filepath]\t- Opens the experiment file. Abbreviated as: o");
            log += ("\trun \t- Runs the experiment");
            log += ("\tstop \t- Stops the running experiment");
            log += ("\texperiment \t- Displays info about currently opened experiment. Abbreviated as: e");
            log += ("\tcomponents\t- Displays loaded components.  Abbreviated as: c");
            log += ("\tworkspace\t- Displays data in the workspace.  Abbreviated as: w");
            log += ("\texit\t- Exits tracelab");
            log += ("\t?\t- This help message.");
        }

        public static void OpenExperiment(string value)
        {
            try
            {
                var experiment = TraceLab.Core.Experiments.ExperimentManager.Load(value, ComponentsLibrary.Instance);
                if (experiment != null)
                {
                    ReloadApplicationViewModel(experiment);
                    //Console.WriteLine("\tExperiment has been opened.");
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
                log += ("Experiment information");
                log += ("\tName:\t {0}"+ ConsoleInstance.Application.Experiment.ExperimentInfo.Name);
                log += ("\tFilePath:\t {0}"+ ConsoleInstance.Application.Experiment.ExperimentInfo.FilePath);
                log += ("\tAuthor:\t {0}"+ ConsoleInstance.Application.Experiment.ExperimentInfo.Author);
                log += ("\tContributors:\t {0}"+ ConsoleInstance.Application.Experiment.ExperimentInfo.Contributors);
                log += ("\tDescription:\t {0}"+ ConsoleInstance.Application.Experiment.ExperimentInfo.Description);
                log += ("\tVertices count:\t {0}"+ ConsoleInstance.Application.Experiment.VertexCount);
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
