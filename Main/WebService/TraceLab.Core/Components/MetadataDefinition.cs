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
using System.Collections;
using System.Collections.Generic;

namespace TraceLab.Core.Components
{
    /// <summary>
    /// Represents base class of metadata definition.
    /// Metadata definition defines the component - in essence this is like a class, while metadata is like an object.
    /// There is only one MetadataDefinition to define each component, but there can be several instances of component with their ComponentMetadata in the experiment.
    /// Basically MetadataDefinition are the ones that user can see in the components library. When definition is dragged into experiment the component metadata is created
    /// with default values and corresponding instance of metadata.
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(ComponentMetadataDefinition))]
    [XmlInclude(typeof(DecisionMetadataDefinition))]
    public abstract class MetadataDefinition
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataDefinition"/> class.
        /// </summary>
        protected MetadataDefinition() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataDefinition"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        protected MetadataDefinition(string id) {
            ID = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataDefinition"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="classname">The classname.</param>
        protected MetadataDefinition(string id, string assembly, string classname) : this(id)
        {
            Assembly = assembly;
            Classname = classname;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataDefinition"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="classname">The classname.</param>
        /// <param name="label">The label.</param>
        /// <param name="version">The version.</param>
        /// <param name="description">The description.</param>
        /// <param name="author">The author.</param>
        /// <param name="tags">The tags.</param>
        protected MetadataDefinition(string id, string assembly, string classname, string label, string version, string description, string author, 
                                     ComponentTags tags, List<DocumentationLink> documentationLinks)
            : this(id, assembly, classname)
        {
            Version = version;
            Label = label;
            Description = description;
            Author = author;
            ID = id;
            Assembly = assembly;
            Classname = classname;
            Tags = tags;

            documentationLinks.Sort(DocumentationLink.SortLinksByOrderComparer);
            m_documentationLinks = documentationLinks;
        }

        #endregion

        #region Properties

        private string m_id;
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>
        /// The ID.
        /// </value>
        public string ID
        {
            get
            {
                return m_id;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("ID cannot be null");
                }
                m_id = value;
            }
        }

        private string m_label;
        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public string Label
        {
            get { return m_label; }
            set { m_label = value; }
        }


        private string m_author;

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        public string Author
        {
            get { return m_author; }
            set { m_author = value; }
        }

        private string m_description;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }


        private string m_version;

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version
        {
            get { return m_version; }
            set { m_version = value; }
        }

        private string m_classname;
        /// <summary>
        /// Gets or sets the classname.
        /// </summary>
        /// <value>
        /// The classname.
        /// </value>
        public string Classname
        {
            get
            {
                return m_classname;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Classname cannot be null");
                }
                m_classname = value;
            }
        }

        private string m_assembly;
        /// <summary>
        /// Gets or sets the assembly.
        /// </summary>
        /// <value>
        /// The assembly.
        /// </value>
        public String Assembly
        {
            get
            {
                return m_assembly;
            }
            set
            {
                m_assembly = value;
            }
        }

        private ComponentTags m_tags;
        /// <summary>
        /// Gets or sets the tags - the tags represents categories which user can locate components by in the components library
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public ComponentTags Tags
        {
            get
            {
                return m_tags; 
            }
            set
            {
                if (m_tags != value)
                {
                    m_tags = value;
                }
            }
        }

        private List<DocumentationLink> m_documentationLinks;

        public IEnumerable<DocumentationLink> DocumentationLinks
        {
            get { return m_documentationLinks; }
        }

        #endregion
    }
}
