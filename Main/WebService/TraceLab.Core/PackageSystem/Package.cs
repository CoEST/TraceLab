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
using System.Xml;
using System.IO;
using System.Xml.XPath;
using TraceLabSDK.PackageSystem;

// HERZUM SPRINT 3.1 TLAB-179
using TraceLabSDK;
// END HERZUM SPRINT 3.1 TLAB-179

namespace TraceLab.Core.PackageSystem
{
    public class Package : MarshalByRefObject, IPackage
    {
        /// <summary>
        /// Where this package is located on disk.
        /// </summary>
        public string Location
        {
            get;
            private set;
        }

        /// <summary>
        /// The name of this package
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// The ID of this package.
        /// </summary>
        public string ID
        {
            get;
            private set;
        }


        /// <summary>
        /// Gets a value indicating whether this Package is dirty.  
        /// 
        /// A package is dirtied when a file is added or removed, when files are updated.  
        /// A package is no longer dirty once it is saved.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is dirty; otherwise, <c>false</c>.
        /// </value>
        public bool IsDirty
        {
            get;
            private set;
        }

        /// <summary>
        /// The files contained in this package
        /// </summary>
        private List<PackageFile> m_files = new List<PackageFile>();
        public IEnumerable<IPackageFile> Files
        {
            get { return m_files; }
        }

        private List<IPackageReference> m_references = new List<IPackageReference>();
        /// <summary>
        /// Gets the IDs of other packaged that are used by this package.
        /// </summary>
        public IEnumerable<IPackageReference> References
        {
            get { return m_references; }
        }

        List<string> m_componentLocations = new List<string>();
        /// <summary>
        /// Where components are located within this Package.
        /// </summary>
        public IEnumerable<string> ComponentLocations
        {
            get { return m_componentLocations; }
        }

        List<string> m_typeLocations = new List<string>();
        /// <summary>
        /// Where workspace types are located within this Package.
        /// </summary>
        public IEnumerable<string> TypeLocations
        {
            get { return m_typeLocations; }
        }

        /// <summary>
        /// Gets the <see cref="TraceLab.Core.PackageSystem.PackageFile"/> with the specified id.
        /// </summary>
        public IPackageFile this[string id]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Creates the package.
        /// </summary>
        /// <param name="openExisting">if set to <c>true</c> [open existing].</param>
        private void createPackage(bool openExisting)
        {
            bool currentlyExists = System.IO.File.Exists(System.IO.Path.Combine(Location, Name + ".manifest"));
            if (openExisting)
            {
                // If a manifest exists, then read it.  Otherwise treat this as a new package.
                if (currentlyExists)
                {
                    ReadManifest();
                }
                else
                {
                    throw new PackageException(Name, string.Empty, "Attempting to open a non-existing package.");
                }
            }
            else
            {
                if (currentlyExists)
                {
                    throw new PackageAlreadyExistsException(Name, string.Empty);
                }
                else
                {
                    ID = Guid.NewGuid().ToString("D");
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Package"/> class.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="openExisting">if set to <c>true</c> [open existing].</param>
        public Package(string directory, bool openExisting)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new ArgumentNullException("directory");
            }
            if (!System.IO.Directory.Exists(directory))
            {
                throw new System.IO.DirectoryNotFoundException("Package directory must exist!");
            }

            DirectoryInfo dirInfo = new DirectoryInfo(directory);
            Name = dirInfo.Name;
            Location = dirInfo.FullName;

            this.createPackage(openExisting);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Package"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="directory">The directory.</param>
        /// <param name="openExisting">if set to <c>true</c> [open existing].</param>
        public Package(string name, string directory, bool openExisting)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }
            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new ArgumentNullException("directory");
            }
            if (!System.IO.Directory.Exists(directory))
            {
                throw new System.IO.DirectoryNotFoundException("Package directory must exist!");
            }

            Name = name;
            Location = directory;
            this.createPackage(openExisting);
        }

        public Package(Stream packageStream)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            XmlReader reader = XmlReader.Create(packageStream, settings);
            XPathDocument doc = new XPathDocument(reader);

            // Read the name so we can know where to create it.
            var nav = doc.CreateNavigator();
            var nameNode = nav.SelectSingleNode("/Package/@Name");
            string name = nameNode.Value;

