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
using System.Xml.Serialization;

namespace TraceLabSDK.Component.Config
{
    /// <summary>
    /// Class represents the base path for FilePath and DirectoryPath.
    /// It can compute relative path in regard to the specified root.
    /// </summary>
    [Serializable]
    public abstract class BasePath : INotifyPropertyChanged, IXmlSerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasePath"/> class.  Used for XML serialization ONLY.
        /// </summary>
        protected BasePath() { }

        /// <summary>
        /// Initializes the Path with the given absolute path.
        /// </summary>
        /// <param name="absolutePath">The absolute path.</param>
        public virtual void Init(string absolutePath)
        {
            this.Init(absolutePath, null);
        }

        /// <summary>
        /// Initializes the Path with the given data root.
        /// </summary>
        /// <param name="absolutePath">The absolute path.</param>
        /// <param name="dataRoot">The data root.</param>
        public virtual void Init(string absolutePath, string dataRoot)
        {
            if (dataRoot != null)
                this.SetDataRoot(dataRoot, true);
            this.Absolute = absolutePath;
        }

        private string m_relative;
        /// <summary>
        /// Gets or sets the string value representing the path itself.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public virtual string Relative
        {
            get { return m_relative; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                value = value.Replace(System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar);
                if (value.Length != 0 && System.IO.Path.IsPathRooted(value))
                {
                    throw new ArgumentException("Argument must be a relative path in order to use this setter.");
                }

                if (string.Equals(m_relative, value) == false)
                {
                    m_relative = value;

                    // Only set the absolute path if the DataRoot isn't null.
                    if (m_relative.Length > 0 && DataRoot != null)
                    {
                        Absolute = System.IO.Path.GetFullPath(System.IO.Path.Combine(DataRoot, m_relative));
                    }
                }
            }
        }

        private string m_absolute;
        /// <summary>
        /// Gets or sets the absolute path represented by this Path
        /// </summary>
        /// <value>
        /// The absolute path.
        /// </value>
        public virtual string Absolute
        {
            get { return m_absolute; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                value = value.Replace(System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar);
                if (value.Length != 0 && System.IO.Path.IsPathRooted(value) == false)
                {
                    throw new ArgumentException("Argument must be an absolute path in order to use this setter.");
                }

                if (value.Length != 0 && DataRoot != null)
                {
                    // check if the root is the same, ie. checks if paths are one the same partition, for example on C:/
                    string root = System.IO.Path.GetPathRoot(value);
                    string dataroot = System.IO.Path.GetPathRoot(DataRoot);
                    if (root.Equals(dataroot, StringComparison.CurrentCultureIgnoreCase) == false)
                    {
                        throw new ArgumentException(string.Format("Path must be on the same partition as experiment. Experiment is on {0}, the selected path is on {1}", dataroot, root));
                    }
                }

                if (string.Equals(m_absolute, value) == false)
                {
                    m_absolute = value;
                    if (DataRoot != null)
                    {
                        if (value.Length > 0)
                        {
                            Relative = DetermineRelativePath(m_absolute, DataRoot);
                        }
                        else
                        {
                            Relative = m_absolute;
                        }
                    }
                }
            }
        }

        private string m_dataRoot;
        /// <summary>
        /// Gets the data root of this Path.
        /// </summary>
        public virtual string DataRoot
        {
            get { return m_dataRoot; }
        }

        /// <summary>
        /// Sets the data root.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="transformRelative">if set to <c>true</c> [transform relative].</param>
        public virtual void SetDataRoot(string value, bool transformRelative)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            if (System.IO.Path.IsPathRooted(value) == false)
                throw new ArgumentException("DataRoot must be an absolute path.");

            value = value.Replace(System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar);
            if (value[value.Length - 1] != System.IO.Path.DirectorySeparatorChar)
            {
                value += System.IO.Path.DirectorySeparatorChar;
            }

            if (m_dataRoot != null)
            {
                string oldRoot = m_dataRoot;
                m_dataRoot = value;

                if (string.IsNullOrEmpty(Relative) == false)
                {
                    if (transformRelative == true)
                    {
                        //transform relative path
                        TransformRelative(m_dataRoot, oldRoot);
                    }
                    else
                    {
                        //set absolute to the new location
                        Absolute = System.IO.Path.GetFullPath(System.IO.Path.Combine(m_dataRoot, Relative));
                    }
                }

            }
            else
            {
                m_dataRoot = value;

                if (string.IsNullOrEmpty(Relative) == false)
                    Absolute = System.IO.Path.GetFullPath(System.IO.Path.Combine(m_dataRoot, Relative));
                else if (Absolute != null && Absolute.StartsWith(m_dataRoot))
                {
                    Relative = DetermineRelativePath(Absolute, m_dataRoot);
                }
            }
        }

        private void TransformRelative(string newRoot, string oldRoot)
        {
            //determine current absolute path in relation to old data root
            string absolute = System.IO.Path.GetFullPath(System.IO.Path.Combine(oldRoot, Relative));

            //update Relative path to be relative to new experiment location
            Relative = DetermineRelativePath(absolute, newRoot);

            //Absolute = absolute;
        }

        /// <summary>
        /// Determines the relative path.
        /// </summary>
        /// <param name="absolutePath">The absolute path.</param>
        /// <param name="relativeTo">relative to.</param>
        /// <returns></returns>
        private static string DetermineRelativePath(string absolutePath, string relativeTo)
        {
            OperatingSystem os = Environment.OSVersion;

            StringComparison comparisonType = StringComparison.CurrentCulture;
            int p = (int)Environment.OSVersion.Platform;
            if (((p == 4) || (p == 6) || (p == 128)) == false)
            {
                //"NOT Running on Unix"
                //if not running on unix, use non case sensitive comparison
                comparisonType = StringComparison.CurrentCultureIgnoreCase;
            }

            char dirSeparatorChar = System.IO.Path.DirectorySeparatorChar;

            string[] absoluteDirectories = absolutePath.Split(dirSeparatorChar);
            string[] relativeDirectories = relativeTo.Split(dirSeparatorChar);

            //Get the shortest of the two paths
            int length = absoluteDirectories.Length < relativeDirectories.Length ? absoluteDirectories.Length : relativeDirectories.Length;

            //Use to determine where in the loop we exited
            int lastCommonRoot = -1;
            int index;

            //Find common root
            for (index = 0; index < length; index++)
            {
                if (absoluteDirectories[index].Equals(relativeDirectories[index], comparisonType))
                    lastCommonRoot = index;
                else
                    break;
            }

            //If we didn't find a common prefix then throw
            if (lastCommonRoot == -1)
                throw new ArgumentException("Paths do not have a common base");

            //Build up the relative path
            StringBuilder relativePath = new StringBuilder();

            //Add on the ..
            for (index = lastCommonRoot + 1; index < relativeDirectories.Length; index++)
            {
                if (relativeDirectories[index].Length > 0)
                    relativePath.Append(".." + dirSeparatorChar);
            }

            //Add on the folders
            for (index = lastCommonRoot + 1; index < absoluteDirectories.Length - 1; index++)
                relativePath.Append(absoluteDirectories[index] + dirSeparatorChar);
            relativePath.Append(absoluteDirectories[absoluteDirectories.Length - 1]);

            return relativePath.ToString();
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="property">The property.</param>
        protected void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion

        #region IXmlSerializable Members

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.
        /// </returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public abstract void ReadXml(System.Xml.XmlReader reader);

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public abstract void WriteXml(System.Xml.XmlWriter writer);

        #endregion
    }
}
