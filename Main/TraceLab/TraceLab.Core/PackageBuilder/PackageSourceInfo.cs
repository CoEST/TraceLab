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
using System.Collections.ObjectModel;

namespace TraceLab.Core.PackageBuilder
{
    /// <summary>
    /// View Model for the Package Builder.
    /// </summary>
    public class PackageSourceInfo
    {
        public static string DefaultName = "Default Package Name";

        public PackageSourceInfo()
        {
            Root = new PackageHeirarchyItem("");
            Name = DefaultName;
        }

        /// <summary>
        /// Gets or sets the name of the package.
        /// </summary>
        /// <value>
        /// The name of the package.
        /// </value>
        public string Name
        {
            get { return Root.Name; }
            set { Root.Name = value; }
        }

        /// <summary>
        /// Root node in the TreeView.
        /// </summary>
        /// <value>
        /// The root node.
        /// </value>
        public PackageHeirarchyItem Root
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the files.
        /// </summary>
        public IEnumerable<PackageFileSourceInfo> Files
        {
            get { return new PackageFileSourceInfo[] { Root }; }
        }
    }
}
