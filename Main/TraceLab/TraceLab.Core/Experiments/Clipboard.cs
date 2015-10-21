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
//

// HERZUM SPRINT 2.3 TLAB-56 TLAB-57 TLAB-58 TLAB-59 CLASS

using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using TraceLab.Core.Components;

namespace TraceLab.Core.Experiments
{
    public class Clipboard
    {
        private static CompositeComponentGraph clipboardComponentGraph = null;

        public Clipboard ()
        {
        }

        public static void Copy(BaseExperiment originalExperiment)
        {
            // HERZUM SPRINT 4: TLAB-215
            if (clipboardComponentGraph != null)
                clipboardComponentGraph.Clear ();
            // END HERZUM SPRINT 4: TLAB-215

            clipboardComponentGraph = CompositeComponentGraph.ConstructGraphFromSelectedNodes (originalExperiment);

            foreach (ExperimentNode node in clipboardComponentGraph.Vertices)
                node.IsSelected = false;

        }

        private static void CutSelectedNodes(BaseExperiment originalExperiment)
        {
            originalExperiment.RemoveSelectedVertices ();
        }

        public static void Cut(BaseExperiment originalExperiment)
        {
            Copy (originalExperiment);
            CutSelectedNodes (originalExperiment);
        }

        public static void Paste(BaseExperiment targetExperiment, double x, double y)
        {

            if (clipboardComponentGraph == null)
                return;

            targetExperiment.CopyAndAdd (clipboardComponentGraph, x, y);
        }

        // HERZUM SPRINT 4.0: TLAB-204
        public static bool IncludeChallenge()
        {

            if (clipboardComponentGraph == null)
                return false;

            return clipboardComponentGraph.IncludeChallenge();
        }
        // END HERZUM SPRINT 4.0: TLAB-204

    }
}