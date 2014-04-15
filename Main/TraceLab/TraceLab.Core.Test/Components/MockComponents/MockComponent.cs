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

using TraceLabSDK;

namespace MockComponents
{
    [Component("5C5CCEB8-6E02-4375-A5E4-259DE877BBE4", "Empty", "Empty", "Nobody", "0.0", null)]
    [IOSpec(IOSpecType.Input, "sourceData", typeof(TraceLabSDK.Types.TLArtifactsCollection))]
    public class BaseMockComponent : BaseComponent
    {
        public BaseMockComponent(ComponentLogger logger) : base(logger) { }

        public override void Compute()
        {
            
        }
    }
}
