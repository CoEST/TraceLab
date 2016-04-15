using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraceLabWeb
{
    public class ConsoleWebAdapter
    {
        #region Initalization


        private ConsoleWebAdapter() { }

        private static ConsoleWebAdapter INSTANCE;

        public static ConsoleWebAdapter Instance
        {
            get
            {
                if (INSTANCE == null)
                {
                    string[] args = { };
                    INSTANCE = new ConsoleWebAdapter();
                    INSTANCE.Initialize();
                }
                return INSTANCE;
            }
        }

        private static String Log;
        private static TraceLab.TraceLabApplication WebService;
        private bool IsStarted;

        private void Initialize()
        {
            WebService = new TraceLabApplicationWeb();
        }

        #endregion

        public void StartWebService(string[] args)
        {
            if (false == IsStarted)
            { 
                WebService.Run(args);
                IsStarted = true;
            }
            //Run Main Engine Code//
            
            //RunUI Method Equivalent//
            //WebUI.Run(TraceLabApplicationWeb.MainViewModel);
        }

        public void RunCommand(String CommandType, String CommandString)
        {
            switch (CommandType)
            {
                case "Open":
                    WebUI.RunCommand(CommandString);
                break;

                case "Run":
                    WebUI.RunCommand(CommandString);
                break;

                case "?":
                    WebUI.RunCommand(CommandString);
                break;
                
                default:
                   // Console.WriteLine("Command is incorrect. Display commands using ?");
                break;
            }
        }

        /// <summary>
        /// Log Related Methods;
        /// </summary>
        /// <returns></returns>
        static String GetLog()
        {
            return Log;
        }

        static void AddToLog( String inString)
        {
            Log += inString;   
        }

        static void ClearLog( )
        {
            Log = "";
        }
    }
}
