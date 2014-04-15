using System;
using System.Collections.Generic;
using System.IO;
using SEMERU.ICPC12.Tools;
using SEMERU.Types.Metrics;
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

namespace SEMERU.ICPC12.Exporters
{
    public static class Effectiveness
    {
        public static DatasetResults Calculate(ref TLSimilarityMatrix sims, ref TLSimilarityMatrix goldset, Dictionary<int, string> qmap, string ModelName)
        {
            TLKeyValuePairsList allall = new TLKeyValuePairsList();
            TLKeyValuePairsList allbest = new TLKeyValuePairsList();
            TLKeyValuePairsList bugall = new TLKeyValuePairsList();
            TLKeyValuePairsList bugbest = new TLKeyValuePairsList();
            TLKeyValuePairsList featall = new TLKeyValuePairsList();
            TLKeyValuePairsList featbest = new TLKeyValuePairsList();
            TLKeyValuePairsList patchall = new TLKeyValuePairsList();
            TLKeyValuePairsList patchbest = new TLKeyValuePairsList();

            sims.Threshold = Double.MinValue;

            foreach (KeyValuePair<int, string> qmapKVP in qmap)
            {
                TLLinksList simList = sims.GetLinksAboveThresholdForSourceArtifact(qmapKVP.Key.ToString());
                simList.Sort();

                bool best = false;
                for (int i = 0; i < simList.Count; i++)
                {
                    if (goldset.IsLinkAboveThreshold(simList[i].SourceArtifactId, simList[i].TargetArtifactId))
                    {
                        KeyValuePair<string, double> recovered = new KeyValuePair<string, double>(simList[i].SourceArtifactId + "_" + simList[i].TargetArtifactId, i);
                        allall.Add(recovered);
                        if (!best)
                        {
                            allbest.Add(recovered);
                            best = true;
                            if (qmapKVP.Value == Trace.GetFeatureSetType(FeatureSet.Bugs))
                            {
                                bugbest.Add(recovered);
                            }
                            else if (qmapKVP.Value == Trace.GetFeatureSetType(FeatureSet.Features))
                            {
                                featbest.Add(recovered);
                            }
                            else if (qmapKVP.Value == Trace.GetFeatureSetType(FeatureSet.Patch))
                            {
                                patchbest.Add(recovered);
                            }
                        }
                        if (qmapKVP.Value == Trace.GetFeatureSetType(FeatureSet.Bugs))
                        {
                            bugall.Add(recovered);
                        }
                        else if (qmapKVP.Value == Trace.GetFeatureSetType(FeatureSet.Features))
                        {
                            featall.Add(recovered);
                        }
                        else if (qmapKVP.Value == Trace.GetFeatureSetType(FeatureSet.Patch))
                        {
                            patchall.Add(recovered);
                        }
                    }
                }
            }

            List<SummaryData> alldata = new List<SummaryData>();
            alldata.Add(CreateSummaryData(allall, "All (all)"));
            alldata.Add(CreateSummaryData(bugall, "Bugs (all)"));
            alldata.Add(CreateSummaryData(featall, "Features (all)"));
            alldata.Add(CreateSummaryData(patchall, "Patches (all)"));

            List<SummaryData> bestdata = new List<SummaryData>();
            bestdata.Add(CreateSummaryData(allbest, "All (best)"));
            bestdata.Add(CreateSummaryData(bugbest, "Bugs (best)"));
            bestdata.Add(CreateSummaryData(featbest, "Features (best)"));
            bestdata.Add(CreateSummaryData(patchbest, "Patches (best)"));

            List<Metric> data = new List<Metric>();
            data.Add(new EffectivenessMetric(alldata, 0.0, "none", ModelName + " all"));
            data.Add(new EffectivenessMetric(bestdata, 0.0, "none", ModelName + " best"));

            return new DatasetResults("", data);
        }

        private static SummaryData CreateSummaryData(TLKeyValuePairsList results, string modelName)
        {
            List<double> data = new List<double>();
            foreach (KeyValuePair<string, double> kvp in results)
            {
                data.Add(kvp.Value);
            }
            return new SummaryData(modelName, data.ToArray());
        }

        #region Export (0.0.1.0)

