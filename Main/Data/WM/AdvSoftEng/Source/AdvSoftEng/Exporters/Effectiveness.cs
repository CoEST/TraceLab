using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceLabSDK.Types;
using System.IO;

namespace AdvSoftEng.Exporters
{
    public static class Effectiveness
    {
        public static void Export(TLArtifactsCollection queries, TLSimilarityMatrix sims, TLSimilarityMatrix gold, String allPath, String bestPath)
        {
            TextWriter all = new StreamWriter(allPath, false);
            TextWriter best = new StreamWriter(bestPath, false);
            TextWriter raw = new StreamWriter(allPath + ".csv", false);
            List<int> rawList = new List<int>();

            foreach (String feature in queries.Keys)
            {
                TLLinksList simList = sims.GetLinksAboveThresholdForSourceArtifact(feature);
                TLLinksList goldList = gold.GetLinksAboveThresholdForSourceArtifact(feature);
                simList.Sort();
                all.WriteLine(feature);
                best.WriteLine(feature);
                bool first = true;
                foreach (TLSingleLink link in goldList)
                {
                    KeyValuePair<int, TLSingleLink> recovered = FindLink(simList, link);
                    if (first)
                    {
                        best.WriteLine(recovered.Value.TargetArtifactId + "\t" + recovered.Key);
                        first = false;
                    }
                    all.WriteLine(recovered.Value.TargetArtifactId + "\t" + recovered.Key);
                    if (recovered.Key != -1)
                    {
                        rawList.Add(recovered.Key);
                    }
                }
            }
            raw.WriteLine(String.Join("\n", rawList));
            all.Flush();
            all.Close();
            best.Flush();
            best.Close();
            raw.Flush();
            raw.Close();
        }

        private static KeyValuePair<int, TLSingleLink> FindLink(TLLinksList list, TLSingleLink source)
        {
            int index;
            for (index = 0; index < list.Count; index++)
            {
                TLSingleLink link = list[index];
                if (link.SourceArtifactId == source.SourceArtifactId
                    && link.TargetArtifactId == source.TargetArtifactId)
                {
                    break;
                }
            }
            if (index == list.Count)
            {
                return new KeyValuePair<int, TLSingleLink>(-1, new TLSingleLink(source.SourceArtifactId, source.TargetArtifactId, -1));
            }
            else
            {
                return new KeyValuePair<int, TLSingleLink>(index, list[index]);
            }
        }
    }
}
