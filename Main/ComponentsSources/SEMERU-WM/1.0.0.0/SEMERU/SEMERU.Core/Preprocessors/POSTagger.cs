using System;
using System.Collections.Generic;
using edu.stanford.nlp.tagger.maxent; // IKVM converted Java JAR
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

namespace SEMERU.Core.Preprocessors
{
    /// <summary>
    /// Responsible for tagging Parts-of-Speech (POS) based on the Stanford NLP POS-tagger
    /// Model files can be found here: http://nlp.stanford.edu/software/tagger.shtml#Download
    /// </summary>
    public static class POSTagger
    {
        /// <summary>
        /// Extracts and returns all terms with the specified POS from a TLArtifactsCollection.
        /// </summary>
        /// <param name="artifacts">List of artifacts</param>
        /// <param name="pos">Part of speech to extract</param>
        /// <param name="modelFile">Training model file location</param>
        /// <returns>TLArtifactsCollection consisting of only the terms with the specified POS</returns>
        public static TLArtifactsCollection Extract(TLArtifactsCollection artifacts, POSTaggerSpeechType pos, string modelFile)
        {
            TLArtifactsCollection extracted = new TLArtifactsCollection();
            foreach (KeyValuePair<string, TLArtifact> artifactKVP in artifacts)
            {
                extracted.Add(artifactKVP.Key, ExtractArtifact(artifactKVP.Value, pos, modelFile));
            }
            return extracted;
        }

        /// <summary>
        /// Extracts and returns all terms with the specified POS from a single TLArtifact.
        /// </summary>
        /// <param name="artifact">Single artifact</param>
        /// <param name="pos">Part of speech to extract</param>
        /// <param name="modelFile">Training model file location</param>
        /// <returns>Single artifact consisting of only the terms with the specified POS</returns>
        public static TLArtifact ExtractArtifact(TLArtifact artifact, POSTaggerSpeechType pos, string modelFile)
        {
            return new TLArtifact(artifact.Id, ExtractPOS(Tag(artifact.Text, modelFile), pos));
        }

        /// <summary>
        /// Tags each term in a string with its POS based on a training model.
        /// List of abbreviations: http://www.computing.dcu.ie/~acahill/tagset.html
        /// </summary>
        /// <param name="text">String to tag</param>
        /// <param name="modelFile">Training model file location</param>
        /// <returns>String with each term tagged with its POS. Ex. bird_NN</returns>
        public static string Tag(string text, string modelFile)
        {
            return new MaxentTagger(modelFile).tagString(text);
        }

        /// <summary>
        /// Extracts and returns all terms with the specified POS from a string.
        /// </summary>
        /// <param name="text">Source string</param>
        /// <param name="pos">Part of speech to extract</param>
        /// <returns>String consisting of only the terms with the specified POS</returns>
        public static string ExtractPOS(string text, POSTaggerSpeechType pos)
        {
            Dictionary<POSTaggerSpeechType, List<string>> dict = new Dictionary<POSTaggerSpeechType, List<string>>();
            foreach (POSTaggerSpeechType type in Enum.GetValues(typeof(POSTaggerSpeechType)))
            {
                dict.Add(type, new List<string>());
            }
            foreach (string term in text.Split())
            {
                string[] kvp = term.Split('_');
                if (kvp.Length < 2)
                    continue;
                if (kvp[1].Length == 0)
                    continue;
                if (kvp[1][0] == 'N')
                {
                    dict[POSTaggerSpeechType.Noun].Add(kvp[0]);
                    continue;
                }
                if (kvp[1][0] == 'V')
                {
                    dict[POSTaggerSpeechType.Verb].Add(kvp[0]);
                    continue;
                }
                dict[POSTaggerSpeechType.Other].Add(kvp[0]);
            }
            return String.Join(" ", dict[pos]);
        }

        /// <summary>
        /// Enum for each part of speech.
        /// Update ExtractPOS when adding new types.
        /// </summary>
        public enum POSTaggerSpeechType
        {
            Noun,
            Verb,
            Other,
        }
    }
}
