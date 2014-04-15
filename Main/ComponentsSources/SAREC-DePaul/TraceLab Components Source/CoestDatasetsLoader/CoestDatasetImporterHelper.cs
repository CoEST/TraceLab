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
using TraceLabSDK.Types;
using System.Xml;
using System.Xml.XPath;
using TraceLabSDK;

namespace CoestDatasetsLoader
{
    class CoestDatasetImporterHelper
    {
        public static ComponentLogger Logger { get; set; }

        public static TLArtifactsCollection ImportArtifacts(string filepath, bool trimValues)
        {
            TLArtifactsCollection artifacts = new TLArtifactsCollection();

            XPathDocument doc = new XPathDocument(filepath);
            XPathNavigator nav = doc.CreateNavigator();

            //read collection info
            artifacts.CollectionId = ReadSingleNode(filepath, nav, "/artifacts_collection/collection_info/id");
            artifacts.CollectionName = ReadSingleNode(filepath, nav, "/artifacts_collection/collection_info/name");
            artifacts.CollectionVersion = ReadSingleNode(filepath, nav, "/artifacts_collection/collection_info/version");
            artifacts.CollectionDescription = ReadSingleNode(filepath, nav, "/artifacts_collection/collection_info/description");

            if (trimValues)
            {
                artifacts.CollectionId = artifacts.CollectionId.Trim();
                artifacts.CollectionName = artifacts.CollectionName.Trim();
                artifacts.CollectionVersion = artifacts.CollectionVersion.Trim();
                artifacts.CollectionDescription = artifacts.CollectionDescription.Trim();
            }

            //check what type of content location the file has
            XPathNavigator iter = nav.SelectSingleNode("/artifacts_collection/collection_info/content_location");
            string content_location_type = "internal"; //default content location is internal
            //if content location has been sprecified read it
            if (iter != null) 
            {
                content_location_type = iter.Value;
            }

            //root dir is going to be needed to external content type, to determine absolute paths of the files
            string rootDir = System.IO.Path.GetDirectoryName(filepath);

            XPathNodeIterator artifactsIterator = nav.Select("/artifacts_collection/artifacts/artifact");

            string artifactId;
            string content;
            while (artifactsIterator.MoveNext())
            {
                iter = artifactsIterator.Current.SelectSingleNode("id");
                artifactId = iter.InnerXml;
                
                iter = artifactsIterator.Current.SelectSingleNode("content");

                if (content_location_type.Equals("external"))
                {
                    content = System.IO.File.ReadAllText(System.IO.Path.Combine(rootDir, iter.InnerXml.Trim()));
                }
                else
                {
                    content = iter.InnerXml;
                }

                if (trimValues)
                {
                    artifactId = artifactId.Trim();
                    content = content.Trim();
                }

                // Checking if ID is already in Artifacts List
                if (!artifacts.ContainsKey(artifactId))
                {
                    TLArtifact artifact = new TLArtifact(artifactId, content);
                    artifacts.Add(artifactId, artifact);
                }
                else
                {
                    CoestDatasetImporterHelper.Logger.Warn(
                        String.Format("Repeated artifact ID '{0}' found in file '{1}'.", artifactId, filepath)
                        );
                }

                //artifacts.Add(artifactId, (new TLArtifact(artifactId, content)));
            }

            return artifacts;
        }

        private static string ReadSingleNode(string filepath, XPathNavigator nav, string xpath)
        {
            XPathNavigator iter = nav.SelectSingleNode(xpath);
            if (iter == null)
            {
                throw new XmlException(String.Format("The format of the given file {0} is not correct. The xml node {1} has not been found in the file.", filepath, xpath));
            }
            return iter.InnerXml;
        }

        /// <summary>
        /// Imports the answer set.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <param name="sourceArtifacts">The source artifacts.</param>
        /// <param name="targetArtifacts">The target artifacts.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="trimValues">if set to <c>true</c> [trim values].</param>
        /// <returns></returns>
        public static TLSimilarityMatrix ImportAnswerSet(string filepath, TLArtifactsCollection sourceArtifacts, TLArtifactsCollection targetArtifacts, ComponentLogger logger, bool trimValues)
        {
            return ImportAnswerSet(filepath, sourceArtifacts, String.Empty, targetArtifacts, String.Empty, logger, trimValues);
        }

