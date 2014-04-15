using System;
using System.Collections.Generic;
using SEMERU.Types.Dataset;
using SEMERU.Types.Metrics;
using TraceLabSDK;
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

namespace SEMERU.Custom.ICSM2011.Components
{
    [Component(Name = "SEMERU - ICSM'11 - DataSetPairsCollection Converter",
               Description = "Converts a DataSetPairsCollection into an object for input to the BoxPlotGUI",
               Author = "SEMERU; Evan Moritz",
               Version = "1.0.0.0")]
    [IOSpec(IOSpecType.Input, "DataSetPairsCollection", typeof(DataSetPairsCollection))]
    [IOSpec(IOSpecType.Output, "ListOfDatasetResults", typeof(List<DatasetResults>))]
    [Tag("SEMERU.Custom.ICSM'11")]
    public class DataSetPairsConverter : BaseComponent
    {
        public DataSetPairsConverter(ComponentLogger log) : base(log) { }

        public override void Compute()
        {
            DataSetPairsCollection dspc = (DataSetPairsCollection)Workspace.Load("DataSetPairsCollection");
            List<DatasetResults> listOfDatasetResults = new List<DatasetResults>();

            List<Metric> ds1 = new List<Metric>();

            // tab 1 - base precision
            List<SummaryData> ds1_tab1 = new List<SummaryData>();
            ds1_tab1.Add(new SummaryData("VSM", ExtractData("VSM", DataSetPairsType.AveragePrecision, dspc)));
            ds1_tab1.Add(new SummaryData("JS", ExtractData("JS", DataSetPairsType.AveragePrecision, dspc)));
            ds1_tab1.Add(new SummaryData("RTM", ExtractData("RTM", DataSetPairsType.AveragePrecision, dspc)));
            ds1.Add(new AveragePrecisionMetric(ds1_tab1, 0.0, ""));//, "Base - Precision"));

            // tab 2 - combined precision
            List<SummaryData> ds1_tab2 = new List<SummaryData>();
            ds1_tab2.Add(new SummaryData("VSM_JS", ExtractData("VSM_JS", DataSetPairsType.AveragePrecision, dspc)));
            ds1_tab2.Add(new SummaryData("VSM_RTM", ExtractData("VSM_RTM", DataSetPairsType.AveragePrecision, dspc)));
            ds1_tab2.Add(new SummaryData("JS_RTM", ExtractData("JS_RTM", DataSetPairsType.AveragePrecision, dspc)));
            ds1.Add(new AveragePrecisionMetric(ds1_tab2, 0.0, ""));//, "Combined - Precision"));

            // tab 3 - combined PCA precision
            List<SummaryData> ds1_tab3 = new List<SummaryData>();
            ds1_tab3.Add(new SummaryData("VSM_JS_PCA", ExtractData("VSM_JS_PCA", DataSetPairsType.AveragePrecision, dspc)));
            ds1_tab3.Add(new SummaryData("VSM_RTM_PCA", ExtractData("VSM_RTM_PCA", DataSetPairsType.AveragePrecision, dspc)));
            ds1_tab3.Add(new SummaryData("JS_RTM_PCA", ExtractData("JS_RTM_PCA", DataSetPairsType.AveragePrecision, dspc)));
            ds1.Add(new AveragePrecisionMetric(ds1_tab3, 0.0, ""));//, "Combined (PCA) - Precision"));

            listOfDatasetResults.Add(new DatasetResults("Dataset", ds1));
            Workspace.Store("ListOfDatasetResults", listOfDatasetResults);
        }

        private double[] ExtractData(string name, DataSetPairsType type, DataSetPairsCollection dspc)
        {
            foreach (DataSetPairs pairs in dspc)
            {
                if (pairs.Name == name)
                {
                    if (pairs.Name == "VSM")
                    {
                        pairs.Name = "VSM";
                    }
                    switch (type)
                    {
                        case DataSetPairsType.Precision:
                            return ExtractDataArray(pairs.PrecisionData);
                        case DataSetPairsType.Recall:
                            return ExtractDataArray(pairs.RecallData);
                        case DataSetPairsType.AveragePrecision:
                            return ExtractDataArray(pairs.AveragePrecisionData);
                        default:
                            throw new ArgumentException("Unknown DataSetPairsType: " + type);
                    }
                }
            }

            throw new ArgumentException("Model not found - " + name);
        }

        private double[] ExtractDataArray(TLKeyValuePairsList pairs)
        {
            List<double> array = new List<double>();
            foreach (KeyValuePair<string, double> pair in pairs)
            {
                if (!Double.IsNaN(pair.Value))
                {
                    array.Add(pair.Value);
                }
            }
            return array.ToArray();
        }
    }
}