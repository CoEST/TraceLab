﻿using System.Collections.Generic;
using QuickGraph;

namespace GraphSharp.Algorithms.Layout.Simple.Hierarchical
{
	public partial class SugiyamaLayoutAlgorithm<TVertex, TEdge, TGraph> 
        where TVertex : class 
        where TEdge : IEdge<TVertex> 
        where TGraph : IVertexAndEdgeListGraph<TVertex, TEdge>
	{
		/// <summary>
		/// Collection of the layers of the vertices.
		/// </summary>
		private class VertexLayerCollection : List<VertexLayer>
		{
		}
	}
}