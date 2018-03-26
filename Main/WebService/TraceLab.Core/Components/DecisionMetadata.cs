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
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml.Serialization;
using System.Xml.XPath;
using TraceLabSDK;
using TraceLab.Core.Settings;

namespace TraceLab.Core.Components
{
    /// <summary>
    /// Decision metadata represents the model class for decision node. 
    /// </summary>
    [XmlRoot("Metadata")]
    [Serializable]
    public class DecisionMetadata : Metadata, IDecision, ILoggable, INotifyPropertyChanged, IXmlSerializable
    {
        #region Constructor

        /// <summary>
        /// Prevents a default instance of the <see cref="DecisionMetadata"/> class from being created.
        /// </summary>
        private DecisionMetadata() 
        {
            UniqueDecisionID = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DecisionMetadata"/> class.
        /// </summary>
        /// <param name="label">The label.</param>
        public DecisionMetadata(string label)
        {
            UniqueDecisionID = Guid.NewGuid().ToString();
            Label = label;
        }

        #endregion

        #region IDecision Members

        private string m_decisionCode;
        /// <summary>
        /// Gets or sets the decision code.
        /// </summary>
        /// <value>
        /// The decision code.
        /// </value>
        public string DecisionCode
        {
            get
            {
                return m_decisionCode;
            }
            set
            {
                if (m_decisionCode != value)
                {
                    m_decisionCode = value;
                    NotifyPropertyChanged("DecisionCode");
                    CompilationStatus = CompilationStatus.Default;
                }
            }
        }

        private bool m_isCodeDirty;
        /// <summary>
        /// Gets or sets a value indicating whether the decision code dirty.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the decision code dirty; otherwise, <c>false</c>.
        /// </value>
        public bool IsCodeDirty
        {
            get
            {
                return m_isCodeDirty;
            }
            set
            {
                if (m_isCodeDirty != value)
                {
                    m_isCodeDirty = value;
                    NotifyPropertyChanged("IsCodeDirty");

                    if (m_isCodeDirty)
                        CompilationStatus = TraceLab.Core.Components.CompilationStatus.Default;
                }
            }
        }

        private CompilationStatus m_compilationStatus = CompilationStatus.Default;
        /// <summary>
        /// Gets or sets the compilation status.
        /// </summary>
        /// <value>
        /// The compilation status.
        /// </value>
        public CompilationStatus CompilationStatus
        {
            get
            {
                return m_compilationStatus;
            }
            set
            {
                if (m_compilationStatus != value)
                {
                    m_compilationStatus = value;
                    NotifyPropertyChanged("CompilationStatus");
                }
            }
        }

        /// <summary>
        /// Occurs when request latest code event has been fired
        /// </summary>
        public event EventHandler RequestLatestCode;

        /// <summary>
        /// Fires the request latest code event
        /// </summary>
        public void FireRequestLatestCode()
        {
            if (RequestLatestCode != null)
                RequestLatestCode(this, new EventArgs());
        }

        private string m_uniqueDecisionId;
        /// <summary>
        /// Gets the unique decision ID.
        /// </summary>
        public string UniqueDecisionID
        {
            get { return m_uniqueDecisionId;  }
            private set
            {
                if (m_uniqueDecisionId != value)
                {
                    m_uniqueDecisionId = value;
                    m_classname = "DecisionModule_" + m_uniqueDecisionId.Replace("-", "_");
                    m_sourceAssembly = System.IO.Path.Combine(TraceLab.Core.Decisions.DecisionCompilationRunner.DecisionDirectoryPath, m_uniqueDecisionId + ".dll");
                    NotifyPropertyChanged("UniqueDecisionID");
                    NotifyPropertyChanged("Classname");
                    NotifyPropertyChanged("SourceAssembly");
                }
            }
        }

        private string m_classname;
        /// <summary>
        /// Gets or sets the classname.
        /// It is used by both DecisionModuleCompilationRunner and DecisionLoader
        /// </summary>
        /// <value>
        /// The assembly.
        /// </value>
        public string Classname
        {
            get
            {
                return m_classname;
            }
        }

        private string m_sourceAssembly;
        /// <summary>
        /// Gets or sets the assembly.
        /// It is temporary dynamic assembly into which decision module is compiled to.
        /// It is used by both DecisionModuleCompilationRunner and DecisionLoader
        /// </summary>
        /// <value>
        /// The assembly.
        /// </value>
        public string SourceAssembly
        {
            get
            {
                return m_sourceAssembly;
            }
        }

        #endregion
        
        #region IXmlSerializable

        /// <summary>
        /// Reads the XML.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public override void ReadXml(System.Xml.XmlReader reader)
        {
            Label = reader.GetAttribute("Label");

            //read attribute indicating if component should wait for all predecessors
            var wait = reader.GetAttribute("WaitsForAllPredecessors", String.Empty);
            if (String.IsNullOrEmpty(wait) || (wait != Boolean.TrueString && wait != Boolean.FalseString)) //if value has not been found set it to true
                WaitsForAllPredecessors = true; //default value
            else
                WaitsForAllPredecessors = Convert.ToBoolean(wait);

            XPathDocument doc = new XPathDocument(reader);
            XPathNavigator nav = doc.CreateNavigator();

            XPathNavigator iter = nav.SelectSingleNode("/Metadata/DecisionCode");
            DecisionCode = iter.Value;
        }

        /// <summary>
        /// Writes the XML.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("type", this.GetType().GetTraceLabQualifiedName());
            writer.WriteAttributeString("Label", Label);
            writer.WriteAttributeString("WaitsForAllPredecessors", WaitsForAllPredecessors.ToString());
            writer.WriteElementString("DecisionCode", DecisionCode);
        }

        /// <summary>
        /// Gets the schema.
        /// </summary>
        /// <returns></returns>
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
            info.AddValue("m_decisionCode", m_decisionCode);
            info.AddValue("m_uniqueDecisionId", m_uniqueDecisionId);
            info.AddValue("m_classname", m_classname);
            info.AddValue("m_sourceAssembly", m_sourceAssembly);
        }

        #endregion

        #region Deserialization Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DecisionMetadata"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected DecisionMetadata(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            m_decisionCode = (string)info.GetValue("m_decisionCode", typeof(string));
            m_uniqueDecisionId = (string)info.GetValue("m_uniqueDecisionId", typeof(string));
            m_classname = (string)info.GetValue("m_classname", typeof(string));
            m_sourceAssembly = (string)info.GetValue("m_sourceAssembly", typeof(string));
        }

        #endregion

        #region Copy & Clone

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override Metadata Clone()
        {
            var clone = new DecisionMetadata();
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

            DecisionMetadata metadata = (DecisionMetadata)other;
            UniqueDecisionID = metadata.UniqueDecisionID;
            CompilationStatus = metadata.CompilationStatus;
            IsCodeDirty = metadata.IsCodeDirty;
            DecisionCode = metadata.DecisionCode;
        }

        #endregion

        #region Modification

        private bool m_isModified;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is modified.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is modified; otherwise, <c>false</c>.
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
