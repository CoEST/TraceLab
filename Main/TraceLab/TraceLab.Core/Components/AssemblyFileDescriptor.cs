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
using System.Collections.ObjectModel;
using System.IO;

namespace TraceLab.Core.Components
{
    /// <summary>
    /// Sub-class of FileDescriptor for assembly files. Has a collection of MetadataDefinition
    /// for all components stored inside the assembly.
    /// </summary>
    [Serializable]
    class AssemblyFileDescriptor : FileDescriptor
    {
        #region Members

        /// <summary>
        /// Collection of MetadataDefinition for all components inside the assembly.
        /// </summary>
        private List<ComponentMetadataDefinition> m_componentsMetadata;
        public List<ComponentMetadataDefinition> MetadataCollection
        {
            get { return this.m_componentsMetadata; }
        }

        /// <summary>
        /// Corresponding map of new Guid to old Guid for backward compatibility with old components.
        /// </summary>
        private Dictionary<string, string> m_newGuidToOldGuid;
        public Dictionary<string, string> NewGuidToOldGuid
        {
            get { return this.m_newGuidToOldGuid; }
        }

        #endregion Members

        public AssemblyFileDescriptor(string filePath) : base(filePath)
        {
            this.m_componentsMetadata = new List<ComponentMetadataDefinition>();
            this.m_newGuidToOldGuid = new Dictionary<string, string>();
        }
    }
}
