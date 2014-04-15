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

namespace Tracer
{
    public class SimilarityMetricFactory
    {
        private SimilarityMetricFactory() { }

        public static SimilarityMetric GetSimiliarityMetric(SimilarityMetricMethod method)
        {
            switch (method)
            {
                case SimilarityMetricMethod.Cosine: return new SimilarityMetricCosine();

                case SimilarityMetricMethod.Dice: return new SimilarityMetricDice();

                case SimilarityMetricMethod.Jaccard: return new SimilarityMetricJaccard();

                case SimilarityMetricMethod.SimpleMatching: 

                default: return new SimilarityMetricSimpleMatching();
            }

        }

        public static SimilarityMetric GetSimiliarityMetric(string method)
        {
            if (!Enum.IsDefined(typeof(SimilarityMetricMethod), method))
            {
                throw new ArgumentException("Similarity metric does not match any of available metrics. Choose from 'Cosine', 'Dice', 'Jaccard', 'SimpleMatching'");
            }
            SimilarityMetricMethod similarityMethod  = (SimilarityMetricMethod)Enum.Parse(typeof(SimilarityMetricMethod), method, true);
            return GetSimiliarityMetric(similarityMethod);
        }

    }
}
