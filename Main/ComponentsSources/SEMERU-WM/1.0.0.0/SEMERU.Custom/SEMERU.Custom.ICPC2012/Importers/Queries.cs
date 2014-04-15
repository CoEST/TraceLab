using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TraceLabSDK.Types;
using System.Text.RegularExpressions;

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

namespace SEMERU.ICPC12.Importers
{
    public static class Queries
    {
        public static Dictionary<int, string> CreateMap(string bugs, string features, string patches)
        {
            Dictionary<int, string> map = new Dictionary<int, string>();
            Map(ref map, "bug", bugs);
            Map(ref map, "feature", features);
            Map(ref map, "patch", patches);
            return map;
        }

        public static void Map(ref Dictionary<int, string> map, string label, string IDFile)
        {
            StreamReader file = new StreamReader(IDFile);
            string idstring;
            while ((idstring = file.ReadLine()) != null)
            {
                int id = Convert.ToInt32(idstring);
                map.Add(id, label);
            }
        }

        public static TLArtifactsCollection Import(string directory)
        {
            TLArtifactsCollection artifacts = new TLArtifactsCollection();
            char[] split = new char[] { '\\' };
            foreach (string filename in Directory.EnumerateFiles(directory))
            {
                string id = Regex.Replace(filename.Split(split, StringSplitOptions.RemoveEmptyEntries).Last(), "[^0-9]", "");
                StreamReader file = new StreamReader(filename);
                string text = file.ReadToEnd();
                if (artifacts.ContainsKey(id))
                {
                    artifacts[id].Text += " " + text;
                }
                else
                {
                    artifacts.Add(new TLArtifact(id, text));
                }
                file.Close();
            }
            return artifacts;
        }
    }
}
