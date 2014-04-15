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
using System.ComponentModel;
using TraceLabSDK.Component.Config;

namespace CoestDatasetsLoader
{
    /// <summary>
    /// Represents the config for AnswerSetImporter and ArtifactsImporter
    /// </summary>
    public class ImporterConfig
    {
        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        [DisplayName("File to import")]
        [Description("Specifies the xml file from which artifacts are going to be imported from, if listOfArtifacts has not been found in Workspace.")]
        public TraceLabSDK.Component.Config.FilePath FilePath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [trim element values].
        /// </summary>
        /// <value>
        /// <c>true</c> if [trim element values]; otherwise, <c>false</c>.
        /// </value>
        [DisplayName("Trim elements values")]
        [Description("If checked all values read from xml are going to be trimmed.")]
        public bool TrimElementValues
        {
            get;
            set;
        }
    }
}
