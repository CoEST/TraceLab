using System.Collections.Generic;
using System.IO;
using Rscript;
using SEMERU.Core.IO;
using SEMERU.Core.Metrics;
using SEMERU.Types.Dataset;
using TraceLabSDK.Types;

namespace ResultsCheck
{
    public static class ConvertRoccoResults
    {
        public static void Run(ref Info info)
        {
            DirectoryInfo directory = Directory.CreateDirectory(info.OutputDirectory + @"\RoccoResults");
            string resultsBase = @"C:\Temp\CSMR'13\results";
            // EasyClinic UC->CC
            TLSimilarityMatrix oracle = Oracle.Import(directory.FullName + @"\EasyClinic UC-CC.oracle");
            // EasyClinic UC->CC VSM
            TLSimilarityMatrix vsm = ConvertRocco(ref info, resultsBase + @"\rlist.csv", directory.FullName + @"\EasyClinic UC-CC.VSM.sims");
            MyMetrics(ref info, directory.FullName + @"\EasyClinic UC-CC.VSM.MYmetrics", vsm, oracle);
            RoccoMetrics(ref info, directory.FullName + @"\EasyClinic UC-CC.VSM.metrics", resultsBase + @"\rlist.csv");
            // EasyClinic UC->CC VSM + UD-CSTI
            TLSimilarityMatrix vsmudcsti = ConvertRocco(ref info, resultsBase + @"\frlist.csv", directory.FullName + @"\EasyClinic UC-CC.VSM_UDCSTI.sims");
            MyMetrics( ref info,  directory.FullName + @"\EasyClinic UC-CC.VSM_UDCSTI.MYmetrics", vsmudcsti, oracle);
            RoccoMetrics(ref info, directory.FullName + @"\EasyClinic UC-CC.VSM_UDCSTI.metrics", resultsBase + @"\frlist.csv");

        }

        private static void MyMetrics(ref Info info, string output, TLSimilarityMatrix matrix, TLSimilarityMatrix oracle)
        {
            DataSetPairsCollection metrics = OverallMetricsComputation.ComputeAll(matrix, oracle);
            TextWriter dataFile = File.CreateText(output);
            for (int i = 0, j = 10; i < metrics.Count; i++, j += 10)
            {

                dataFile.WriteLine("{0} {1}", j, metrics[i].PrecisionData[0].Value);
            }
            dataFile.Flush();
            dataFile.Close();
        }

        private static void RoccoMetrics(ref Info info, string output, string input)
        {
            List<KeyValuePair<double, double>> metrics = RanklistImporter.ImportMetrics(input);
            TextWriter dataFile = File.CreateText(output);
            foreach (KeyValuePair<double, double> kvp in metrics)
            {

                dataFile.WriteLine("{0} {1}", kvp.Key, kvp.Value);
            }
            dataFile.Flush();
            dataFile.Close();
        }

        private static TLSimilarityMatrix ConvertRocco(ref Info info, string input, string output)
        {
            TLSimilarityMatrix matrix = RanklistImporter.Import(input);
            Similarities.Export(matrix, output);
            return matrix;
        }
    }
}
