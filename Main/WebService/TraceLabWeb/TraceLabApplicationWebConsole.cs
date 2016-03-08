using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraceLab;

namespace TraceLabWeb
{
    class TraceLabApplicationWebConsole : TraceLab.TraceLabApplication
    {
        protected override void RunUI()
        {
            WebConsoleUI.Run(MainViewModel);
        }
    }
}
