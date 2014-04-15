using System.Collections.Generic;
using SEMERU.Types.Custom;
using TraceLabSDK;
using SEMERU.Types.Custom;

/// SEMERU Component Library Extension - Custom additions to the SEMERU Component Library
/// Copyright © 2012-2013 SEMERU
/// 
/// This file is part of the SEMERU Component Library Extension.
/// 
/// The SEMERU Component Library Extension is free software: you can redistribute it
/// and/or modify it under the terms of the GNU General Public License as published
/// by the Free Software Foundation, either version 3 of the License, or (at your
/// option) any later version.
/// 
/// The SEMERU Component Library Extension is distributed in the hope that it will
/// be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public
/// License for more details.
/// 
/// You should have received a copy of the GNU General Public License along with the
/// SEMERU Component Library Extension.  If not, see <http://www.gnu.org/licenses/>.

namespace SEMERU.Custom.ICSM2011.Components
{
    [Component(Name = "SEMERU - ICSM'11 - Collect RTMIteration results",
               Description = "",
               Author = "SEMERU; Evan Moritz",
               Version = "1.0.0.0")]
    [IOSpec(IOSpecType.Input, "CurrentICSM11DataSet", typeof(int))]
    [IOSpec(IOSpecType.Input, "ListOfICSM11DataSets", typeof(List<ICSM11DataSet>))]
    [IOSpec(IOSpecType.Input, "Alpha", typeof(double))]
    [IOSpec(IOSpecType.Input, "Beta", typeof(double))]
    [IOSpec(IOSpecType.Input, "Eta", typeof(double))]
    [IOSpec(IOSpecType.Input, "NumTopics", typeof(int))]
    [IOSpec(IOSpecType.Output, "ListOfICSM11DataSets", typeof(List<ICSM11DataSet>))]
    [Tag("SEMERU.Custom.ICSM'11")]
    public class CollectRTMIterationResults : BaseComponent
    {
        public CollectRTMIterationResults(ComponentLogger log) : base(log) { }

        public override void Compute()
        {
            List<ICSM11DataSet> list = (List<ICSM11DataSet>)Workspace.Load("ListOfICSM11DataSets");
            int current = (int)Workspace.Load("CurrentICSM11DataSet") - 1;
            list[current].RTM.Alpha = (double)Workspace.Load("Alpha");
            list[current].RTM.Beta = (double)Workspace.Load("Beta");
            list[current].RTM.Eta = (double)Workspace.Load("Eta");
            list[current].RTM.NumTopics = (int)Workspace.Load("NumTopics");
            Workspace.Store("ListOfICSM11DataSets", list);
        }
    }
}