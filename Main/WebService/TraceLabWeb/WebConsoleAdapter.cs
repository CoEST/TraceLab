using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraceLab;

namespace TraceLabWeb
{
    public class WebConsoleAdapter
    {
        static TraceLabApplicationWebConsole application;

        public static void Run()
        {
            application = new TraceLabApplicationWebConsole();
            application.RunThis();
        }

        public static string GetLog()
        {
            return application.GetLog();
        }
        
    }
}