        public static void Export(ref TLSimilarityMatrix sims, ref TLSimilarityMatrix goldset, Dictionary<int, string> qmap, string dir, string prefix)
        {
            TextWriter allall = new StreamWriter(dir + prefix + ".all.allmeasures", false);
            TextWriter allbest = new StreamWriter(dir + prefix + ".all.bestmeasures", false);
            TextWriter bugall = new StreamWriter(dir + prefix + ".bugs.allmeasures", false);
            TextWriter bugbest = new StreamWriter(dir + prefix + ".bugs.bestmeasures", false);
            TextWriter featall = new StreamWriter(dir + prefix + ".features.allmeasures", false);
            TextWriter featbest = new StreamWriter(dir + prefix + ".features.bestmeasures", false);
            TextWriter patchall = new StreamWriter(dir + prefix + ".patch.allmeasures", false);
            TextWriter patchbest = new StreamWriter(dir + prefix + ".patch.bestmeasures", false);

            sims.Threshold = Double.MinValue;

            foreach (KeyValuePair<int, string> qmapKVP in qmap)
            {
                TLLinksList simList = sims.GetLinksAboveThresholdForSourceArtifact(qmapKVP.Key.ToString());
                TLLinksList goldList = goldset.GetLinksAboveThresholdForSourceArtifact(qmapKVP.Key.ToString());
                simList.Sort();
                allall.WriteLine(qmapKVP.Key.ToString());
                allbest.WriteLine(qmapKVP.Key.ToString());

                if (qmapKVP.Value == Trace.GetFeatureSetType(FeatureSet.Bugs))
                {
                    bugall.WriteLine(qmapKVP.Key.ToString());
                    bugbest.WriteLine(qmapKVP.Key.ToString());
                }
                else if (qmapKVP.Value == Trace.GetFeatureSetType(FeatureSet.Features))
                {
                    featall.WriteLine(qmapKVP.Key.ToString());
                    featbest.WriteLine(qmapKVP.Key.ToString());
                }
                else if (qmapKVP.Value == Trace.GetFeatureSetType(FeatureSet.Patch))
                {
                    patchall.WriteLine(qmapKVP.Key.ToString());
                    patchbest.WriteLine(qmapKVP.Key.ToString());
                }

                KeyValuePair<int, TLSingleLink> best = new KeyValuePair<int,TLSingleLink>(Int32.MaxValue, new TLSingleLink("null", "null", 0));
                foreach (TLSingleLink link in goldList)
                {
                    KeyValuePair<int, TLSingleLink> recovered = FindLink(simList, link);

                    if (recovered.Key != -1 && recovered.Key < best.Key)
                    {
                        best = recovered;
                    }
                    allall.WriteLine(recovered.Value.TargetArtifactId + "\t" + recovered.Key);
                    if (qmapKVP.Value == Trace.GetFeatureSetType(FeatureSet.Bugs))
                    {
                        bugall.WriteLine(recovered.Value.TargetArtifactId + "\t" + recovered.Key);
                    }
                    else if (qmapKVP.Value == Trace.GetFeatureSetType(FeatureSet.Features))
                    {
                        featall.WriteLine(recovered.Value.TargetArtifactId + "\t" + recovered.Key);
                    }
                    else if (qmapKVP.Value == Trace.GetFeatureSetType(FeatureSet.Patch))
                    {
                        patchall.WriteLine(recovered.Value.TargetArtifactId + "\t" + recovered.Key);
                    }
                }
                allbest.WriteLine(best.Value.TargetArtifactId + "\t" + best.Key);
                if (qmapKVP.Value == Trace.GetFeatureSetType(FeatureSet.Bugs))
                {
                    bugbest.WriteLine(best.Value.TargetArtifactId + "\t" + best.Key);
                }
                else if (qmapKVP.Value == Trace.GetFeatureSetType(FeatureSet.Features))
                {
                    featbest.WriteLine(best.Value.TargetArtifactId + "\t" + best.Key);
                }
                else if (qmapKVP.Value == Trace.GetFeatureSetType(FeatureSet.Patch))
                {
                    patchbest.WriteLine(best.Value.TargetArtifactId + "\t" + best.Key);
                }
            }
            allall.Flush();
            allall.Close();
            allbest.Flush();
            allbest.Close();
            bugall.Flush();
            bugall.Close();
            bugbest.Flush();
            bugbest.Close();
            featall.Flush();
            featall.Close();
            featbest.Flush();
            featbest.Close();
            patchall.Flush();
            patchall.Close();
            patchbest.Flush();
            patchbest.Close();
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

        #endregion
    }
}
