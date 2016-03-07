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
using System.Xml.Serialization;
using TraceLab.Core.Experiments;
using TraceLabSDK;
using System.Xml.XPath;
using System.Xml;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace TraceLab.Core.Components
{
    /// <summary>
    /// Represents loop scope metadata
    /// Loop scope metadata includes both its own scope sub graph and decision code that determines condition until when the scope graph should be repeatadly executed
    /// </summary>
    [XmlRoot("Metadata")]
    [Serializable]
    public class LoopScopeMetadata : ScopeBaseMetadata, IDecision
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LoopScopeMetadata"/> class.
        /// </summary>
        protected LoopScopeMetadata()
            : base()
        {
            UniqueDecisionID = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoopScopeMetadata"/> class.
        /// </summary>
        /// <param name="compositeComponentGraph">The composite component graph.</param>
        /// <param name="label">The label.</param>
        /// <param name="experimentLocationRoot">The experiment location root.</param>
        internal LoopScopeMetadata(CompositeComponentEditableGraph compositeComponentGraph, string label, string experimentLocationRoot)
            : base(compositeComponentGraph, label, experimentLocationRoot)
        {
            UniqueDecisionID = Guid.NewGuid().ToString();
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
            get { return m_uniqueDecisionId; }
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
            //note that base.ReadXml is not called directly because it is not possible to create XPathDocument twice over the XmlReader
            //XmlReader provides only forward one time read. Thus the protected base method ReadXMLInternal is called instead.

            XPathNavigator nav = ReadXmlInternal(reader);

            XPathNavigator iter = nav.SelectSingleNode("/Metadata/DecisionCode");
            
            DecisionCode = iter.Value;
        }
        
        /// <summary>
        /// Writes the XML.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            base.WriteXml(writer);

            writer.WriteElementString("DecisionCode", DecisionCode);
        }

        #endregion IXmlSerializable
        
        #region Copy & Clone

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override Metadata Clone()
        {
            var clone = new LoopScopeMetadata();
            clone.CopyFrom(this);

            return clone;
        }

        /// <summary>
        /// Performs a deep copy of the data in this object to another instance of the Metadata.
        /// </summary>
        /// <param name="other">The other.</param>
        protected override void CopyFrom(Metadata other)
        {
            base.CopyFrom(other);

            LoopScopeMetadata metadata = (LoopScopeMetadata)other;
            UniqueDecisionID = metadata.UniqueDecisionID;
            CompilationStatus = metadata.CompilationStatus;
            IsCodeDirty = metadata.IsCodeDirty;
            DecisionCode = metadata.DecisionCode;
        }

        #endregion
    }
}
