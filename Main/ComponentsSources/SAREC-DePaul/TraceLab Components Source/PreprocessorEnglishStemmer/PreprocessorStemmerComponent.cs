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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceLabSDK;
using TraceLabSDK.Types;

namespace Preprocessor
{
    [Component(GuidIDString = "420775E4-1AFC-4142-9145-F32A7D1ED8C4",
        Name = "English Porter Stemmer", 
        DefaultLabel = "English Porter Stemmer",
        Description = "This Pre-Processor stemms the words to their roots.  It uses the Porter stemming algorithm.",
        Author = "DePaul RE Lab Team",
        Version = "1.0",
        ConfigurationType=null)]
    [IOSpec(IOSpecType.Input, "listOfArtifacts", typeof(TraceLabSDK.Types.TLArtifactsCollection))]
    [IOSpec(IOSpecType.Output, "listOfArtifacts", typeof(TraceLabSDK.Types.TLArtifactsCollection))]
    [Tag("Preprocessors")]
    public class PreprocessorStemmerComponent : BaseComponent
    {
        public PreprocessorStemmerComponent(ComponentLogger log) : base(log) { }

        public override void Compute()
        {
            TLArtifactsCollection listOfArtifacts = (TLArtifactsCollection)Workspace.Load("listOfArtifacts");

            ProcessArtifacts(listOfArtifacts);

            Workspace.Store("listOfArtifacts", listOfArtifacts);
        }

        private static void ProcessArtifacts(TLArtifactsCollection listOfArtifacts)
        {
            if (listOfArtifacts == null)
            {
                throw new ComponentException("Recieved Null listofArtifacts");
            }

            foreach (TLArtifact artifact in listOfArtifacts.Values)
            {
                artifact.Text = PreprocessorStemmer.Process(artifact.Text);
            }
        }
    }
}
