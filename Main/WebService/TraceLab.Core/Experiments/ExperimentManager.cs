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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using TraceLab.Core.ExperimentExecution;
using TraceLab.Core.Settings;
using TraceLab.Core.Workspaces;
using TraceLabSDK;
using System.Xml.Serialization;
using TraceLab.Core.Exceptions;
using TraceLab.Core.Components;
using System.Xml;
using TraceLab.Core.Utilities;

namespace TraceLab.Core.Experiments
{
    /// <summary>
    /// Collection of static methods for the experiment.
    /// </summary>
    public static class ExperimentManager
    {
        private const string NewExperimentName = "New Experiment";
        
        #region New

        /// <summary>
        /// Creates new m_experiment.
        /// </summary>
        /// <returns>Returns newly created m_experiment.</returns>
        public static Experiment New()
        {
            Experiment newExperiment = new Experiment(NewExperimentName, string.Empty);
            ExperimentStartNode start = new ExperimentStartNode();
            start.Data.X = 200;
            start.Data.Y = 100;
            newExperiment.AddVertex(start);
            
            ExperimentEndNode end = new ExperimentEndNode();
            end.Data.X = 200;
            end.Data.Y = 200;
            newExperiment.AddVertex(end);

            newExperiment.ReloadStartAndEndNode();
            newExperiment.ResetModifiedFlag();

            return newExperiment;
        }

        #endregion New

        #region Load

        /// <summary>
        /// Loads a experiment from the specified file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <exception cref="TraceLab.Core.Exceptions.ExperimentLoadException">throws if experiment load fails</exception>
        /// <returns>
        /// Returns loaded m_experiment. If loading failed it returns null.
        /// </returns>
        public static Experiment Load(string fileName, TraceLab.Core.Components.ComponentsLibrary library)
        {
            Experiment experiment = null;
            try
            {
                using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create(fileName))
                {
                    experiment = ExperimentSerializer.DeserializeExperiment(reader, library, fileName);
                }

                if (experiment != null)
                {
                    experiment.ResetModifiedFlag();
                }

            }
            catch (ArgumentException e)
            {
                throw new ExperimentLoadException("The experiment file could not be loaded. Filename cannot be empty. ", e);
            }
            catch (System.Security.SecurityException e)
            {
                throw new ExperimentLoadException("The experiment file could not be loaded.", e);
            }
            catch (System.IO.FileNotFoundException e)
            {
                throw new ExperimentLoadException("The experiment file has not been found.", e);
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                throw new ExperimentLoadException("The directory has not been found.", e);
            }
            catch (UriFormatException e)
            {
                throw new ExperimentLoadException("The experiment is corrupted and could not be loaded.", e);
            }
            catch (System.Xml.XmlException e)
            {
                throw new ExperimentLoadException("The experiment is corrupted and could not be loaded.", e);
            }
            
            return experiment;
        }

        /// <summary>
        /// Loads an experiment from the specified STREAM.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <exception cref="TraceLab.Core.Exceptions.ExperimentLoadException">throws if experiment load fails</exception>
        /// <returns>
        /// Returns loaded m_experiment. If loading failed it returns null.
        /// </returns>
        /// TLAB-66 TLAB-68 TLAB-69
        public static Experiment Load(Stream fileName, string originalFileName, TraceLab.Core.Components.ComponentsLibrary library)
        {
            Experiment experiment = null;
            try
            {   
                using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create(fileName))
                {
                    experiment = ExperimentSerializer.DeserializeExperiment(reader, library, originalFileName);

                }

                if (experiment != null)
                {
                    experiment.ResetModifiedFlag();
                }
            }
            catch (ArgumentException e)
            {
                throw new ExperimentLoadException("The experiment file could not be loaded. Filename cannot be empty. ", e);
            }
            catch (System.Security.SecurityException e)
            {
                throw new ExperimentLoadException("The experiment file could not be loaded.", e);
            }
            catch (System.IO.FileNotFoundException e)
            {
                throw new ExperimentLoadException("The experiment file has not been found.", e);
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                throw new ExperimentLoadException("The directory has not been found.", e);
            }
            catch (UriFormatException e)
            {
                throw new ExperimentLoadException("The experiment is corrupted and could not be loaded.", e);
            }
            catch (System.Xml.XmlException e)
            {
                throw new ExperimentLoadException("The experiment is corrupted and could not be loaded.", e);
            }

