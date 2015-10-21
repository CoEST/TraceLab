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

// HERZUM SPRINT 2.4 TLAB-56 TLAB-57 TLAB-58 TLAB-59 CLASS

using System;
using TraceLab.Core.Experiments;

namespace TraceLab.UI.GTK
{
    public class DragClipboard
    {

        private static CompositeComponentGraph clipboardComponentGraph = null;

        public DragClipboard ()
        {
        }

        public static void Copy(ApplicationContext applicationContext, BaseExperiment originalExperiment)
        {
            clipboardComponentGraph = CompositeComponentGraph.ConstructGraphFromSelectedNodes (originalExperiment);

            foreach (ExperimentNode node in clipboardComponentGraph.Vertices)
                node.IsSelected = false;

            BasicNodeControl componentControl;
            foreach (ExperimentNode originalNode in originalExperiment.Vertices)
                foreach (ExperimentNode node in clipboardComponentGraph.Vertices)
                    if (originalNode.ID.Equals(node.ID))
                        if(applicationContext.NodeControlFactory.TryGetNodeControl(originalNode, out componentControl)) {
                        node.Data.X = componentControl.DisplayBox.X;
                        node.Data.Y = componentControl.DisplayBox.Y;
                    }
        }

        public static void CutSelectedNodes(BaseExperiment originalExperiment)
        {
            originalExperiment.RemoveSelectedVertices ();
        }

        public static void Cut(ApplicationContext applicationContext, BaseExperiment originalExperiment)
        {
            Copy (applicationContext, originalExperiment);
            CutSelectedNodes (originalExperiment);
        }

        public static void Paste(ApplicationContext applicationContext, BaseExperiment targetExperiment, double x, double y)
        {

            if (clipboardComponentGraph == null)
                return;

            targetExperiment.CopyAndAdd (clipboardComponentGraph, x, y);
        }
    }
}