using System;
using System.IO;
using TraceLabSDK.Types;

/// SEMERU Component Library - TraceLab Component Plugin
/// Copyright © 2012-2013 SEMERU
/// 
/// This file is part of the SEMERU Component Library.
/// 
/// The SEMERU Component Library is free software: you can redistribute it and/or
/// modify it under the terms of the GNU General Public License as published by the
/// Free Software Foundation, either version 3 of the License, or (at your option)
/// any later version.
/// 
/// The SEMERU Component Library is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
/// or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for
/// more details.
/// 
/// You should have received a copy of the GNU General Public License along with the
/// SEMERU Component Library.  If not, see <http://www.gnu.org/licenses/>.

namespace SEMERU.Core.IO
{
    /// <summary>
    /// Responsible for importing answer sets
    /// </summary>
    public static class Oracle
    {
        /// <summary>
        /// Imports an answer set from file in the form (each line):
        /// SOURCE TARGET1 TARGET2 ...
        /// </summary>
        /// <param name="filename">File location</param>
        /// <returns>Similarity matrix (link score 1)</returns>
        public static TLSimilarityMatrix Import(String filename)
        {
            StreamReader file = new StreamReader(filename);
            TLSimilarityMatrix answer = new TLSimilarityMatrix();
            String line;
            while ((line = file.ReadLine()) != null)
            {
                String[] artifacts = line.Split();
                String source = artifacts[0];
                for (int i = 1; i < artifacts.Length; i++)
                {
                    String target = artifacts[i].Trim();
                    if (target != "")
                    {
                        answer.AddLink(source, target, 1);
                    }
                }
            }
            return answer;
        }
    }
}
