﻿using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Windows;
using QuickGraph;

namespace GraphSharp.Algorithms.Layout.Compound
{
    public class CompoundLayoutContext<TVertex, TEdge, TGraph> 
        : LayoutContext<TVertex, TEdge, TGraph>, ICompoundLayoutContext<TVertex, TEdge, TGraph>
        where TEdge : IEdge<TVertex>
        where TGraph : class, IBidirectionalGraph<TVertex, TEdge>
    {
        public CompoundLayoutContext(
            TGraph graph,
            IDictionary<TVertex, Point> positions,
            IDictionary<TVertex, Size> sizes,
            LayoutMode mode,
            IDictionary<TVertex, Thickness> vertexBorders,
            IDictionary<TVertex, CompoundVertexInnerLayoutType> layoutTypes)
            : base( graph, positions, sizes, mode )
        {
            Contract.Requires( sizes != null );
            Contract.Requires( vertexBorders != null );
            Contract.Requires( layoutTypes != null );

            VertexBorders = vertexBorders;
            LayoutTypes = layoutTypes;
        }

        public IDictionary<TVertex, Thickness> VertexBorders { get; private set; }
        public IDictionary<TVertex, CompoundVertexInnerLayoutType> LayoutTypes { get; private set; }
    }
}
