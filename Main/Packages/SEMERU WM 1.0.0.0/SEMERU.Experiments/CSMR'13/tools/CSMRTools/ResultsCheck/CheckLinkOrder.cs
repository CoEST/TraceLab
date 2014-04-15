using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SEMERU.Types.Custom;
using TraceLabSDK.Types;
using SEMERU.Core.IO;
using System.IO;

namespace ResultsCheck
{
    public static class CheckLinkOrder
    {
        public static void Run(ref Info info, CSMR13DataSet dataset)
        {
            Console.WriteLine("Running CheckLinkOrder on {0}", dataset.Name);
            Directory.CreateDirectory(info.OutputDirectory + @"\CheckLinkOrder");
            TLSimilarityMatrix oracle = Oracle.Import(dataset.Oracle);
            foreach (string IRModel in info.IRModels)
            {
                WriteSims(ref info, dataset, oracle, IRModel);
                foreach (string StructuralModel in info.StructuralModels)
                {
                    WriteSims(ref info, dataset, oracle, IRModel + "_" + StructuralModel);
                }
            }
        }

        private static void WriteSims(ref Info info, CSMR13DataSet dataset, TLSimilarityMatrix oracle, string model)
        {
            TextWriter Output = File.CreateText(info.OutputDirectory + @"\CheckLinkOrder\" + SharedUtils.CleanFileName(dataset.Name) + "." + model + ".txt");
            TLSimilarityMatrix sims = Similarities.Import(info.ResultsDirectory.FullName + @"\" + SharedUtils.CleanFileName(dataset.Name) + @"\sims\" + model + ".sims");
            TLLinksList simList = sims.AllLinks;
            simList.Sort();
            int pos = 1;
            foreach (TLSingleLink link in simList)
            {
                if (oracle.IsLinkAboveThreshold(link.SourceArtifactId, link.TargetArtifactId))
                {
                    Output.WriteLine("[{0}]\t{1}\t{2}\t{3}", pos, link.SourceArtifactId, link.TargetArtifactId, link.Score);
                }
                pos++;
            }
            Output.Flush();
            Output.Close();
        }
    }
}
