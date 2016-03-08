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
   
        public void RunThis()
        {
            this.RunUI();
        }

        protected override void RunUI()
        {
            WebConsoleUI.Run(MainViewModel);
        }

        public string GetLog()
        {
            return WebConsoleUI.log;
        }

        public void DisplayHelp()
        {
            WebConsoleUI.DisplayHelp();
        }
    }
}
