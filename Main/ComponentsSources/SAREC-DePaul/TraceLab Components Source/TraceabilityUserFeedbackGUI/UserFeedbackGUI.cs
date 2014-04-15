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
using TraceLabSDK.Component.Config;
using System.Collections.Generic;
using System.ComponentModel;

namespace TraceabilityUserFeedbackGUI
{
    [Component( Name = "User Feedback GUI",
                Description = "",
                Author = "",
                Version = "1.0",
                ConfigurationType = typeof(Config) 
                )]

    [IOSpec(IOType = IOSpecType.Input, Name = "originalSourceArtifacts", DataType = typeof(TLArtifactsCollection), Description = "Oryginal Source Artifacts")]
    [IOSpec(IOType = IOSpecType.Input, Name = "originalTargetArtifacts", DataType = typeof(TLArtifactsCollection), Description = "Original Target Artifacts")]
    [IOSpec(IOType = IOSpecType.Input, Name = "similarityMatrix", DataType = typeof(TLSimilarityMatrix), Description = "Similarity Matrix")]

 //   [IOSpec(IOType = IOSpecType.Output, Name = "OutpuySimilarityMatrixUserFeedback", DataType = typeof(SimilarityMatrixUserFeedback), Description = "Similarity Matrix User Feedback output for saving or further processing")]

    [IOSpec(IOType = IOSpecType.Output, Name = "OutpuySimilarityMatrixUserFeedback", DataType = typeof(TLSimilarityMatrix), Description = "Similarity Matrix User Feedback output for saving or further processing")]
   
    //[IOSpec(IOType = IOSpecType.Output, Name = "outputName", DataType = typeof(int))]
    
    public class UserFeedbackGUI : BaseComponent
    {
        private Config config;
        private SimilarityMatrixUserFeedback extendedSimMatrix;

        public UserFeedbackGUI(ComponentLogger log) : base(log) 
        {
            config = new Config();
            Configuration = config;
        }

        
        public override void Compute()
        {

            Logger.Trace("Start component UserFeedbackGUI");
       
            TLArtifactsCollection originalSrcList = (TLArtifactsCollection)Workspace.Load("originalSourceArtifacts");
            TLArtifactsCollection originalTrgList = (TLArtifactsCollection)Workspace.Load("originalTargetArtifacts");
            var matrix = (TLSimilarityMatrix)Workspace.Load("similarityMatrix");
            if (matrix == null)
                throw new ComponentException("Received matrix is null.");
           if (originalSrcList == null)
               throw new ComponentException("Received source artifacts list is null.");
           if (originalTrgList == null)
               throw new ComponentException("Received target artifacts list is null.");

            extendedSimMatrix = matrix as SimilarityMatrixUserFeedback;
            if (extendedSimMatrix == null)
            {    
                extendedSimMatrix = new SimilarityMatrixUserFeedback(matrix);
            }

            GUIForm theForm = new GUIForm(originalSrcList, originalTrgList, extendedSimMatrix, config);
            theForm.ShowDialog();

            Workspace.Store("OutpuySimilarityMatrixUserFeedback", theForm.ExtendedSimilarityMatrix);

            if (config.OutSavePath != null)
            {
                saveToFile();
            }
        }

        private bool saveToFile()
        {
            string outPath = this.config.OutSavePath;

            if (outPath != null)
            {
                Logger.Trace("Saving work file to: " + outPath);

                System.Xml.XmlTextWriter xmlWriter = new System.Xml.XmlTextWriter(outPath, null);
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("root");
                    extendedSimMatrix.WriteXml(xmlWriter);
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Flush();
                
                xmlWriter.Close();
            }
            else
            {
                Logger.Trace("File not saved as no file path has been provided.");
            }
            return true;
        }
    }
}