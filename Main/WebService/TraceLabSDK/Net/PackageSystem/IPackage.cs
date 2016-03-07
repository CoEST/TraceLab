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
namespace TraceLabSDK.PackageSystem
{
    /// <summary>
    /// An interface used to represent a Package to the PackageSystem.  The primary purpose of this interface is to facilitate testing.
    /// </summary>
    public interface IPackage
    {
        /// <summary>
        /// Where components are located within this Package.
        /// </summary>
        System.Collections.Generic.IEnumerable<string> ComponentLocations { get; }

        /// <summary>
        /// The files contained in this package
        /// </summary>
        System.Collections.Generic.IEnumerable<IPackageFile> Files { get; }

        /// <summary>
        /// The ID of this package.
        /// </summary>
        string ID { get; }

        /// <summary>
        /// Where this package is located on disk.
        /// </summary>
        string Location { get; }

        /// <summary>
        /// The name of this package
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the IDs of other packaged that are used by this package.
        /// </summary>
        System.Collections.Generic.IEnumerable<IPackageReference> References { get; }

        /// <summary>
        /// Gets the <see cref="TraceLabSDK.PackageSystem.IPackageFile"/> with the specified id.
        /// </summary>
        IPackageFile this[string id] { get; }

        /// <summary>
        /// Where workspace types are located within this Package.
        /// </summary>
        System.Collections.Generic.IEnumerable<string> TypeLocations { get; }

        /// <summary>
        /// Gets the absolute path of a file, given the file's ID within the package.
        /// </summary>
        /// <param name="fileID">The file ID.</param>
        /// <returns></returns>
        string GetAbsolutePath(string fileID);
    }
}
