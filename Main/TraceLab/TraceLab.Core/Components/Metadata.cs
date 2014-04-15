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
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml.Serialization;
using System.Collections.Generic;
using TraceLab.Core.Settings;
using TraceLab.Core.Utilities;

namespace TraceLab.Core.Components
{
    [Serializable]
    [XmlInclude(typeof(ComponentMetadata))]
    [XmlInclude(typeof(DecisionMetadata))]
    [XmlInclude(typeof(StartNodeMetadata))]
    [XmlInclude(typeof(EndNodeMetadata))]
    [XmlInclude(typeof(ScopeMetadata))]
    [XmlInclude(typeof(LoopScopeMetadata))]
    [XmlInclude(typeof(CompositeComponentMetadata))]
    [XmlInclude(typeof(ComponentTemplateMetadata))]
    [XmlInclude(typeof(ExitDecisionMetadata))]
    [DebuggerDisplay("{Label}")]
    public abstract class Metadata : INotifyPropertyChanged, IXmlSerializable, ISerializable, IModifiable
    {
        private string m_label;
        [XmlAttribute("Label")]
        public string Label
        {
            get
            {
                return m_label;
            }
            set
            {
                if (m_label != value)
                {
                    m_label = value;
                    NotifyPropertyChanged("Label");
                    m_isMetadataModified = true;
                    IsModified = true;
                }

            }
        }

        private bool m_waitsForAllPredecessors = true;
        [XmlAttribute("WaitsForAllPredecessors")]
        public bool WaitsForAllPredecessors
        {
            get
            {
                return m_waitsForAllPredecessors;
            }
            set
            {
                if (m_waitsForAllPredecessors != value)
                {
                    m_waitsForAllPredecessors = value;
                    NotifyPropertyChanged("WaitsForAllPredecessors");
                    m_isMetadataModified = true;
                    IsModified = true;
                }

            }
        }
        
        private bool m_deserializationError/* = false*/;
        [XmlIgnore]
        public bool HasDeserializationError
        {
            get
            {
                return m_deserializationError;
            }
            set
            {
                if (m_deserializationError != value)
                {
                    m_deserializationError = value;
                    NotifyPropertyChanged("HasDeserializationError");
                }
            }
        }

        private string m_deserializationErrorMessage;
        [XmlIgnore]
        public string DeserializationErrorMessage
        {
            get
            {
                return m_deserializationErrorMessage;
            }
            set
            {
                if (m_deserializationErrorMessage != value)
                {
                    m_deserializationErrorMessage = value;
                    NotifyPropertyChanged("DeserializationErrorMessage");
                }
            }
        }

        #region Logging

        protected virtual void InitLoggingNodeSettings()
        {
            var logLevels = new LogLevelItem[6];
            logLevels[0] = new LogLevelItem(NLog.LogLevel.Info, false);
            logLevels[1] = new LogLevelItem(NLog.LogLevel.Trace, false);
            logLevels[2] = new LogLevelItem(NLog.LogLevel.Debug, false);
            logLevels[3] = new LogLevelItem(NLog.LogLevel.Warn, false);
            logLevels[4] = new LogLevelItem(NLog.LogLevel.Error, true, true);
            logLevels[5] = new LogLevelItem(NLog.LogLevel.Fatal, true, true);

            foreach (LogLevelItem logLevel in logLevels)
            {
                LogLevelItemLookup.Add(logLevel.Level, logLevel);
            }
        }
                
        public IEnumerable<LogLevelItem> LogLevels
        {
            get
            {
                return LogLevelItemLookup.Values;
            }
        }

        protected Dictionary<NLog.LogLevel, LogLevelItem> LogLevelItemLookup = new Dictionary<NLog.LogLevel, LogLevelItem>();

        /// <summary>
        /// Enables or disables the log level for the node
        /// </summary>
        /// <param name="logLevel">The log level that is going to be enabled or disabled.</param>
        /// <param name="isEnabled">Enables or disables the given log level.</param>
        public virtual void SetLogLevel(NLog.LogLevel logLevel, bool isEnabled)
        {
            LogLevelItem logLevelItem;
            if (LogLevelItemLookup.TryGetValue(logLevel, out logLevelItem))
            {
                logLevelItem.IsEnabled = isEnabled;
            }
        }

        /// <summary>
        /// Enables/disables and locks/unlocks the log level for the node
        /// </summary>
        /// <param name="logLevel">The log level that is going to be enabled or disabled.</param>
        /// <param name="isEnabled">Enables or disables the given log level.</param>
        /// <param name="isLocked">Locks or unlocks the given log level.</param>
        public void SetLogLevel(NLog.LogLevel logLevel, bool isEnabled, bool isLocked)
        {
            LogLevelItem logLevelItem;
            if (LogLevelItemLookup.TryGetValue(logLevel, out logLevelItem))
            {
                logLevelItem.IsEnabled = isEnabled;
                logLevelItem.IsLocked = isLocked;
            }
        }