            Name = name;
            ReadPackage(nav);
        }

        private Package()
        {
        }

        /// <summary>
        /// Unpacks this package into the specified directory.
        /// </summary>
        /// <param name="baseDirectory">The directory to use.</param>
        public static Package Unpack(string rootLocation, Stream packageStream)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            XmlReader reader = XmlReader.Create(packageStream, settings);
            XPathDocument doc = new XPathDocument(reader);

            // Read the name so we can know where to create it.
            var nav = doc.CreateNavigator();
            var nameNode = nav.SelectSingleNode("/Package/@Name");
            string name = nameNode.Value;

            string packageDir = System.IO.Path.Combine(rootLocation, name);
            System.IO.Directory.CreateDirectory(packageDir);

            Package newPackage = new Package(packageDir, false);
            newPackage.ReadPackage(nav);
            newPackage.SaveManifest();

            return newPackage;
        }

        /// <summary>
        /// Packs this instance.  Note that this does not save the manifest file.
        /// </summary>
        /// <returns></returns>
        public void Pack(Stream streamToUse)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.NamespaceHandling = NamespaceHandling.OmitDuplicates;
            settings.Indent = true;
            settings.CloseOutput = false;

            using (XmlWriter writer = XmlWriter.Create(streamToUse, settings))
            {
                WritePackage(writer, false);
            }
        }

        public void Unpack(string rootLocation)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;

            string usableName = Name;

            string directory = string.Empty;
            bool canUseDirectory = true;
            bool packageExists = false;
            int num = 1;
            do
            {
                directory = Path.Combine(rootLocation, usableName);
                string manifest = Path.Combine(directory, usableName + ".manifest");
                if (Directory.Exists(directory) && File.Exists(manifest))
                {
                    XmlReader reader = XmlReader.Create(manifest, settings);
                    XPathDocument doc = new XPathDocument(reader);

                    // Read the name so we can know where to create it.
                    var nav = doc.CreateNavigator();
                    var idnode = nav.SelectSingleNode("/Package/@ID");
                    if (idnode == null)
                        canUseDirectory = false;

                    if (idnode != null)
                    {
                        if (!string.Equals(idnode.Value, ID, StringComparison.InvariantCultureIgnoreCase))
                        {
                            canUseDirectory = false;
                        }
                        else
                        {
                            packageExists = true;
                        }
                    }
                }

                if (canUseDirectory == false)
                {
                    usableName = Name + " " + num.ToString();

                    ++num;
                }
            } while (!canUseDirectory);

            if (!packageExists)
            {
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                Location = directory;

                SaveManifest();
                foreach (PackageFile file in m_files)
                {
                    file.Unpack();
                }
            }
        }


        /// <summary>
        /// Adds the given file path.  File path can be absolute, or relative to the package location.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <exception cref="System.ArgumentException">Thrown if path is empty.</exception>
        public void AddFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path not be empty.");

            // HERZUM SPRINT 3.1 TLAB-179
            if (TraceLabSDK.RuntimeInfo.OperatingSystem == RuntimeInfo.OS.Windows)
            // END // HERZUM SPRINT 3.1 TLAB-179
                path = path.Replace('/', '\\');
            string relativePath = path;
            // Validate that the file exists:
            if (System.IO.Path.IsPathRooted(path))
            {
                System.IO.FileInfo info = new FileInfo(path);
                path = info.FullName;
                if (path.StartsWith(Location))
                {
                    // HERZUM SPRINT 3.1 TLAB-179
                    // relativePath = ".\\" + path.Remove(0, Location.Length + 1);
                    if (TraceLabSDK.RuntimeInfo.OperatingSystem == RuntimeInfo.OS.Windows)
                        relativePath = ".\\" + path.Remove(0, Location.Length + 1);
                    else
                        relativePath = "./" + path.Remove(0, Location.Length + 1);
                    // END HERZUM SPRINT 3.1 TLAB-179
                }
                else
                {
                    throw new ArgumentException("An path must be within the Package's location.");
                }
            }

            if (!System.IO.File.Exists(System.IO.Path.Combine(Location, relativePath)))
            {
                throw new ArgumentException("File must exist.");
            }

            var file = new PackageFile(this, relativePath);
            m_files.Add(file);
        }

        public void RemoveFile(IPackageFile file)
        {
            PackageFile realFile = file as PackageFile;
            m_files.Remove(realFile);
        }

        public void AddReference(IPackageReference reference)
        {
            m_references.Remove(reference);
        }

        public void RemoveReference(IPackageReference reference)
        {
            m_references.Remove(reference);
        }

        public void SetDirectoryHasComponents(string directory, bool hasComponents)
        {
            if (hasComponents)
            {
                if (!m_componentLocations.Contains(directory))
                {
                    m_componentLocations.Add(directory);
                }
            }
            else
            {
                m_componentLocations.Remove(directory);
            }
        }

        public void SetDirectoryHasTypes(string directory, bool hasTypes)
        {
            if (hasTypes)
            {
                if (!m_typeLocations.Contains(directory))
                {
                    m_typeLocations.Add(directory);
                }
            }
            else
            {
                m_typeLocations.Remove(directory);
            }
        }

        private string GetValue(string xpath, XPathNavigator nav)
        {
            var valXml = nav.SelectSingleNode(xpath);
            if (valXml == null)
            {
                throw new System.Xml.Schema.XmlSchemaException();
            }

            return valXml.Value;
        }

        private void ReadManifest()
        {
            string manifestFile = System.IO.Path.Combine(Location, Name + ".manifest");
            using (FileStream stream = System.IO.File.OpenRead(manifestFile))
            {
                XPathDocument doc = new XPathDocument(stream);
                var nav = doc.CreateNavigator();
                ReadPackage(nav);
            }
        }

        private void ReadPackage(XPathNavigator nav)
        {
            var id = nav.SelectSingleNode("./Package/@ID");
            Guid guid;
            if (id == null || Guid.TryParse(id.Value, out guid) == false)
            {
                throw new System.Xml.Schema.XmlSchemaException();
            }
            ID = id.Value;

            //bool isPacked = false;
            //var isPackedNode = nav.SelectSingleNode("./Package/@IsPacked");
            //if (isPackedNode != null)
            //{
            //    isPacked = true;
            //}

            Name = GetValue("./Package/@Name", nav);

            var filesXml = nav.Select("./Package/DataFiles/PackageFile");
            while (filesXml.MoveNext())
            {
                PackageFile file = new PackageFile(this, filesXml.Current);
                m_files.Add(file);
            }

            var references = new List<IPackageReference>();
            var referencesXml = nav.Select("./Package/References/PackageReference");
            while (referencesXml.MoveNext())
            {
                var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(PackageSystem.PackageReference), Type.EmptyTypes);
                PackageReference reference = (PackageReference)serializer.Deserialize(referencesXml.Current.ReadSubtree());
                references.Add(reference);
            }
            m_references = references;

            var componentLocations = new List<string>();
            var componentLocationsXml = nav.Select("./Package/ComponentLocations/Location");
            while (componentLocationsXml.MoveNext())
            {
                componentLocations.Add(componentLocationsXml.Current.Value);
            }
            m_componentLocations = componentLocations;

            var typeLocations = new List<string>();
            var typeLocationsXml = nav.Select("./Package/TypeLocations/Location");
            while (typeLocationsXml.MoveNext())
            {
                typeLocations.Add(typeLocationsXml.Current.Value);
            }
            m_typeLocations = typeLocations;
        }

        public void SaveManifest()
        {
            string manifestFile = System.IO.Path.Combine(Location, Name + ".manifest");
            using (FileStream stream = System.IO.File.OpenWrite(manifestFile))
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.NamespaceHandling = NamespaceHandling.OmitDuplicates;
                settings.Encoding = Encoding.UTF8;
                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    WritePackage(writer, true);
                }
            }
        }

        /// <summary>
        /// Writes the package using the give writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="manifestOnly">if set to <c>true</c> then only the manifest information about the PackageFiles is written.  If set to <c>false</c> then all PackageFile data is written.</param>
        private void WritePackage(XmlWriter writer, bool manifestOnly)
        {
            writer.WriteStartElement("Package");
            {
                writer.WriteAttributeString("ID", ID);
                writer.WriteAttributeString("Name", Name);
                if (!manifestOnly)
                {
                    writer.WriteAttributeString("IsPacked", true.ToString());
                }
                writer.WriteStartElement("DataFiles");

                foreach (PackageFile file in m_files)
                {
                    writer.WriteStartElement("PackageFile");
                    if (manifestOnly)
                    {
                        file.WriteManifestInfo(writer);
                    }
                    else
                    {
                        file.WritePackedInfo(writer);
                        //using (MemoryStream newStream = new MemoryStream())
                        //{
                        //    file.PackToStream(newStream);
                        //    writer.WriteString(System.Convert.ToBase64String(newStream.GetBuffer(), 0, (int)newStream.Length));
                        //}
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement(); // DataFiles

                writer.WriteStartElement("References");
                foreach (PackageSystem.PackageReference reference in m_references)
                {
                    var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(PackageReference), Type.EmptyTypes);
                    serializer.Serialize(writer, reference);
                }
                writer.WriteEndElement(); // References

                writer.WriteStartElement("ComponentLocations");
                foreach (string loc in m_componentLocations)
                {
                    writer.WriteElementString("Location", loc);
                }
                writer.WriteEndElement(); // ComponentLocations

                writer.WriteStartElement("TypeLocations");
                foreach (string loc in m_typeLocations)
                {
                    writer.WriteElementString("Location", loc);
                }
                writer.WriteEndElement(); // TypeLocations
            }
            writer.WriteEndElement();
        }

        internal string GetAbsolutePath(PackageFile file)
        {
            return System.IO.Path.Combine(Location, file.Path);
        }

        internal static string ComputeHash(Stream stream)
        {
            System.Security.Cryptography.SHA256 shaM = new System.Security.Cryptography.SHA256Managed();
            var result = shaM.ComputeHash(stream);

            return System.Convert.ToBase64String(result, 0, result.Length);
        }

        public string GetAbsolutePath(string fileID)
        {
            string ret = null;
            var foundFile = Files.Where((A) => { return A.ID == fileID; });
            IPackageFile file = foundFile.ElementAtOrDefault(0);
            if (file != null)
            {
                ret = System.IO.Path.Combine(Location, file.Path);
            }

            return ret;
        }
    }
}
