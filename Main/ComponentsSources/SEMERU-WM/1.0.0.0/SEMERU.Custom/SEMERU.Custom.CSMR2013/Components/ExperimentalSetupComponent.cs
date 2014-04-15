using System.Collections.Generic;
using System.ComponentModel;
using SEMERU.Custom.CSMR2012.Models;
using SEMERU.Types.Custom;
using TraceLabSDK;
using TraceLabSDK.Component.Config;

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

namespace SEMERU.Custom.CSMR2012.Components
{
    [Component(Name = "SEMERU - CSMR'13 - Experimental Setup",
        Description = "Experimental setup loader.",
        Author = "SEMERU; Evan Moritz",
        Version = "1.0.0.0",
        ConfigurationType = typeof(ExperimentalSetupConfig))]
    [IOSpec(IOSpecType.Output, "ListOfDatasets", typeof(List<CSMR13DataSet>))]
    [IOSpec(IOSpecType.Output, "CurrentDataset", typeof(int))]
    [IOSpec(IOSpecType.Output, "NumberOfDatasets", typeof(int))]
    [Tag("SEMERU.Custom.CSMR'13")]
    public class ExperimentalSetupComponent : BaseComponent
    {
        private ExperimentalSetupConfig _config;

        public ExperimentalSetupComponent(ComponentLogger log)
            : base(log)
        {
            _config = new ExperimentalSetupConfig();
            Configuration = _config;
        }

        public ExperimentalSetupComponent(ComponentLogger log, ExperimentalSetupConfig config)
            : base(log)
        {
            _config = config;
            Configuration = _config;
        }

        public override void Compute()
        {
            List<CSMR13DataSet> datasets = ExperimentalSetup.Import(_config.SetupFile.Absolute);
            Workspace.Store("ListOfDatasets", datasets);
            Workspace.Store("CurrentDataset", 0);
            Workspace.Store("NumberOfDatasets", datasets.Count);
        }
    }

    public class ExperimentalSetupConfig
    {
        [DisplayName("Setup file")]
        [Description("Experiment XML setup file")]
        public FilePath SetupFile { get; set; }
    }
}
