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
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using TraceLab.Core.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections;
using System.Text.RegularExpressions;

namespace TraceLab.Core.Experiments
{
    [Serializable]
    public class ExperimentInfo : INotifyPropertyChanged, IXmlSerializable, ISerializable, IModifiable
    {
        protected const long CurrentVersion = 2;

        public ExperimentInfo ()
            : this(Guid.NewGuid())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExperimentInfo"/> class from a similar item.
        /// </summary>
        /// <param name="pInfo">Another experiment info to copy basic information.</param>
        public ExperimentInfo (ExperimentInfo pInfo)
        {
            this.m_name = pInfo.m_name;
            this.m_author = pInfo.m_author;
            this.m_contributors = pInfo.m_contributors;
            this.m_description = pInfo.m_description;

            //Herzum Sprint 2 - Challenge TLAB-66
            this.m_isChallenge = pInfo.m_isChallenge;
            this.m_tags = pInfo.m_tags;
        }

        internal ExperimentInfo (Guid guid)
        {
            GuidId = guid;
        }
        #region Members
        /// <summary>
        /// Experiment's ID
        /// </summary>
        public string Id {
            get {
                return GuidId.ToString ();
            }
            protected set {
                GuidId = Guid.Parse (value);
            }
        }

        private Guid m_guid;

        /// <summary>
        /// The Guid version of the Id for this m_experiment.
        /// </summary>
        [XmlIgnore]
        public Guid GuidId {
            get { return m_guid; }
            set {
                if (m_guid.Equals (value) == false) {
                    m_guid = value;
                    OnPropertyChanged ("GuidId");
                    OnPropertyChanged ("Id");
                }
            }
        }

        /// <summary>
        /// Experiment's name
        /// </summary>
        private string m_name;

        [XmlElement("Name")]
        public string Name {
            get { return m_name; }
            set {
                if (m_name != value) {
                    m_name = value;
                    OnPropertyChanged ("Name");
                    if (this.m_isBeingRead == false) {
                        IsModified = true;
                    }
                }
            }
        }

        /// <summary>
        /// tells wether the experiment is a challenge or not
        /// </summary>
        /// TLAB-66 Herzum Challenge
        private string m_isChallenge;
        [XmlElement("IsChallenge")]
        public string IsChallenge {
            get { return m_isChallenge; }
            set {
                if (m_isChallenge != value) {
                    m_isChallenge = value;
                    OnPropertyChanged ("IsChallenge");
                    if (this.m_isBeingRead == false) {
                        IsModified = true;
                    }
                }
            }
        }

        //TLAB-65
        private string m_experimentResultsUnitname;
        [XmlElement("ExperimentResultsUnitname")]
        public string ExperimentResultsUnitname {
            get { return m_experimentResultsUnitname; }
            set {
                if (m_experimentResultsUnitname != value) {
                    m_experimentResultsUnitname = value;
                    OnPropertyChanged ("ExperimentResultsUnitname");
                    if (this.m_isBeingRead == false) {
                        IsModified = true;
                    }
                }
            }
        }


        // HERZUM SPRINT 3: TLAB-86
        private string m_experimentResultsUnittype;
        [XmlElement("ExperimentResultsUnittype")]
        public string ExperimentResultsUnittype {
            get { return m_experimentResultsUnittype; }
            set {
                if (m_experimentResultsUnittype != value) {
                    m_experimentResultsUnittype = value;
                    OnPropertyChanged ("ExperimentResultsUnittype");
                    if (this.m_isBeingRead == false) {
                        IsModified = true;
                    }
                }
            }
        }

        private string m_experimentResultsUnitvalue;
        [XmlElement("ExperimentResultsUnitvalue")]
        public string ExperimentResultsUnitvalue {
            get { return m_experimentResultsUnitvalue; }
            set {
                if (m_experimentResultsUnitvalue != value) {
                    m_experimentResultsUnitvalue = value;
                    OnPropertyChanged ("ExperimentResultsUnitvalue");
                    if (this.m_isBeingRead == false) {
                        IsModified = true;
                    }
                }
            }
        }

