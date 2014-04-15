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

// <copyright file="FileWrapperStreamTest.cs" company="Microsoft">Copyright © Microsoft 2011</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TraceLab.Serialization.StreamSystem;

namespace TraceLab.Serialization.StreamSystem
{
    [TestClass]
    [PexClass(typeof(FileWrapperStream))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class FileWrapperStreamTest
    {
        [PexMethod(MaxRunsWithoutNewTests = 200)]
        internal void WriteByte([PexAssumeUnderTest]FileWrapperStream target, byte value)
        {
            target.WriteByte(value);
            // TODO: add assertions to method FileWrapperStreamTest.WriteByte(FileWrapperStream, Byte)
        }
        [PexMethod(MaxRunsWithoutNewTests = 200)]
        internal void PositionSet([PexAssumeUnderTest]FileWrapperStream target, long value)
        {
            target.Position = value;
            // TODO: add assertions to method FileWrapperStreamTest.PositionSet(FileWrapperStream, Int64)
            Assert.IsTrue(target.Position >= 0);
            Assert.AreEqual(value, target.Position);
        }
    }
}
