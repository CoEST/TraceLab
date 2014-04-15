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
using System.Xml;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TraceLab.Core.Workspaces.Serialization
{
    public class CachingSerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CachingSerializer"/> class.
        /// </summary>
        /// <param name="streamManager">The stream manager.</param>
        /// <param name="path">The path.</param>
        /// <param name="cachePath">The cache path.</param>
        /// <param name="supportedTypes">The supported types.</param>
        /// <param name="writeToDisc">if set to <c>true</c> the streams are written to disc. Note, if the value is false, then the writeXml does not matter, cause xml is not going to be written to disc anyway</param>
        /// <param name="writeXml">if set to <c>true</c> [write XML].</param>
        public CachingSerializer(StreamManager streamManager, string path, string cachePath, Type[] supportedTypes, bool writeToDisc, bool writeXml)
        {
            StreamMgr = streamManager;
            DataPath = path; //assures path is not null and that it is absolute path
            CachePath = cachePath; //assures path is not null and that it is absolute path
            SupportedTypes = supportedTypes;
            WriteXml = writeXml;
            WriteToDisc = writeToDisc;
        }

        public void SetPaths(string newDataPath, string newCachePath)
        {
            DataPath = newDataPath; //assures path is not null and that it is absolute path
            CachePath = newCachePath; //assures path is not null and that it is absolute path
        }

        #region Deserialize (retained for testing convenience)

        public T Deserialize<T>()
        {
            return (T)this.Deserialize(typeof(T));
        }

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public Object Deserialize(Type type)
        {
            var dataFileInfo = new FileInfo(DataPath);
            var cacheFileInfo = new FileInfo(CachePath);

            object retVal = null;

            //if write cache to disc is false, then nothing is stored on the disc
            //simply get representation from memory cache
            if (WriteToDisc == false)
            {
                retVal = DeserializeCache(type);
            }
            else if ((WriteXml && dataFileInfo.Exists) || (WriteXml == false && cacheFileInfo.Exists))
            {
                // If we're writing the XML, then the xml file needs to exist, since it's the master.

                if (cacheFileInfo.Exists && dataFileInfo.LastWriteTime <= cacheFileInfo.LastWriteTime)
                {
                    retVal = DeserializeCache(type);
                }

                // If we failed to load from the cache, then load from the data.
                if(WriteXml && retVal == null)
                {
                    retVal = DeserializeData(type);
                }
            }
            else
            {
                retVal = null;
            }

            return retVal;
        }
        
        /// <summary>
        /// Deserializes the data.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private Object DeserializeData(Type type)
        {
            Stream dataStream = StreamMgr.GetStream(DataPath, false); //always deserialize from xml file

            string path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString());
            System.IO.Directory.CreateDirectory(path);

            var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(type, SupportedTypes);
            object obj = null;

            try
            {
                obj = serializer.Deserialize(dataStream);
            }
            catch (Exception)
            {
                // If there is an error while deserializing the data, then our data cachefile is bad - 
                // delete it and return null
                obj = null;
                StreamMgr.CloseStream(DataPath);
                File.Delete(DataPath);
            }

            if (obj != null)
            {
                //update the cache
                SerializeCache(obj);
            }

            StreamMgr.CloseStream(DataPath);

            return obj;
        }

        /// <summary>
        /// Deserializes the cache.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private Object DeserializeCache(Type type)
        {
            object obj = null;

            Stream cacheStream = StreamMgr.GetStream(CachePath, !WriteToDisc);
            if (cacheStream.Length != 0)
            {
                long cachePos = cacheStream.Position;

                try
                {
                    if (type.GetInterface(typeof(TraceLabSDK.IRawSerializable).FullName) != null)
                    {
                        obj = Activator.CreateInstance(type, true);
                        var reader = new System.IO.BinaryReader(cacheStream);
                        ((TraceLabSDK.IRawSerializable)obj).ReadData(reader);
                    }
                    else
                    {
                        var binaryFormatter = new BinaryFormatter();
                        obj = binaryFormatter.Deserialize(cacheStream);
                    }
                }
                catch
                {
                    obj = null;
                    // If we failed to deserialize the cache, the cache is
                    // corrupt or invalid - attempt to delete the binary cache
                    File.Delete(CachePath);
                }

                cacheStream.Position = cachePos;
            }
            
            return obj;
        }

        #endregion

        #region Serialize (retained for testing convenience)

        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Serialize(Object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value", "Object to be serialized cannot be null.");
            }

            Type type = value.GetType();

            if (WriteXml)
            {
                SerializeData(type, value);
            }
            SerializeCache(value);
        }

        /// <summary>
        /// Serializes the data.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="obj">The obj.</param>
        private void SerializeData(Type type, Object obj)
        {
            Stream dataStream = StreamMgr.GetStream(DataPath, false); //always write xml file

            string path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString());
            System.IO.Directory.CreateDirectory(path);

            var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(type, SupportedTypes);
            serializer.Serialize(dataStream, obj);
            StreamMgr.Flush(DataPath);
            //close data stream
            StreamMgr.CloseStream(DataPath);
        }

        /// <summary>
        /// Serializes the cache.
        /// </summary>
        /// <param name="obj">The obj.</param>
        private void SerializeCache(object obj)
        {
            Stream cacheStream = StreamMgr.GetStream(CachePath, !WriteToDisc);

            long cachePos = cacheStream.Position;

            var rawObject = obj as TraceLabSDK.IRawSerializable;
            if (rawObject != null)
            {
                var writer = new System.IO.BinaryWriter(cacheStream);
                rawObject.WriteData(writer);
            }
            else
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(cacheStream, obj);
            }
            StreamMgr.Flush(CachePath);

            cacheStream.Position = cachePos;
        }

        #endregion

        #region Get Byte Representation

        public byte[] GetByteRepresentation()
        {
            var dataFileInfo = new FileInfo(DataPath);
            var cacheFileInfo = new FileInfo(CachePath);

            byte[] retVal = null;

            //if write cache to disc is false, then nothing is stored on the disc
            //simply get representation from memory cache
            if (WriteToDisc == false)
            {
                retVal = GetByteRepresentationFromCache();
            } 
            else if ((WriteXml && dataFileInfo.Exists) || (WriteXml == false && cacheFileInfo.Exists))
            {
                // If we're writing the XML, then the xml file needs to exist, since it's the master.

                if (cacheFileInfo.Exists && dataFileInfo.LastWriteTime <= cacheFileInfo.LastWriteTime)
                {
                    retVal = GetByteRepresentationFromCache();
                }

                // If we failed to load from the cache, then load from the data.
                if (WriteXml && retVal == null)
                {
                    retVal = GetByteRepresentationFromXml();
                }
            }
            else
            {
                retVal = null;
            }

            // If the byte stream is empty, then return null as our 'invalid data' value.
            if (retVal != null && retVal.Length == 0)
            {
                retVal = null;
            }

            return retVal;
        }

        private byte[] GetByteRepresentationFromCache()
        {
            Stream cacheStream = StreamMgr.GetStream(CachePath, !WriteToDisc);

            long cachePos = cacheStream.Position;
            
            byte[] bytes = new byte[cacheStream.Length];
          
            try
            {
                cacheStream.Read(bytes, 0, bytes.Length);
            }
            finally
            {
                cacheStream.Position = cachePos;
            }

            return bytes;
        }
        
        private byte[] GetByteRepresentationFromXml()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Serialize Bytes

        public void SetBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes", "Bytes to save cannot be null.");
            }

            //Type type = value.GetType();

            if (WriteXml)
            {
                //SerializeBytesToXml(type, value);
            }
            SetBytesToCache(bytes);
        }

        private void SerializeBytesToXml(Type type, Object obj)
        {
            throw new NotImplementedException();
        }

        private void SetBytesToCache(byte[] bytes)
        {
            Stream cacheStream = StreamMgr.GetStream(CachePath, !WriteToDisc);

            long cachePos = cacheStream.Position;

            cacheStream.Write(bytes, 0, bytes.Length);
            StreamMgr.Flush(CachePath);

            cacheStream.Position = cachePos;
        }

        #endregion

        private Type[] SupportedTypes
        {
            set;
            get;
        }

        private StreamManager m_streamManager;
        private StreamManager StreamMgr
        {
            get
            {
                return m_streamManager;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value", "Stream manager cannot be null");

                m_streamManager = value;
            }
           
        }

        private string m_dataPath;
        private string DataPath
        {
            get
            {
                return m_dataPath;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value", "Data path cannot be null");
                if (!System.IO.Path.IsPathRooted(value))
                    throw new ArgumentException("Absolute data path information is required.", "value");

                m_dataPath = value;
            }
        }

        private string m_cachePath;
        private string CachePath
        {
            get
            {
                return m_cachePath;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value", "Cache path cannot be null");
                if (!System.IO.Path.IsPathRooted(value))
                    throw new ArgumentException("Absolute cache path information is required.", "value");

                m_cachePath = value;
            }
        }

        private bool WriteXml { get; set; }
        private bool WriteToDisc { get; set; }
    }
}

