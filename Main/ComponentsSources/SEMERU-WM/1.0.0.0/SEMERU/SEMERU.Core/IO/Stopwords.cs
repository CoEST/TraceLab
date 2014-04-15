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
    /// Taken from TraceLab's Components.Importer.StopWordsReader
    /// </summary>
    public static class Stopwords
    {
        public static TLStopwords Import(string filepath)
        {
            TLStopwords stopwords = new TLStopwords();
            TextReader reader = new StreamReader(filepath);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                stopwords.Add(line);
            }
            return stopwords;
        }
    }
}
