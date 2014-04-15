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
using System.Xml.Serialization;

namespace TraceabilityUserFeedbackGUI
{

    /// <summary>
    /// is a direct extension of the TLSimilarityMatrix type.
    /// It provides extra fields ...................................................will write something really cool here :]
    /// </summary>
    [Serializable]
    [WorkspaceType]
    public class SimilarityMatrixUserFeedback : TLSimilarityMatrix, IXmlSerializable, IRawSerializable
    {
        /// <summary>
        /// The possible answers given to the source artifacts
        /// </summary>
        public enum sourceSatisfactionState
        {
            notSet,
            undecided,
            satisfied,
            notSatisfied
        };

        /// <summary>
        /// The possible answers given to a combination of source artifacts and target artifacts
        /// </summary>
        public enum linkStates
        {
            notSet,
            undecided,
            link,
            notLink
        };


        /// <summary>
        /// The answers of the source artifacts
        /// </summary>
        private Dictionary<String, sourceSatisfactionState> sourceAnswers = new Dictionary<string, sourceSatisfactionState>();

        /// <summary>
        /// The structure holding decisions regarding links. Orderd like so: lindDecisions[sourceArtifactId][targetArtifactId].value
        /// </summary>
        private Dictionary<string, Dictionary<string, linkStates>> linkDecisions = new Dictionary<string, Dictionary<string, linkStates>>();


        public SimilarityMatrixUserFeedback()
        {
            
        }

        /// <summary>
        /// Construct from TLSimilarityMatrix object
        /// </summary>
        public SimilarityMatrixUserFeedback(TLSimilarityMatrix matrixIn)
            :base(matrixIn)
        {
            foreach(String sourceArtifactId in this.SourceArtifactsIds)
            {
                sourceAnswers.Add(sourceArtifactId, sourceSatisfactionState.notSet);
          
                //and create a copy of the list of links:
                TLLinksList listOfLinks = GetLinksAboveThresholdForSourceArtifact(sourceArtifactId);      
                Dictionary<string,linkStates> tempTargets = new Dictionary<string,linkStates>();

                foreach(TLSingleLink link in listOfLinks)
                {
                    tempTargets.Add(link.TargetArtifactId,linkStates.notSet);
                }

                linkDecisions.Add(sourceArtifactId, tempTargets);
            }

        }


        /// <summary>
        /// Returns the user made decision regarding the source artifact
        /// </summary>
        public sourceSatisfactionState getSourceSatisfactionDecision(String sourceId)
        {
            return sourceAnswers[sourceId];
        }

        public void setSourceSatisfactionDecision(String sourceId, sourceSatisfactionState newState)
        {
            sourceAnswers[sourceId] = newState;
        }

        public void setTargetLinkDecision(string sourceArtifactId, string targetArtifactId, linkStates decision)
        {
            linkDecisions[sourceArtifactId][targetArtifactId] = decision;
        }

        public linkStates getTargetLinkDecision(string sourceArtifactId, string targetArtifactId)
        {
            return linkDecisions[sourceArtifactId][targetArtifactId];
        }




        #region IXmlSerializable Members

