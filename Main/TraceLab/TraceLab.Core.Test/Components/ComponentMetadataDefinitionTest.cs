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

namespace TraceLab.Core.Components.Test
{
    [TestClass]
    public class ComponentMetadataDefinitionTest
    {
        private ComponentMetadataDefinition m_compMetaDef;

        public ComponentMetadataDefinitionTest() { }

        [TestInitialize]
        public void TestSetup()
        {
            this.m_compMetaDef = new ComponentMetadataDefinition("test", @"C:\test.dll", "test");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorNullID()
        {
            this.m_compMetaDef = new ComponentMetadataDefinition(null, @"C:\test.dll", "test");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorNullClassname()
        {
            this.m_compMetaDef = new ComponentMetadataDefinition("test", @"C:\test.dll", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetID()
        {
            m_compMetaDef.ID = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetClassname()
        {
            m_compMetaDef.Classname = null;
        }
    }
}
