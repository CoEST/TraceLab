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
using TraceLabSDK.Component.Config; 

namespace Exporter
{
    [Component("6040D584-499D-11E0-8D72-42BEDFD72085",
        "Excel Similarity Matrix Exporter",
        "The components exports similarity matrix to excel spreadsheet. Significantly slower that exporting to CSV.",
        "DePaul RE Lab Team",
        "1.0",
        typeof(Config))]
    [IOSpec(IOSpecType.Input, "similarityMatrix", typeof(TLSimilarityMatrix))]
    [IOSpec(IOSpecType.Input, "answerMatrix", typeof(TLSimilarityMatrix))]
    [Tag("Exporters.TLSimilarityMatrix.To XLS")]
    public class ExcelSimilarityMatrixExporter : BaseComponent
    {
        private Config Config;

        public ExcelSimilarityMatrixExporter(ComponentLogger log) : base(log)
        {
            Config = new Config();
            Configuration = Config;
        }

        public override void Compute()
        {
            TLSimilarityMatrix similarityMatrix = (TLSimilarityMatrix)Workspace.Load("similarityMatrix");
            TLSimilarityMatrix answerMatrix = (TLSimilarityMatrix)Workspace.Load("answerMatrix");

            CreateExcelReport(similarityMatrix, answerMatrix, Config.Path);

            Logger.Info(String.Format("Matrix has been saved into xsl file '{0}'", Config.Path.Absolute));
        }

        private static void CreateExcelReport(TLSimilarityMatrix similarityMatrix, TLSimilarityMatrix answerMatrix, string outputPath)
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

            if (outputPath.EndsWith(".xsl", StringComparison.CurrentCultureIgnoreCase) == false)
            {
                outputPath = outputPath + ".xsl";
            }

            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);

            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            ReadSimilarityMatrixToExcelWorksheet(similarityMatrix, answerMatrix, xlWorkSheet);

            xlWorkBook.SaveAs(outputPath, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

            
        }

        private static void ReadSimilarityMatrixToExcelWorksheet(TLSimilarityMatrix similarityMatrix, TLSimilarityMatrix answerMatrix, Excel.Worksheet xlWorkSheet)
        {
            //header
            int row = 1;

            xlWorkSheet.Cells[row, 1] = "Source Artifact Id";
            xlWorkSheet.Cells[row, 2] = "Target Artifact Id";
            xlWorkSheet.Cells[row, 3] = "Probability";
            xlWorkSheet.Cells[row, 4] = "Is correct";

            row++;

            foreach (string sourceArtifact in similarityMatrix.SourceArtifactsIds)
            {
                var traceLinks = similarityMatrix.GetLinksAboveThresholdForSourceArtifact(sourceArtifact);
                traceLinks.Sort();

                foreach (TLSingleLink link in traceLinks)
                {
                    xlWorkSheet.Cells[row, 1] = link.SourceArtifactId;
                    xlWorkSheet.Cells[row, 2] = link.TargetArtifactId;
                    xlWorkSheet.Cells[row, 3] = link.Score;
                    xlWorkSheet.Cells[row, 4] = (answerMatrix.IsLinkAboveThreshold(link.SourceArtifactId, link.TargetArtifactId)) ? "1" : "0";
                    row++;
                }
            }

        }

        private static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                throw new Exception("Exception Occured while releasing excel object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }


    [Serializable]
    public class Config
    {
        public Config() { }

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