        // END HERZUM SPRINT 3: TLAB-86

        //TLAB-69   
        private string m_challengePassword;
        [XmlElement("ChallengePassword")]
        public string ChallengePassword {
            get { return m_challengePassword; }
            set {
                if (m_challengePassword != value) {
                    m_challengePassword = value;
                    OnPropertyChanged ("ChallengePassword");
                    if (this.m_isBeingRead == false) {
                        IsModified = true;
                    }
                }
            }
        }
       
        //TLAB-69   
        private string m_experimentPassword;
        [XmlElement("ExperimentPassword")]
        public string ExperimentPassword {
            get { return m_experimentPassword; }
            set {
                if (m_experimentPassword != value) {
                    m_experimentPassword = value;
                    OnPropertyChanged ("ExperimentPassword");
                    if (this.m_isBeingRead == false) {
                        IsModified = true;
                    }
                }
            }
        }

        //TLAB-69   
        private string m_publishChallenge;
        [XmlElement("PublishChallenge")]
        public string PublishChallenge {
            get { return m_publishChallenge; }
            set {
                if (m_publishChallenge != value) {
                    m_publishChallenge = value;
                    OnPropertyChanged ("PublishChallenge");
                    if (this.m_isBeingRead == false) {
                        IsModified = true;
                    }
                }
            }
        }

        //TLAB-69
        private string m_publishBaseline;
        [XmlElement("PublishBaseline")]
        public string PublishBaseline {
            get { return m_publishBaseline; }
            set {
                if (m_publishBaseline != value) {
                    m_publishBaseline = value;
                    OnPropertyChanged ("PublishBaseline");
                    if (this.m_isBeingRead == false) {
                        IsModified = true;
                    }
                }
            }
        }

        //TLAB-69
        private string m_deadline;
        [XmlElement("Deadline")]
        public string Deadline {
            get { return m_deadline; }
            set {
                if (m_deadline != value) {
                    m_deadline = value;
                    OnPropertyChanged ("Deadline");
                    if (this.m_isBeingRead == false) {
                        IsModified = true;
                    }
                }
            }
        }

        /// <summary>
        /// Experiment's tags
        /// </summary>
        /// TLAB-66 Herzum Challenge
        private string m_tags;

        [XmlElement("Tags")]
        public string Tags {
            get { return m_tags; }
            set {
                if (m_tags != value) {
                    m_tags = value;
                    OnPropertyChanged ("Tags");
                    if (this.m_isBeingRead == false) {
                        IsModified = true;
                    }
                }
            }
        }

        /// <summary>
        /// Experiment's graph drawing algorithm
        /// </summary>
        private string m_layoutName;

        [XmlElement("LayoutName")]
        public string LayoutName {
            get { return m_layoutName; }
            set {
                if (m_layoutName != value) {
                    m_layoutName = value;
                    OnPropertyChanged ("LayoutName");
                    if (this.m_isBeingRead == false) {
                        IsModified = true;
                    }
                }
            }
        }

        /// <summary>
        /// Experiment's authors
        /// </summary>
        private string m_author;

        [XmlElement("Author")]
        public string Author {
            get { return m_author; }
            set {
                if (m_author != value) {
                    m_author = value;
                    OnPropertyChanged ("Author");
                    if (this.m_isBeingRead == false) {
                        IsModified = true;
                    }
                }
            }
        }

        /// <summary>
        /// Experiment's contributors list
        /// </summary>
        private string m_contributors;

        [XmlElement("Contributors")]
        public string Contributors {
            get { return m_contributors; }
            set {
                if (m_contributors != value) {
                    m_contributors = value;
                    OnPropertyChanged ("Contributors");
                    if (this.m_isBeingRead == false) {
                        IsModified = true;
                    }
                }
            }
        }

