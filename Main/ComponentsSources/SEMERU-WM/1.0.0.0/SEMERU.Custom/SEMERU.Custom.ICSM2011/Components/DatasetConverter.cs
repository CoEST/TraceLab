using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceLabSDK;
using SEMERU.Types.Custom;
using SEMERU.Types.Dataset;

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
    [Component(Name = "SEMERU - ICSM'11 - Dataset Converter",
        Description = "Converts a list of datasets to the proper format for viewing in the GUI.",
        Author = "SEMERU; Evan Moritz",
        Version = "1.0.0.0")]
    [IOSpec(IOSpecType.Input, "ListOfDatasets", typeof(List<ICSM11DataSet>))]
    [IOSpec(IOSpecType.Output, "ConvertedListOfDatasets", typeof(List<DataSet>))]
    [Tag("SEMERU.Custom.ICSM'11")]
    public class DatasetConverter : BaseComponent
    {
        public DatasetConverter(ComponentLogger log) : base(log)
        {

        }

        public override void  Compute()
        {
            List<ICSM11DataSet> ds1 = (List<ICSM11DataSet>)Workspace.Load("ListOfDatasets");
            List<DataSet> ds2 = new List<DataSet>();
            foreach (ICSM11DataSet ds in ds1)
            {
                ds2.Add(ds);
            }
            Workspace.Store("ConvertedListOfDatasets", ds2);
        }
    }
}
