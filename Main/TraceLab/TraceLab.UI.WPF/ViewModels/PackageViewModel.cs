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
using System.Linq;
using System.Text;
using TraceLabSDK.PackageSystem;

namespace TraceLab.UI.WPF.ViewModels
{
    class PackageContentCollection : KeyedCollection<string,PackageContentItem>
    {
        protected override string GetKeyForItem(PackageContentItem item)
        {
            return item.Label;
        }
    }

    class PackageReferenceCollection : KeyedCollection<string,IPackageReference>
    {
        protected override string GetKeyForItem(IPackageReference item)
        {
            return item.ID;
        }
    }
    
    class PackageViewModel : PackageFolderContentItem
    {
        private IPackage m_package;

        public PackageViewModel(IPackage package) : base(string.Empty, null)
        {
            m_package = package;
            PackageReferenceCollection references = new PackageReferenceCollection();
            foreach(IPackageReference reference in m_package.References)
            {
                references.Add(reference);
            }
            Imports = references;
            Label = m_package.Name;
            Icon = null; // TODO: Initialize to correct icon.

            // Building package contents hierarchy
            char[] separators = new char[] { '/', '\\' };
            foreach (TraceLabSDK.PackageSystem.IPackageFile file in m_package.Files)
            {
                PackageFolderContentItem pkgFolder = this;
                string[] pathLevels = file.Path.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 1; i < pathLevels.Length; ++i )
                {
                    string levelName = pathLevels[i];
                    if (i < pathLevels.Length - 1)
                    {
                        PackageFolderContentItem folder;
                        if (pkgFolder.Contents.Contains(levelName))
                        {
                            folder = (PackageFolderContentItem)pkgFolder.Contents[levelName];
                        }
                        else
                        {
                            folder = new PackageFolderContentItem(levelName, pkgFolder);
                            pkgFolder.Contents.Add(folder);
                        }
                        pkgFolder = folder;
                    }
                    else
                    {
                        if (pkgFolder.Contents.Contains(levelName) == false)
                        {
                            PackageFileContentItem pkgFile = new PackageFileContentItem(levelName, pkgFolder, file.ID);
                            pkgFolder.Contents.Add(pkgFile);
                        }
                    }
                }
            }

            var visibleList = new List<object>();
            visibleList.Add(Imports);
            visibleList.Add(Contents);
            Visible = visibleList;
        }

        /// <summary>
        /// Gets the packages that this Package requires.
        /// </summary>
        public PackageReferenceCollection Imports
        {
            get;
            private set;
        }

        public IEnumerable<object> Visible
        {
            get;
            private set;
        }

        private bool m_isChecked;
        public event EventHandler<System.EventArgs> IsCheckedChanged;
        public bool IsChecked
        {
            get { return m_isChecked; }
            set
            {
                if (m_isChecked != value)
                {
                    m_isChecked = value;
                    if (IsCheckedChanged != null)
                    {
                        IsCheckedChanged(this, new System.EventArgs());
                    }
                }
            }
        }


        public IPackage Package
        {
            get { return m_package; }
        }
    }
}