        /// <summary>
        /// Experiment's description
        /// </summary>
        private string m_description;

        [XmlElement("Description")]
        public string Description {
            get { return m_description; }
            set {
                if (m_description != value) {
                    m_description = value;
                    OnPropertyChanged ("Description");
                    if (this.m_isBeingRead == false) {
                        IsModified = true;
                    }
                }
            }
        }

        /// <summary>
        /// Filepath for experiment location
        /// </summary>
        private string m_filePath;

        [XmlIgnore]
        public string FilePath {
            get { return m_filePath; }
            set {
                if (m_filePath != value) {
                    m_filePath = value;
                    OnPropertyChanged ("FilePath");
                    if (this.m_isBeingRead == false) {
                        IsModified = true;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        private bool m_isBeingRead = false;
        #endregion
        [NonSerialized]
        private PropertyChangedEventHandler m_propertyChanged;

        public event PropertyChangedEventHandler PropertyChanged {
            add {
                m_propertyChanged += value;
            }
            remove {
                m_propertyChanged -= value;
            }
        }

        protected void OnPropertyChanged (string property)
        {
            OnPropertyChanged (new PropertyChangedEventArgs (property));
        }

        private void OnPropertyChanged (PropertyChangedEventArgs e)
        {
#if _DEBUG
            Assert.IsTrue(this.GetType().GetProperty(property) != null);
#endif

            if (m_propertyChanged != null) {
                m_propertyChanged (this, e);
            }
        }
        #region Xml Serialization
        public System.Xml.Schema.XmlSchema GetSchema ()
        {
            return null;
        }

        public virtual void ReadXml (System.Xml.XmlReader topReader)
        {
            this.m_isBeingRead = true;

            string infoName = topReader.Name;
            XmlReader reader = topReader.ReadSubtree ();
            XPathDocument doc = new XPathDocument (reader);
            var nav = doc.CreateNavigator ();
            XPathNavigator iter = nav.SelectSingleNode (String.Format ("/{0}/Version", infoName));
            if (iter == null) {
                throw new XmlException ("ExperimentInfo is missing version information.");
            } else {
                long ver = iter.ValueAsLong;
                if (ver == CurrentVersion) {
                    ReadCurrentVersion (nav, infoName);
                } else if (ver == 1) {
                    ReadVersion1 (nav, infoName);
                } else {
                    throw new InvalidOperationException ("ExperimentInfo has an invalid version number");
                }
            }

            this.m_isBeingRead = false;
        }

        protected virtual void ReadCurrentVersion (XPathNavigator nav, string infoName)
        {
            ReadVersion1 (nav, infoName);
            //var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(ObservableCollection<string>), Type.EmptyTypes);
            //var iter = nav.SelectSingleNode(string.Format("/{0}/PackageReferences", infoName));
            //if (iter != null)
            //{
            //    var reader = iter.ReadSubtree();
            //    m_packageReferences = (ObservableCollection<string>)serializer.Deserialize(reader);
            //}
        }

        protected virtual void ReadVersion1 (XPathNavigator nav, string infoName)
        {
            Id = ReadString (nav, String.Format ("/{0}/Id", infoName));
            Name = ReadString (nav, String.Format ("/{0}/Name", infoName));
            LayoutName = ReadString (nav, String.Format ("/{0}/LayoutName", infoName));
            Author = ReadString (nav, String.Format ("/{0}/Author", infoName));
            Contributors = ReadString (nav, String.Format ("/{0}/Contributors", infoName));
            Description = ReadString (nav, String.Format ("/{0}/Description", infoName));

            //TLAB-65
            IsChallenge = ReadString (nav, String.Format ("/{0}/IsChallenge", infoName));
            PublishChallenge = ReadString (nav, String.Format ("/{0}/PublishChallenge", infoName));
            PublishBaseline = ReadString (nav, String.Format ("/{0}/PublishBaseline", infoName));
            Deadline = ReadString (nav, String.Format ("/{0}/Deadline", infoName));
            ChallengePassword = ReadString (nav, String.Format ("/{0}/ChallengePassword", infoName));
            ExperimentPassword = ReadString (nav, String.Format ("/{0}/ExperimentPassword", infoName));
            ExperimentResultsUnitname = ReadString (nav, String.Format ("/{0}/ExperimentResultsUnitname", infoName));
            // HERZUM SPRINT 3: TLAB-86
            ExperimentResultsUnittype = ReadString (nav, String.Format ("/{0}/ExperimentResultsUnittype", infoName));
            ExperimentResultsUnitvalue = ReadString (nav, String.Format ("/{0}/ExperimentResultsUnitvalue", infoName));
            // END HERZUM SPRINT 3: TLAB-86
            Tags = ReadTags (nav);
        }

        //TLAB-66
        protected string ReadTags (XPathNavigator nav)
        {
            string listOfTags="";
            XPathNodeIterator iter = nav.Select ("//ExperimentInfo/Tags/Tag");

            while (iter.MoveNext()) {
                string item = iter.Current.Value;
                listOfTags += item + ", ";
            }

            if (!String.IsNullOrEmpty (listOfTags)) {
                listOfTags = listOfTags.Remove(listOfTags.LastIndexOf(","));
            }
            return listOfTags;
        }

        protected string ReadString (XPathNavigator nav, string path)
        {
            string value;
            var iter = nav.SelectSingleNode (path);
            if (iter != null) {
                value = iter.Value;
            } else {
                value = null;
            }

            return value;
        }

        /// <summary>
        /// Writes out the ExperimentInfo in XML in the current version.
        /// </summary>
        /// <param name="writer"></param>
        public virtual void WriteXml (System.Xml.XmlWriter writer)
        {
            // Always write out the version and ID.
            writer.WriteElementString ("Version", CurrentVersion.ToString (CultureInfo.CurrentCulture));
            writer.WriteElementString ("Id", Id);
           
            ///TLAB-66
            if (!string.IsNullOrEmpty (IsChallenge)) {
                writer.WriteElementString ("IsChallenge", IsChallenge); 
            } else {
                writer.WriteElementString ("IsChallenge", "False"); 
            }

            ///TLAB-65
            if (!string.IsNullOrEmpty (ChallengePassword)) {
                writer.WriteElementString ("ChallengePassword", ChallengePassword); 
            }

            ///TLAB-65
            if (!string.IsNullOrEmpty (ExperimentPassword)) {
                writer.WriteElementString ("ExperimentPassword", ExperimentPassword); 
            }

            ///TLAB-69
            if (!string.IsNullOrEmpty (PublishBaseline)) {
                writer.WriteElementString ("PublishBaseline", PublishBaseline); 
            }

            ///TLAB-69
            if (!string.IsNullOrEmpty (PublishChallenge)) {
                writer.WriteElementString ("PublishChallenge", PublishChallenge); 
            }

            ///TLAB-69
            if (!string.IsNullOrEmpty (Deadline)) {
                writer.WriteElementString ("Deadline", Deadline); 
            }   

            ///TLAB-65
            if (!string.IsNullOrEmpty (ExperimentResultsUnitname)) {
                writer.WriteElementString ("ExperimentResultsUnitname", ExperimentResultsUnitname); 
            }    

            // HERZUM SPRINT 3: TLAB-86
            if (!string.IsNullOrEmpty (ExperimentResultsUnittype)) {
                writer.WriteElementString ("ExperimentResultsUnittype", ExperimentResultsUnittype); 
            }

            if (!string.IsNullOrEmpty (ExperimentResultsUnitvalue)) {
                writer.WriteElementString ("ExperimentResultsUnitvalue", ExperimentResultsUnitvalue); 
            }
            // END HERZUM SPRINT 3: TLAB-86

            ///TLAB-66
            if (!string.IsNullOrEmpty (Tags)) {
                writer.WriteStartElement ("Tags");
                string[] tags = Regex.Split(Tags, @", | ,|,");
                foreach (string s in tags) {
                    if(!String.IsNullOrEmpty(s))
                        writer.WriteElementString ("Tag", s);                     
                }

                writer.WriteEndElement ();            
            }
        

            // Only write out the other properties if they're not null.
            if (!string.IsNullOrEmpty (Name)) {
                writer.WriteElementString ("Name", Name);
            }

            if (!string.IsNullOrEmpty (LayoutName)) {
                writer.WriteElementString ("LayoutName", LayoutName);
            }

            if (!string.IsNullOrEmpty (Author)) {
                writer.WriteElementString ("Author", Author);
            }

            if (!string.IsNullOrEmpty (Contributors)) {
                writer.WriteElementString ("Contributors", Contributors);
            }

            if (!string.IsNullOrEmpty (Description)) {
                writer.WriteElementString ("Description", Description);
            }
        }
        #endregion
        #region ISerializable Implementation
        protected ExperimentInfo (SerializationInfo info, StreamingContext context)
        {
            m_name = (string)info.GetValue ("m_name", typeof(string));
            m_layoutName = (string)info.GetValue ("m_layoutName", typeof(string));
            m_author = (string)info.GetValue ("m_author", typeof(string));
            m_contributors = (string)info.GetValue ("m_contributors", typeof(string));
            m_description = (string)info.GetValue ("m_shortDescription", typeof(string));
            m_filePath = (string)info.GetValue ("m_filePath", typeof(string));
             
            ///TLAB-66 TLAB-65
            m_isChallenge = (string)info.GetValue ("m_isChallenge", typeof(string));
            m_tags = (string)info.GetValue ("m_tags", typeof(string));
            m_challengePassword = (string)info.GetValue ("m_challengePassword", typeof(string));
            m_experimentPassword = (string)info.GetValue ("m_experimentPassword", typeof(string));
            m_experimentResultsUnitname = (string)info.GetValue ("m_experimentResultsUnitname", typeof(string));
            m_publishBaseline = (string)info.GetValue ("m_publishBaseline", typeof(string));
            m_publishChallenge = (string)info.GetValue ("m_publishChallenge", typeof(string));
            m_deadline = (string)info.GetValue ("m_deadline", typeof(string));
            // HERZUM SPRINT 3: TLAB-86
            m_experimentResultsUnittype = (string)info.GetValue ("m_experimentResultsUnittype", typeof(string));
            m_experimentResultsUnitvalue = (string)info.GetValue ("m_experimentResultsUnitvalue", typeof(string));
            // END HERZUM SPRINT 3: TLAB-86
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData (SerializationInfo info, StreamingContext context)
        {
            info.AddValue ("m_name", m_name);
            info.AddValue ("m_layoutName", m_layoutName);
            info.AddValue ("m_author", m_author);
            info.AddValue ("m_contributors", m_contributors);
            info.AddValue ("m_shortDescription", m_description);
            info.AddValue ("m_filePath", m_filePath);

            ///TLAB-66 TLAB-65
            info.AddValue ("m_isChallenge", m_isChallenge);
            info.AddValue ("m_tags", m_tags);
            info.AddValue ("m_challengePassword", m_challengePassword);
            info.AddValue ("m_experimentPassword", m_experimentPassword);
            info.AddValue ("m_experimentResultsUnitname", m_experimentResultsUnitname);
            info.AddValue ("m_deadline", m_deadline);
            info.AddValue ("m_publishBaseline", m_publishBaseline);
            info.AddValue ("m_publishChallenge", m_publishChallenge);
            // HERZUM SPRINT 3: TLAB-86
            info.AddValue ("m_experimentResultsUnittype", m_experimentResultsUnittype);
            info.AddValue ("m_experimentResultsUnitvalue", m_experimentResultsUnitvalue);
            // END HERZUM SPRINT 3: TLAB-86
        }
        #endregion

        internal ExperimentInfo Clone ()
        {
            ExperimentInfo clone = new ExperimentInfo ();

            clone.Author = Author;
            clone.Contributors = Contributors;
            clone.Description = Description;
            clone.FilePath = FilePath;
            clone.GuidId = GuidId;
            clone.Id = Id;
            clone.LayoutName = LayoutName;
            clone.Name = Name;
            clone.IsChallenge = IsChallenge;
            clone.Tags = Tags;
            clone.ChallengePassword = ChallengePassword;
            clone.ExperimentPassword = ExperimentPassword;
            clone.ExperimentResultsUnitname = ExperimentResultsUnitname;
            clone.Deadline = Deadline;
            clone.PublishBaseline = PublishBaseline;
            clone.PublishChallenge = PublishChallenge;
            // HERZUM SPRINT 3: TLAB-86
            clone.ExperimentResultsUnittype = ExperimentResultsUnittype;
            clone.ExperimentResultsUnitvalue = ExperimentResultsUnitvalue;
            // END HERZUM SPRINT 3: TLAB-86
            return clone;
        }

        // HERZUM SPRINT 2.3 TLAB-56 TLAB-57 TLAB-58 TLAB-59
        internal ExperimentInfo CloneWithNewId()
        {
            ExperimentInfo clone = new ExperimentInfo();

            clone.Author = Author;
            clone.Contributors = Contributors;
            clone.Description = Description;
            clone.FilePath = FilePath;
            clone.Id = Guid.NewGuid().ToString();
            clone.LayoutName = LayoutName;
            clone.Name = Name;
            clone.IsChallenge = IsChallenge;
            clone.ChallengePassword = ChallengePassword;
            clone.ExperimentPassword = ExperimentPassword;
            clone.ExperimentResultsUnitname = ExperimentResultsUnitname;
            clone.Deadline = Deadline;
            clone.PublishBaseline = PublishBaseline;
            clone.PublishChallenge = PublishChallenge;
            // HERZUM SPRINT 3: TLAB-86
            clone.ExperimentResultsUnittype = ExperimentResultsUnittype;
            clone.ExperimentResultsUnitvalue = ExperimentResultsUnitvalue;
            // END HERZUM SPRINT 3: TLAB-86
            return clone;
        }
        // END HERZUM SPRINT 2.3 TLAB-56 TLAB-57 TLAB-58 TLAB-59

        #region IModifiable Members
        private bool m_isModified;

        public bool IsModified {
            get {
                return m_isModified;
            }
            set {
                if (m_isModified != value) {
                    m_isModified = value;
                    OnPropertyChanged ("IsModified");
                }
            }
        }

        public void ResetModifiedFlag ()
        {
            IsModified = false;
        }
        #endregion
        public override bool Equals (object obj)
        {
            if (obj == null) {
                return false;
            }

            ExperimentInfo info = obj as ExperimentInfo;
            if (info == null) {
                return false;
            }

            //finally compare all values
            bool equals = Author == info.Author &&
                Contributors == info.Contributors &&
                Description == info.Description &&
                FilePath == info.FilePath &&
                GuidId == info.GuidId &&
                Id == info.Id &&
                LayoutName == info.LayoutName &&
                Name == info.Name && 
                IsChallenge == info.IsChallenge &&
                Tags == info.Tags &&
                ChallengePassword == info.ChallengePassword &&
                ExperimentPassword == info.ExperimentPassword &&
                ExperimentResultsUnitname == info.ExperimentResultsUnitname &&
                // HERZUM SPRINT 3: TLAB-86
                ExperimentResultsUnittype == info.ExperimentResultsUnittype &&
                ExperimentResultsUnitvalue == info.ExperimentResultsUnitvalue &&
                // END HERZUM SPRINT 3: TLAB-86
                Deadline == info.Deadline &&
                PublishBaseline == info.PublishBaseline &&
                PublishChallenge == info.PublishChallenge
                    ;

            return equals;
        }
    }
}
