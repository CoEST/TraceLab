using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SEMERU.Core.Models;
using SEMERU.Core.IO;

/// SEMERU Component Library - TraceLab Component Plugin
/// Copyright © 2012-2013 SEMERU
/// 
/// This file is part of the SEMERU Component Library.
/// 
/// The SEMERU Component Library is free software: you can redistribute it and/or
/// modify it under the terms of the GNU General Public License as published by the
/// Free Software Foundation, either version 3 of the License, or (at your option)
/// any later version.
/// 
/// The SEMERU Component Library is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
/// or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for
/// more details.
/// 
/// You should have received a copy of the GNU General Public License along with the
/// SEMERU Component Library.  If not, see <http://www.gnu.org/licenses/>.

namespace SEMERU.Tests.Models
{
    [TestFixture]
    public class TermDocumentMatrixTest
    {
        [Test]
        public void ConstructorTest_Artifacts()
        {
            string data = @"../../Data/SimpleCorpus.";
            TermDocumentMatrix matrix = new TermDocumentMatrix(Artifacts.Import(data + "input.target.txt"));
            TermDocumentMatrix answer = TermDocumentMatrix.Load(data + "output.target.matrix.txt");
            // counts
            Assert.AreEqual(matrix.NumDocs, answer.NumDocs);
            Assert.AreEqual(matrix.NumTerms, answer.NumTerms);
            // matrix
            for (int i = 0; i < answer.NumDocs; i++)
            {
                Assert.AreEqual(matrix.GetDocumentName(i), answer.GetDocumentName(i));
                Assert.AreEqual(matrix.GetDocument(i).Length, answer.NumTerms);
                for (int j = 0; j < answer.NumTerms; j++)
                {
                    Assert.AreEqual(matrix.GetTermName(j), answer.GetTermName(j));
                    Assert.AreEqual(matrix[i, j], answer[i, j], 0.0);
                }
            }
        }
    }
}
