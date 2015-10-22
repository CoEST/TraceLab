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
//

// HERZUM SPRINT 1.0 CLASS

using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using TraceLabSDK;

using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Security.Permissions;
using System.Xml.XPath;
using TraceLab.Core.Settings;

namespace TraceLab.Core.Components
{
    [XmlRoot("Metadata")]
    [Serializable]
    public class CommentMetadata : Metadata
    {

        #region IComment Members

        private string m_comment;
        /// <summary>
        /// Gets or sets the decision code.
        /// </summary>
        /// <value>
        /// The decision code.
        /// </value>
        public string Comment
        {
            get
            {
                return m_comment;
            }
            set
            {
                if (m_comment != value)
                {
                    m_comment = value;
                    NotifyPropertyChanged("Comment");
                }
            }
        }

        #endregion

        public CommentMetadata()
        {
            Label = "Comment";
        }

        public CommentMetadata(string label)
        {
            Label = label;
        }

        #region Deserialization Constructor


        protected CommentMetadata(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            m_comment = (string)info.GetValue("m_comment", typeof(string));
        }

        #endregion



        #region IXmlSerializable

        public override void ReadXml(System.Xml.XmlReader reader)
        {
            Label = reader.GetAttribute("Label");
            Comment = reader.GetAttribute("Comment");
        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("type", this.GetType().GetTraceLabQualifiedName());
            writer.WriteAttributeString("Label", Label);
            writer.WriteAttributeString("Comment", Comment);
        }

        public override System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        #endregion

        #region ISerialization Implementation

        /// <summary>
        /// Gets the object data. Needed for cross app domain calls.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("m_comment", m_comment);
        }

        #endregion

        #region Copy & Clone

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override Metadata Clone()
        {
            var clone = new CommentMetadata();
            clone.CopyFrom(this);
            return clone;
        }

        /// <summary>
        /// Performs a deep copy of the data in this object to another instance of the Metadata.
        /// </summary>
        /// <param name="other">The other.</param>
        protected override void CopyFrom(Metadata other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            base.CopyFrom(other);

            CommentMetadata metadata = (CommentMetadata)other;
            m_comment = metadata.Comment;
        }

        #endregion

        #region Modification

        private bool m_isModified;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is modified.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is modified; otherwise, <c>false</c>.
        /// </value>
        public override bool IsModified
        {
            get
            {
                return base.IsModified;
            }
            set
            {
                m_isModified = value;
                base.IsModified = value;
            }
        }

        /// <summary>
        /// Calculates the modification.
        /// </summary>
        /// <returns></returns>
        protected override bool CalculateModification()
        {
            return m_isModified | base.CalculateModification();
        }

        /// <summary>
        /// Resets the modified flag.
        /// </summary>
        public override void ResetModifiedFlag()
        {
            base.ResetModifiedFlag();
            m_isModified = false;
        }

        #endregion

    }
}


