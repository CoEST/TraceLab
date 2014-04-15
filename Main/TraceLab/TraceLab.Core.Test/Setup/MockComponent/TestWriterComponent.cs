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
    [Component(GuidIDString = "3e60ccf8-5ed0-4ee4-aa27-d3ae0ee2f0cc",
                Name = "Test writer",
                DefaultLabel = "Test writer",
                Description = "Stores int variable in workspace",
                Author = "Test",
                Version = "1.0",
                ConfigurationType=typeof(WriterConfig))]
    [IOSpec(IOSpecType.Output, "testoutput", typeof(int))]
    public class TestWriterComponent : BaseComponent
    {
        public TestWriterComponent(ComponentLogger log)
            : base(log)
        {
            config = new WriterConfig();
            Configuration = config;
        }

        private WriterConfig config;

        public override void Compute()
        {
            Logger.Trace("TestWriter Computing");
            int y = config.Value;
            Workspace.Store("testoutput", y);
            System.Diagnostics.Trace.WriteLine("-> TestWriter stored testoutput: " + y);
        }
    }

    [Serializable]
    public class WriterConfig
    {
        public WriterConfig() { }

        public int Value
        {
            get;
            set;
        }
    }
}
