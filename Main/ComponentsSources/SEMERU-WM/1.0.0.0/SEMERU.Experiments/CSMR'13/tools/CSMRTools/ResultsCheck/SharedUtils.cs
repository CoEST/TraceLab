using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TraceLabSDK.Types;

namespace ResultsCheck
{
    public static class SharedUtils
    {
        /// <summary>
        /// Scrubs string of invalid characters.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string CleanFileName(string fileName)
        {
            string newfile = Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
            return Path.GetInvalidPathChars().Aggregate(newfile, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        public static int GetLinkPos(TLSimilarityMatrix sims, TLSingleLink link)
        {
            TLLinksList list = sims.AllLinks;
            list.Sort();
            int pos = 1;
            foreach (TLSingleLink query in list)
            {
                if (query.SourceArtifactId.Equals(link.SourceArtifactId) && query.TargetArtifactId.Equals(link.TargetArtifactId))
                    return pos;
                pos++;
            }
            return -1;
        }
    }
}
