using System.IO;
using SEMERU.Core.IO;
using SEMERU.Custom.CSMR2012.Models;
using SEMERU.Types.Dataset;
using TraceLabSDK.Types;
using SEMERU.Core.Metrics;

namespace ResultsCheck
{
    public static class RunModelsWithRoccoResults
    {
        public static void Run(ref Info info)
        {
            TLSimilarityMatrix vsm = Similarities.Import(info.ResultsDirectory + @"\CSMRTools\RoccoResults\EasyClinic UC-CC.VSM.sims");
            TLSimilarityMatrix oracle = Oracle.Import(info.ResultsDirectory + @"\CSMRTools\RoccoResults\EasyClinic UC-CC.oracle");
            TLSimilarityMatrix usage = Oracle.Import(info.ResultsDirectory + @"\CSMRTools\RoccoResults\EasyClinic UC-CC.relationships");
            TLSimilarityMatrix udcsti = UDCSTI.Compute(vsm, usage, oracle);
            Similarities.Export(udcsti, info.ResultsDirectory + @"\CSMRTools\RoccoResults\EasyClinic UC-CC.VSM_UDCSTI.MYsims");
            DataSetPairsCollection metrics = OverallMetricsComputation.ComputeAll(udcsti, oracle);
            TextWriter dataFile = File.CreateText(info.ResultsDirectory + @"\CSMRTools\RoccoResults\EasyClinic UC-CC.VSM_UDCSTI.MYsims.metrics");
            for (int i = 0, j = 10; i < metrics.Count; i++, j += 10)
            {

                dataFile.WriteLine("{0} {1}", j, metrics[i].PrecisionData[0].Value);
            }
            dataFile.Flush();
            dataFile.Close();
        }
    }
}
