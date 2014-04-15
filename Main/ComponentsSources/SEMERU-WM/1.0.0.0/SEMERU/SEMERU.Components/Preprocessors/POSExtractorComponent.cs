using System.Collections.Generic;
using System.ComponentModel;
using SEMERU.Core.Preprocessors;
using TraceLabSDK;
using TraceLabSDK.Component.Config;
using TraceLabSDK.Types;

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

namespace SEMERU.Components
{
    [Component(Name = "SEMERU - POS Extractor",
               Description = "Extracts terms from a TLArtifactsCollection based on the part of speech specified. Uses the Stanford NLP POS-tagger.",
               Author = "SEMERU; Evan Moritz",
               Version = "1.0.0.0",
               ConfigurationType=typeof(POSExtractorConfig))]
    [IOSpec(IOSpecType.Input,  "ListOfArtifacts", typeof(TLArtifactsCollection))]
    [IOSpec(IOSpecType.Output, "ListOfArtifacts", typeof(TLArtifactsCollection))]
    [Tag("SEMERU.Preprocessors")]
    [Tag("Preprocessors")]
    public class POSExtractorComponent : BaseComponent
    {
        private POSExtractorConfig _config;

        public POSExtractorComponent(ComponentLogger log)
            : base(log)
        {
            _config = new POSExtractorConfig();
            Configuration = _config;
        }

        public override void Compute()
        {
            Logger.Trace("Starting POSExtractor. This may take awhile (especially the bidirectional models)....");
            TLArtifactsCollection artifacts = (TLArtifactsCollection)Workspace.Load("ListOfArtifacts");
            //TLArtifactsCollection extracted = POSTagger.Extract(artifacts, _config.POS, _config.ModelFile);
            TLArtifactsCollection extracted = new TLArtifactsCollection();
            int count = 1;
            foreach (KeyValuePair<string, TLArtifact> artifactKVP in artifacts)
            {
                extracted.Add(artifactKVP.Key, POSTagger.ExtractArtifact(artifactKVP.Value, _config.POS, _config.ModelFile));
                Logger.Trace("Extracted " + count + "/" + artifacts.Count);
                count++;
            }
            Workspace.Store("ListOfArtifacts", extracted);
        }
    }

    public class POSExtractorConfig
    {
        [DisplayName("Part of Speech")]
        [Description("The desired part of speech for extraction.")]
        public POSTagger.POSTaggerSpeechType POS { get; set; }

        [DisplayName("Training Model")]
        [Description("The training model used to tag the parts of speech.")]
        public FilePath ModelFile { get; set; }
    }
}