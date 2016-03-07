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
using System.Xml.Schema;
using System.Xml.Serialization;
using TraceLabSDK.Component.Config;
using TraceLabSDK.PackageSystem;

namespace TraceLab.Core.Components
{
    [Serializable]
    [XmlRoot("FilePath")]
    public class TraceLabFilePath : FilePath
    {
        private void ClearPackageInfo()
        {
            PackageSource = null;
            PackageFile = null;
        }

        public bool IsPackageFile
        {
            get { return m_packageSource != null; }
        }

        private IPackageReference m_packageSource;
        public IPackageReference PackageSource
        {
            get { return m_packageSource; }
            set
            {
                if (m_packageSource != value)
                {
                    m_packageSource = value;
                    NotifyPropertyChanged("PackageSource");
                    NotifyPropertyChanged("IsPackageFile");
                    NotifyPropertyChanged("Absolute");
                    NotifyPropertyChanged("Relative");
                    NotifyPropertyChanged("DataRoot");
                }
            }
        }

        private string m_packageFile;
        public string PackageFile
        {
            get { return m_packageFile; }
            set
            {
                if (m_packageFile != value)
                {
                    m_packageFile = value;
                    NotifyPropertyChanged("PackageFile");
                    NotifyPropertyChanged("Absolute");
                    NotifyPropertyChanged("Relative");
                }
            }
        }

        /// <summary>
        /// Inits the TraceLabFilePath with the specified file from the given package.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="id">The id.</param>
        public void Init(IPackageReference source, string id)
        {
            PackageSource = source;
            PackageFile = id;
        }

        /// <summary>
        /// Initializes the Path with the given absolute path.
        /// </summary>
        /// <param name="absolutePath">The absolute path.</param>
        public override void Init(string absolutePath)
        {
            ClearPackageInfo();
            base.Init(absolutePath);
        }

        /// <summary>
        /// Initializes the Path with the given data root.
        /// </summary>
        /// <param name="absolutePath">The absolute path.</param>
        /// <param name="dataRoot">The data root.</param>
        public override void Init(string absolutePath, string dataRoot)
        {
            ClearPackageInfo();
            base.Init(absolutePath, dataRoot);
        }

        /// <summary>
        /// Gets the data root of this FilePath.
        /// </summary>
        public override string DataRoot
        {
            get
            {
                string value = null;
                if (m_packageSource != null)
                {
                    var package = PackageSystem.PackageManager.Instance.GetPackage(m_packageSource);
                    if (package != null)
                    {
                        value = package.Location;
                    }
                }
                else
                {
                    value = base.DataRoot;
                }

                return value;
            }
        }

        public override void SetDataRoot(string value, bool transformRelative)
        {
            if (!IsPackageFile)
            {
                base.SetDataRoot(value, transformRelative);
            }

            // Don't do anything for package files.
        }

        /// <summary>
        /// Gets or sets the string value representing the path itself.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public override string Relative
        {
            get
            {

                string value = null;
                if (IsPackageFile)
                {
                    var package = PackageSystem.PackageManager.Instance.GetPackage(m_packageSource);
                    if (package != null)
                    {
                        value = package.GetAbsolutePath(PackageFile);
                        value = value.Remove(0, package.Location.Length);
                    }
                }
                else
                {
                    value = base.Absolute;
                }

                return value;
            }
            set
            {
                if (IsPackageFile)
                {
                    throw new InvalidOperationException("Cannot set a relative path to a package.  Use a package file ID instead.");
                }
                else
                {
                    base.Relative = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the absolute path represented by this FilePath
        /// </summary>
        /// <value>
        /// The absolute path.
        /// </value>
        public override string Absolute
        {
            get
            {
                string value = null;
                if (IsPackageFile)
                {
                    var package = PackageSystem.PackageManager.Instance.GetPackage(m_packageSource);
                    if (package != null)
                    {
                        value = package.GetAbsolutePath(PackageFile);
                    }
                }
                else
                {
                    value = base.Absolute;
                }

                return value;
            }
            set
            {
                ClearPackageInfo();
                base.Absolute = value;
            }
        }


        #region IXmlSerializable
        
        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            if (!IsPackageFile)
            {
                base.WriteXml(writer);
            }
            else
            {
                writer.WriteAttributeString("FileID", m_packageFile);

                var sourceSerializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(m_packageSource.GetType(), null);
                sourceSerializer.Serialize(writer, m_packageSource);
            }
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public override void ReadXml(System.Xml.XmlReader reader)
        {
            string fileId = reader.GetAttribute("FileID");
            if (fileId == null)
            {
                base.ReadXml(reader);
            }
            else
            {
                var sourceSerializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(PackageSystem.PackageReference), null);
                m_packageFile = fileId;

                System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(reader);
                var nav = doc.CreateNavigator();
                var iter = nav.SelectSingleNode("/FilePath/PackageReference");
                if(iter == null)
                    throw new XmlSchemaException();
                m_packageSource = (IPackageReference)sourceSerializer.Deserialize(iter.ReadSubtree());

                
            }
        }

        #endregion
    }
}