            return experiment;
        }


        #endregion

        #region Save

        /// <summary>
        /// Saves the experiment to specified filename.
        /// </summary>
        /// <param name="experiment">The experiment to be saved.</param>
        /// <param name="fileName">The filename.</param>
        /// <param name="watchFile">if set to <c>true</c> it adds FileSystemWatcher to monitore directory in which the file is located. It is optional, because there are situations,
        /// where experiment is being saved and loaded from temporary file folder.</param>
        /// <returns>true if saving was successful, otherwise false</returns>
        public static bool Save(IExperiment experiment, string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            string oldExperimentFilePath = experiment.ExperimentInfo.FilePath;

            bool success = false;

            try
            {
                //update its FilePath info to the new file location
                experiment.ExperimentInfo.FilePath = fileName;
                success = Save(experiment);
            }
            finally
            {
                if (success == false)
                    experiment.ExperimentInfo.FilePath = oldExperimentFilePath; //go back to old filepath
            }

            return success;
        }

        public static bool SaveToCrypt(IExperiment experiment, string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            string oldExperimentFilePath = experiment.ExperimentInfo.FilePath;

            bool success = false;

            try
            {
                //update its FilePath info to the new file location
                experiment.ExperimentInfo.FilePath = fileName;
                success = SaveToCrypt(experiment);
            }
            finally
            {
                if (success == false)
                    experiment.ExperimentInfo.FilePath = oldExperimentFilePath; //go back to old filepath
            }

            return success;
        }
        
        /// <summary>
        /// Saves the experiment to specified filename.
        /// </summary>
        /// <param name="experiment">The experiment to be saved.</param>
        /// <param name="fileName">The filename.</param>
        /// <returns>true if saving was successful, otherwise false</returns>
        private static bool Save(IExperiment experiment)
        {
            if (experiment == null)
                throw new ArgumentNullException("experiment");
            if (String.IsNullOrEmpty(experiment.ExperimentInfo.FilePath))
                throw new InvalidOperationException("experiment filepath");
            if (IsFileReadOnly(experiment.ExperimentInfo.FilePath))
                throw new System.IO.IOException("File Is Read Only");

            bool success = SerializeExperimentToXml(experiment, experiment.ExperimentInfo.FilePath);

            if (success)
            {
                experiment.ResetModifiedFlag();
            }

            return success;
        }

        // HERZUM SPRINT 3: TLAB-86
        public static bool PublishTeml(IExperiment experiment, string filePath)
        {

            if (experiment == null)
                throw new ArgumentNullException("experiment");

            if (IsFileReadOnly(filePath))
                throw new System.IO.IOException("File Is Read Only");

            bool success = SerializeExperimentToXml(experiment, filePath);

            if (success)
            {
                experiment.ResetModifiedFlag();
            }

            return success;
        }
        // END HERZUM SPRINT 3: TLAB-86

        private static bool SaveToCrypt(IExperiment experiment)
        {
            if (experiment == null)
                throw new ArgumentNullException("experiment");
            if (String.IsNullOrEmpty(experiment.ExperimentInfo.FilePath))
                throw new InvalidOperationException("experiment filepath");
            if (IsFileReadOnly(experiment.ExperimentInfo.FilePath))
                throw new System.IO.IOException("File Is Read Only");

            bool success = SerializeExperimentToStream(experiment, experiment.ExperimentInfo.FilePath);

            if (success)
            {
                experiment.ResetModifiedFlag();
            }

            return success;
        }

        /// <summary>
        /// Serializes the experiment to XML file.
        /// </summary>
        /// <param name="experiment">The experiment to be saved.</param>
        /// <param name="filename">The filename.</param>
        /// <returns>true if saving was successful, otherwise false</returns>
        private static bool SerializeExperimentToXml(IExperiment experiment, string filename)
        {
            bool success = false;

            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.NamespaceHandling = NamespaceHandling.OmitDuplicates;
                settings.OmitXmlDeclaration = true;

                //create the xml writer
                using (XmlWriter writer = XmlWriter.Create(filename, settings))
                {
                    ExperimentSerializer.SerializeExperiment(writer, experiment);
                }

                success = true;
            }
            catch (System.Security.SecurityException ex)
            {
                System.Diagnostics.Debug.Write("Security exception while serializing experiment to Xml " + ex);
            }

            return success;
        }

        private static bool SerializeExperimentToStream(IExperiment experiment, string filename)
        {
            bool success = false;

            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.NamespaceHandling = NamespaceHandling.OmitDuplicates;
                settings.OmitXmlDeclaration = true;
                settings.ConformanceLevel = ConformanceLevel.Fragment;
                settings.CloseOutput = false;

                //create the xml writer
                MemoryStream fileStream = new MemoryStream();
                using (XmlWriter writer = XmlWriter.Create(fileStream, settings))
                {
                    ExperimentSerializer.SerializeExperiment(writer, experiment);
                    writer.Flush();
                    writer.Close();

                    //we enrcypt the file before save it
                    Crypto.EncryptFile(fileStream, filename);
                }

                success = true;
            }
            catch (System.Security.SecurityException ex)
            {
                System.Diagnostics.Debug.Write("Security exception while serializing experiment to Xml " + ex);
            }

            return success;
        }

        /// <summary>
        /// Determines whether the file is read only
        /// </summary>
        /// <param name="FileName">Name of the file. </param>
        /// <returns>
        ///   <c>true</c> if [is file read only] [the specified file name]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsFileReadOnly(string FileName)
        {
            // Create a new FileInfo object.
            System.IO.FileInfo fInfo = new System.IO.FileInfo(FileName);

            //if file already exists 
            if(fInfo.Exists) {
                // Return the IsReadOnly property value.
                return fInfo.IsReadOnly;
            } 
            else 
            {
                return false;
            }
        }

        #endregion Save

        #region SaveAs

        /// <summary>
        /// Save as allows saving experiment in different location with referenced files copied. 
        /// </summary>
        /// <param name="experiment">The experiment.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="referencedFilesProcessing">The referenced files processing.</param>
        /// <returns></returns>
        public static bool SaveAs(Experiment experiment, string fileName, ReferencedFiles referencedFilesProcessing)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            string oldExperimentFilePath = experiment.ExperimentInfo.FilePath;

            bool success = false;

            try
            {
                //update its FilePath info to the new file location
                experiment.ExperimentInfo.FilePath = fileName;

                if (referencedFilesProcessing == ReferencedFiles.KEEPREFERENCE)
                {
                    //Filepath of experiment has changed so update all referenced files before saving the file
                    //only this changes the Relative properties of FilePaths and DirectoryPaths
                    //the Relative property is the only one that is serialized
                    SetNewRootLocationToReferencedFiles(experiment, true);
                }

                //we check if the experiment is a challenge and if there is a password: in ths case we must encrypt the file
                if((!string.IsNullOrEmpty(experiment.ExperimentInfo.ChallengePassword) || !string.IsNullOrEmpty(experiment.ExperimentInfo.ExperimentPassword)) && (!string.IsNullOrEmpty(experiment.ExperimentInfo.IsChallenge) && experiment.ExperimentInfo.IsChallenge.Equals("True"))){
                    success = SaveToCrypt(experiment);
                } else
                    success = Save(experiment);

                if (success == true)
                {
                    // process the components Path config values in the experiment - to update their Absolute and DataRoot properties to new location
                    // if copy has been selected copy files accordingly
                    // note KeepReference is not processed, as it was already processed before (because Relative property is being serialized.
                    switch (referencedFilesProcessing)
                    {
                        case ReferencedFiles.COPY:
                            CopyReferencedFiles(experiment, oldExperimentFilePath, false);
                            break;
                        case ReferencedFiles.COPYOVERWRITE:
                            CopyReferencedFiles(experiment, oldExperimentFilePath, true);
                            break;
                        case ReferencedFiles.IGNORE:
                            SetNewRootLocationToReferencedFiles(experiment, false);
                            break;
                    }
                }

            }
            finally
            {
                if (success == false)
                {
                    experiment.ExperimentInfo.FilePath = oldExperimentFilePath; //go back to old filepath
                    //Roll back the changes to Relative properties of FilePaths and DirectoriesPaths.
                    SetNewRootLocationToReferencedFiles(experiment, true);
                }
            }
            
            return success;
        }

        /// <summary>
        /// Method updates config relative path, so that they are relative to new location
        /// </summary>
        public static void SetNewRootLocationToReferencedFiles(Experiment experiment, bool transformRelative)
        {
            if (String.IsNullOrEmpty(experiment.ExperimentInfo.FilePath) == false)
            {
                string experimentLocation = System.IO.Path.GetDirectoryName(experiment.ExperimentInfo.FilePath);
                foreach (ExperimentNode node in experiment.Vertices)
                {
                    var metadata = node.Data.Metadata as TraceLab.Core.Components.IConfigurableAndIOSpecifiable;
                    if (metadata != null && ((metadata is TraceLab.Core.Components.ComponentTemplateMetadata) == false))
                    {
                        metadata.ConfigWrapper.SetExperimentLocationRoot(experimentLocation, transformRelative);
                    }
                }
            }
        }

        /// <summary>
        /// Copies the referenced files by experiment. Overwrite files if they already exists at new location. 
        /// Errors contain the list of files that could not have been overwritten.
        /// </summary>
        /// <param name="experiment">The experiment.</param>
        public static void CopyReferencedFiles(Experiment experiment, string oldExperimentFilePath, bool overwrite)
        {
            if (String.IsNullOrEmpty(experiment.ExperimentInfo.FilePath) == false)
            {
                string experimentLocation = System.IO.Path.GetDirectoryName(experiment.ExperimentInfo.FilePath);
                string oldExperimentLocation = System.IO.Path.GetDirectoryName(oldExperimentFilePath);
                foreach (ExperimentNode node in experiment.Vertices)
                {
                    var metadata = node.Data.Metadata as TraceLab.Core.Components.IConfigurableAndIOSpecifiable;
                    if (metadata != null && ((metadata is TraceLab.Core.Components.ComponentTemplateMetadata) == false))
                    {
                        metadata.ConfigWrapper.CopyReferencedFiles(experimentLocation, oldExperimentLocation, overwrite);
                    }
                }
            }

        }

        #endregion Save As

        #region Reload experiments on components library rescan

        /// <summary>
        /// Refreshes all experiments... needed when Components Library has been rescanned.
        /// </summary>
        /// <param name="experiment">The experiment.</param>
        /// <param name="library">The library.</param>
        /// <returns>experiment</returns>
        public static Experiment ReloadExperiment(Experiment experiment, TraceLab.Core.Components.ComponentsLibrary library)
        {
            if (experiment != null) {
                //cache current experiment file path
                string currentPath = experiment.ExperimentInfo.FilePath;

                string tempFile = System.IO.Path.GetTempFileName ();

                //reload experiment by saving and loading it
                SerializeExperimentToXml (experiment, tempFile);
                Experiment refreshedExperiment = ExperimentManager.Load (tempFile, library);

                //set m_experiment path to the actual path - from temporary path
                refreshedExperiment.ExperimentInfo.FilePath = currentPath;

                SetNewRootLocationToReferencedFiles (refreshedExperiment, false);

                return refreshedExperiment;
            } else 
                return null;
        }

        #endregion Reload experiments on components library rescan

        #region Delete File

        public static bool DeleteFile(string pathToFile) {
            if (!string.IsNullOrEmpty (pathToFile)) {
                File.Delete (pathToFile);
                return true;
            } else 
                return false;
        }

        #endregion Delete File
    }
}
