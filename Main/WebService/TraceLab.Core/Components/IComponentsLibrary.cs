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
namespace TraceLab.Core.Components
{
    /// <summary>
    /// Defines interface of component library
    /// </summary>
    public interface IComponentsLibrary : System.ComponentModel.INotifyPropertyChanged
    {
        System.Collections.Generic.IEnumerable<MetadataDefinition> Components { get; }

        /// <summary>
        /// Gets the component definition with a given ID
        /// </summary>
        /// <param name="componentID">The component ID.</param>
        /// <returns>The definition with the given ID.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if componentID is null</exception>
        /// <exception cref="TraceLab.Core.Exceptions.ComponentsLibraryException">Thrown if the library doesn't contain any definitions by the given ID.</exception>
        MetadataDefinition GetComponentDefinition(string componentID);

        /// <summary>
        /// Tries to get the component definition with a given ID.
        /// </summary>
        /// <param name="componentID">The component ID.</param>
        /// <param name="metadataDefinition">The metadata definition.</param>
        /// <returns>True if the definition was found in the library, otherwise false.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if componentID is null</exception>
        bool TryGetComponentDefinition(string componentID, out MetadataDefinition metadataDefinition);

        /// <summary>
        /// Gets a library that is capable of using the referenced packages.
        /// </summary>
        /// <param name="references">The references to use.</param>
        /// <returns>Returns a version of the library that will search through the given references.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="references"/> is null.</exception>
        IComponentsLibrary GetPackageAwareLibrary(IEnumerable<TraceLabSDK.PackageSystem.IPackageReference> references);

        /// <summary>
        /// Gets a value indicating whether the library is beind rescanned.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the library is being rescanned; otherwise, <c>false</c>.
        /// </value>
        bool IsRescanning { get; }

        /// <summary>
        /// Occurs when the library finsished rescanning
        /// </summary>
        event EventHandler Rescanned;
        
        /// <summary>
        /// Occurs when library started rescanning
        /// </summary>
        event EventHandler Rescanning;
    }
}
