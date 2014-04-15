using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SEMERU.Core.IO;
using SEMERU.Types.Custom;
using TraceLabSDK;
using TraceLabSDK.Component.Config;
using TraceLabSDK.Types;

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
    [Component(Name = "SEMERU - CSMR'13 - Dataset results exporter",
        Description = "Exports results to disk.",
        Author = "SEMERU; Evan Moritz",
        Version = "1.0.0.0",
        ConfigurationType = typeof(DataSetResultsExporterConfig))]
    [IOSpec(IOSpecType.Input, "ListOfDatasets", typeof(List<CSMR13DataSet>))]
    [Tag("SEMERU.Custom.CSMR'13")]
    public class DataSetResultsExporter : BaseComponent
    {
        private DataSetResultsExporterConfig _config;

        public DataSetResultsExporter(ComponentLogger log)
            : base(log)
        {
            _config = new DataSetResultsExporterConfig();
            Configuration = _config;
        }

        public DataSetResultsExporter(ComponentLogger log, DataSetResultsExporterConfig config)
            : base(log)
        {
            _config = config;
            Configuration = _config;
        }

        public override void Compute()
        {
            List<CSMR13DataSet> lds = (List<CSMR13DataSet>)Workspace.Load("ListOfDatasets");
            foreach (CSMR13DataSet ds in lds)
            {
                DirectoryInfo directory = Directory.CreateDirectory(_config.ResultsDirectory.Absolute + @"\" + CleanFileName(ds.Name));
                DirectoryInfo metricsDir = Directory.CreateDirectory(directory.FullName + @"\metrics");
                TextWriter infoFile = File.CreateText(directory.FullName + @"\info.txt");
                infoFile.Write(ds.ToOutputString());
                infoFile.Flush();
                infoFile.Close();
                TextWriter dataFile = Console.Out;
                for (int i = 0, j = 10; i < ds.Metrics.Count; i++, j += 10)
                {
                    if (j == 10)
                    {
                        dataFile = File.CreateText(metricsDir.FullName + @"\" + CleanFileName(ds.Metrics[i].Name.Replace(" @R10", "").Replace(' ', '_')));
                    }
                    dataFile.WriteLine("{0} {1}", j, ds.Metrics[i].PrecisionData[0].Value);
                    if (j == 100)
                    {
                        dataFile.Flush();
                        dataFile.Close();
                        j = 0;
                    }
                }
                DirectoryInfo simsDir = Directory.CreateDirectory(directory.FullName + @"\sims");
                foreach (TLSimilarityMatrix matrix in ds.Similarities)
                {
                    Similarities.Export(matrix, simsDir.FullName + @"\" + matrix.Name + ".sims");
                }
            }
        }

        private string CleanFileName(string fileName)
        {
            string newfile = Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
            return Path.GetInvalidPathChars().Aggregate(newfile, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

    }

    public class DataSetResultsExporterConfig
    {
        public DirectoryPath ResultsDirectory { get; set; }
    }
}
