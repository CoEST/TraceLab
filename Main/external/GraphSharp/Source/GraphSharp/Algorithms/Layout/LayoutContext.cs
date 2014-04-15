﻿using System.Collections.Generic;
using System.Diagnostics.Contracts;
using QuickGraph;
using System.Windows;

namespace GraphSharp.Algorithms.Layout
{
    public class LayoutContext<TVertex, TEdge, TGraph> : ILayoutContext<TVertex, TEdge, TGraph>
        where TEdge : IEdge<TVertex>
        where TGraph : IVertexAndEdgeListGraph<TVertex, TEdge>
    {
        public IDictionary<TVertex, Point> Positions { get; private set; }

        public IDictionary<TVertex, Size> Sizes { get; private set; }

        public TGraph Graph { get; private set; }

        public LayoutMode Mode { get; private set; }

        public LayoutContext( TGraph graph, IDictionary<TVertex, Point> positions, IDictionary<TVertex, Size> sizes, LayoutMode mode )
        {
            Contract.Requires( graph != null );
            Contract.Requires( sizes != null );

            Graph = graph;
            Positions = positions;
            Sizes = sizes;
            Mode = mode;
        }
    }
}