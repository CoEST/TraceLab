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
    [Component(GuidIDString = "E8244E98-2D98-11E0-9317-5E44E0D72085",
            Name = "Test Empty Component",
            DefaultLabel = "Test Empty Component",
            Description = "Does nothing. ",
            Author = "Test",
            Version = "1.0")]
    public class TestEmptyComponent : BaseComponent
    {
        public TestEmptyComponent(ComponentLogger log)
            : base(log)
        {
        }

        public override void Compute()
        {
            //do nothing
            Logger.Trace("TestEmpty doing nothing");
        }
    }
}
