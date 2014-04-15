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
using TraceLabSDK.Component.Config;
using System.IO;

namespace TraceabilityUserFeedbackGUI
{
 
    /// <summary>
    /// is a direct extension of the TLSimilarityMatrix type.
    /// It provides extra fields ...................................................will write something really cool here :]
    /// </summary>
    [Component("SimilarityMatrixUserFeedbackExporter",
    "Exports the work done in the SimilarityMatrixUserFeedback component.",
    "DePaul RE Lab Team",
    "1.0",
    typeof(SimilarityMatrixUserFeedbackExporterConfig))]
    [IOSpec(IOSpecType.Input, "SimilaritymatrixUserFeedback", typeof(TraceabilityUserFeedbackGUI.SimilarityMatrixUserFeedback))]
    [Tag("Exporters")]
    public class SimilarityMatrixUserFeedbackExporter : BaseComponent
    {
        private SimilarityMatrixUserFeedbackExporterConfig config; 

        public SimilarityMatrixUserFeedbackExporter(ComponentLogger log): base(log)
        {
            config = new SimilarityMatrixUserFeedbackExporterConfig();
            this.Configuration = config;
        }
    
        public override void  Compute()
        {
            Logger.Trace("Start Export of SimilarityMatrixUserFeedback");

            SimilarityMatrixUserFeedback SimMatrUserFeedback = (SimilarityMatrixUserFeedback)Workspace.Load("SimilaritymatrixUserFeedback");

            //get file name
            // use xml serializer to write to file
            // close file
            //done
        }
}


    public class SimilarityMatrixUserFeedbackExporterConfig
    {
        private FilePath m_path;
        public FilePath Path
        {
            get
            {
                return m_path;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Path cannot be null.");
                }
                if (!System.IO.Path.IsPathRooted(value))
                {
                    throw new ArgumentException("Absolute path is required.");
                }
                if (!File.Exists(value))
                {
                    throw new ArgumentException("File does not exist.");
                }
                m_path = value;
            }
        }
    }
    



}
