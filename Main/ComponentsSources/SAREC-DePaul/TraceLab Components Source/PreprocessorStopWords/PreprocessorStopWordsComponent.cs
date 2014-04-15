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
    [Component("449F5E1F-B66E-4DB1-AC70-BA0A0A54A3BA",
        "Stopwords Remover", 
        "This Pre-Processor removes common stop words, such as 'a', 'the', 'will', etc. It uses a list of stopwords previously imported to Collective Data.",
        "DePaul RE Lab Team",
        "1.0",
        null)]
    [IOSpec(IOSpecType.Input, "listOfArtifacts", typeof(TraceLabSDK.Types.TLArtifactsCollection))]
    [IOSpec(IOSpecType.Input, "stopwords", typeof(TraceLabSDK.Types.TLStopwords))]
    [IOSpec(IOSpecType.Output, "listOfArtifacts", typeof(TraceLabSDK.Types.TLArtifactsCollection))]
    [Tag("Preprocessors")]
    public class PreprocessorStopWordsComponent : BaseComponent
    {
        public PreprocessorStopWordsComponent(ComponentLogger log) : base(log) { }

        public override void Compute()
        {
            TLArtifactsCollection listOfArtifacts = (TLArtifactsCollection)Workspace.Load("listOfArtifacts");
            TLStopwords stopwords = (TLStopwords)Workspace.Load("stopwords");

            ProcessArtifacts(listOfArtifacts, stopwords);

            Workspace.Store("listOfArtifacts", listOfArtifacts);
        }

        private static void ProcessArtifacts(TLArtifactsCollection listOfArtifacts, TLStopwords stopwords)
        {
            if (listOfArtifacts == null)
            {
                throw new ComponentException("Recieved null listofArtifacts");
            }

            if (stopwords == null)
            {
                throw new ComponentException("Recieved null stopwords");
            }

            foreach (TLArtifact artifact in listOfArtifacts.Values)
            {
                artifact.Text = PreprocessorStopWords.Process(artifact.Text, stopwords);
            }
        }
    }
}
