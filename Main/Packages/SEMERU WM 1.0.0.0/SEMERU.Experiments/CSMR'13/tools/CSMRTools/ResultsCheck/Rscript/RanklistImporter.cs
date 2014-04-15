using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceLabSDK.Types;
using System.IO;

namespace Rscript
{
	public static class RanklistImporter
	{
        /// <summary>
        /// FORMAT
        /// ======
        /// Line 1  - "","UC","CC","Similarity","Oracle","Precision","Recall","feedback"
        /// Line 2+ - values
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static TLSimilarityMatrix Import(string path)
        {
            TLSimilarityMatrix matrix = new TLSimilarityMatrix();
            matrix.Threshold = Double.MinValue;
            TextReader file = new StreamReader(path);
            file.ReadLine();
            string line;
            while ((line = file.ReadLine()) != null)
            {
                string[] item = line.Split(new char[] { ',', '"' }, StringSplitOptions.RemoveEmptyEntries);
                matrix.AddLink(item[1], item[2], Convert.ToDouble(item[3]));
            }
            return matrix;
        }

        public static List<KeyValuePair<double, double>> ImportMetrics(string path)
        {
            List<KeyValuePair<double, double>> metrics = new List<KeyValuePair<double, double>>();
            TextReader file = new StreamReader(path);
            file.ReadLine();
            string line;
            while ((line = file.ReadLine()) != null)
            {
                string[] item = line.Split(new char[] { ',', '"' }, StringSplitOptions.RemoveEmptyEntries);
                metrics.Add(new KeyValuePair<double, double>(Convert.ToDouble(item[6]), Convert.ToDouble(item[5])));
            }
            return metrics;
        }
	}
}
