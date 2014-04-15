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
using System.Windows.Data;

using TraceLab.Core.Components;

namespace TraceLab.UI.WPF.ViewModels
{
    class CLVReferenceNode : CLVBaseNode
    {
        TraceLabSDK.PackageSystem.IPackageReference m_reference;

        public CLVReferenceNode(TraceLabSDK.PackageSystem.IPackageReference reference)
        {
            m_reference = reference;
            Exists = TraceLab.Core.PackageSystem.PackageManager.Instance.Contains(reference);
        }

        public override string Label
        {
            get { return m_reference.Name; }
        }

        public string ID
        {
            get { return m_reference.ID; }
        }

        public string Name
        {
            get { return m_reference.Name; }
        }

        public bool Exists
        {
            get;
            private set;
        }

        public TraceLabSDK.PackageSystem.IPackageReference Reference
        {
            get { return m_reference; }
        }
    }
}