        private void GlobalLogLevelSettingChangedEventHandler(object sender, GlobalLogLevelSettingEventArgs args)
        {
            SetLogLevel(args.LogLevelSetting, args.IsLogEnabled);
        }

        private TraceLab.Core.Settings.Settings m_settings;
        public void ListenToGlobalLogLevelSettingChange(TraceLab.Core.Settings.Settings settings)
        {
            if (m_settings != settings)
            {
                if (m_settings != null && m_settings.ExperimentSettings != null)
                {
                    //detach listener
                    m_settings.ExperimentSettings.GlobalLogLevelSettingChanged -= GlobalLogLevelSettingChangedEventHandler;
                }

                m_settings = settings;

                if (m_settings != null && m_settings.ExperimentSettings != null)
                {
                    //listen to the changes of log settings
                    m_settings.ExperimentSettings.GlobalLogLevelSettingChanged += GlobalLogLevelSettingChangedEventHandler;
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged

        [NonSerialized]
        private PropertyChangedEventHandler m_propertyChanged;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                m_propertyChanged += value;
            }
            remove
            {
                m_propertyChanged -= value;
            }
        }

        protected void NotifyPropertyChanged(string property)
        {
            NotifyPropertyChanged(new PropertyChangedEventArgs(property));
        }

        private void NotifyPropertyChanged(PropertyChangedEventArgs e)
        {
            if (m_propertyChanged != null)
                m_propertyChanged(this, e);
        }
        
        #endregion

        public virtual System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public virtual void ReadXml(System.Xml.XmlReader reader)
        {
            IsInitialized = false;
        }

        public virtual void WriteXml(System.Xml.XmlWriter writer) { }

        public virtual void PostProcessReadXml(Components.IComponentsLibrary library, string experimentLocationRoot)
        {
            IsInitialized = true;
            IsModified = false;
        }

        public virtual void UpdateFromDefinition(Components.IComponentsLibrary library)
        {
        }

        private bool m_isInitialized;
        protected bool IsInitialized
        {
            get { return m_isInitialized; }
            set { m_isInitialized = value; }
        }

        private bool m_isModifiedCached;
        private bool m_isMetadataModified;
        private bool m_deferModification;
        public virtual bool IsModified
        {
            get 
            {
                if (m_deferModification)
                {
                    m_isModifiedCached = CalculateModification();
                    m_deferModification = false;
                }

                return m_isModifiedCached; 
            }
            set
            {
                if (m_isModifiedCached != value)
                {
                    m_deferModification = true;
                    NotifyPropertyChanged("IsModified");
                }
            }
        }

        protected virtual bool CalculateModification()
        {
            return m_isMetadataModified;
        }

        public virtual void ResetModifiedFlag()
        {
            m_isMetadataModified = false;
            IsModified = false;
        }

        #region Constructor

        protected Metadata()
        {
        }

        #endregion


        #region Deserialization Constructor

        protected Metadata(SerializationInfo info, StreamingContext context)
        {
            m_label = (string)info.GetValue("m_label", typeof(string));
            m_waitsForAllPredecessors = info.GetBoolean("m_waitsForAllPredecessors");
            m_deserializationError = (bool)info.GetValue("m_deserializationError", typeof(bool));
            m_deserializationErrorMessage = (string)info.GetValue("m_deserializationError", typeof(string));
        }

        #endregion

        #region ISerializable Implementation

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("m_label", m_label);
            info.AddValue("m_waitsForAllPredecessors", m_waitsForAllPredecessors);
            info.AddValue("m_deserializationError", m_deserializationError);
            info.AddValue("m_deserializationErrorMessage", m_deserializationErrorMessage);
        }

        #endregion

        #region ICloneable Members

        public abstract Metadata Clone();

        /// <summary>
        /// Performs a deep copy of the data in this object to another instance of the Metadata.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <remarks>All fields and properties are copied.</remarks>
        protected virtual void CopyFrom(Metadata other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            Label = other.Label;
            WaitsForAllPredecessors = other.WaitsForAllPredecessors;
            DeserializationErrorMessage = other.DeserializationErrorMessage;
            HasDeserializationError = other.HasDeserializationError;

            LogLevelItemLookup = new Dictionary<NLog.LogLevel, LogLevelItem>();
            foreach (KeyValuePair<NLog.LogLevel, LogLevelItem> pair in other.LogLevelItemLookup)
            {
                LogLevelItemLookup.Add(pair.Key, new LogLevelItem(pair.Value.Level, pair.Value.IsEnabled, pair.Value.IsLocked));
            }
        }

        #endregion
    }
}

