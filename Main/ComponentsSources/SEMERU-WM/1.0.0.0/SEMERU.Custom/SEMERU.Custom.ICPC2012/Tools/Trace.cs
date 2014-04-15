using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using SEMERU.ICPC12.Importers;
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

namespace SEMERU.ICPC12.Tools
{
    public enum FeatureSet
    {
        All,
        Bugs,
        Features,
        Patch,
    };

    public static class Trace
    {
        public static TLSimilarityMatrix Extract(ref TLSimilarityMatrix sims, string tracedir, FeatureSet set, Dictionary<int, string> qmap)
        {
            TLSimilarityMatrix newsims = new TLSimilarityMatrix();

            if (set == FeatureSet.All)
            {
                ExtractFeature(ref sims, ref newsims, tracedir + @"\" + GetFeatureSetDir(FeatureSet.Bugs) + "\\");
                ExtractFeature(ref sims, ref newsims, tracedir + @"\" + GetFeatureSetDir(FeatureSet.Features) + "\\");
                ExtractFeature(ref sims, ref newsims, tracedir + @"\" + GetFeatureSetDir(FeatureSet.Patch) + "\\");
            }
            else
            {
                RemoveNonFeature(ref sims, set, qmap);
                ExtractFeature(ref sims, ref newsims, tracedir + GetFeatureSetDir(set) + "\\");
            }

            return newsims;
        }

        private static void RemoveNonFeature(ref TLSimilarityMatrix sims, FeatureSet set, Dictionary<int, string> qmap)
        {
            TLSimilarityMatrix target = new TLSimilarityMatrix();
            string feature = GetFeatureSetType(set);
            foreach (TLSingleLink link in sims.AllLinks)
            {
                if (qmap[Convert.ToInt32(link.SourceArtifactId)] == feature)
                {
                    target.AddLink(link.SourceArtifactId, link.TargetArtifactId, link.Score);
                }
            }
            sims = target;
        }

        public static void ExtractFeature(ref TLSimilarityMatrix sims, ref TLSimilarityMatrix newsims, string tracedir)
        {
            foreach (String file in Directory.GetFiles(tracedir))
            {
                String feature = Similarities.ExtractFeatureID(file);
                Dictionary<string, int> trace = Lookup(feature, tracedir);
                RemoveNonExecutedMethods(ref sims, ref newsims, feature, trace);
            }
        }

        public static Dictionary<string, int> Lookup(String featureID, String directory)
        {
            Dictionary<string, int> trace = new Dictionary<string, int>();
            StreamReader links = new StreamReader(directory + "trace" + featureID + ".log");
            String link;

            // skip 1st lines
            links.ReadLine();

            while ((link = links.ReadLine()) != null)
            {
                String[] line = link.Split('\t');
                if (line.Length == 1)
                    continue;

                String[] methods = line[1].Split(new string[] { "  --  " }, StringSplitOptions.None);
                if (methods.Length == 1)
                    continue;

                String method;

                if (methods[1].IndexOf('$') != -1)
                {
                    methods[1] = methods[1].Replace('$', '.');
                }

                if (methods[0] == "<clinit>" || methods[0] == "<init>")
                {
                    method = GetConstructor(methods[1]);
                }
                else
                {
                    method = methods[1] + "." + methods[0];
                }

                if (trace.ContainsKey(method))
                {
                    trace[method]++;
                }
                else
                {
                    trace.Add(method, 1);
                }
            }

            return trace;
        }

        /* DO NOT TRY TO USE THIS ON 6GB OF TRACES!!!
        public static Dictionary<String, Dictionary<string, int>> Import(String directory)
        {
            Dictionary<String, Dictionary<string, int>> collection = new Dictionary<string, Dictionary<string, int>>();

            foreach (String file in Directory.GetFiles(directory))
            {
                String feature = CustomData.ExtractFeatureID(file);
                Console.WriteLine(DateTime.Now + " ExecutionTrace: Reading " + feature + " ...");
                Dictionary<string, int> trace = Lookup(feature, directory);
                collection.Add(feature, trace);
            }

            return collection;
        }
        */

        private static string GetConstructor(string p)
        {
            String[] parts = p.Split('.');
            return p + "." + parts[parts.Length - 1];
        }

        // this takes a long time
        public static void RemoveNonExecutedMethods(ref TLSimilarityMatrix sourceMatrix, ref TLSimilarityMatrix targetMatrix, String feature, Dictionary<string, int> executedMethods)
        {
            foreach (TLSingleLink link in sourceMatrix.AllLinks)
            {
                if (link.SourceArtifactId == feature && executedMethods.ContainsKey(Regex.Replace(link.TargetArtifactId, "(\\(.*\\))", "")))
                {
                    targetMatrix.AddLink(link.SourceArtifactId, link.TargetArtifactId, link.Score);
                }
            }
        }

        internal static string GetFeatureSetType(FeatureSet set)
        {
            switch (set)
            {
                case FeatureSet.Bugs:
                    return "bug";
                case FeatureSet.Features:
                    return "feature";
                case FeatureSet.Patch:
                    return "patch";
                default:
                    return "all";
            }
        }

        internal static string GetFeatureSetDir(FeatureSet set)
        {
            switch (set)
            {
                case FeatureSet.Bugs:
                    return "Bugs";
                case FeatureSet.Features:
                    return "Features";
                case FeatureSet.Patch:
                    return "Patch";
                default:
                    return "All";
            }
        }
    }
}
