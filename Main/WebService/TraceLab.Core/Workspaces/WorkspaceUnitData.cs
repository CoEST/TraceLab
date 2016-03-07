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
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;
using TraceLab.Core;
using TraceLabSDK;

namespace TraceLab.Core.Workspaces
{
    [Serializable]
    public sealed class WorkspaceUnitData : ISerializable, IXmlSerializable, IRawSerializable
    {
        private const long CurrentVersion = 3;

        private WorkspaceUnitData()
        {
        }

        public WorkspaceUnitData(object data)
        {
            if (data == null)
                throw new ArgumentNullException("data", "Unable to serialize null data");

            Data = data;
        }

        /// <summary>
        /// Whether this WorkspaceUnitData should be treated as a temporary object
        /// </summary>
        public bool IsTemporary { get; set; }

        ///// <summary>
        ///// The type of the item that was stored in the workspace.
        ///// </summary>
        //private  Type DataType { get; set; }

        /// <summary>
        /// The data of the item that was stored in the workspace
        /// </summary>
        public object Data { get; set; }

        #region Binary Serialization

        private WorkspaceUnitData(SerializationInfo info, StreamingContext context)
        {
            long ver = info.GetInt64("Version");

            if (ver == CurrentVersion)
            {
                ReadCurrentVersion(info);
            }
            else if (ver == 1)
            {
                ReadVersion1(info);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private void ReadCurrentVersion(SerializationInfo info)
        {
            IsTemporary = info.GetBoolean("IsTemporary");
            var dataType = (Type)info.GetValue("DataType", typeof(Type));
            Data = info.GetValue("Data", dataType);
        }

        private void ReadVersion1(SerializationInfo info)
        {
            IsTemporary = false;
            var dataType = (Type)info.GetValue("DataType", typeof(Type));
            Data = info.GetValue("Data", dataType);
        }


        /// <summary>
        /// Serializes the WorkspaceUnitData to a binary stream.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            info.AddValue("Version", CurrentVersion);
            info.AddValue("IsTemporary", IsTemporary);
            Type dataType = Data != null ? Data.GetType() : null;
            info.AddValue("DataType", dataType, typeof(Type));
            info.AddValue("Data", Data, dataType);
        }



        #endregion

        #region Xml Serialization

        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Writing always writes the latest version.
        /// </summary>
        /// <param name="writer"></param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.WriteElementString("Version", CurrentVersion.ToString(CultureInfo.CurrentCulture));
            writer.WriteElementString("IsTemporary", IsTemporary ? "1" : "0");
            writer.WriteStartElement("DataType");
            var dataType = Data != null ? Data.GetType() : null;
            writer.WriteValue(dataType != null ? dataType.GetTraceLabQualifiedName() : "null");
            writer.WriteEndElement();

            // Serialize the data.
            writer.WriteStartElement("Data");

            if (dataType != null)
            {
                var serial = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(dataType, null);
                serial.Serialize(writer, Data);
            }

            writer.WriteEndElement();
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            if (reader.LocalName == "WorkspaceUnitData")
            {
                XmlReader subReader = reader.ReadSubtree();
                XPathDocument doc = new XPathDocument(subReader);
                var nav = doc.CreateNavigator();

                XPathNavigator iter = nav.SelectSingleNode("/WorkspaceUnitData/Version");
                if (iter == null)
                    throw new XmlSchemaException("WorkspaceUnit does not have a version element");

                long ver = iter.ValueAsLong;

                if (ver == CurrentVersion)
                {
                    ReadCurrentVersion(nav);
                }
                else if (ver == 2 || ver == 1)
                {
                    throw new NotSupportedException(string.Format("WorkspaceUnitData version {0} is no longer supported.", ver));
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        private void ReadCurrentVersion(XPathNavigator nav)
        {
            ReadIsTemporary(nav);
            ReadDataPayload(nav);
        }

        private void ReadDataPayload(XPathNavigator nav)
        {
            var iter = nav.SelectSingleNode("/WorkspaceUnitData/DataType");
            if (iter == null)
                throw new XmlSchemaException("WorkspaceUnit elements does not match the required elements for this version.");
            var dataType = Type.GetType(iter.Value);

            if (iter.Value != "null")
            {
                iter = nav.SelectSingleNode("/WorkspaceUnitData/Data");
                if (iter == null)
                    throw new XmlSchemaException("WorkspaceUnit elements does not match the required elements for this version.");

                XmlReader dataReader = iter.ReadSubtree();
                dataReader.MoveToContent();
                dataReader.Read();
                XmlSerializer serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(dataType, null);
                Data = serializer.Deserialize(dataReader);
            }
        }

        private void ReadIsTemporary(XPathNavigator nav)
        {
            var iter = nav.SelectSingleNode("/WorkspaceUnitData/IsTemporary");
            if (iter == null)
                throw new XmlSchemaException("WorkspaceUnit elements does not match the required elements for this version.");

            IsTemporary = iter.ValueAsBoolean;
        }

        //private bool shouldWriteVersion1;
        //public void WriteVersion1()
        //{
        //    shouldWriteVersion1 = true;
        //}

        //private void WriteVersion1(XmlWriter writer)
        //{
        //    writer.WriteElementString("Version", "1");

        //    writer.WriteStartElement("DataType");
        //    var dataType = Data != null ? Data.GetType() : null;
        //    writer.WriteValue(dataType != null ? dataType.GetTraceLabQualifiedName() : "null");
        //    writer.WriteEndElement();

        //    // Serialize the data.
        //    writer.WriteStartElement("Data");

        //    if (dataType != null)
        //    {
        //        var serial = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(dataType, null);
        //        serial.Serialize(writer, Data);
        //    }

        //    writer.WriteEndElement();
        //}


        #endregion



        #region IRawSerializable Members

        public void ReadData(System.IO.BinaryReader reader)
        {
            var dataVersion = reader.ReadInt64();
            if (dataVersion == CurrentVersion)
            {
                ReadCurrentRawVersion(reader);
            }
        }

        void ReadCurrentRawVersion(System.IO.BinaryReader reader)
        {
            IsTemporary = reader.ReadBoolean();
            string dataType = reader.ReadString();
            var type = Type.GetType(dataType);

            int dataSize = reader.ReadInt32();
            var dataBytes = reader.ReadBytes(dataSize);

            var mem = new System.IO.MemoryStream(dataBytes);

            var rawInterface = type.GetInterface(typeof(IRawSerializable).FullName);
            if (rawInterface != null)
            {
                System.IO.BinaryReader dataReader = new System.IO.BinaryReader(mem);

                IRawSerializable rawData = (IRawSerializable)Activator.CreateInstance(type, true);
                rawData.ReadData(dataReader);

                Data = rawData;
            }
            else
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                Data = formatter.Deserialize(mem);
            }
        }

        public void WriteData(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.Write(CurrentVersion);
            writer.Write(IsTemporary);
            Type dataType = Data != null ? Data.GetType() : null;
            writer.Write(dataType.AssemblyQualifiedName);

            var mem = new System.IO.MemoryStream();

            IRawSerializable rawInterface = Data as IRawSerializable;
            if (rawInterface != null)
            {
                System.IO.BinaryWriter dataWriter = new System.IO.BinaryWriter(mem);
                rawInterface.WriteData(dataWriter);
            }
            else
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(mem, Data);
            }

            // A more difficult way to solve this is to break the data into chunks if it is larger than int.MaxValue 
            // This is a problem because BinaryReader.ReadBytes cannot read more than int.MaxValue at a time.
            if (mem.Length > int.MaxValue)
            {
                throw new InvalidOperationException("Data size is too big");
            }

            writer.Write((int)mem.Length);
            writer.Write(mem.GetBuffer());
        }

        #endregion
    }


}