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
using TraceLabSDK.Types.Generics.Collections;

namespace HelperComponents
{
    [Component(GuidIDString = "f5f6ee84-18a2-4ec3-96a9-4c6410e384e2",
                Name = "GetStringFromStringList",
                DefaultLabel = "String Getter from String list",
                Description = "This simple helper component allows getting string from the specified collection of string at the specified index, and outputting it to Workspace.",
                Author = "DePaul RE Team",
                Version = "1.0")]
    [IOSpec(IOSpecType.Input, "listOfStrings", typeof(StringList))]
    [IOSpec(IOSpecType.Input, "index", typeof(int))]
    [IOSpec(IOSpecType.Output, "selectedString", typeof(System.String))]
    [Tag("Helper components")]
    public class GetStringFromStringList : BaseComponent
    {
        public GetStringFromStringList(ComponentLogger log) : base(log) { }

        public override void Compute()
        {
            StringList listOfStrings = (StringList)Workspace.Load("listOfStrings");
            if (listOfStrings == null)
            {
                throw new ComponentException("Received null listOfStrings");
            }
            if (listOfStrings.Count == 0)
            {
                throw new ComponentException("Received empty listOfStrings");
            }
            
            int index = (int)Workspace.Load("index");

            if (index < 0)
            {
                throw new ComponentException("Received negative index");
            }
            if (index > listOfStrings.Count)
            {
                throw new ComponentException("Received index greater than amount of elements in the listOfStrings");
            }

            string outputString = listOfStrings[index];

            Workspace.Store("selectedString", outputString);
        }
    }
}