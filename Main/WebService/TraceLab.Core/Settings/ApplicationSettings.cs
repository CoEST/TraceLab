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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Schema;
using System.Xml;

namespace TraceLab.Core.Settings
{
    class ApplicationSettings : INotifyPropertyChanged
    {
        private const int CurrentVersion = 1;

        private TimeSpan m_experimentSettingsTTL = new TimeSpan(14, 0, 0, 0);
        private ObservableCollection<SettingsPath> m_componentPaths = new ObservableCollection<SettingsPath>();
        private string m_defaultExperiment = String.Empty;
        private ObservableCollection<SettingsPath> m_typePaths = new ObservableCollection<SettingsPath>();
        private ObservableCollection<SettingsPath> m_packagePaths = new ObservableCollection<SettingsPath>();

        static ApplicationSettings()
        {
            IsRunningOnMono = Type.GetType("Mono.Runtime") != null;
            AppDataDirectory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TraceLab");
            CreateAppDataDirectory();
        }

        private static void CreateAppDataDirectory()
        {
            if (System.IO.Directory.Exists(AppDataDirectory) == false)
            {
                //attempt to create the directory (on windows it should have been already created. 
                try
                {
                    System.IO.Directory.CreateDirectory(AppDataDirectory);
                }
                catch (Exception)
                {
                    NLog.LogManager.GetCurrentClassLogger().Warn("Application data folder does not exist and failed to be created. Components library cache, user components tags, and tracelab settings wod after TraceLab closes");
                }
            }
        }

        public ApplicationSettings()
        {
            MainWindowRect = new MainWindowData();
            MainWindowRect.Left = 200;
            MainWindowRect.Top = 100;
            MainWindowRect.Height = 600;
            MainWindowRect.Width = 800;
        }

        public static bool IsRunningOnMono
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the app data directory. (%APPDATA% on windows, or /user/.config/TraceLab
        /// </summary>
        public static string AppDataDirectory
        {
            get;
            private set;
        }
        
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

        public ObservableCollection<SettingsPath> PackagePaths
        {
            get { return m_packagePaths; }
        }

        public ObservableCollection<SettingsPath> ComponentPaths
        {
            get
            {
                return m_componentPaths;
            }
        }

        public string DefaultExperiment
        {
            get { return m_defaultExperiment; }
            set
            {
                if (m_defaultExperiment != value && (!string.IsNullOrEmpty(m_defaultExperiment) || !string.IsNullOrEmpty(value)))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        m_defaultExperiment = string.Empty;
                    }
                    else
                    {
                        m_defaultExperiment = value;
                    }

                    NotifyPropertyChanged("DefaultExperiment");
                }
            }
        }

