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
    [Component(GuidIDString = "D45519DE-360C-11E0-9A39-3E21E0D72085",
            Name = "Incrementer",
            DefaultLabel = "Incrementer",
            Description = "Increments given int input by 1.",
            Author = "Test",
            Version = "1.0")]
    [IOSpec(IOSpecType.Input, "testinput", typeof(int))]
    [IOSpec(IOSpecType.Output, "testoutput", typeof(int))]
    public class Incrementer : BaseComponent
    {
        public Incrementer(ComponentLogger log)
            : base(log)
        {
        }

        public override void Compute()
        {
            int x = (int)Workspace.Load("testinput");
            int y = x + 1;
            Workspace.Store("testoutput", y);

            //output log
            string log = String.Format("-> Incrementer incremented input {0} and stored output {1}", x, y);
            System.Diagnostics.Trace.WriteLine(log);
            Logger.Trace(log);
        }
    }
}
