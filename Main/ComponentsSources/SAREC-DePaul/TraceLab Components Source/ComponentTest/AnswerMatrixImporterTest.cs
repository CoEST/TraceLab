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
using Importer;
using TraceLabSDK;
using System.Reflection;
using TraceLabSDK.Component.Config;
using System.IO;

namespace ComponentTest
{
    /// <summary>
    /// Summary description for AnswerMatrixImporterTest
    /// </summary>
    [TestClass]
    public class AnswerMatrixImporterTest : ImporterTest
    {
        protected AnswerMatrixImporter TestImporter
        {
            get
            {
                return (AnswerMatrixImporter)base.TestComponent;
            }
            set
            {
                base.TestComponent = value;
            }
        }

        protected override void CreateImporter(ComponentLogger logger)
        {
            TestImporter = new AnswerMatrixImporter(logger);
        }

        //[TestMethod]
        //[ExpectedException(typeof(IOException))]
        //public void TestFileDeletion()
        //{
        //    string filePath = AbsoluteFilePath + "test.txt";
        //    CreateFile(filePath);

        //    // the file still exists so this will work
        //    ((AnswerMatrixImporterConfig)TestImporter.Configuration).Path = new FilePath(filePath);
        //    ((AnswerMatrixImporterConfig)TestImporter.Configuration).Separator = ",";

        //    File.Delete(filePath);

        //    // should fail now that the file is not there
        //    TestImporter.Compute();
        //}

        //[TestMethod]
        //[ExpectedException(typeof(ArgumentNullException))]
        //public void NullSeperatorTest()
        //{
        //    string filePath = AbsoluteFilePath + "test.txt";
        //    CreateFile(filePath);

        //    // do not assign the seperator only the filePath            
        //    ((AnswerMatrixImporterConfig)TestImporter.Configuration).Path = new FilePath(filePath);

        //    TestImporter.Compute();
        //}

        //[TestMethod]
        //public void EmptyTextFileTest()
        //{
        //    string filePath = AbsoluteFilePath + "test.txt";
        //    CreateFile(filePath);

        //    // do not assign the seperator only the filePath            
        //    ((AnswerMatrixImporterConfig)TestImporter.Configuration).Path = new FilePath(filePath);
        //    ((AnswerMatrixImporterConfig)TestImporter.Configuration).Separator = ",";

        //    TestImporter.Compute();
        //}

        //[TestMethod]
        //[ExpectedException(typeof(ArgumentException))]
        //public void UnSeperatedTextFileTest()
        //{
        //    string filePath = AbsoluteFilePath = "UnseperatedAnswerMatrix.txt";

        //    ((AnswerMatrixImporterConfig)TestImporter.Configuration).Path = new FilePath(filePath);
        //    ((AnswerMatrixImporterConfig)TestImporter.Configuration).Separator = ",";

        //    TestImporter.Compute();
        //}

        //[TestMethod]
        //public void SeperatedTextFileTest()
        //{
        //    string filePath = AbsoluteFilePath + "SeperatedAnswerMatrix.txt";

        //    ((AnswerMatrixImporterConfig)TestImporter.Configuration).Path = new FilePath(filePath);
        //    ((AnswerMatrixImporterConfig)TestImporter.Configuration).Separator = ",";

        //    TestImporter.Compute();
        //}

        //[TestMethod]
        //[ExpectedException(typeof(IOException))]
        //public void IncorrectFileTypeTest()
        //{
        //    string filePath = AbsoluteFilePath + "word.doc";

        //    ((AnswerMatrixImporterConfig)TestImporter.Configuration).Path = new FilePath(filePath);
        //    ((AnswerMatrixImporterConfig)TestImporter.Configuration).Separator = ",";

        //    TestImporter.Compute();
        //}
    }
}
