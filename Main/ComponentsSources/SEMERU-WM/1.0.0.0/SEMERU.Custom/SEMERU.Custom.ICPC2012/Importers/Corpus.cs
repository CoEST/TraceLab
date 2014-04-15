using System;
using System.Collections.Generic;
using System.IO;
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
    public static class Corpus
    {
        public static TLArtifactsCollection Import(String idPath, String docPath)
        {
            TLArtifactsCollection artifacts = new TLArtifactsCollection();

            StreamReader idFile = new StreamReader(idPath);
            StreamReader docFile = new StreamReader(docPath);

            String origid;
            String doc;

            while ((origid = idFile.ReadLine()) != null)
            {
                // read doc
                doc = docFile.ReadLine().Trim();

                // set vars
                String id = origid.Trim();
                int num = 0;

                while (artifacts.ContainsKey(id))
                {
                    num++;
                    id = origid.Trim() + "_" + num.ToString();
                }

                artifacts.Add(new TLArtifact(id, doc));
            }

            idFile.Close();
            return artifacts;
        }

        public static List<String> Map(String idPath)
        {
            List<String> map = new List<String>();
            StreamReader idFile = new StreamReader(idPath);
            String origid;

            while ((origid = idFile.ReadLine()) != null)
            {
                // set vars
                String id = origid.Trim();
                int num = 0;

                while (map.Contains(id))
                {
                    num++;
                    id = origid.Trim() + "_" + num.ToString();
                }

                map.Add(id);
            }

            return map;
        }
    }
}
