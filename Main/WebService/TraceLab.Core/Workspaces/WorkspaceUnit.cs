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
using System.Xml;
using TraceLab.Core.Workspaces.Serialization;
using System.Runtime.Serialization;

namespace TraceLab.Core.Workspaces
{
    public class WorkspaceUnit : INotifyPropertyChanged
    {
        #region Fields

        private string m_friendlyUnitName;
        private string m_realUnitName;
        private string m_shortUnitName;
        private DateTime m_timestamp;
        private Type m_unitType;

        #endregion

        internal WorkspaceUnit(string realUnitName, CachingSerializer serializer)
        {
            if (serializer == null)
                throw new ArgumentNullException("serializer", "m_workspace serializer is not allowed to be null");

            //note currently the short name and friendly name are the same, but potentially friendly name can include scope name, etc, but we do not want to mix the two
            ShortUnitName = GetShortUnitName(realUnitName);
            FriendlyUnitName = ShortUnitName;

            RealUnitName = realUnitName;
            Serializer = serializer;
        }

        internal void Rename(string newRealUnitName, string newDataPath, string newCachePath)
        {
            FriendlyUnitName = GetShortUnitName(newRealUnitName);
            RealUnitName = newRealUnitName;
            Serializer.SetPaths(newDataPath, newCachePath);
        }

        private static string GetShortUnitName(string realUnitName)
        {
            string unit = realUnitName.Remove(0, realUnitName.LastIndexOf(Workspace.NAMESPACE_DELIMITER) + 1);
            return unit;
        }

        /// <summary>
        /// Gets the name of the unit for display.
        /// </summary>
        /// <value>
        /// The name of the friendly unit.
        /// </value>
        public string FriendlyUnitName
        {
            get { return m_friendlyUnitName; }
            private set
            {
                m_friendlyUnitName = value;
                OnPropertyChanged("FriendlyUnitName");
            }
        }

        /// <summary>
        /// Gets the name of the real unit with full namespace path
        /// </summary>
        /// <value>
        /// The name of the real unit.
        /// </value>
        public string RealUnitName
        {
            get { return m_realUnitName; }
            private set
            {
                m_realUnitName = value;
                OnPropertyChanged("RealUnitName");
            }
        }

        /// <summary>
        /// Gets the short name of the unit - without namespace.
        /// </summary>
        /// <value>
        /// The short name of the unit.
        /// </value>
        internal string ShortUnitName
        {
            get { return m_shortUnitName; }
            private set
            {
                m_shortUnitName = value;
                OnPropertyChanged("ShortUnitName");
            }
        }

        #region Data

        /// <summary>
        /// Data property contains object stored in the workspace. 
        /// Note that TraceLab during experiment execution uses DataBytes instead to avoid double serialization
        /// when crossing AppDomains border.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public object Data
        {
            get { return GetData(); }
            set { SetData(value); }
        }

        /// <summary>
        /// Gets the data.
        /// Note that TraceLab during experiment execution uses GetDataBytes instead to avoid double serialization
        /// when crossing AppDomains border.
        /// </summary>
        /// <returns></returns>
        private object GetData()
        {
            if (Serializer == null)
                throw new InvalidOperationException();

            object ret = null;

            try
            {
                lock (Serializer)
                {
                    var workspaceData = (WorkspaceUnitData)Serializer.Deserialize(typeof(WorkspaceUnitData));
                    if (workspaceData != null)
                    {
                        ret = workspaceData.Data;
                    }
                }
            }
            catch (XmlException) { }
            catch (NotImplementedException) { }
            catch (InvalidOperationException) { }
            catch (NotSupportedException) { }

            return ret;
        }

        /// <summary>
        /// Sets the data.
        /// Note that TraceLab during experiment execution uses SetDataBytes instead to avoid double serialization
        /// when crossing AppDomains border.
        /// </summary>
        /// <param name="data">The data.</param>
        private void SetData(object data)
        {
            lock (Serializer)
            {
                if (data != null)
                {
                    var dataType = data.GetType();
                    if (dataType.IsClass && !dataType.IsEnum && dataType != typeof(string) && dataType.GetConstructor(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic, null, Type.EmptyTypes, null) == null)
                    {
                        throw new SerializationException("Object must have a parameterless constructor");
                    }

                    var workspaceData = new WorkspaceUnitData(data);
                    Serializer.Serialize(workspaceData);

                    //store also data type, so it can be accessed without deserialization
                    Type = dataType;
                    
                    OnPropertyChanged("Data");
                }
            }
        }

        #endregion

        #region DataBytes

        /// <summary>
        /// Gets the data bytes.
        /// Preferred way of accessing data across AppDomains. Used during experiment execution.
        /// </summary>
        /// <returns></returns>
        internal byte[] GetDataBytes()
        {
            byte[] bytes = null;

            try
            {
                lock (Serializer)
                {
                    bytes = Serializer.GetByteRepresentation();
                }
            }
            catch (XmlException) { }
            catch (InvalidOperationException) { }
            catch (NotSupportedException) { }

            return bytes;
        }

        /// <summary>
        /// Set the data bytes.
        /// Preferred way of setting data across AppDomains. Used during experiment execution.
        /// </summary>
        /// <param name="bytes">Bytes representing unit object</param>
        /// <param name="unitType">The type of the object that is stored. It allows checking the unit type without deserilizing data each time.</param>
        internal void SetDataBytes(byte[] bytes, Type unitType)
        {
            lock (Serializer)
            {
                if (bytes != null)
                {
                    Serializer.SetBytes(bytes);

                    Type = unitType;
                    OnPropertyChanged("Data");
                }
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>
        /// The timestamp.
        /// </value>
        public DateTime Timestamp
        {
            get { return m_timestamp; }
            internal set
            {
                if (m_timestamp != value)
                {
                    m_timestamp = value;
                    OnPropertyChanged("Timestamp");
                }
            }
        }

        public Type Type
        {
            get { return m_unitType; }
            private set 
            {
                if (m_unitType != value)
                {
                    m_unitType = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        private CachingSerializer Serializer { get; set; }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}
