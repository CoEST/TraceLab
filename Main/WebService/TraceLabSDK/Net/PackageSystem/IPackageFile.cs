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
    /// IPackageFile represents file in the package
    /// </summary>
    public interface IPackageFile
    {
        /// <summary>
        /// Gets the SHA256 of the DataFile, when compressed
        /// </summary>
        string CompressedHash { get; }

        /// <summary>
        /// Gets the size of the PackageFile - only valid while IsCompressed is true
        /// </summary>
        /// <value>
        /// The size of the compressed.
        /// </value>
        long CompressedSize { get; }

        /// <summary>
        /// Gets the data of the package file
        /// </summary>
        string Data { get; }

        /// <summary>
        /// Gets the absolute path.
        /// </summary>
        /// <returns></returns>
        string GetAbsolutePath();

        /// <summary>
        /// Gets the ID of the PackageFile
        /// </summary>
        string ID { get; }

        /// <summary>
        /// Gets a value indicating whether the data is compressed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the data is compressed; otherwise, <c>false</c>.
        /// </value>
        bool IsCompressed { get; }

        /// <summary>
        /// Gets the owner package
        /// </summary>
        IPackage Owner { get; }

        /// <summary>
        /// Gets or sets the Path of this file (filename/directories), relative to the PackageRoot
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        string Path { get; set; }

        /// <summary>
        /// Gets the SHA256 of the DataFile, when uncompressed
        /// </summary>
        string UncompressedHash { get; }

        /// <summary>
        /// Gets the size of the PackageFile - only valid while IsCompressed is false
        /// </summary>
        /// <value>
        /// The size of the uncompressed.
        /// </value>
        long UncompressedSize { get; }
    }
}
