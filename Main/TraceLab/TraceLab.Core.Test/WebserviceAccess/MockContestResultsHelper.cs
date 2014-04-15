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
using System.Xml.Serialization;

namespace TraceLab.Core.Test.WebserviceAccess
{
    class MockContestResultsHelper
    {
        static Random random = new Random();

        internal static IXmlSerializable CreateDummyBaseData()
        {
            var baseData = new TraceLabSDK.Types.Contests.TLSimilarityMatricesCollection();

            var matrix1 = new TraceLabSDK.Types.TLSimilarityMatrix();
            matrix1.Name = "Matrix 1";
            matrix1.AddLink("fakeSourceArtifactId_0", "fakeTargetArtifactId_0", 1);
            matrix1.AddLink("fakeSourceArtifactId_1", "fakeTargetArtifactId_1", 1);
            matrix1.AddLink("fakeSourceArtifactId_2", "fakeTargetArtifactId_2", 1);

            var matrix2 = new TraceLabSDK.Types.TLSimilarityMatrix();
            matrix2.Name = "Matrix 2";
            matrix2.AddLink("fakeSourceArtifactId_0", "fakeTargetArtifactId_0", 1);
            matrix2.AddLink("fakeSourceArtifactId_1", "fakeTargetArtifactId_1", 1);
            matrix2.AddLink("fakeSourceArtifactId_2", "fakeTargetArtifactId_2", 1);

            baseData.Add(matrix1);
            baseData.Add(matrix2);

            return baseData;
        }

        internal static TraceLabSDK.Types.Contests.BoxSummaryData CreateDummyBoxSummaryMetricResults(string boxPlotMetricName, string metricDescription)
        {
            var boxMetric = new TraceLabSDK.Types.Contests.BoxSummaryData(boxPlotMetricName, metricDescription);

            for (int i = 0; i < 3; i++)
            {
                double[] randomValues = new double[100];
                for (int j = 0; j < 100; j++)
                {
                    randomValues[j] = random.Next(200);
                }
                boxMetric.AddPoint(new TraceLabSDK.Types.Contests.BoxPlotPoint(randomValues));
            }

            return boxMetric;
        }

        internal static TraceLabSDK.Types.Contests.LineSeries CreateDummySeriesMetricResults(string seriesMetricName, string metricDescription)
        {
            var metric = new TraceLabSDK.Types.Contests.LineSeries(seriesMetricName, metricDescription);

            for (double i = 0.0; i < 1.0; i += 0.1)
            {
                metric.AddPoint(new TraceLabSDK.Types.Contests.Point(i, random.Next(10)));
            }
            return metric;
        }

        internal static TraceLabSDK.Types.Contests.DatasetResults CreateDummyDatasetResults(string datasetName, 
                                                         TraceLabSDK.Types.Contests.LineSeries lineSeriesMetric, 
                                                         TraceLabSDK.Types.Contests.BoxSummaryData boxSummaryMetric)
        {
            var datasetResults = new TraceLabSDK.Types.Contests.DatasetResults(datasetName);
            datasetResults.AddBoxSummaryMetric(boxSummaryMetric);
            datasetResults.AddLineSeriesMetric(lineSeriesMetric);
            return datasetResults;
        }
    }
}
