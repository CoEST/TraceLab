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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using TraceLabSDK.Types;
using TraceLabSDK.Types.Generics.Collections;
using TraceLabSDK.Types.Contests;

namespace TraceLab.Core.Test.Serialization
{
    [TestClass]
    public class RawSerializationTest
    {
        [TestMethod]
        public void SimilarityMatrixRawSerializationTest()
        {
            string[] sources = new string[] { "source1", "source2", "source3", "source4", "source5", "source6", "source7", "source8", "source9", "source10" };
            string[] targets = new string[] { "target1", "target2", "target3", "target4", "target5", "target6", "target7", "target8", "target9", "target10" };

            TLSimilarityMatrix matrixIn = new TLSimilarityMatrix();
            for (int i = 0; i < sources.Length; i++)
            {
                matrixIn.AddLink(sources[i], targets[i], (double)i);
            }

            BinaryWriter binWriter = new BinaryWriter(new MemoryStream());
            BinaryReader binReader = new BinaryReader(binWriter.BaseStream);

            matrixIn.WriteData(binWriter);

            binReader.BaseStream.Position = 0;

            TLSimilarityMatrix matrixOut = new TLSimilarityMatrix();
            matrixOut.ReadData(binReader);

            Assert.AreEqual(matrixIn.Count, matrixOut.Count);

            StringHashSet setIn = matrixIn.SourceArtifactsIds;
            StringHashSet setOut = matrixOut.SourceArtifactsIds;

            foreach (string artifact in setIn)
            {
                Assert.IsTrue(setOut.Contains(artifact));
            }
        }

        [TestMethod]
        public void ExperimentResultsRawSerializationTest()
        {
            int n = 0;
            TLExperimentResults expResultsIn = new TLExperimentResults("Technique " + n++);
            for (int k = 0; k < 5; k++)
            {
                DatasetResults dataResults = new DatasetResults("Dataset " + n++);
                for (int i = 0; i < 10; i++)
                {
                    LineSeries line = new LineSeries("Line " + i, "Description " + n++);
                    for (int j = 1000 * i; j < 1000; j++)
                    {
                        line.AddPoint(new Point(j, j + 1));
                    }
                    dataResults.AddMetric(line);

                    BoxSummaryData box = new BoxSummaryData("Box " + i, "Description " + n++);
                    for (int j = 0; j < 100; j++)
                    {
                        box.AddPoint(new BoxPlotPoint(j, j + 1, j + 2, j + 3, j + 4, j + 5, j + 6, j + 7));
                    }
                    dataResults.AddMetric(box);
                }
                expResultsIn.AddDatasetResult(dataResults);
            }

            BinaryWriter binWriter = new BinaryWriter(new MemoryStream());
            BinaryReader binReader = new BinaryReader(binWriter.BaseStream);
            expResultsIn.WriteData(binWriter);
            binReader.BaseStream.Position = 0;
            TLExperimentResults expResultsOut = (TLExperimentResults)Activator.CreateInstance(typeof(TLExperimentResults), true);
            expResultsOut.ReadData(binReader);

            Assert.AreEqual(expResultsIn.TechniqueName, expResultsOut.TechniqueName);
            Assert.AreEqual(expResultsIn.DatasetsResults.Count(), expResultsOut.DatasetsResults.Count());

            foreach (DatasetResults result1 in expResultsIn.DatasetsResults)
            {
                bool sameDatasetResultExists = false;
                DatasetResults result2 = null;
                foreach (DatasetResults res in expResultsOut.DatasetsResults)
                {
                    if (res.DatasetName == result1.DatasetName)
                    {
                        sameDatasetResultExists = true;
                        result2 = res;
                        break;
                    }
                }
                Assert.IsTrue(sameDatasetResultExists);

                Assert.AreEqual(result1.DatasetName, result2.DatasetName);
                Assert.AreEqual(result1.Metrics.Count(), result2.Metrics.Count());

                foreach (Metric m1 in result1.Metrics)
                {
                    bool sameMetricExists = false;
                    Metric m2 = null;
                    foreach (Metric metric in result2.Metrics)
                    {
                        if (m1.MetricName == metric.MetricName)
                        {
                            sameMetricExists = true;
                            m2 = metric;
                            break;
                        }
                    }

                    Assert.IsTrue(sameMetricExists);
                    Assert.AreEqual(m1.Description, m2.Description);

                    if (m1 is LineSeries)
                    {
                        Assert.IsTrue(m2 is LineSeries);
                        LineSeries l1 = (LineSeries)m1;
                        LineSeries l2 = (LineSeries)m2;

                        Assert.AreEqual(l1.Points.Count(), l2.Points.Count());
                    }
                    else
                    {
                        Assert.IsTrue(m2 is BoxSummaryData);
                        BoxSummaryData b1 = (BoxSummaryData)m1;
                        BoxSummaryData b2 = (BoxSummaryData)m2;

                        Assert.AreEqual(b1.Points.Count(), b2.Points.Count());
                    }
                }
            }
        }
    }
}
