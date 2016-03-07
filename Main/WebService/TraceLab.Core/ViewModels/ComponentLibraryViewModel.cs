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
using TraceLab.Core.Components;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Reflection;
using TraceLab.Core.Experiments;
using System.Collections.Specialized;

namespace TraceLab.Core.ViewModels
{
    public class ComponentLibraryViewModel : INotifyPropertyChanged
    {
        private TraceLab.Core.Components.ComponentsLibrary m_componentsLibraryInstance;
        private IEnumerable<string> m_workspaceTypeDirectories;
        private HashSet<string> m_validProperties = new HashSet<string>();
        private IExperiment m_experiment;
        private IComponentsLibrary m_packageAwareComponentLibrary;

        public ComponentLibraryViewModel(TraceLab.Core.Components.ComponentsLibrary componentsLibraryInstance, IEnumerable<string> workspaceTypeDirectories)
        {
            if (componentsLibraryInstance == null)
                throw new ArgumentNullException("componentsLibraryInstance");

            // We'll use this collection to determine which PropertyChanged notifications to pass on.
            foreach (PropertyInfo prop in typeof(ComponentLibraryViewModel).GetProperties())
            {
                m_validProperties.Add(prop.Name);
            }

            m_componentsLibraryInstance = componentsLibraryInstance;
            m_componentsLibraryInstance.PropertyChanged += m_componentsLibraryInstance_PropertyChanged;
            m_componentsLibraryInstance.Rescanning += m_componentsLibraryInstance_Rescanning;
            m_componentsLibraryInstance.Rescanned += m_componentsLibraryInstance_Rescanned;
            m_workspaceTypeDirectories = workspaceTypeDirectories;
        }

        void References_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            m_packageAwareComponentLibrary = m_componentsLibraryInstance.GetPackageAwareLibrary(m_experiment.References);
            foreach (ExperimentNode node in m_experiment.Vertices)
            {
                if (node.Data != null && node.Data.Metadata != null)
                {
                    node.Data.Metadata.UpdateFromDefinition(m_packageAwareComponentLibrary);
                }
            }
        }

        void m_componentsLibraryInstance_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (m_validProperties.Contains(e.PropertyName))
            {
                NotifyPropertyChanged(e.PropertyName);
            }
#if DEBUG
            else
            {
                System.Diagnostics.Debug.WriteLine("ComponentsLibrary has unwrapped property: " + e.PropertyName);
            }
#endif
        }

        public IExperiment Experiment
        {
            get { return m_experiment; }
            set
            {
                if (m_experiment != null)
                    throw new InvalidOperationException("Experiment can be set only once on ComponentLibraryViewModel");

                m_experiment = value;
                if (m_experiment != null && m_experiment.References != null)
                {
                    m_experiment.References.CollectionChanged += References_CollectionChanged;
                    m_packageAwareComponentLibrary = m_componentsLibraryInstance.GetPackageAwareLibrary(m_experiment.References);
                } 
            }
        }

        public IEnumerable<MetadataDefinition> ComponentsCollection
        {
            get 
            {
                IEnumerable<MetadataDefinition> collection;
                if (m_packageAwareComponentLibrary != null)
                {
                    collection = m_packageAwareComponentLibrary.Components;
                }
                else
                {
                    collection = m_componentsLibraryInstance.Components;
                }

                return collection; 
            }
        }

        public IEnumerable<string> PackageTypeDirectories
        {
            get { return this.m_componentsLibraryInstance.PackageTypeDirectories; }
        }

        public void ClearLoadErrors()
        {
            m_componentsLibraryInstance.ClearLoadErrors();
        }

        /// <summary>
        /// Gets the available types which components library can be filter by.
        /// The keys represent user friendly type names, and their values are a full type names.
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> AvailableFilterTypes
        {
            get { return m_componentsLibraryInstance.AvailableFilterTypes; }
        }

        /// <summary>
        /// Rescans the library.
        /// </summary>
        public void Rescan()
        {
            m_componentsLibraryInstance.Rescan(PackageSystem.PackageManager.Instance, m_workspaceTypeDirectories, true);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this component library can be rescanned.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can rescan; otherwise, <c>false</c>.
        /// </value>
        public bool CanRescan
        {
            get { return m_componentsLibraryInstance.CanRescan; }
        }

        public event EventHandler Rescanned;

        /// <summary>
        /// Invokes the Rescanned event; called whenever the library has been rescanned
        /// </summary>
        /// <param name="e"></param>
        private void OnRescanned()
        {
            if (Rescanned != null)
                Rescanned(this, new EventArgs());
        }

        public event EventHandler Rescanning;

        /// <summary>
        /// Invokes the Rescanning event; called whenever the library starts rescanning
        /// </summary>
        /// <param name="e"></param>
        private void OnRescanning()
        {
            if (Rescanning != null)
                Rescanning(this, new EventArgs());
        }

        public bool IsRescanning
        {
            get { return m_componentsLibraryInstance.IsRescanning; }
        }

        void m_componentsLibraryInstance_Rescanned(object sender, EventArgs e)
        {
            OnRescanned();
        }

        void m_componentsLibraryInstance_Rescanning(object sender, EventArgs e)
        {
            OnRescanning();
        }

        public void SaveUserTags(string path)
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                using (XmlWriter writer = XmlWriter.Create(path, settings))
                {
                    ComponentTagCollection allTags = new ComponentTagCollection();
                    foreach (MetadataDefinition metadata in m_componentsLibraryInstance.Components)
                    {
                        if (metadata.Tags != null)
                        {
                            allTags.Add(metadata.Tags.GetUserTags());
                        }
                    }

                    XmlSerializer serial = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(ComponentTagCollection), null);
                    serial.Serialize(writer, allTags);
                }
            }
            catch (Exception)
            {
            }
        }

        public void LoadUserTags(string path)
        {
            ComponentsLibrary.ReadTags(path, (a) => { return m_componentsLibraryInstance.GetComponentDefinition(a); });
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion

        public static explicit operator ComponentsLibrary(ComponentLibraryViewModel wrapper)
        {
            if (wrapper == null)
                throw new ArgumentNullException("wrapper");

            return wrapper.m_componentsLibraryInstance;
        }

        /// <summary>
        /// Clones the instance of view model. It has the components library instance, as this object, and same workspace type directories.
        /// </summary>
        /// <returns></returns>
        public ComponentLibraryViewModel Clone()
        {
            return new ComponentLibraryViewModel(m_componentsLibraryInstance, m_workspaceTypeDirectories);
        }
    }
}
