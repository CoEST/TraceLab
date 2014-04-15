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
using TraceLabSDK;
using Importer;
using System.Reflection;
using TraceLabSDK.Component.Config;
using System.IO;

namespace ComponentTest
{
    [TestClass]
    public class StopWordsImporterTest : ImporterTest
    {
        private StopwordsImporter TestImporter
        {
            get
            {
                return (StopwordsImporter)base.TestComponent;
            }
            set
            {
                base.TestComponent = value;
            }
        }

        protected override void CreateImporter(ComponentLogger logger)
        {
            TestImporter = new StopwordsImporter(logger);
        }

        //[TestMethod]
        //public void CorrectFileTest()
        //{
        //    string filePath = AbsoluteFilePath + "test.txt";
        //    CreateFile(filePath);
        //    ((ImporterConfig)TestImporter.Configuration).Path = new FilePath(filePath);
        //    TestImporter.Compute();
        //}

        //[TestMethod]
        //[ExpectedException(typeof(IOException))]
        //public void FileDeletedTest()
        //{
        //    string filePath = AbsoluteFilePath + "test.txt";
        //    CreateFile(filePath);

        //    // the file still exists so this will work
        //    ((ImporterConfig)TestImporter.Configuration).Path = new FilePath(filePath);

        //    File.Delete(filePath);

        //    // should fail now that the file is not there
        //    TestImporter.Compute();
        //}

        ///// <summary>
        ///// An Attempt to use keywords such as \n and " " to break stopwords
        ///// compute method of extracting from a text file
        ///// </summary>
        //[TestMethod]
        //public void KeywordStopWordsTest()
        //{
        //    string filePath = AbsoluteFilePath + "special Keywords.txt";
        //    ComputeFilePath(filePath);
        //}

        //[TestMethod]
        //public void UnicodeTextFileTest()
        //{
        //    // expecting the file to be there
        //    string filePath = AbsoluteFilePath + "unicoded.txt";
        //    ComputeFilePath(filePath);
        //}

        //[TestMethod]
        //[ExpectedException(typeof(IOException))]
        //public void OtherFileTypeTest()
        //{
        //    string filePath = AbsoluteFilePath + "word.doc";
        //    ComputeFilePath(filePath);
        //}

        //private void ComputeFilePath(string filePath)
        //{
        //    ((ImporterConfig)TestImporter.Configuration).Path = new FilePath(filePath);
        //    TestImporter.Compute();
        //}
    }
}
