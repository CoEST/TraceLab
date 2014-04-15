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
using TraceLab.Core.Experiments;
using TraceLabSDK;
using System.Collections;
using System.Collections.ObjectModel;
using TraceLab.Core.Components;

namespace TraceLab.Core.Utilities
{
    public class ExperimentHelper
    {
        #region BFS Traverse Experiment

        internal static void BFSTraverseExperiment(IExperiment experiment, ExperimentNode startFromNode, TraverseTask Task)
        {
            //traverse graph down from the node
            Queue<ExperimentNode> traversingQueue = new Queue<ExperimentNode>();
            HashSet<ExperimentNode> foundVertices = new HashSet<ExperimentNode>();
            traversingQueue.Enqueue(startFromNode);

            while (traversingQueue.Count > 0)
            {
                ExperimentNode currentNode = traversingQueue.Dequeue();

                //do some stuff
                Task(currentNode);

                foreach (ExperimentNodeConnection edge in experiment.OutEdges(currentNode))
                {
                    if (foundVertices.Contains(edge.Target) == false)
                    {
                        traversingQueue.Enqueue(edge.Target);
                        foundVertices.Add(edge.Target);
                    }
                }
            }
        }

        #endregion

        public static void DetermineGraphBoundaries(IExperiment compositeComponentGraph, out double leftX, out double rightX, out double topY, out double bottomY)
        {
            leftX = 0;
            rightX = 0;
            topY = 0;
            bottomY = 0;

            //determine boundaries of new composite graph, so that we know where to position start and end node
            var enumerator = compositeComponentGraph.Vertices.GetEnumerator();
            enumerator.MoveNext();
            var node = enumerator.Current;

            if (node != null)
            {
                leftX = node.Data.X;
                rightX = node.Data.X;
                topY = node.Data.Y;
                bottomY = node.Data.Y;

                while (enumerator.MoveNext())
                {
                    node = enumerator.Current;

                    if (node.Data.X < leftX)
                        leftX = node.Data.X;

                    if (node.Data.Y < topY)
                        topY = node.Data.Y;

                    if (node.Data.X > rightX)
                        rightX = node.Data.X;

                    if (node.Data.Y > bottomY)
                        bottomY = node.Data.Y;
                }
            }
        }

        /// <summary>
        /// Moves the graph closer to origin point.
        /// </summary>
        /// <param name="compositeComponentGraph">The composite component graph.</param>
        /// <param name="xDistanceFromOrigin">The x distance from origin.</param>
        /// <param name="yDistanceFromOrigin">The y distance from origin.</param>
        public static void MoveGraphCloserToOriginPoint(IExperiment compositeComponentGraph, double xDistanceFromOrigin, double yDistanceFromOrigin)
        {
            double startX = compositeComponentGraph.StartNode.Data.X;
            double startY = compositeComponentGraph.StartNode.Data.Y;

            double xTransition = (startX > 0) ? startX - xDistanceFromOrigin : startX + xDistanceFromOrigin;
            double yTranstion = (startY > 0) ? startY - yDistanceFromOrigin : startY + yDistanceFromOrigin;

            foreach (ExperimentNode node in compositeComponentGraph.Vertices)
            {
                node.Data.X -= xTransition;
                node.Data.Y -= yTranstion;
            }

            //move also all route points of all connections
            foreach (ExperimentNodeConnection connection in compositeComponentGraph.Edges)
            {
                foreach (RoutePoint point in connection.RoutePoints)
                {
                    point.X -= xTransition;
                    point.Y -= yTranstion;
                }
            }
        }

        #region IO Highlighting in experiment

        public static void HighlightIOInExperiment(IExperiment experiment, string mapping)
        {
            UpdateHighlight(experiment,
                (metadata) => { metadata.IOSpec.HighlightIO(mapping); }
            );
        }

        public static void ClearHighlightIOInExperiment(IExperiment experiment)
        {
            UpdateHighlight(experiment,
                (metadata) => { metadata.IOSpec.ClearHightlightIO(); }
            );
        }

        private static void UpdateHighlight(IExperiment experiment, Action<IConfigurableAndIOSpecifiable> highlightAction)
        {
            foreach (ExperimentNode node in experiment.Vertices)
            {
                var metadata = node.Data.Metadata as IConfigurableAndIOSpecifiable;
                if (metadata != null)
                {
                    highlightAction(metadata);
                }
            }
        }

        #endregion
    }

    internal delegate void TraverseTask(ExperimentNode currentNode);
}
