using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TraceLabWeb
{
    class TraceLabApplicationWeb : TraceLab.TraceLabApplication
    {
        protected override void RunUI()
        {
            WebUI.Run(MainViewModel);
        }
    }
}
