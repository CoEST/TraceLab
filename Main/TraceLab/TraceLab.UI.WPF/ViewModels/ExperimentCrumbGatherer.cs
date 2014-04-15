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
using TraceLab.UI.WPF.Controls;

namespace TraceLab.UI.WPF.ViewModels
{
    public class ExperimentCrumbGatherer : CrumbGatherer
    {
        public override Crumb[] GatherCrumbs(object source)
        {
            Crumb[] value = null;

            var experiment = source as BaseLevelExperimentViewModel;
            if (experiment != null)
            {
                List<Crumb> crumbs = new List<Crumb>();

                PushParent(experiment, crumbs);
                crumbs.Add(new ExperimentCrumb(experiment));

                value = crumbs.ToArray();
            }

            return value;
        }

        private void PushParent(BaseLevelExperimentViewModel experiment, List<Crumb> crumbs)
        {
            SubLevelExperimentViewModel subExperimentVM = experiment as SubLevelExperimentViewModel;
            if (subExperimentVM != null)
            {
                BaseLevelExperimentViewModel parent = subExperimentVM.Owner;
                PushParent(parent, crumbs);

                Crumb crumb = new ExperimentCrumb(parent);
                crumbs.Add(crumb);
            }
        }
    }
}
