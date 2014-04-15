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
using System.IO;
using System.Reflection;

namespace ComponentTest
{
    /// <summary>
    /// Summary description for ImporterTest
    /// </summary>
    [TestClass]
    public abstract class ImporterTest : ComponentTest
    {
        protected static string AbsoluteFilePath;

        //protected abstract void CreateImporter(ComponentLogger logger);

        protected override void InitImporter()
        {
            AbsoluteFilePath = GetAbsoluteFilePathToTestFiles();
            base.InitImporter();
        }

        protected static string GetAbsoluteFilePathToTestFiles()
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

            stringPath += "\\TestFiles\\";
            return stringPath;
        }

        protected static void CreateFile(string filePath)
        {
            StreamWriter stream = File.CreateText(filePath);
            stream.Close();
        }

        /// <summary>
        /// Tests the importer without every setting any of its configuration
        /// Thus leaving the file Path unset and null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ComponentException))]
        public void NullFilePathTest()
        {
            TestComponent.Compute();
        }
    }
}
