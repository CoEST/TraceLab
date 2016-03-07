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
using TraceLabSDK.PackageSystem;

namespace TraceLab.Core.Components
{
    public class PackageAwareComponentLibrary : IComponentsLibrary
    {
        private ComponentsLibrary m_wrappedLibrary;
        private IEnumerable<IPackageReference> m_references;

        public PackageAwareComponentLibrary(ComponentsLibrary wrappedLibrary, IEnumerable<IPackageReference> references)
        {
            m_wrappedLibrary = wrappedLibrary;
            m_references = references;
        }

        #region IComponentsLibrary Members

        public IEnumerable<MetadataDefinition> Components
        {
            get
            {
                var collection = m_wrappedLibrary.GetComponentsDefinitionFromReferences(m_references);
                collection.UnionWith(m_wrappedLibrary.Components);

                return collection;
            }
        }

        /// <summary>
        /// Gets a library that is capable of using the referenced packages.
        /// </summary>
        /// <param name="references">The references to use.</param>
        /// <returns>
        /// Returns a version of the library that will search through the given references.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="references"/> is null.</exception>
        public IComponentsLibrary GetPackageAwareLibrary(IEnumerable<IPackageReference> references)
        {
            if (references == null)
                throw new ArgumentNullException();

            return new PackageAwareComponentLibrary(m_wrappedLibrary, references);
        }

        /// <summary>
        /// Gets the component definition with a given ID
        /// </summary>
        /// <param name="componentID">The component ID.</param>
        /// <returns>
        /// The definition with the given ID.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown if componentID is null</exception>
        ///   
        /// <exception cref="TraceLab.Core.Exceptions.ComponentsLibraryException">Thrown if the library doesn't contain any definitions by the given ID.</exception>
        public MetadataDefinition GetComponentDefinition(string componentID)
        {
            return m_wrappedLibrary.GetComponentDefinition(componentID, m_references);
        }

        /// <summary>
        /// Tries to get the component definition with a given ID.
        /// </summary>
        /// <param name="componentID">The component ID.</param>
        /// <param name="metadataDefinition">The metadata definition.</param>
        /// <returns>
        /// True if the definition was found in the library, otherwise false.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown if componentID is null</exception>
        public bool TryGetComponentDefinition(string componentID, out MetadataDefinition metadataDefinition)
        {
            return m_wrappedLibrary.TryGetComponentDefinition(componentID, m_references, out metadataDefinition);
        }

        public bool IsRescanning
        {
            get { return m_wrappedLibrary.IsRescanning; }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged
        {
            add { m_wrappedLibrary.PropertyChanged += value; }
            remove { m_wrappedLibrary.PropertyChanged -= value; }
        }

        public event EventHandler Rescanned
        {
            add { m_wrappedLibrary.Rescanned += value; }
            remove { m_wrappedLibrary.Rescanned -= value; }
        }

        public event EventHandler Rescanning
        {
            add { m_wrappedLibrary.Rescanning += value; }
            remove { m_wrappedLibrary.Rescanning -= value; }
        }

        #endregion

        #region IComponentsLibrary Members


        public string DecisionsDirectoryPath
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
