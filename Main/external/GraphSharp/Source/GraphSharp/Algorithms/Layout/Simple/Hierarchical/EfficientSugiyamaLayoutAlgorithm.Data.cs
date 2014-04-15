﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using QuickGraph;
using System.Diagnostics;
using System.Windows;

namespace GraphSharp.Algorithms.Layout.Simple.Hierarchical
{
    public partial class EfficientSugiyamaLayoutAlgorithm<TVertex, TEdge, TGraph>
        where TVertex : class
        where TEdge : IEdge<TVertex>
        where TGraph : IVertexAndEdgeListGraph<TVertex, TEdge>
    {
        protected class SugiEdge : TaggedEdge<SugiVertex, TEdge>
        {
            [ContractInvariantMethod]
            protected void ContractInvariants()
            {
                Contract.Invariant(OriginalEdge == null ||
                    (!(Source.Type == VertexTypes.Original && Target.Type == VertexTypes.Original) ||
                    ((OriginalEdge.Source == Source.OriginalVertex && OriginalEdge.Target == Target.OriginalVertex) ||
                    (OriginalEdge.Target == Source.OriginalVertex && OriginalEdge.Source == Target.OriginalVertex))),
                    "The endpoints of the SugiEdge should be the 'same' as the ones of the original edge.");
            }

            public SugiEdge(TEdge originalEdge, SugiVertex source, SugiVertex target)
                : base(source, target, originalEdge) { }

            /// <summary>
            /// Gets the original edge of this SugiEdge.
            /// </summary>
            public TEdge OriginalEdge { get { return this.Tag; } }

            /// <summary>
            /// Gets or sets that the edge is included in a 
            /// type 1 conflict as a non-inner segment (true) or not (false).
            /// </summary>
            public bool Marked = false;

            public bool TempMark = false;

            public void SaveMarkedToTemp()
            {
                TempMark = Marked;
            }

            public void LoadMarkedFromTemp()
            {
                Marked = TempMark;
            }
        }



        protected enum VertexTypes
        {
            Original,
            PVertex,
            QVertex,
            RVertex
        }

        protected enum EdgeTypes
        {
            NonInnerSegment,
            InnerSegment
        }

        protected interface IData
        {
            int Position { get; set; }
        }

        protected abstract class Data : IData
        {
            public int Position { get; set; }

            /* Used by horizontal assignment */
            public readonly Data[] Sinks = new Data[4];
            public readonly double[] Shifts = new double[4] { double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity };
        }

        protected abstract class SugiVertex<TVertex> : Data
        {
            public TVertex OriginalVertex;
            public VertexTypes Type;
            public Segment Segment;
            public int LayerIndex { get; set; }
            public double MeasuredPosition { get; set; }


            [ContractInvariantMethod]
            protected void InvariantContracts()
            {
                Contract.Invariant(!(Type == VertexTypes.Original || Type == VertexTypes.RVertex) || Segment == null,
                    "Implication: if the vertex is Original or R-vertex the Segment must be null");

                Contract.Invariant(!(Type != VertexTypes.Original) || Segment != null,
                    "Implication: if the vertex is a dummy vertex the segment should not be null");

                Contract.Invariant(
                    (OriginalVertex == null && Type != VertexTypes.Original) ||
                    (OriginalVertex != null && Type == VertexTypes.Original));
            }

            public SugiVertex() { }

            public SugiVertex(TVertex originalVertex)
            {
                Contract.Requires(originalVertex != null);

                OriginalVertex = originalVertex;
                Type = VertexTypes.Original;
                Segment = null;
            }
        }

        [DebuggerDisplay("{Type}: {OriginalVertex} - {Position} ; {MeasuredPosition} on layer {LayerIndex}")]
        protected class SugiVertex : SugiVertex<TVertex>
        {
            public readonly double[] HorizontalPositions = new double[4] { double.NaN, double.NaN, double.NaN, double.NaN };
            public double HorizontalPosition = double.NaN;
            public double VerticalPosition = double.NaN;
            public readonly SugiVertex[] Roots = new SugiVertex[4];
            public readonly SugiVertex[] Aligns = new SugiVertex[4];
            public readonly double[] BlockWidths = new double[4] { double.NaN, double.NaN, double.NaN, double.NaN };
            public int IndexInsideLayer;
            public int PermutationIndex;
            public int TempPosition;
            public bool DoNotOpt;
            public readonly Size Size;

            public SugiVertex()
            {
                Size = new Size();
            }

            public SugiVertex(TVertex originalVertex, Size size)
                : base(originalVertex)
            {
                Size = size;
            }

            public void SavePositionToTemp()
            {
                TempPosition = Position;
            }

            public void LoadPositionFromTemp()
            {
                Position = TempPosition;
            }
        }

        protected class Segment : Data
        {
            /// <summary>
            /// Gets or sets the p-vertex of the segment.
            /// </summary>
            public SugiVertex PVertex;

            /// <summary>
            /// Gets or sets the q-vertex of the segment.
            /// </summary>
            public SugiVertex QVertex;

            [ContractInvariantMethod]
            protected void InvariantContracts()
            {
                Contract.Invariant(PVertex != null);
                Contract.Invariant(QVertex != null);
            }
        }
    }
}
