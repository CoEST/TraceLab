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
using System.IO;
using System.ComponentModel;
using TraceLabSDK.Component.Config;

namespace Importer
{
    /// <summary>
    /// Represents the config for Importers
    /// </summary>
    //[Serializable]
    public class ImporterConfig
    {
        public ImporterConfig() { }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        private TraceLabSDK.Component.Config.FilePath m_path;
        [DisplayName("File to import")]
        public TraceLabSDK.Component.Config.FilePath Path
        {
            get
            {
                return this.m_path;
            }
            set
            {
                this.m_path = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to trim element values.
        /// </summary>
        /// <value>
        /// <c>true</c> if [trim element values]; otherwise, <c>false</c>.
        /// </value>
        private bool m_trim;
        [DisplayName("Trim Element Values")]
        public bool TrimElementValues
        {
            get
            {
                return this.m_trim;
            }
            set
            {
                this.m_trim = value;
            }
        }
    }
}
