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
using TraceLabSDK;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.XPath;
using TraceLab.Core.Utilities;
using System.Xml.Schema;

namespace TraceLab.Core.Settings
{
    class GlobalLogLevelCollection : ObservableKeyedCollection<NLog.LogLevel, GlobalLogLevelSetting>
    {
        protected override NLog.LogLevel GetKeyForItem(GlobalLogLevelSetting item)
        {
            return item.Level;
        }
    }

    public class ExperimentSettings : INotifyPropertyChanged
    {
        private static GlobalLogLevelCollection s_defaultSettings;

        private const int CurrentVersion = 1;
        private readonly SerializableDictionary<LogLevel, GlobalLogLevelSetting> m_globalLogLevelSettingLookup;
        private ObservableCollection<GlobalLogLevelSetting> m_globalLogLevelSettings = new ObservableCollection<GlobalLogLevelSetting>();
        private ApplicationSettings m_appSettings;
        private string m_experimentID;
        private bool m_isDefault;

        static ExperimentSettings()
        {
            GlobalLogLevelCollection logLevels = new GlobalLogLevelCollection();
            logLevels.Add(new GlobalLogLevelSetting(NLog.LogLevel.Info, true, false));
            logLevels.Add(new GlobalLogLevelSetting(NLog.LogLevel.Trace, true, false));
            logLevels.Add(new GlobalLogLevelSetting(NLog.LogLevel.Debug, true, false));
            logLevels.Add(new GlobalLogLevelSetting(NLog.LogLevel.Warn, true, false));
            logLevels.Add(new GlobalLogLevelSetting(NLog.LogLevel.Error, true, true));
            logLevels.Add(new GlobalLogLevelSetting(NLog.LogLevel.Fatal, true, true));

            s_defaultSettings = logLevels;
        }

        internal ExperimentSettings(ApplicationSettings appSettings, string id)
        {
            if (appSettings == null)
                throw new ArgumentNullException("appSettings");
            // if it's whitespace
            if (id != null && string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException("id");

            m_globalLogLevelSettingLookup = new SerializableDictionary<LogLevel, GlobalLogLevelSetting>();
            SetupDefaultGlobalLogLevels();

            m_appSettings = appSettings;
            m_experimentID = id;

            m_isDefault = true;
        }


        internal ExperimentSettings Clone()
        {
            return Clone(m_experimentID);
        }

        internal ExperimentSettings Clone(string id)
        {
            ExperimentSettings clone = new ExperimentSettings(m_appSettings, id);
            foreach (GlobalLogLevelSetting setting in m_globalLogLevelSettings)
            {
                clone.SetGlobalLogLevel(setting.Level, setting.IsEnabled);
            }

            clone.m_isDefault = m_isDefault;

            return clone;
        }

        public GlobalLogLevelSetting GetGlobalLogLevel(NLog.LogLevel level)
        {
            GlobalLogLevelSetting val = null;
            LogLevel lev = level;
            m_globalLogLevelSettingLookup.TryGetValue(lev, out val);
            return val;
        }

        [XmlIgnore]
        public IEnumerable<SettingsPath> ComponentPaths
        {
            get { return m_appSettings.ComponentPaths; }
        }

        [XmlIgnore]
        public IEnumerable<SettingsPath> TypePaths
        {
            get { return m_appSettings.TypePaths; }
        }

        [XmlAttribute("ExperimentID")]
        public string ExperimentID
        {
            get
            {
                return m_experimentID;
            }
            set
            {
                if (m_experimentID != value)
                {
                    m_experimentID = value;
                    NotifyPropertyChanged("ExperimentID");
                }
            }
        }

        [XmlIgnore]
        public bool IsDefault
        {
            get { return m_isDefault; }
            internal set { m_isDefault = value; }
        }

        private DateTime m_lastOpened;
        [XmlIgnore]
        public DateTime LastOpened
        {
            get { return m_lastOpened; }
            set
            {
                if (m_lastOpened != value)
                {
                    m_lastOpened = value;
                    NotifyPropertyChanged("LastOpened");
                }
            }
        }

        /// <summary>
        /// We need a special property for DateTime objects because they don't serialize properly to Xml (they have trouble with always being local-time)
        /// </summary>
        [XmlAttribute("LastOpened")]
        public string LastOpenedXml
        {
            get { return LastOpened.ToString("yyyy-MM-ddTHH:mm:ss.fffffff"); }
            set
            {
                LastOpened = XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.Utc);
            }
        }

        #region GlobalLogLevelSettings

        /// <summary>
        /// Occurs when the global (per experiment) log level setting changes. All nodes in the experiment listens to this event.
        /// </summary>
        public event EventHandler<GlobalLogLevelSettingEventArgs> GlobalLogLevelSettingChanged;