        public override void ReadXml(System.Xml.XmlReader reader)
        {



        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("originalSymMatrix");
            
                writer.WriteAttributeString("Version", version.ToString());
                writer.WriteAttributeString("Threshold", Threshold.ToString());
                if (String.IsNullOrEmpty(Name) == false)
                    writer.WriteAttributeString("Name", Name.ToString());

                foreach (TLSingleLink link in AllLinks)
                {
                    writer.WriteStartElement("Link");
                    writer.WriteAttributeString("sourceArtifactId", link.SourceArtifactId);
                    writer.WriteAttributeString("targetArtifactId", link.TargetArtifactId);
                    writer.WriteAttributeString("score", link.Score.ToString());
                    writer.WriteEndElement();
                }

            writer.WriteEndElement();

            ///*the extended values: */
            writer.WriteStartElement("extendedValues");
                writer.WriteStartElement("sourceDecisions");
                    foreach (KeyValuePair<String, sourceSatisfactionState> sourceAns in sourceAnswers)
                    {
                        writer.WriteStartElement("sourceDecision");
                        writer.WriteAttributeString("sourceArtifactId", sourceAns.Key);
                        writer.WriteAttributeString("SatisfactionDecision", sourceAns.Value.GetHashCode().ToString());
                        writer.WriteEndElement();
                    }
                writer.WriteEndElement();

                writer.WriteStartElement("linkDecisions");
                foreach (KeyValuePair<string, Dictionary<string, linkStates>> linkDecision in linkDecisions)
                {
                    writer.WriteStartElement("sourceArtifact");
                    writer.WriteAttributeString("sourceArtifactId", linkDecision.Key);
                    foreach (KeyValuePair<string, linkStates> sourceToTargetDecision in linkDecision.Value)
                    {
                        int linkStateDecision = ((linkStates)sourceToTargetDecision.Value).GetHashCode();
                        writer.WriteStartElement("targetArtifac");
                            writer.WriteAttributeString("ID", sourceToTargetDecision.Key);
                            writer.WriteAttributeString("decision", ((linkStates)sourceToTargetDecision.Value).GetHashCode().ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            
            writer.WriteEndElement();
        }

        #endregion


        #region IRawSerializable Members

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public override void ReadData(System.IO.BinaryReader reader)
        {
            base.ReadData(reader);

            /*read extended values */
            int sourceAnswers_Count = reader.ReadInt32();

            for (int i = 0; i < sourceAnswers_Count; i++)
            {
                string sourceId = reader.ReadString();
                sourceSatisfactionState sourceSatisfaction = (sourceSatisfactionState)reader.ReadInt32();
                    
                sourceAnswers.Add(sourceId, sourceSatisfaction);
             }

            int linkDecisions_Count = reader.ReadInt32();

            for (int i = 0; i < linkDecisions_Count; i++)
            {
                String sourceArtifactId = reader.ReadString();

                Dictionary<string, linkStates> linkForSourceDictionary = new Dictionary<string, linkStates>();
                    
                int linkForSource_Count = reader.ReadInt32();
                for (int j = 0; j < linkForSource_Count; j++)
                {
                    string targetArtifactId = reader.ReadString();
                    linkStates linkStateDecision = (linkStates)reader.ReadInt32();

                    linkForSourceDictionary.Add(targetArtifactId, linkStateDecision);
                }

                linkDecisions.Add(sourceArtifactId, linkForSourceDictionary);
            }


        }

        /// <summary>
        /// Writes the data.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void WriteData(System.IO.BinaryWriter writer)
        {
            base.WriteData(writer);
            
            /*write extended values */
            writer.Write(sourceAnswers.Count);
            foreach (KeyValuePair<String, sourceSatisfactionState> sourceAnswer in sourceAnswers)
            {
                string sourceId = sourceAnswer.Key;
                int satisfyState = ((sourceSatisfactionState)sourceAnswer.Value).GetHashCode();

                writer.Write(sourceId);
                writer.Write(satisfyState);
            }


            writer.Write(linkDecisions.Count);
            foreach(KeyValuePair<string, Dictionary<string,linkStates>> linkDecision in linkDecisions)
            {
                string sourceArtifactId = linkDecision.Key;
                
                writer.Write(sourceArtifactId); 
                writer.Write(linkDecision.Value.Count);
                foreach (KeyValuePair<string, linkStates> sourceToTargetDecision in linkDecision.Value)
                {
                    string targetArtifactId = sourceToTargetDecision.Key;
                    int linkStateDecision =((linkStates)sourceToTargetDecision.Value).GetHashCode();

                    writer.Write(targetArtifactId);
                    writer.Write(linkStateDecision); // the value of the decision made
                }
            }
        }

        #endregion
    }
}
