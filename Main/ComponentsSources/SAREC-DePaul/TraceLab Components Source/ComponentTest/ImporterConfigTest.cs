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
using TraceLabSDK.Component.Config;
using System.IO;
using TraceLabSDK;

namespace ComponentTest
{
    [TestClass]
    public class ImporterConfigTest
    {   
        private ImporterConfig TestConfig
        {
            get;
            set;
        }

        [TestInitialize]
        public void TestSetup()
        {
            TestConfig = new ImporterConfig();
        }

        [TestMethod]
        public void CorrectFilePathTest()
        {
            string stringPath = GetAbsoluteFilePathToTestFiles("test.txt");

            //TestConfig.Path = new FilePath(stringPath);
            TestConfig.Path = new FilePath( );
            TestConfig.Path.Init(stringPath);
        }

        private static string GetAbsoluteFilePathToTestFiles(string fileToGet)
        {
            string stringPath = System.IO.Directory.GetCurrentDirectory();
            string[] split = stringPath.Split(new string[] { "\\" }, StringSplitOptions.None);
            stringPath = split[0]; // expecting root drive usually C:
            for (int i = 1; i < split.Length; ++i)
            {
                // the known directory in our solution that we want to avoid going into
                // and fork our path
                if (split[i] == "TestResults")
                {
                    break;
                }
                else
                {
                    stringPath += "\\" + split[i];
                }

            }

            stringPath += "\\TestFiles\\" + fileToGet;
            return stringPath;
        }
    }
}
