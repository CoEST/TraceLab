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
using System.Xml.XPath;
using TraceLabSDK.PackageSystem;

namespace TraceLab.Core.PackageSystem
{
    public class PackageFile: MarshalByRefObject, IPackageFile
    {
        const int CurrentVersion = 1;

        public override object InitializeLifetimeService()
        {
            return null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageFile"/> class.  
        /// The instance is initialized as uncompressed and depends on the file existing prior to creation of the PackageFile
        /// </summary>
        /// <param name="ownerPackage">The owner package.</param>
        public PackageFile(IPackage ownerPackage, string path)
        {
            if (ownerPackage == null)
                throw new ArgumentNullException("PackageFiles must be created with an owning Package");
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("Path must be non-null and not whitespace.");
            string absolutePath = System.IO.Path.Combine(ownerPackage.Location, path);
            if (System.IO.File.Exists(absolutePath) == false)
                throw new System.IO.FileNotFoundException("File must exist");

            Owner = ownerPackage;
            ID = Guid.NewGuid().ToString("D");
            Path = path;

            using (var stream = new System.IO.FileStream(absolutePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
            {
                UncompressedSize = stream.Length;
                UncompressedHash = Package.ComputeHash(stream);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageFile"/> class.  Used when constructing a PackageFile from a transport stream
        /// </summary>
        /// <param name="ownerPackage">The owner package.</param>
        /// <param name="path">The path.</param>
        /// <param name="id">The id.</param>
        /// <param name="uncompressedSize">Size of the uncompressed.</param>
        /// <param name="uncompressedHash">The uncompressed hash.</param>
        /// <param name="compressedSize">Size of the compressed.</param>
        /// <param name="compressedHash">The compressed hash.</param>
        /// <param name="data">The data.</param>
        public PackageFile(IPackage ownerPackage, string path, Guid id, long uncompressedSize, string uncompressedHash, long compressedSize, string compressedHash, string data)
        {
            Owner = ownerPackage;
            ID = id.ToString("D");
            Path = path;
            UncompressedSize = uncompressedSize;
            UncompressedHash = uncompressedHash;
            CompressedSize = compressedSize;
            CompressedHash = compressedHash;
            IsCompressed = true;
            Data = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageFile"/> class.  Used to construct a PackageFile from the manifest
        /// </summary>
        /// <param name="nav">The nav.</param>
        internal PackageFile(IPackage ownerPackage, XPathNavigator nav)
        {
            Owner = ownerPackage;
            var id = nav.SelectSingleNode("./@ID");
            Guid guid;
            if (id == null || Guid.TryParse(id.Value, out guid) == false)
            {
                throw new System.Xml.Schema.XmlSchemaException();
            }
            ID = id.Value;

            Path = GetValue("./@Path", nav);
            CompressedHash = GetValue("./@CompressedHash", nav);
            CompressedSize = GetValueAsLong("./@CompressedSize", nav);
            UncompressedHash = GetValue("./@UncompressedHash", nav);
            UncompressedSize = GetValueAsLong("./@UncompressedSize", nav);

            Data = nav.Value;
        }

        internal PackageFile(IPackage ownerPackage, System.IO.Stream stream)
        {
            Owner = ownerPackage;

            UnpackFromStream(stream);
        }

        private long GetValueAsLong(string xpath, XPathNavigator nav)
        {
            var valXml = nav.SelectSingleNode(xpath);
            if (valXml == null)
            {
                throw new System.Xml.Schema.XmlSchemaException();
            }

            return valXml.ValueAsLong;
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

        /// <summary>
        /// Gets the ID of the PackageFile
        /// </summary>
        public string ID
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the SHA256 of the DataFile, when compressed
        /// </summary>
        public string CompressedHash
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the SHA256 of the DataFile, when uncompressed
        /// </summary>
        public string UncompressedHash
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the Path of this file (filename/directories), relative to the PackageRoot
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path
        {
            get;
            set;
        }

        private Int64 m_uncompressedSize = Int64.MinValue;
        /// <summary>
        /// Gets the size of the PackageFile - only valid while IsCompressed is false
        /// </summary>
        /// <value>
        /// The size of the uncompressed.
        /// </value>
        public Int64 UncompressedSize
        {
            get { return m_uncompressedSize; }
            private set { m_uncompressedSize = value; }
        }

        private Int64 m_compressedSize = Int64.MinValue;
        /// <summary>
        /// Gets the size of the PackageFile - only valid while IsCompressed is true
        /// </summary>
        /// <value>
        /// The size of the compressed.
        /// </value>
        public Int64 CompressedSize
        {
            get { return m_compressedSize; }
            private set { m_compressedSize = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the data is compressed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the data is compressed; otherwise, <c>false</c>.
        /// </value>
        public bool IsCompressed
        {
            get;
            private set;
        }

        private string m_data = null;
        /// <summary>
        /// Gets the data of the package file
        /// </summary>
        public string Data
        {
            get
            {
                return m_data;
            }
            private set
            {
                m_data = value;
            }
        }

        private IPackage m_owner;
        /// <summary>
        /// Gets the owner package
        /// </summary>
        public IPackage Owner
        {
            get { return m_owner; }
            internal set
            {
                m_owner = value;
            }
        }

        /// <summary>
        /// Unpacks this PackageFile to disk.
        /// </summary>
        public void Unpack()
        {
            if (Path == null)
                throw new InvalidOperationException("No path to unpack this PackageFile to.");
            if (Data == null)
                throw new InvalidOperationException("PackageFile has no data to unpack.");

            string outFilePath = System.IO.Path.Combine(Owner.Location, Path);
            var outDirectory = System.IO.Path.GetDirectoryName(outFilePath);
            if (!System.IO.Directory.Exists(outDirectory))
            {
                System.IO.Directory.CreateDirectory(outDirectory);
            }

            using(var outFile = System.IO.File.OpenWrite(outFilePath))
            {
                using (var inFile = new System.IO.MemoryStream(System.Convert.FromBase64String(Data)))
                {
                    DecompressStream(inFile, outFile);
                }
            }

            IsCompressed = false;
            Data = null;
        }

        /// <summary>
        /// Packs the file and prepares it for transport
        /// </summary>
        public void PackFile()
        {
            string inFilePath = System.IO.Path.Combine(Owner.Location, Path);
            using (var inFile = System.IO.File.OpenRead(inFilePath))
            {
                using (var outFile = new System.IO.MemoryStream())
                {
                    CompressStream(inFile, outFile);
                    outFile.Seek(0, System.IO.SeekOrigin.Begin);

                    CompressedSize = outFile.Length;
                    CompressedHash = Package.ComputeHash(outFile);
                    Data = System.Convert.ToBase64String(outFile.GetBuffer(), 0, (int)outFile.Length);
                    IsCompressed = true;
                }
            }
        }

        public void PackToStream(System.IO.Stream stream)
        {
            if (!IsCompressed)
                PackFile();


            System.IO.StreamWriter writer = new System.IO.StreamWriter(stream);
            writer.WriteLine(CurrentVersion);

            writer.WriteLine(ID);
            writer.WriteLine(Path);
            writer.WriteLine(UncompressedHash);
            writer.WriteLine(UncompressedSize);
            writer.WriteLine(CompressedHash);
            writer.WriteLine(CompressedSize);
            writer.WriteLine(Data);

            writer.Flush();
        }

        private void UnpackFromStream(System.IO.Stream stream)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(stream);
            var val = reader.ReadLine();
            var version = int.Parse(val.Trim());
            if (version == CurrentVersion)
            {
                val = reader.ReadLine();
                ID = val.Trim();

                val = reader.ReadLine();
                Path = val.Trim();

                val = reader.ReadLine();
                UncompressedHash = val.Trim();

                val = reader.ReadLine();
                UncompressedSize = long.Parse(val.Trim());

                val = reader.ReadLine();
                CompressedHash = val.Trim();

                val = reader.ReadLine();
                CompressedSize = long.Parse(val.Trim());

                val = reader.ReadLine();
                Data = val.Trim();
            }
        }

        public static void Compress(System.IO.FileInfo fi)
        {
            // Get the stream of the source file.
            using (System.IO.FileStream inFile = fi.OpenRead())
            {
                // Prevent compressing hidden and 
                // already compressed files.
                if ((System.IO.File.GetAttributes(fi.FullName) & System.IO.FileAttributes.Hidden) != System.IO.FileAttributes.Hidden && fi.Extension != ".gz")
                {
                    // Create the compressed file.
                    using (System.IO.FileStream outFile = System.IO.File.Create(fi.FullName + ".gz"))
                    {
                        CompressStream(inFile, outFile);
                    }
                }
            }
        }

        internal static void CompressStream(System.IO.Stream inFile, System.IO.Stream outFile)
        {
            using (System.IO.Compression.GZipStream Compress = new System.IO.Compression.GZipStream(outFile, System.IO.Compression.CompressionMode.Compress, true))
            {
                // Copy the source file into 
                // the compression stream.
                inFile.CopyTo(Compress);
            }
        }

        private static void DecompressStream(System.IO.Stream inFile, System.IO.Stream outFile)
        {
            using (System.IO.Compression.GZipStream Decompress = new System.IO.Compression.GZipStream(inFile, System.IO.Compression.CompressionMode.Decompress, true))
            {
                // Copy the decompression stream 
                // into the output file.
                Decompress.CopyTo(outFile);
            }
        }

        internal void WriteManifestInfo(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("ID", ID);

            writer.WriteAttributeString("Path", Path);
            writer.WriteAttributeString("CompressedHash", CompressedHash);
            writer.WriteAttributeString("CompressedSize", CompressedSize.ToString());
            writer.WriteAttributeString("UncompressedHash", UncompressedHash);
            writer.WriteAttributeString("UncompressedSize", UncompressedSize.ToString());
        }

        internal void WritePackedInfo(System.Xml.XmlWriter writer)
        {
            if (!IsCompressed)
                PackFile();

            WriteManifestInfo(writer);
            writer.WriteValue(Data);
        }

        /// <summary>
        /// Gets the absolute path.
        /// </summary>
        /// <returns></returns>
        public string GetAbsolutePath()
        {
            string ret = null;
            if (Owner != null)
            {
                ret = Owner.GetAbsolutePath(ID);
            }

            return ret;
        }
    }
}
