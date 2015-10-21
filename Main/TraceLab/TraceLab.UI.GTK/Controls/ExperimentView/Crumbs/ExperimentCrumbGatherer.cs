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
using System;
using TraceLab.Core.Experiments;
using System.Collections.Generic;
using TraceLab.Core.Components;

namespace TraceLab.UI.GTK
{
    public static class ExperimentCrumbGatherer
    {
        public static List<Crumb> GatherCrumbs(CompositeComponentMetadata metadata) 
        {
            List<Crumb> crumbs = new List<Crumb>();

            PushParents(metadata.ComponentGraph, crumbs);
            crumbs.Add(new ExperimentCrumb(metadata.Label, metadata.ComponentGraph));

            return crumbs;
        }

        private static void PushParents(CompositeComponentGraph graph, List<Crumb> crumbs) 
        {

            IExperiment parent = graph.OwnerNode.Owner;

            CompositeComponentGraph graphAbove = parent as CompositeComponentGraph;
           
            if(graphAbove != null) 
            {
                PushParents(graphAbove, crumbs);
                ExperimentNode ownerNode = graphAbove.OwnerNode;
                // HERZUM SPRINT 2.5: TLAB-173
                if (graphAbove.OwnerNode is ScopeNode || graphAbove.OwnerNode is LoopScopeNode || graphAbove.OwnerNode is ChallengeNode)
                    return;
                // END HERZUM SPRINT 2.5: TLAB-173
                crumbs.Add(new ExperimentCrumb(ownerNode.Data.Metadata.Label, graphAbove));
            }
            else if(parent is Experiment)
            {
                crumbs.Add(new ExperimentCrumb(parent.Title, parent));
            }
        }
    }
}

