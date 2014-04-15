using System.Collections.Generic;
using SEMERU.Custom.ICSM2011.Importers;
using SEMERU.Types.Dataset;
using TraceLabSDK;
using TraceLabSDK.Component.Config;
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
    [Component(Name = "SEMERU - ICSM'11 - Experimental Setup",
               Description = "Imports experiment settings from an XML file.",
               Author = "SEMERU; Evan Moritz",
               Version = "1.0.0.0",
               ConfigurationType=typeof(ExperimentalSetupConfig))]
    [IOSpec(IOSpecType.Output, "NumberOfDatasets", typeof(int))]
    [IOSpec(IOSpecType.Output, "CurrentDataset", typeof(int))]
    [IOSpec(IOSpecType.Output, "ListOfDatasets", typeof(List<ICSM11DataSet>))]
    [IOSpec(IOSpecType.Output, "PCASkip", typeof(bool))]
    [IOSpec(IOSpecType.Output, "RTMSkip", typeof(bool))]
    [Tag("SEMERU.Custom.ICSM'11")]
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
            List<ICSM11DataSet> datasets = Setup.ImportDatasets(_config.SettingsFile);
            Workspace.Store("NumberOfDatasets", datasets.Count);
            Workspace.Store("CurrentDataset", 1);
            Workspace.Store("ListOfDatasets", datasets);
            if (_config.SkipPCA == PCASkip.Yes)
                Workspace.Store("PCASkip", true);
            else
                Workspace.Store("PCASkip", false);
            if (_config.SkipRTM == RTMSkip.Yes)
                Workspace.Store("RTMSkip", true);
            else
                Workspace.Store("RTMSkip", false);
        }
    }

    public class ExperimentalSetupConfig
    {
        private FilePath _file;
        public FilePath SettingsFile
        {
            get
            {
                return _file;
            }
            set
            {
                _file = value;
            }
        }

        public PCASkip SkipPCA { get; set; }
        public RTMSkip SkipRTM { get; set; }
    }

    public enum PCASkip
    {
        Yes,
        No,
    }

    public enum RTMSkip
    {
        Yes,
        No,
    }
}