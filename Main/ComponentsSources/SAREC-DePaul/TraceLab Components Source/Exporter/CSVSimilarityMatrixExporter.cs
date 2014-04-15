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
// Located in c:\Program Files (x86)\COEST\TraceLab\lib\TraceLabSDK.dll
using TraceLabSDK;
using TraceLabSDK.Types;
using System.IO;

namespace Exporter
{
    [Component(GuidIDString = "ba70903b-066f-4991-a71f-fdbfc22c7a14",
                Name = "CSV Similarity Matrix Exporter",
                DefaultLabel = "CSV Similarity Matrix Exporter",
                Description = "The components exports similarity matrix to csv text file.",
                Author = "DePaul RE Lab Team",
                Version = "1.0",
                ConfigurationType = typeof(CSVConfig))]
    [IOSpec(IOSpecType.Input, "similarityMatrix", typeof(TLSimilarityMatrix))]
    [Tag("Exporters.TLSimilarityMatrix.To CSV")]
    public class CSVSimilarityMatrixExporter : BaseComponent
    {
        private CSVConfig Config;

        public CSVSimilarityMatrixExporter(ComponentLogger log) : base(log)
        {
            Config = new CSVConfig();
            Configuration = Config;
        }

        public override void Compute()
        {
            TLSimilarityMatrix similarityMatrix = (TLSimilarityMatrix)Workspace.Load("similarityMatrix");

            CreateCSVReport(similarityMatrix, Config.Path);

            Logger.Info(String.Format("Matrix has been saved into csv file '{0}'", Config.Path.Absolute));
        }

        private static void CreateCSVReport(TLSimilarityMatrix similarityMatrix, string outputPath)
        {
            if (similarityMatrix == null)
            {
                throw new ComponentException("Received similarity matrix is null!");
            }

            if (outputPath == null)
            {
                throw new ComponentException("Output path cannot be null.");
            }

            if (!System.IO.Path.IsPathRooted(outputPath))
            {
                throw new ComponentException(String.Format("Absolute output path is required. Given path is '{0}'", outputPath));
            }

            if (outputPath.EndsWith(".csv", StringComparison.CurrentCultureIgnoreCase) == false)
            {
                outputPath = outputPath + ".csv";
            }

            using (System.IO.TextWriter writeFile = new StreamWriter(outputPath))
            {
                ReadSimilarityMatrixToFile(similarityMatrix, writeFile);
                writeFile.Flush();
                writeFile.Close();
            }       
        }

        private static void ReadSimilarityMatrixToFile(TLSimilarityMatrix similarityMatrix, System.IO.TextWriter writeFile)
        {
            //header
            writeFile.WriteLine("Source Artifact Id,Target Artifact Id,Probability");

            foreach (string sourceArtifact in similarityMatrix.SourceArtifactsIds)
            {
                var traceLinks = similarityMatrix.GetLinksAboveThresholdForSourceArtifact(sourceArtifact);
                traceLinks.Sort();

                foreach (TLSingleLink link in traceLinks)
                {
                    writeFile.WriteLine("{0},{1},{2}", link.SourceArtifactId, link.TargetArtifactId, link.Score);
                }
            }

        }
    }
}