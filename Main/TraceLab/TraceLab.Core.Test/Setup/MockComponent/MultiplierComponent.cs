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
using TraceLabSDK;


namespace MockComponent
{
    [Component(GuidIDString = "5653b972-3e37-4cf7-826f-f33a68030fa3",
        Name = "Multiplier",
        DefaultLabel = "Multiplier",
        Description = "Multiplies given int input by 2.",
        Author = "Test",
        Version = "1.0")]
    [IOSpec(IOSpecType.Input, "testinput", typeof(int))]
    [IOSpec(IOSpecType.Output, "testoutput", typeof(int))]
    public class MultiplierComponent : BaseComponent
    {
        public MultiplierComponent(ComponentLogger log)
            : base(log)
        {
        }

        public override void Compute()
        {
            //force to sleep
            string log = "-> Multiplier sleeps for 500ms";
            System.Diagnostics.Trace.WriteLine(log);
            Logger.Trace(log);
            System.Threading.Thread.Sleep(500);

            //load input and multiply by 2
            int x = (int)Workspace.Load("testinput");
            int y = x*2;
            Workspace.Store("testoutput", y);
            
            //output log
            log = String.Format("-> Multiplier multiplied input {0} by 2 and stored result output {1}", x, y);
            System.Diagnostics.Trace.WriteLine(log);
            Logger.Trace(log);
        }
    }
}
