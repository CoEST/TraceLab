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
using TraceLabSDK.PackageSystem;
using TraceLab.Core.Experiments;
using TraceLab.Core.PackageSystem;

namespace TraceLab.UI.WPF.ViewModels
{
    class PackageManagerViewModel
    {
        private IExperiment m_experiment;

        private ObservableCollection<PackageViewModel> m_packages = new ObservableCollection<PackageViewModel>();
        public ObservableCollection<PackageViewModel> Packages
        {
            get { return m_packages; }
        }

        public PackageManagerViewModel(IExperiment experiment)
        {
            m_experiment = experiment;
            var references = new HashSet<string>(experiment.References.Select((a) => { return a.ID; }));

            var manager = TraceLab.Core.PackageSystem.PackageManager.Instance;

            foreach (TraceLabSDK.PackageSystem.IPackage package in manager)
            {
                var packageVM = new PackageViewModel(package);
                packageVM.IsCheckedChanged += packageVM_IsCheckedChanged;
                if (references.Contains(package.ID))
                {
                    packageVM.IsChecked = true;
                }

                m_packages.Add(packageVM);
            }
        }

        void packageVM_IsCheckedChanged(object sender, System.EventArgs e)
        {
            var packageVM = (PackageViewModel)sender;
            var packageReference = new TraceLab.Core.PackageSystem.PackageReference(packageVM.Package);

            if (!packageVM.IsChecked) 
            {
                PackagesViewModelHelper.RemoveReference(m_experiment, packageReference);
            }
            else if(packageVM.IsChecked && !m_experiment.References.Contains(packageReference))
            {
                PackagesViewModelHelper.AddReference(m_experiment, packageReference);
            }
        }
    }
}
