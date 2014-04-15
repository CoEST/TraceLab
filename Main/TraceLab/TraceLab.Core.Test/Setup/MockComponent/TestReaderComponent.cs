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
    [Component(GuidIDString = "25b373b0-e3ae-41a7-8915-914cc0c8637b",
                Name = "Test reader",
                DefaultLabel = "Test reader",
                Description = "Reads int variable",
                Author = "Test",
                Version = "1.0")]
    [IOSpec(IOSpecType.Input, "testinput", typeof(int))]
    public class TestReaderComponent : BaseComponent
    {
        public TestReaderComponent(ComponentLogger log)
            : base(log)
        {
        }

        public override void Compute()
        {
            Logger.Trace("TestReader Computing");
            int x = (int)Workspace.Load("testinput");
            System.Diagnostics.Trace.WriteLine("-> TestReader loaded testinput: " + x);
        }
    }
}