        private string m_webserviceAddress;
        public string WebserviceAddress
        {
            get { return m_webserviceAddress; }
            set
            {
                if (m_webserviceAddress != value && (!string.IsNullOrEmpty(m_webserviceAddress) || !string.IsNullOrEmpty(value)))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        m_webserviceAddress = string.Empty;
                    }
                    else
                    {
                        m_webserviceAddress = value;
                    }

                    NotifyPropertyChanged("WebserviceAddress");
                }
            }
        }

        private string m_defaultExperimentsDirectory;
        public string DefaultExperimentsDirectory
        {
            get { return m_defaultExperimentsDirectory; }
            set
            {
                if (m_defaultExperimentsDirectory != value)
                {
                    m_defaultExperimentsDirectory = value;
                    NotifyPropertyChanged("DefaultExperimentsDirectory");
                }
            }
        }


        public ObservableCollection<SettingsPath> TypePaths
        {
            get
            {
                return m_typePaths;
            }
        }

        /// <summary>
        /// The default location for the main window.
        /// </summary>
        public MainWindowData MainWindowRect
        {
            get;
            private set;
        }

        /// <summary>
        /// How long ExperimentSettings will be kept after the last open.  Default ExperimentSettings are always discarded.
        /// </summary>
        [XmlIgnore]
        public TimeSpan ExperimentSettingsTTL
        {
            get { return m_experimentSettingsTTL; }
            set
            {
                m_experimentSettingsTTL = value;
                NotifyPropertyChanged("ExperimentSettingsTTL");
            }
        }

        #region IXmlSerializable Members

        public void ReadXml(XPathNavigator nav)
        {
            if (nav == null)
                throw new ArgumentNullException("reader");

            if (nav.Name != "ApplicationSettings")
                throw new InvalidOperationException();

            XPathNavigator iter = nav.SelectSingleNode("./@Version");
            if (iter == null)
                throw new XmlSchemaException("ApplicationSettings does not have a version element");

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

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("ApplicationSettings");

            writer.WriteAttributeString("Version", CurrentVersion.ToString());

            if (m_componentPaths != null)
            {
                List<SettingsPath> paths = new List<Core.Settings.SettingsPath>(m_componentPaths.Where(x => x.IsTemporary == false && !string.IsNullOrWhiteSpace(x.Path)).Distinct());

                if (paths.Count > 0)
                {
                    // Serialize the data.
                    writer.WriteStartElement("ComponentPaths");

                    var serial = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(paths.GetType(), null);
                    serial.Serialize(writer, paths);

                    writer.WriteEndElement();
                }
            }

            if (m_typePaths != null)
            {
                List<SettingsPath> paths = new List<Core.Settings.SettingsPath>(m_typePaths.Where(x => x.IsTemporary == false));
                if (paths.Count > 0)
                {
                    // Serialize the data.
                    writer.WriteStartElement("TypePaths");

                    var serial = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(paths.GetType(), null);
                    serial.Serialize(writer, paths);

                    writer.WriteEndElement();
                }
            }

            if (!string.IsNullOrWhiteSpace(DefaultExperiment))
            {
                writer.WriteElementString("DefaultExperiment", DefaultExperiment);
            }

            if (!string.IsNullOrWhiteSpace(DefaultExperimentsDirectory))
            {
                writer.WriteElementString("DefaultExperimentsDirectory", DefaultExperimentsDirectory);
            }

            if (!string.IsNullOrWhiteSpace(WebserviceAddress) && TraceLab.Core.Utilities.UrlValidation.IsValidUrl(WebserviceAddress))
            {
                writer.WriteElementString("WebserviceAddress", WebserviceAddress);
            }

            {
                var serial = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(MainWindowData), null);
                serial.Serialize(writer, MainWindowRect);
            }

            writer.WriteEndElement(); // ApplicationSettings
        }

        private void ReadCurrentVersion(XPathNavigator nav)
        {
            ReadVersion1(nav);
        }

        private void ReadVersion1(XPathNavigator nav)
        {
            var iter = nav.SelectSingleNode("./ComponentPaths");
            if (iter != null)
            {
                XmlReader pathReader = iter.ReadSubtree();
                pathReader.MoveToContent();
                if (pathReader != null && !pathReader.IsEmptyElement)
                {
                    pathReader.ReadStartElement("ComponentPaths");
                    var serial = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(List<SettingsPath>), null);
                    var paths = (List<SettingsPath>)serial.Deserialize(pathReader);
                    foreach (SettingsPath path in paths)
                    {
                        if(!m_componentPaths.Contains(path))
                        m_componentPaths.Add(path);

                        path.ResetModifiedFlag();
                    }

                    pathReader.ReadEndElement();
                }
            }

            iter = nav.SelectSingleNode("./TypePaths");
            if (iter != null)
            {
                XmlReader pathReader = iter.ReadSubtree();
                pathReader.MoveToContent();
                if (pathReader != null && !pathReader.IsEmptyElement)
                {
                    pathReader.ReadStartElement("TypePaths");
                    var serial = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(List<SettingsPath>), null);
                    var paths = (List<SettingsPath>)serial.Deserialize(pathReader);
                    foreach (SettingsPath path in paths)
                    {
                        if (Directory.Exists(path.Path))
                        {
                            if(!m_typePaths.Contains(path))
                            {
                                m_typePaths.Add(path);
                            }
                            path.ResetModifiedFlag();
                        }
                        else
                        {
                            NLog.LogManager.GetCurrentClassLogger().Warn(String.Format(Messages.TypeDirectoryDoesNotExists, path.Path));
                        }
                    }
                    pathReader.ReadEndElement();
                }
            }

            iter = nav.SelectSingleNode("./DefaultExperiment");
            if (iter != null)
            {
                DefaultExperiment = iter.Value;
            }

            iter = nav.SelectSingleNode("./DefaultExperimentsDirectory");
            if (iter != null)
            {
                DefaultExperimentsDirectory = iter.Value;
            }

            iter = nav.SelectSingleNode("./WebserviceAddress");
            if (iter != null)
            {
                WebserviceAddress = iter.Value;
            }

            iter = nav.SelectSingleNode("./MainWindowData");
            if (iter != null)
            {
                XmlReader pathReader = iter.ReadSubtree();
                pathReader.MoveToContent();
                //pathReader.Read();
                XmlSerializer serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(MainWindowData), null);
                MainWindowRect = (MainWindowData)serializer.Deserialize(pathReader);
            }

            iter = nav.SelectSingleNode("./ExperimentSettingsTTL");
            if (iter != null)
            {
                try
                {
                    TimeSpan val = XmlConvert.ToTimeSpan(iter.Value);
                    ExperimentSettingsTTL = val;
                }
                catch (FormatException)
                {
                }
            }
        }

        #endregion
    }
}
