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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TraceLab.Core.ExperimentExecution;
using TraceLab.Core.Experiments;
using TraceLab.Core.Test.Setup;
using TraceLab.Core.Components;
using System.Collections.Generic;

namespace TraceLab.Core.Test.Models
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class CompositeComponentTest
    {
        public TestContext TestContext
        {
            get;
            set;
        }

        internal TraceLabTestApplication AppContext
        {
            get;
            set;
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
        }

        [ClassCleanup]
        public static void FixtureTeardown()
        {
        }

        [TestInitialize]
        public void Setup()
        {
            AppContext = new TraceLabTestApplication(TestContext);
        }

        [TestCleanup]
        public void TearDown()
        {
        }
    }
}