        /// <summary>
        /// Imports the answer set.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <param name="sourceArtifacts">The source artifacts.</param>
        /// <param name="sourceArtifactsFilePath">The source artifacts file path.</param>
        /// <param name="targetArtifacts">The target artifacts.</param>
        /// <param name="targetArtifactsFilePath">The target artifacts file path.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="trimValues">if set to <c>true</c> [trim values].</param>
        /// <returns></returns>
        public static TLSimilarityMatrix ImportAnswerSet(string filepath, TLArtifactsCollection sourceArtifacts, string sourceArtifactsFilePath, TLArtifactsCollection targetArtifacts, string targetArtifactsFilePath, ComponentLogger logger, bool trimValues)
        {
            string friendlyAnswerSetFilename = System.IO.Path.GetFileName(filepath);
            string friendlySourceArtifactsFilename = System.IO.Path.GetFileName(sourceArtifactsFilePath); 
            string friendlyTargetArtifactsFilename = System.IO.Path.GetFileName(targetArtifactsFilePath);

            TLSimilarityMatrix answerSet = new TLSimilarityMatrix();

            XPathDocument doc = new XPathDocument(filepath);
            XPathNavigator nav = doc.CreateNavigator();

            //read collection info
            XPathNavigator iter = nav.SelectSingleNode("/answer_set/answer_info/source_artifacts_collection");
            string source_artifacts_collection_id = iter.Value;
            if (source_artifacts_collection_id.Equals(sourceArtifacts.CollectionId) == false)
            {
                throw new ArgumentException(String.Format("The answer set refers to source artifact collection with id '{0}', while loaded artifacts collection has different id '{1}'. Importing answer set from {2}", 
                                                    source_artifacts_collection_id, sourceArtifacts.CollectionId, filepath));
            }

            iter = nav.SelectSingleNode("/answer_set/answer_info/target_artifacts_collection");
            string target_artifacts_collection_id = iter.Value;
            if (target_artifacts_collection_id.Equals(targetArtifacts.CollectionId) == false)
            {
                throw new ArgumentException(String.Format("The answer set refers to target artifact collection with id '{0}', while loaded artifacts collection has different id '{1}'. Importing answer set from {2}", 
                                                    target_artifacts_collection_id, targetArtifacts.CollectionId, filepath));
            }

            XPathNodeIterator linksIterator = nav.Select("/answer_set/links/link");

            string source_artifact_id;
            string target_artifact_id;
            double confidence_score;
            while (linksIterator.MoveNext())
            {
                // Parse Source Artifact Id
                iter = linksIterator.Current.SelectSingleNode("source_artifact_id");
                if (iter == null)
                {
                    throw new XmlException(String.Format("The source_artifact_id has not been provided for the link. File location: {0}", filepath));
                }
                
                source_artifact_id = iter.Value;
                if (trimValues)
                {
                    source_artifact_id = source_artifact_id.Trim();
                }

                if (sourceArtifacts.ContainsKey(source_artifact_id) == false)
                {
                    logger.Warn(String.Format("The source artifact id '{0}' referenced in the answer set {1} has not been found in the source artifacts {2}. Therefore, this link has been removed in this experiment.", source_artifact_id, friendlyAnswerSetFilename, friendlySourceArtifactsFilename));
                }

                // Parse Target Artifact Id
                iter = linksIterator.Current.SelectSingleNode("target_artifact_id");
                if (iter == null)
                {
                    throw new XmlException(String.Format("The target_artifact_id has not been provided for the link. File location: {0}", filepath));
                }

                target_artifact_id = iter.Value;
                if (trimValues)
                {
                    target_artifact_id = target_artifact_id.Trim();
                }

                if (targetArtifacts.ContainsKey(target_artifact_id) == false)
                {
                    logger.Warn(String.Format("The target artifact id '{0}' referenced in the answer set {1} has not been found in the target artifacts {2}. Therefore, this link has been removed in this experiment.", target_artifact_id, friendlyAnswerSetFilename, friendlyTargetArtifactsFilename));
                }

                //Parse confidence score
                iter = linksIterator.Current.SelectSingleNode("confidence_score");
                if (iter == null)
                {
                    //if confidence score is not provided set it to default value 1
                    confidence_score = 1.0;
                }
                else
                {
                    string tmpValue = iter.Value;
                    if (trimValues) tmpValue = tmpValue.Trim();

                    if (double.TryParse(tmpValue, out confidence_score) == false)
                    {
                        throw new XmlException(String.Format("The confidence score provided for link from source artifact {0} to target artifact is in incorrect format {1}. File location: {2}", source_artifact_id, target_artifact_id, filepath));
                    }
                }

                answerSet.AddLink(source_artifact_id, target_artifact_id, confidence_score);
            }

            return answerSet;
        }

