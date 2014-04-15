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
using System.Collections.ObjectModel;
using System.Collections.Generic;
namespace TraceLabSDK.PackageSystem
{
    /// <summary>
    /// The top-level interface for the Package management system
    /// </summary>
    public interface IPackageManager
    {
        /// <summary>
        /// Gets all of the type locations that this package might try to load types from.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <returns>A list of all the type locations that this package might load types from.</returns>
        System.Collections.Generic.IEnumerable<string> GetDependantTypeLocations(IPackage package);

        /// <summary>
        /// Gets the packages referenced by the given package.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <param name="missingReferences">OUT paramaeter - The references that do not exist in the PackageManager.</param>
        /// <returns>A list of the references that <paramref name="package"/> refers to.</returns>
        System.Collections.Generic.IEnumerable<IPackage> GetReferencedPackages(IPackage package, out System.Collections.Generic.IEnumerable<IPackageReference> missingReferences);

        /// <summary>
        /// Determines whether <paramref name="package"/>'s references are loaded.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <returns>
        ///   <c>true</c> if the manager has loaded all of <paramref name="package"/>'s references; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>This is equivilent to calling GetReferencedPackages and checking if the missingReferences contains anything.</remarks>
        bool HasAllReferences(IPackage package);

        /// <summary>
        /// Gets the locations that packages can be loaded from.
        /// </summary>
        ReadOnlyCollection<string> PackageLocations { get; }

        /// <summary>
        /// Gets the absolute path for the given package file
        /// </summary>
        /// <param name="packageID">The ID of the package.</param>
        /// <param name="packageFileID">The ID of the package file.</param>
        /// <returns>The absolute path of the package file, or null if either the package or the package file doesn't exist.</returns>
        string GetAbsolutePath(string packageID, string packageFileID);
    }
}
