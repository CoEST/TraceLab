using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
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

namespace SEMERU.ICPC12.Importers
{
    class Similarities
    {
        public static TLSimilarityMatrix Import(String directory, List<String> map)
        {
            TLSimilarityMatrix sims = new TLSimilarityMatrix();

            foreach (String file in Directory.GetFiles(directory))
            {
                String feature = ExtractFeatureID(file);
                StreamReader idFile = new StreamReader(file);
                String line;

                while ((line = idFile.ReadLine()) != null)
                {
                    String[] vars = line.Split(' ');
                    sims.AddLink(feature, map[Convert.ToInt32(vars[0]) - 1], Convert.ToDouble(vars[2]));
                }
                idFile.Close();
            }

            return sims;
        }

        internal static String ExtractFeatureID(String file)
        {
            String[] parts = (new FileInfo(file).Name).Split('.');
            if (parts.Length == 1)
            {
                return file;
            }
            return Regex.Replace(parts[0], "[^0-9]", "");
        }
    }
}
