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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using TraceLabSDK;
using TraceLabSDK.Types;

namespace Preprocessor
{
    [Component("85CB9977-F2A6-426F-B3DF-BB0CCA26B276",
        "Cleanup Preprocessor",  
        "This Pre-Processor cleans up the text, eliminating all 'non characters' (punctuation marks, numbers, etc).",
        "DePaul RE Lab Team",
        "1.0",
        typeof(PreprocessorCleanUpComponentConfig))]
    [IOSpec(IOSpecType.Input, "listOfArtifacts", typeof(TraceLabSDK.Types.TLArtifactsCollection))]
    [IOSpec(IOSpecType.Output, "listOfArtifacts", typeof(TraceLabSDK.Types.TLArtifactsCollection))]
    [Tag("Preprocessors")]
    public class PreprocessorCleanUpComponent : BaseComponent
    {
        private PreprocessorCleanUpComponentConfig _config;

        public PreprocessorCleanUpComponent(ComponentLogger log)
            : base(log)
        {
            _config = new PreprocessorCleanUpComponentConfig();
            Configuration = _config;
        }

        public override void Compute()
        {
            TLArtifactsCollection listOfArtifacts = (TLArtifactsCollection)Workspace.Load("listOfArtifacts");
            
            ProcessArtifacts(listOfArtifacts, _config);

            Workspace.Store("listOfArtifacts", listOfArtifacts);
        }

        private static void ProcessArtifacts(TLArtifactsCollection listOfArtifacts, PreprocessorCleanUpComponentConfig config)
        {
            if (listOfArtifacts == null)
            {
                throw new ComponentException("Received null listofArtifacts");
            }

            foreach (TLArtifact artifact in listOfArtifacts.Values)
            {
                artifact.Text = PreprocessorCleanUp.Process(artifact.Text, config.RemoveDigits);
            }
        }
    }

    public class PreprocessorCleanUpComponentConfig
    {
        [DisplayName("Remove digits?")]
        [Description("Option to remove lone numbers from text during cleanup (ex. 1001)")]
        public PreprocessorCleanUpComponentEnum RemoveDigits { get; set; }
    }

    public enum PreprocessorCleanUpComponentEnum
    {
        No,
        Yes
    }
}
