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
using TraceLab.Core.Utilities;

namespace TraceLab.Core.Test.Utilities
{
    [TestClass]
    public class TypesHelperTest
    {
        [TestMethod]
        public void TestGetFriendlyName()
        {
            // basic types
            Assert.AreEqual(typeof(int).Name, TypesHelper.GetFriendlyName(typeof(int).FullName));
            Assert.AreEqual(typeof(TypesHelperTest).Name, TypesHelper.GetFriendlyName(typeof(TypesHelperTest).FullName));

            // generic types
            string fulltype = typeof(List<String>).FullName;
            Assert.AreEqual("List<String>", TypesHelper.GetFriendlyName(fulltype));

            fulltype = typeof(Dictionary<String, HashSet<int>>).FullName;
            Assert.AreEqual("Dictionary<String, HashSet<Int32>>", TypesHelper.GetFriendlyName(fulltype));
        }
    }
}
