using System;
using System.Collections.Generic;
using SEMERU.Types.Custom;
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
    [Component(Name = "SEMERU - ICSM'11 - Dataset Boxplot Converter",
        Description = "Converts a dataset for use in the SEMERU BoxPlot GUI",
        Author = "SEMERU; Evan Moritz",
        Version = "1.0.0.0")]
    [IOSpec(IOSpecType.Input, "ListOfDatasets", typeof(List<ICSM11DataSet>))]
    [IOSpec(IOSpecType.Output, "ListOfDatasetResults", typeof(List<DatasetResults>))]
    [Tag("SEMERU.Custom.ICSM'11")]
    public class BoxplotConverter : BaseComponent
    {
        public BoxplotConverter(ComponentLogger log)
            : base(log)
        {

        }

        public override void Compute()
        {
            List<ICSM11DataSet> ListOfDatasets = (List<ICSM11DataSet>)Workspace.Load("ListOfDatasets");
            List<DatasetResults> ListOfResults = new List<DatasetResults>();
            string[] IRmodels = new string[] { "VSM", "JS", "RTM" };

            foreach (ICSM11DataSet dataset in ListOfDatasets)
            {
                // one dataset
                List<Metric> ds = new List<Metric>();

                // tabs
                List<SummaryData> ds_P = new List<SummaryData>();
                List<SummaryData> ds_R = new List<SummaryData>();
                List<SummaryData> ds_AP = new List<SummaryData>();

                // add data to tab
                // base IR
                foreach (string IRmodel in IRmodels)
                {
                    ds_P.Add(ExtractSummaryData(dataset, IRmodel, DataSetPairsType.Precision));
                    ds_R.Add(ExtractSummaryData(dataset, IRmodel, DataSetPairsType.Recall));
                    ds_AP.Add(ExtractSummaryData(dataset, IRmodel, DataSetPairsType.AveragePrecision));
                }
                // base combinations
                for (int i = 0; i < IRmodels.Length; i++)
                {
                    for (int j = i + 1; j < IRmodels.Length; j++)
                    {
                        ds_P.Add(ExtractSummaryData(dataset, IRmodels[i] + "+" + IRmodels[j], DataSetPairsType.Precision));
                        ds_R.Add(ExtractSummaryData(dataset, IRmodels[i] + "+" + IRmodels[j], DataSetPairsType.Recall));
                        ds_AP.Add(ExtractSummaryData(dataset, IRmodels[i] + "+" + IRmodels[j], DataSetPairsType.AveragePrecision));
                    }
                }
                // PCA combinations
                for (int i = 0; i < IRmodels.Length; i++)
                {
                    for (int j = i + 1; j < IRmodels.Length; j++)
                    {
                        ds_P.Add(ExtractSummaryData(dataset, IRmodels[i] + "+" + IRmodels[j] + "(PCA)", DataSetPairsType.Precision));
                        ds_R.Add(ExtractSummaryData(dataset, IRmodels[i] + "+" + IRmodels[j] + "(PCA)", DataSetPairsType.Recall));
                        ds_AP.Add(ExtractSummaryData(dataset, IRmodels[i] + "+" + IRmodels[j] + "(PCA)", DataSetPairsType.AveragePrecision));
                    }
                }

                // create Metric holders and add to dataset
                ds.Add(new PrecisionMetric(ds_P, 0.0, "no test"));
                ds.Add(new RecallMetric(ds_R, 0.0, "no test"));
                ds.Add(new AveragePrecisionMetric(ds_AP, 0.0, "no test"));

                // add dataset to structure
                ListOfResults.Add(new DatasetResults(dataset.Name, ds));
            }

            Workspace.Store("ListOfDatasetResults", ListOfResults);
        }

        private SummaryData ExtractSummaryData(ICSM11DataSet ds, string modelName, DataSetPairsType type)
        {
            DataSetPairs model = null;
            foreach (DataSetPairs dsp in ds.Metrics)
            {
                if (dsp.Name.Split(new char[] { '.' })[0].Equals(modelName))
                {
                    model = dsp;
                }
            }
            if (model == null)
                throw new ArgumentException("Model \"" + modelName + "\" not found.");
            switch (type)
            {
                case DataSetPairsType.Precision:
                    return CreateSummaryData(model.PrecisionData, modelName);
                case DataSetPairsType.Recall:
                    return CreateSummaryData(model.RecallData, modelName);
                case DataSetPairsType.AveragePrecision:
                    return CreateSummaryData(model.AveragePrecisionData, modelName);
                case DataSetPairsType.MeanAveragePrecision:
                    return CreateSummaryData(model.MeanAveragePrecisionData, modelName);
                default:
                    throw new ArgumentException("Unknown DataSetPairsType");
            }
        }

        private SummaryData CreateSummaryData(TLKeyValuePairsList results, string modelName)
        {
            List<double> data = new List<double>();
            foreach (KeyValuePair<string, double> kvp in results)
            {
                data.Add(kvp.Value);
            }
            return new SummaryData(modelName, data.ToArray());
        }
    }
}
