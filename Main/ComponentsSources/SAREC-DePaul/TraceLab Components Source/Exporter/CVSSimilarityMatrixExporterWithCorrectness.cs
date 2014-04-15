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
using TraceLabSDK;
using TraceLabSDK.Types;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using TraceLabSDK.Component.Config; 

namespace Exporter
{
    [Component("7C742474-49D2-11E0-8B6E-11F7DFD72085",
        "CSV Similarity Matrix Exporter (with correctness)",
        "The components exports similarity matrix to csv text file. In addition to similarity matrix that is being exported, " +
        "it also takes answer matrix, so that correctness of the link could be determined.",
        "DePaul RE Lab Team",
        "1.0",
        typeof(CSVConfig))]
    [IOSpec(IOSpecType.Input, "similarityMatrix", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Input, "answerMatrix", typeof(TLSimilarityMatrix))]
    [Tag("Exporters.TLSimilarityMatrix.To CSV")]
    public class CSVSimilarityMatrixExporterWithCorrectness : BaseComponent
    {
        private CSVConfig Config;

        public CSVSimilarityMatrixExporterWithCorrectness(ComponentLogger log)
            : base(log)
        {
            Config = new CSVConfig();
            Configuration = Config;
        }

        public override void Compute()
        {
            TLSimilarityMatrix similarityMatrix = (TLSimilarityMatrix)Workspace.Load("similarityMatrix");
            TLSimilarityMatrix answerMatrix = (TLSimilarityMatrix)Workspace.Load("answerMatrix");

            CreateCSVReport(similarityMatrix, answerMatrix, Config.Path);

            Logger.Info(String.Format("Matrix has been saved into csv file '{0}'", Config.Path.Absolute));
        }

        private static void CreateCSVReport(TLSimilarityMatrix similarityMatrix, TLSimilarityMatrix answerMatrix, string outputPath)
        {
            if (similarityMatrix == null)
            {
                throw new ComponentException("Received similarity matrix is null!");
            }

            if (answerMatrix == null)
            {
                throw new ComponentException("Received answer similarity matrix is null!");
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

                ReadSimilarityMatrixToFile(similarityMatrix, answerMatrix, writeFile);

                writeFile.Flush();
                writeFile.Close();
            }
        }

        private static void ReadSimilarityMatrixToFile(TLSimilarityMatrix similarityMatrix, TLSimilarityMatrix answerMatrix, System.IO.TextWriter writeFile)
        {
            //header
            writeFile.WriteLine("Source Artifact Id,Target Artifact Id,Probability,Is correct");

            foreach (string sourceArtifact in similarityMatrix.SourceArtifactsIds)
            {
                var traceLinks = similarityMatrix.GetLinksAboveThresholdForSourceArtifact(sourceArtifact);
                traceLinks.Sort();

                foreach (TLSingleLink link in traceLinks)
                {
                    writeFile.WriteLine("{0},{1},{2},{3}", link.SourceArtifactId, link.TargetArtifactId, link.Score, (answerMatrix.IsLinkAboveThreshold(link.SourceArtifactId, link.TargetArtifactId)) ? "1" : "0");
                }
            }

        }
    }

    [Serializable]
    public class CSVConfig
    {
        public CSVConfig() { }

        private FilePath m_path;
        public FilePath Path
        {
            get
            {
                return m_path;
            }
            set
            {
                m_path = value;
            }
        }
    }

}