        /// <summary>
        /// Imports the answer set without validation against source and target artifacts
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="trimValues">if set to <c>true</c> [trim values].</param>
        /// <returns></returns>
        public static TLSimilarityMatrix ImportAnswerSet(string filepath, ComponentLogger logger, bool trimValues)
        {
            string friendlyAnswerSetFilename = System.IO.Path.GetFileName(filepath);
            
            TLSimilarityMatrix answerSet = new TLSimilarityMatrix();

            XPathDocument doc = new XPathDocument(filepath);
            XPathNavigator nav = doc.CreateNavigator();

            //read collection info
            XPathNavigator iter = nav.SelectSingleNode("/answer_set/answer_info/source_artifacts_collection");
            string source_artifacts_collection_id = iter.Value;
            
            iter = nav.SelectSingleNode("/answer_set/answer_info/target_artifacts_collection");
            string target_artifacts_collection_id = iter.Value;
            
            XPathNodeIterator linksIterator = nav.Select("/answer_set/links/link");
            
            string source_artifact_id;
            string target_artifact_id;
            double confidence_score;
            while (linksIterator.MoveNext())
            {
                // Parse Source Artifact Id
                iter = linksIterator.Current.SelectSingleNode("source_artifact_id");
                if (iter == null)
                {
                    throw new XmlException(String.Format("The source_artifact_id has not been provided for the link. File location: {0}", filepath));
                }

                source_artifact_id = iter.Value;
                if (trimValues)
                {
                    source_artifact_id = source_artifact_id.Trim();
                }
                            
                // Parse Target Artifact Id
                iter = linksIterator.Current.SelectSingleNode("target_artifact_id");
                if (iter == null)
                {
                    throw new XmlException(String.Format("The target_artifact_id has not been provided for the link. File location: {0}", filepath));
                }

                target_artifact_id = iter.Value;
                if (trimValues)
                {
                    target_artifact_id = target_artifact_id.Trim();
                }
                
                //Parse confidence score
                iter = linksIterator.Current.SelectSingleNode("confidence_score");
                if (iter == null)
                {
                    //if confidence score is not provided set it to default value 1
                    confidence_score = 1.0;
                }
                else
                {
                    string tmpValue = iter.Value;
                    if (trimValues) tmpValue = tmpValue.Trim();

                    if (double.TryParse(tmpValue, out confidence_score) == false)
                    {
                        throw new XmlException(String.Format("The confidence score provided for link from source artifact {0} to target artifact is in incorrect format {1}. File location: {2}", source_artifact_id, target_artifact_id, filepath));
                    }
                }

                answerSet.AddLink(source_artifact_id, target_artifact_id, confidence_score);
            }

            return answerSet;
        }

        public static bool ValidatePath(string filePath, string parameterName, out string error)
        {
            if (filePath == null)
            {
                error = String.Format("{0} to import has not been specified.", parameterName);
                return false;
            }
            if (!System.IO.File.Exists(filePath))
            {
                error = String.Format("{0} '{1}' does not exist.", parameterName, filePath);
                return false;
            }
            if (System.IO.Path.GetExtension(filePath) != ".xml")
            {
                error = String.Format("{0} '{1}' must be of type .xml", parameterName, filePath);
                return false;
            }

            error = String.Empty;
            return true;
        }
    }
}
