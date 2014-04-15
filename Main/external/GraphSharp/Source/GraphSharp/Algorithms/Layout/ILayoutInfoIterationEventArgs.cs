﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickGraph;

namespace GraphSharp.Algorithms.Layout
{
    public interface ILayoutInfoIterationEventArgs<TVertex, TEdge>
        : ILayoutIterationEventArgs<TVertex>
        where TVertex : class
        where TEdge : IEdge<TVertex>
    {
        object GetVertexInfo(TVertex vertex);

        object GetEdgeInfo(TEdge edge);
    }

    public interface ILayoutInfoIterationEventArgs<TVertex, TEdge, TVertexInfo, TEdgeInfo>
        : ILayoutInfoIterationEventArgs<TVertex, TEdge>
        where TVertex : class
        where TEdge : IEdge<TVertex>
    {
        IDictionary<TVertex, TVertexInfo> VertexInfos { get; }
        IDictionary<TEdge, TEdgeInfo> EdgeInfos { get; }
    }
}
