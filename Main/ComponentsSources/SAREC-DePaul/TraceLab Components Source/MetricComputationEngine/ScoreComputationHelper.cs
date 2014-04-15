// TraceLab - Software Traceability Instrument to Facilitate and Empower Traceability Research
// Copyright (C) 2012-2013 CoEST - National Science Foundation MRI-R2 Grant # CNS: 0959924
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see<http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceLabSDK.Types.Contests;
using TraceLabSDK.Types;

namespace MetricComputationEngine
{
    class ScoreComputationHelper
    {
        /// <summary>
        /// Computes delta values (the differences) between values in one dictionary and another dictionary. 
        /// The keys in both dictionary has to match. 
        /// </summary>
        /// <param name="valuesOne">The values one.</param>
        /// <param name="valuesTwo">The values two.</param>
        /// <exception cref="ArgumentException">If count of values in two given dictionaries do not match</exception>
        /// <exception cref="ArgumentException">If keys in two dictionaries do not match</exception>
        /// <returns></returns>
        public static SortedDictionary<string, double> Delta(SortedDictionary<string, double> valuesOne, SortedDictionary<string, double> valuesTwo)
        {
            if (valuesOne.Count != valuesTwo.Count)
            {
                throw new ArgumentException("The count of results from two techniques are different.");
            }

            IEnumerator<KeyValuePair<string, double>> iter1 = valuesOne.GetEnumerator();
            IEnumerator<KeyValuePair<string, double>> iter2 = valuesTwo.GetEnumerator();

            while (iter1.MoveNext() && iter2.MoveNext())
            {
                string queryid1 = iter1.Current.Key;
                string queryid2 = iter2.Current.Key;
                if (queryid1.Equals(queryid2) == false)
                {
                    throw new ArgumentException("The query ids in both collections do not match");
                }
            }

            SortedDictionary<string, double> differencesResults = new SortedDictionary<string, double>();

            foreach (KeyValuePair<string, double> pair in valuesOne)
            {
                string queryId = pair.Key;
                double score = pair.Value;
                double score2 = valuesTwo[queryId];

                differencesResults.Add(queryId, score2 - score); //substract from new technique value the baseline value
            }

            return differencesResults;
        }
    }
}
