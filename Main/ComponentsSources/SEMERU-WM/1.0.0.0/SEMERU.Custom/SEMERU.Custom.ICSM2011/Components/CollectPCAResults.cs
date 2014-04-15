using System.Collections.Generic;
using SEMERU.Types.Custom;
using TraceLabSDK;

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
    [Component(Name = "SEMERU - ICSM'11 - Collect PCA results",
               Description = "",
               Author = "SEMERU; Evan Moritz",
               Version = "1.0.0.0")]
    [IOSpec(IOSpecType.Input, "CurrentDataset", typeof(int))]
    [IOSpec(IOSpecType.Input, "ListOfDatasets", typeof(List<ICSM11DataSet>))]
    [IOSpec(IOSpecType.Input, "VSM_JS", typeof(double))]
    [IOSpec(IOSpecType.Input, "VSM_RTM", typeof(double))]
    [IOSpec(IOSpecType.Input, "JS_RTM", typeof(double))]
    [IOSpec(IOSpecType.Output, "ListOfDatasets", typeof(List<ICSM11DataSet>))]
    [Tag("SEMERU.Custom.ICSM'11")]
    public class CollectPCAResults : BaseComponent
    {
        public CollectPCAResults(ComponentLogger log) : base(log) { }

        public override void Compute()
        {
            List<ICSM11DataSet> list = (List<ICSM11DataSet>)Workspace.Load("ListOfDatasets");
            int current = (int)Workspace.Load("CurrentDataset") - 1;
            list[current].PCA.VSM_JS  = (double)Workspace.Load("VSM_JS");
            list[current].PCA.VSM_RTM = (double)Workspace.Load("VSM_RTM");
            list[current].PCA.JS_RTM  = (double)Workspace.Load("JS_RTM");
        }
    }
}