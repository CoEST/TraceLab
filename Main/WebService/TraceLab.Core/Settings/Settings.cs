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

namespace TraceLab.Core.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;
    using System.Xml.XPath;

    using TraceLabSDK;

    public sealed class Settings : INotifyPropertyChanged
    {
        #region Fields

        private const int CurrentVersion = 3;

        private static ApplicationSettings s_appSettings = null;
        private static Dictionary<string, ExperimentSettings> s_experimentSettings = new Dictionary<string, ExperimentSettings>();

        private ExperimentSettings m_experimentSettings;

        #endregion Fields

        #region Constructors

        static Settings()
        {
            SettingsPath = System.IO.Path.Combine(ApplicationSettings.AppDataDirectory, "settings.xml");
            LayoutPath = System.IO.Path.Combine(ApplicationSettings.AppDataDirectory, "layout.xml");
            UserTagsPath = System.IO.Path.Combine(ApplicationSettings.AppDataDirectory, "usertags.xml");
            RecentExperimentsPath = System.IO.Path.Combine(ApplicationSettings.AppDataDirectory, "recentexperiments.xml");
        }

        private Settings()
        {
            if (s_appSettings != null)
            {
                s_appSettings.PropertyChanged += NotifyApplicationSettingsPropertyChanged;
            }
        }

        private void NotifyApplicationSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(e.PropertyName);
        }

        #endregion Constructors

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Properties

        public ObservableCollection<SettingsPath> PackagePaths
        {
            get { return s_appSettings.PackagePaths; }
        }

        public ObservableCollection<SettingsPath> ComponentPaths
        {
            get { return s_appSettings.ComponentPaths; }
        }

        public ObservableCollection<SettingsPath> TypePaths
        {
            get { return s_appSettings.TypePaths; }
        }

        public ExperimentSettings ExperimentSettings
        {
            get { return m_experimentSettings; }
        }

        public string DefaultExperiment
        {
            get { return s_appSettings.DefaultExperiment; }
            set { s_appSettings.DefaultExperiment = value; }
        }

        public string WebserviceAddress
        {
            get { return s_appSettings.WebserviceAddress; }
            set 
            {
                if (s_appSettings.DefaultExperimentsDirectory != value)
                {
                    s_appSettings.WebserviceAddress = value;
                }
            }
        }

        public string DefaultExperimentsDirectory
        {
            get { return s_appSettings.DefaultExperimentsDirectory; }
            set { s_appSettings.DefaultExperimentsDirectory = value; }
        }

        public MainWindowData MainWindowRect
        {
            get { return s_appSettings.MainWindowRect; }
        }

        #region Static Properties

        /// <summary>
        /// Gets or sets the settings path.
        /// </summary>
        /// <value>
        /// The settings path.
        /// </value>
        private static string SettingsPath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the path to the current layout file.
        /// </summary>
        /// <value>
        /// The path to the layout file to use.
        /// </value>
        public static string LayoutPath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user tags path.
        /// </summary>
        /// <value>
        /// The user tags path.
        /// </value>
        public static string UserTagsPath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the recent experiments path.
        /// </summary>
        /// <value>
        /// The recent experiments path.
        /// </value>
        public static string RecentExperimentsPath
        {
            get;
            set;
        }

        #endregion Static Properties

        #endregion Properties

        #region Methods


        public static Settings GetSettings()
        {
            if (s_appSettings == null)
            {
                s_appSettings = new ApplicationSettings();

                try
                {
                    if (System.IO.File.Exists(TraceLab.Core.Settings.Settings.SettingsPath))
                    {
                        //deserilize the settings
                        using (var reader = XmlReader.Create(TraceLab.Core.Settings.Settings.SettingsPath))
                        {
                            Deserialize(reader);
                        }
                    }
                }
                catch (Exception)
                {
                    NLog.LogManager.GetCurrentClassLogger().Warn("Settings failed to be restored from previously saved settings.xml (in %APPDATA%/TraceLab). File was corrupted. Application restored default settings.");
                }
            }

            Settings settings = new Settings();
            return settings;
        }

        public static Settings GetSettings(string experimentID)
        {
            Settings settings = GetSettings();
            ExperimentSettings expSettings = null;
            if (experimentID != null)
            {
                if (string.IsNullOrWhiteSpace(experimentID))
                {
                    throw new ArgumentException("experimentID", "Experiment ID cannot be whitespace.");
                }

                if (!s_experimentSettings.TryGetValue(experimentID, out expSettings))
                {
                    expSettings = new ExperimentSettings(s_appSettings, experimentID);
                    s_experimentSettings.Add(experimentID, expSettings);
                }
            }

            settings.m_experimentSettings = expSettings;

            return settings;
        }

        private static void Deserialize(XmlReader topReader)
        {
            if (topReader == null)
                throw new ArgumentNullException("topReader");

            XPathDocument doc = new XPathDocument(topReader);
            var nav = doc.CreateNavigator();

            XPathNavigator iter = nav.SelectSingleNode("/Settings/@Version");
            if (iter == null)
            {
                iter = nav.SelectSingleNode("/Settings/Version");
                if (iter == null)
                {
                    throw new XmlSchemaException("Settings does not have a version element");
                }
            }

            long ver = iter.ValueAsInt;
            if (ver == CurrentVersion)
            {
                ReadCurrentVersion(nav);
            }
            else
            {
                int top = 0;
                int left = 0;
                int height = 0;
                int width = 0;

                // Only the window size is supported from versions 1 and 2.
                left = nav.SelectSingleNode("/Settings/MainWindowData/Left").ValueAsInt;
                top = nav.SelectSingleNode("/Settings/MainWindowData/Top").ValueAsInt;
                height = nav.SelectSingleNode("/Settings/MainWindowData/Height").ValueAsInt;
                width = nav.SelectSingleNode("/Settings/MainWindowData/Width").ValueAsInt;

                s_appSettings.MainWindowRect.Left = left;
                s_appSettings.MainWindowRect.Top = top;
                s_appSettings.MainWindowRect.Height = height;
                s_appSettings.MainWindowRect.Width = width;
            }
        }

        public static void SaveSettings(Settings settings)
        {
            XmlWriter writer = null;
            
            try
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(SettingsPath));

                XmlWriterSettings xmlSettings = new XmlWriterSettings();
                xmlSettings.Indent = true;

                writer = XmlWriter.Create(SettingsPath, xmlSettings);

                settings.WriteXml(writer);
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Error("Settings failed to be saved", ex);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public Settings Clone()
        {
            Settings clone = new Settings();
            
            if(m_experimentSettings != null)
                clone.m_experimentSettings = m_experimentSettings.Clone();

            return clone;
        }


        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            if (reader.LocalName == "Settings")
            {
                XmlReader topReader = reader.ReadSubtree();
                XPathDocument doc = new XPathDocument(topReader);
                var nav = doc.CreateNavigator();

                XPathNavigator iter = nav.SelectSingleNode("/Settings/Version");
                if (iter == null)
                    throw new XmlSchemaException("Settings does not have a version element");

            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.WriteStartElement("Settings");

            writer.WriteAttributeString("Version", CurrentVersion.ToString(CultureInfo.CurrentCulture));

            string appName = System.Reflection.Assembly.GetAssembly(this.GetType()).Location;
            var assemblyName = System.Reflection.AssemblyName.GetAssemblyName(appName);
            var traceLabVersion = assemblyName.Version.ToString();

            writer.WriteElementString("TraceLabVersion", traceLabVersion);

            s_appSettings.WriteXml(writer);

            foreach (ExperimentSettings setting in s_experimentSettings.Values)
            {
                setting.WriteXml(writer);
            }

            writer.WriteEndElement(); // Settings
        }

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private static void ReadCurrentVersion(XPathNavigator nav)
        {
            // Current version is 3.
            ReadVersion3(nav);
        }

        //private void ReadVersion1(XPathNavigator nav)
        //{
        //    var iter = nav.SelectSingleNode("/Settings/GlobalLogLevels");
        //    if (iter == null)
        //        throw new XmlSchemaException("Settings elements does not match the required elements for this version.");
        //    else if (iter.Value != "null")
        //    {
        //        XmlReader pathReader = iter.ReadSubtree();
        //        pathReader.MoveToContent();
        //        pathReader.Read();
        //        XmlSerializer serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(SerializableDictionary<LogLevel, GlobalLogLevelSetting>), null);
        //        var logLevels = (SerializableDictionary<LogLevel, GlobalLogLevelSetting>)serializer.Deserialize(pathReader);

        //        foreach (KeyValuePair<LogLevel, GlobalLogLevelSetting> pair in logLevels)
        //        {
        //            m_globalLogLevelSettingLookup[pair.Key].IsEnabled = pair.Value.IsEnabled;
        //            m_globalLogLevelSettingLookup[pair.Key].IsLocked = pair.Value.IsLocked;
        //        }
        //    }

        //    iter = nav.SelectSingleNode("/Settings/MainWindowData");
        //    if (iter == null)
        //        throw new XmlSchemaException("Settings elements does not match the required elements for this version.");
        //    else if (!string.IsNullOrEmpty(iter.Value) && iter.Value != "null")
        //    {
        //        XmlReader pathReader = iter.ReadSubtree();
        //        //pathReader.MoveToContent();
        //        //pathReader.Read();
        //        XmlSerializer serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(MainWindowData), null);
        //        MainWindowRect = (MainWindowData)serializer.Deserialize(pathReader);
        //    }
        //}

        //private void ReadVersion2(XPathNavigator nav)
        //{
        //    // Version 1 data hasn't changed.
        //    ReadVersion1(nav);
        //}

        private static void ReadVersion3(XPathNavigator nav)
        {
            var child = nav.SelectSingleNode("/Settings/ApplicationSettings");

            s_appSettings.ReadXml(child);

            List<ExperimentSettings> settings = new List<ExperimentSettings>();
            var nodeIterator = nav.Select("/Settings/ExperimentSettings");
            while (nodeIterator.MoveNext())
            {
                ExperimentSettings newSettings = new ExperimentSettings(s_appSettings, null);
                newSettings.ReadXml(nodeIterator.Current);

                settings.Add(newSettings);
            }

            var nonExpiredSettings = settings.Where((o) => 
            {
                return (DateTime.Now - o.LastOpened) < s_appSettings.ExperimentSettingsTTL;
            });

            foreach(ExperimentSettings expSetting in nonExpiredSettings)
            {
                s_experimentSettings.Add(expSetting.ExperimentID, expSetting);
            }
        }

        #endregion Methods
    }
}