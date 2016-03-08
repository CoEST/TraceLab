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

        private void StartListenToLogEvents()
        {
            ((INotifyCollectionChanged)Application.LogViewModel.Events).CollectionChanged += LogEventsCollectionChanged;
        }

        private static void PrintLog(LogInfo logInfo)
        {
            //Console.WriteLine("{0} from '{1}': Message: {2}", logInfo.Level, logInfo.SourceName, logInfo.Message);
            if (logInfo.Exception != null)
            {
         //       Console.WriteLine(logInfo.Exception.ToString());
            }
        }

        private void DisplayExistingLogs()
        {
            foreach (LogInfo logInfo in new System.Collections.ArrayList(Application.LogViewModel.Events))
            {
                UpdateLog(logInfo.Exception.ToString());
                //PrintLog(logInfo);
            }
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
