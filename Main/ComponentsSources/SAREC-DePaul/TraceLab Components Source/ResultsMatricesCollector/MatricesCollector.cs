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
// Located in c:\Program Files (x86)\COEST\TraceLab\lib\TraceLabSDK.dll
using TraceLabSDK;
using TraceLabSDK.Types;
using System.Collections;
using System.Collections.Generic;
using TraceLabSDK.Types.Contests;

namespace ResultsMatricesCollector
{
    [Component(GuidIDString = "8b4ab546-e1b8-43d4-aa74-9f5dd1970f1b",
                Name = "MatricesCollector",
                DefaultLabel = "Matrices Collector",
                Description = "Appends the given similarity matrix into selected lists. Sets also the name of the result matrix. If the list of matrices does not yet exists it creates the new list of matrices.",
                Author = "Re Lab",
                Version = "1.0")]
    [IOSpec(IOType = IOSpecType.Input, Name = "matrix", DataType = typeof(TLSimilarityMatrix))]
    [IOSpec(IOType = IOSpecType.Input, Name = "name", DataType = typeof(string))]
    [IOSpec(IOType = IOSpecType.Input, Name = "listOfMatrices", DataType = typeof(TLSimilarityMatricesCollection))]
    [IOSpec(IOType = IOSpecType.Output, Name = "listOfMatrices", DataType = typeof(TLSimilarityMatricesCollection))]
    [Tag("Contest utilities")]
    public class MatricesCollector : BaseComponent
    {
        public MatricesCollector(ComponentLogger log) : base(log) { }

        public override void Compute()
        {
            TLSimilarityMatricesCollection listOfMatrices = (TLSimilarityMatricesCollection)Workspace.Load("listOfMatrices");
            if (listOfMatrices == null)
            {
                throw new ComponentException("The 'listOfMatrices' is null or has not been found in the Workspace.");
            }

            TLSimilarityMatrix matrix = (TLSimilarityMatrix)Workspace.Load("matrix");

            if (matrix == null)
            {
                throw new ComponentException("The 'matrix' input is null or has not been found in the Workspace.");
            }

            string name = (string)Workspace.Load("name");

            if (name == null)
            {
                throw new ComponentException("The 'name' for matrix is null or has not been found in the Workspace. Please, provide the name for the matrix.");
            }

            matrix.Name = name;

            listOfMatrices.Add(matrix);

            Workspace.Store("listOfMatrices", listOfMatrices);
        }
    }
}