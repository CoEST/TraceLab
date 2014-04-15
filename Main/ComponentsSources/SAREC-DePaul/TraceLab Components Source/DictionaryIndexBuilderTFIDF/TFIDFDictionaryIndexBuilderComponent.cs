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
using TraceLabSDK;
using TraceLabSDK.Types;

namespace DictionaryIndexBuilder
{
    [Component(GuidIDString = "1C30B7B5-3E04-433D-817F-B0BE187B154F",
                Name = "TFIDF Dictionary Index Builder",
                DefaultLabel = "TFIDF Dictionary Index Builder",
                Description = "Creates a tf-idf dictionary. Typically build on collection that is being searched, typically on target collection.",
                Author = "DePaul RE Team",
                Version = "1.0")]
    [IOSpec(IOSpecType.Input, "listOfArtifacts", typeof(TraceLabSDK.Types.TLArtifactsCollection), Description="List of artifacts based on which the dictionary index is going to be build on.")]
    [IOSpec(IOSpecType.Output, "dictionaryIndex", typeof(TraceLabSDK.Types.TLDictionaryIndex), Description="Term dictionary index with tf-idf weighting.")]
    [Tag("Tracers.TFIDF VSM Helpers")]
    public class TFIDFDictionaryIndexBuilderComponent : BaseComponent
    {
        public TFIDFDictionaryIndexBuilderComponent(ComponentLogger log) 
            : base(log) { }
        
        public override void Compute()
        {
            TLArtifactsCollection listOfArtifacts = 
                (TLArtifactsCollection)Workspace.Load("listOfArtifacts");

            TLDictionaryIndex dict = BuildDictionary(listOfArtifacts, Logger);
            
            Workspace.Store("dictionaryIndex", dict);
        }

        private static TLDictionaryIndex BuildDictionary(TLArtifactsCollection listOfArtifacts, ComponentLogger logger)
        {
            if (listOfArtifacts == null)
            {
                throw new ComponentException("Received null listOfArtifacts");
            }

            TLDictionaryIndex dict = TFIDFIndexBuilder.build(listOfArtifacts, logger);

            return dict;
        }
    }
}
