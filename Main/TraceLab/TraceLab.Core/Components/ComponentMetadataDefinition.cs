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
using System.Xml.Serialization;
using System.Collections.Generic;

namespace TraceLab.Core.Components
{
    /// <summary>
    /// Represents component metadata definition.
    /// Component metadata definition consist of all defining elements such as IOSpecDefinition, ConfigurationDefnition, Default Label
    /// without the actual values.
    /// 
    /// It is ComponentMetadata that has IOSpec refering to IOSpecDefinitoin, ConfigWrapper refering to ConfigurationDefinition. 
    /// 
    /// Developer can think of the relation between ComponentMetadataDefinition and ComponentMetadata as a similar relation between class and object.
    /// 
    /// There is only one ComponentMetadataDefinition to define each component, but there can be several instances of component with their ComponentMetadata in the experiment.
    /// </summary>
    [Serializable]
    public class ComponentMetadataDefinition : MetadataDefinition, IMetadataWithIOSpecDefinition
    {
        #region Constructor

        /// <summary>
        /// Prevents a default instance of the <see cref="ComponentMetadataDefinition"/> class from being created.
        /// </summary>
        private ComponentMetadataDefinition() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentMetadataDefinition"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="classname">The classname.</param>
        public ComponentMetadataDefinition(string id, string assembly, string classname) : base(id, assembly, classname)
        {
            IOSpecDefinition = new IOSpecDefinition();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentMetadataDefinition"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="classname">The classname.</param>
        /// <param name="iospec">The iospec.</param>
        /// <param name="label">The label.</param>
        /// <param name="version">The version.</param>
        /// <param name="description">The description.</param>
        /// <param name="author">The author.</param>
        /// <param name="language">The language.</param>
        /// <param name="configurationDefinition">The configuration definition.</param>
        /// <param name="tags">The tags.</param>
        public ComponentMetadataDefinition(string id, string assembly, string classname, IOSpecDefinition iospec, 
                                           string label, string version, string description, string author, Language language,
                                           ConfigWrapperDefinition configurationDefinition, ComponentTags tags, List<DocumentationLink> documentationLinks)
                                         : base(id, assembly, classname, label, version, description, author, tags, documentationLinks)
        {
            Language = language;
            IOSpecDefinition = iospec;
            if (configurationDefinition != null)
            {
                ConfigurationWrapperDefinition = configurationDefinition;
                ConfigurationType = configurationDefinition.ConfigurationTypeFullName;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the IO spec definition.
        /// </summary>
        /// <value>
        /// The IO spec definition.
        /// </value>
        [XmlIgnore]
        public IOSpecDefinition IOSpecDefinition
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public Language Language {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the configuration wrapper definition.
        /// </summary>
        /// <value>
        /// The configuration wrapper definition.
        /// </value>
        [XmlIgnore]
        internal ConfigWrapperDefinition ConfigurationWrapperDefinition
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the configuration.
        /// </summary>
        /// <value>
        /// The type of the configuration.
        /// </value>
        [XmlIgnore]
        public String ConfigurationType
        {
            get;
            set;
        }

        #endregion
    }

    public enum Language { NET, JAVA }
}