        public IEnumerable<GlobalLogLevelSetting> GlobalLogLevelsSettings
        {
            get
            {
                return m_globalLogLevelSettings;
            }
            set
            {
                foreach (GlobalLogLevelSetting setting in value)
                {
                    SetGlobalLogLevel(setting.Level, setting.IsEnabled);
                }
            }
        }

        /// <summary>
        /// Enables/Disables the log level per entire experiment. 
        /// </summary>
        /// <param name="level">The level to be enabled/disabled</param>
        /// <param name="enable">Enable or disable</param>
        public void SetGlobalLogLevel(NLog.LogLevel level, bool enable)
        {
            GlobalLogLevelSetting logLevelSetting;
            if (m_globalLogLevelSettingLookup.TryGetValue(level, out logLevelSetting))
            {
                if (!logLevelSetting.IsLocked)
                {
                    logLevelSetting.IsEnabled = enable;

                    if (GlobalLogLevelSettingChanged != null)
                    {
                        GlobalLogLevelSettingChanged(this, new GlobalLogLevelSettingEventArgs(logLevelSetting.Level, logLevelSetting.IsEnabled));
                    }
                }
            }
        }

        /// <summary>
        /// Sets the global log level setting.
        /// </summary>
        /// <param name="logLevelSetting">The log level setting.</param>
        public void SetGlobalLogLevelSetting(GlobalLogLevelSetting logLevelSetting)
        {
            if (logLevelSetting == null)
                throw new ArgumentNullException("logLevelSetting");

            //update global log level settings
            SetGlobalLogLevel(logLevelSetting.Level, logLevelSetting.IsEnabled);
        }

        private void SetupDefaultGlobalLogLevels()
        {
            foreach (GlobalLogLevelSetting logLevel in s_defaultSettings)
            {
                var setting = new GlobalLogLevelSetting(logLevel.Level, logLevel.IsEnabled, logLevel.IsLocked);
                m_globalLogLevelSettingLookup.Add(setting.Level, setting);
                m_globalLogLevelSettings.Add(setting);
            }
        }
        #endregion

        #region INotifyPropertyChanged Members

        private void NotifyPropertyChanged(string property)
        {
            // ExperimentID is not to be considered when determining if settings are "default" settings.
            if (property != "ExperimentID")
                IsDefault = false;

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("ExperimentSettings");
            writer.WriteAttributeString("Version", CurrentVersion.ToString());

            writer.WriteElementString("ExperimentID", ExperimentID);
            writer.WriteElementString("LastOpened", LastOpened.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffffff"));

            // ExperimentLogLevels
            {
                List<GlobalLogLevelSetting> settings = new List<GlobalLogLevelSetting>();

                foreach (GlobalLogLevelSetting setting in m_globalLogLevelSettings)
                {
                    GlobalLogLevelSetting defaultVersion = s_defaultSettings[setting.Level];
                    if (setting.IsEnabled != defaultVersion.IsEnabled)
                    {
                        settings.Add(setting);
                    }
                }

                if (settings.Count > 0)
                {
                    XmlSerializer serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(List<GlobalLogLevelSetting>), null);
                    serializer.Serialize(writer, settings);
                }
            }

            writer.WriteEndElement(); // ExperimentSettings
        }

        internal void ReadXml(XPathNavigator nav)
        {
            var iter = nav.SelectSingleNode("./@Version");
            if (iter == null)
                throw new XmlSchemaException("Settings does not have a version element");

            long ver = iter.ValueAsInt;
            if (ver == CurrentVersion)
            {
                ReadCurrentVersion(nav);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private void ReadCurrentVersion(XPathNavigator nav)
        {
            ReadVersion1(nav);
        }

        private void ReadVersion1(XPathNavigator nav)
        {
            var iter = nav.SelectSingleNode("./ExperimentID");
            if (iter == null)
            {
                throw new XmlSchemaException("ExperimentSettings does not have the correct elements.");
            }

            m_experimentID = iter.Value;

            iter = nav.SelectSingleNode("./LastOpened");
            if (iter == null)
            {
                throw new XmlSchemaException("ExperimentSettings does not have the correct elements.");
            }

            m_lastOpened = XmlConvert.ToDateTime(iter.Value, XmlDateTimeSerializationMode.Utc);

            iter = nav.SelectSingleNode("./ArrayOfGlobalLogLevelSetting");
            if (iter != null)
            {
                //iter.MoveToChild(XPathNodeType.Element);
                List<GlobalLogLevelSetting> settings = null;

                XmlReader pathReader = iter.ReadSubtree();
                pathReader.MoveToContent();
                XmlSerializer serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(List<GlobalLogLevelSetting>), null);
                settings = (List<GlobalLogLevelSetting>)serializer.Deserialize(pathReader);

                if (settings.Count > 0)
                {
                    GlobalLogLevelsSettings = settings;
                }
            }
        }
    }
}
