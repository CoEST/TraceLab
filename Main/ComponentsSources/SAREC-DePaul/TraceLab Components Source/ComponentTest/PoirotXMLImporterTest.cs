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
using System.IO;
using TraceLabSDK.Component.Config;
using TraceLabSDK;

namespace ComponentTest
{
    [TestClass]
    public class PoirotXMLImporterTest : ImporterTest
    {
        protected PoirotXMLImporter TestImporter
        {
            get { return (PoirotXMLImporter)base.TestComponent; }
            set { base.TestComponent = value; }
        }

        protected override void CreateImporter(TraceLabSDK.ComponentLogger logger)
        {
            TestImporter = new PoirotXMLImporter(logger);
        }

        [TestMethod]
        [ExpectedException(typeof(ComponentException))]
        public void FileDeletionTest()
        {
            string filePath = AbsoluteFilePath + "test.xml";
            CreateFile(filePath);

            // the file still exists so this will work
            //((ImporterConfig)TestImporter.Configuration).Path = new FilePath(filePath);
            ((ImporterConfig)TestImporter.Configuration).Path = new FilePath();
            ((ImporterConfig)TestImporter.Configuration).Path.Init(filePath);

            File.Delete(filePath);

            // should fail now that the file is not there
            TestImporter.Compute();
        }

        [TestMethod]
        [ExpectedException(typeof(System.Xml.XmlException))]
        public void EmptyFileTest()
        {
            // create an empty xml file
            string filePath = AbsoluteFilePath + "test.xml";
            CreateFile(filePath);

            //((ImporterConfig)TestImporter.Configuration).Path = new FilePath(filePath);
            ((ImporterConfig)TestImporter.Configuration).Path = new FilePath();
            ((ImporterConfig)TestImporter.Configuration).Path.Init(filePath);

            TestImporter.Compute();
        }

        [TestMethod]
        public void CorrectlyFormatedXMLTest()
        {
            string filePath = AbsoluteFilePath + "CorrectlyFormated.xml";
            //((ImporterConfig)TestImporter.Configuration).Path = new FilePath(filePath);
            ((ImporterConfig)TestImporter.Configuration).Path = new FilePath();
            ((ImporterConfig)TestImporter.Configuration).Path.Init(filePath);

            TestImporter.Compute();
        }

        [TestMethod]
        [ExpectedException(typeof(System.Xml.XmlException))]
        public void IncorrectlyFormatedXMLTest()
        {
            string filePath = AbsoluteFilePath + "IncorrectlyFormated.xml";
            //((ImporterConfig)TestImporter.Configuration).Path = new FilePath(filePath);
            ((ImporterConfig)TestImporter.Configuration).Path = new FilePath();
            ((ImporterConfig)TestImporter.Configuration).Path.Init(filePath);

            TestImporter.Compute();
        }

        [TestMethod]
        [ExpectedException(typeof(System.Xml.XmlException))]
        public void OtherFileTypeTest()
        {
            string filePath = AbsoluteFilePath + "word.doc";
            //((ImporterConfig)TestImporter.Configuration).Path = new FilePath(filePath);
            ((ImporterConfig)TestImporter.Configuration).Path = new FilePath();
            ((ImporterConfig)TestImporter.Configuration).Path.Init(filePath);

            TestImporter.Compute();
        }
    }
}
