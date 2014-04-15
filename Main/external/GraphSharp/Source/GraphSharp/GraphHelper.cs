using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using QuickGraph;
using QuickGraph.Algorithms.ShortestPath;
using System.Diagnostics;

namespace GraphSharp
{
    public static class GraphHelper
    {
        /// <summary>
        /// Returns with the adjacent vertices of the <code>vertex</code>.
        /// </summary>
        /// <param name="g">The graph.</param>
        /// <param name="vertex">The vertex which neighbours' we want to get.</param>
        /// <returns>List of the adjacent vertices of the <code>vertex</code>.</returns>
        public static IEnumerable<TVertex> GetNeighbours<TVertex, TEdge>( this IBidirectionalGraph<TVertex, TEdge> g, TVertex vertex )
            where TEdge : IEdge<TVertex>
        {
            return ( ( from e in g.InEdges( vertex ) select e.Source )
                .Concat(
                ( from e in g.OutEdges( vertex ) select e.Target ) ) ).Distinct();
        }


        public static IEnumerable<TVertex> GetOutNeighbours<TVertex, TEdge>( this IVertexAndEdgeListGraph<TVertex, TEdge> g, TVertex vertex )
            where TEdge : IEdge<TVertex>
        {
            return ( from e in g.OutEdges( vertex )
                     select e.Target ).Distinct();
        }


        /// <summary>
        /// If the graph g is directed, then returns every edges which source is one of the vertices in the <code>set1</code>
        /// and the target is one of the vertices in <code>set2</code>.
        /// </summary>
        /// <typeparam name="TVertex">Type of the vertex.</typeparam>
        /// <typeparam name="TEdge">Type of the edge.</typeparam>
        /// <param name="g">The graph.</param>
        /// <param name="set1"></param>
        /// <param name="set2"></param>
        /// <returns>Return the list of the selected edges.</returns>
        public static IEnumerable<TEdge> GetEdgesBetween<TVertex, TEdge>( this IVertexAndEdgeListGraph<TVertex, TEdge> g, List<TVertex> set1, List<TVertex> set2 )
            where TEdge : IEdge<TVertex>
        {
            var edgesBetween = new List<TEdge>();

            //vegig kell menni az osszes vertex minden elen, es megnezni, hogy a target hol van
            foreach ( TVertex v in set1 )
            {
                foreach ( TEdge edge in g.OutEdges( v ) )
                {
                    if ( set2.Contains( edge.Target ) )
                        edgesBetween.Add( edge );
                }
            }

            return edgesBetween;
        }


        /// <summary>
        /// Returns with the sources in the graph.
        /// </summary>
        /// <typeparam name="TVertex"></typeparam>
        /// <typeparam name="TEdge"></typeparam>
        /// <param name="g">The graph.</param>
        /// <returns>Returns with the sources in the graph.</returns>
        public static IEnumerable<TVertex> GetSources<TVertex, TEdge>( this IBidirectionalGraph<TVertex, TEdge> g )
            where TEdge : IEdge<TVertex>
        {
            return from v in g.Vertices
                   where g.InDegree( v ) == 0
                   select v;
        }

        ///// <summary>
        ///// Gets the diameter of a graph.
        ///// The diameter is the greatest distance between two vertices.
        ///// </summary>
        ///// <param name="g">The graph.</param>
        ///// <returns>The diameter of the Graph <code>g</code>.</returns>
        //public static double GetDiameter<Vertex, Edge, Graph>( this Graph g )
        //    where Edge : IEdge<Vertex>
        //    where Graph : IBidirectionalGraph<Vertex, Edge>
        //{
        //    double[,] distances;
        //    return g.GetDiameter<Vertex, Edge, Graph>( out distances );
        //}

        ///// <summary>
        ///// Gets the diameter of a graph.
        ///// The diameter is the greatest distance between two vertices.
        ///// </summary>
        ///// <param name="g">The graph.</param>
        ///// <param name="distances">This is an out parameter. It gives the distances between every vertex-pair.</param>
        ///// <returns>The diameter of the Graph <code>g</code>.</returns>
        //public static double GetDiameter<Vertex, Edge, Graph>( this Graph g, out double[,] distances )
        //    where Edge : IEdge<Vertex>
        //    where Graph : IBidirectionalGraph<Vertex, Edge>
        //{
        //    distances = GetDistances<Vertex, Edge, Graph>( g );

        //    int n = g.VertexCount;
        //    double distance = double.NegativeInfinity;
        //    for ( int i = 0; i < n - 1; i++ )
        //    {
        //        for ( int j = i + 1; j < n; j++ )
        //        {
        //            if ( double.MaxValue == distances[i, j] )
        //                continue;

        //            distance = Math.Max( distance, distances[i, j] );
        //        }
        //    }
        //    return distance;
        //}

        ///// <param name="g">The graph.</param>
        ///// <returns>Returns with the distance between the vertices (distance: number of the edges).</returns>
        //public static double[,] GetDistances<Vertex, Edge, Graph>( Graph g )
        //    where Edge : IEdge<Vertex>
        //    where Graph : IBidirectionalGraph<Vertex, Edge>
        //{
        //    var distances = new double[g.VertexCount, g.VertexCount];
        //    for ( int k = 0; k < g.VertexCount; k++ )
        //    {
        //        for ( int j = 0; j < g.VertexCount; j++ )
        //        {
        //            distances[k, j] = double.PositiveInfinity;
        //        }
        //    }

        //    var undirected = new UndirectedBidirectionalGraph<Vertex, Edge>( g );
        //    //minden �let egy hosszal vesz�nk figyelembe - unweighted
        //    var weights = new Dictionary<Edge, double>();
        //    foreach ( Edge edge in undirected.Edges )
        //    {
        //        weights[edge] = 1;
        //    }

        //    //compute the distances from every vertex: O(n(n^2 + e)) complexity
        //    int i = 0;
        //    foreach ( Vertex source in g.Vertices )
        //    {
        //        //compute the distances from the 'source'
        //        var spaDijkstra =
        //            new UndirectedDijkstraShortestPathAlgorithm<Vertex, Edge>( undirected, ( edge ) => weights[edge], ShortestDistanceRelaxer.Instance );
        //        spaDijkstra.Compute( source );

        //        int j = 0;
        //        foreach ( Vertex v in undirected.Vertices )
        //        {
        //            double d = spaDijkstra.Distances[v];
        //            distances[i, j] = Math.Min( distances[i, j], d );
        //            distances[i, j] = Math.Min( distances[i, j], distances[j, i] );
        //            distances[j, i] = Math.Min( distances[i, j], distances[j, i] );
        //            j++;
        //        }
        //        i++;
        //    }

        //    return distances;
        //}

        public static BidirectionalGraph<TVertex, Edge<TVertex>> CreateGraph<TVertex, TOtherEdge>(
            IEnumerable<TVertex> vertices,
            IEnumerable<TOtherEdge> edges,
            string sourcePropertyName,
            string targetPropertyName )
            where TVertex : class
        {
            var graph = new BidirectionalGraph<TVertex, Edge<TVertex>>();

            graph.AddVertexRange( vertices );

            //get the property infos
            System.Reflection.PropertyInfo spi = typeof( TOtherEdge ).GetProperty( sourcePropertyName );
            System.Reflection.PropertyInfo tpi = typeof( TOtherEdge ).GetProperty( targetPropertyName );

            //creating the new edges
            foreach ( TOtherEdge oe in edges )
            {
                var edge = new Edge<TVertex>(
                    spi.GetValue( oe, null ) as TVertex,
                    tpi.GetValue( oe, null ) as TVertex );
                graph.AddEdge( edge );
            }

            return graph;
        }



        /// <summary>
        /// Creates a new BidirectionalGraph with the given types from the 
        /// list of vertices, and the list of edges.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices.</typeparam>
        /// <typeparam name="TEdge">The type of the edges in the new graph.</typeparam>
        /// <typeparam name="TOtherEdge">The type of the items in the list of the edges.</typeparam>
        /// <param name="vertices">The list of the vertices.</param>
        /// <param name="edges">The list of the edges.</param>
        /// <param name="factoryMethod">Delegate which converts an edge from the type <code>TOtherEdge</code>
        /// to the type <code>TEdge</code>.</param>
        /// <param name="allowParallelEdges">Parallel edges are allowed or not?.</param>
        /// <returns>The new BidirectionalGraph</returns>
        public static BidirectionalGraph<TVertex, TEdge> CreateGraph<TVertex, TEdge, TOtherEdge>(
            IEnumerable<TVertex> vertices,
            IEnumerable<TOtherEdge> edges,
            Func<TOtherEdge, TEdge> factoryMethod,
            bool allowParallelEdges )
            where TEdge : IEdge<TVertex>
        {
            var graph = new BidirectionalGraph<TVertex, TEdge>( allowParallelEdges );

            //add the vertices to the graph
            graph.AddVertexRange( vertices );

            //create the edges
            foreach ( TOtherEdge oe in edges )
            {
                TEdge e = factoryMethod( oe );
                graph.AddEdge( e );
            }

            return graph;
        }



        public static BidirectionalGraph<TVertex, TEdge> CreateGraph<TVertex, TEdge, TOtherEdge>(
            IEnumerable<TVertex> vertices,
            IEnumerable<TOtherEdge> edges,
            Func<TOtherEdge, TEdge> factoryMethod )
            where TEdge : IEdge<TVertex>
        {
            return CreateGraph( vertices, edges, factoryMethod, true );
        }



        public static TVertex OtherVertex<TVertex>( this IEdge<TVertex> edge, TVertex thisVertex )
        {
            return edge.Source.Equals( thisVertex ) ? edge.Target : edge.Source;
        }



        public static void AddEdgeRange<TVertex, TEdge>( this IMutableEdgeListGraph<TVertex, TEdge> graph, IEnumerable<TEdge> edges )
            where TEdge : IEdge<TVertex>
        {
            foreach ( var edge in edges )
                graph.AddEdge( edge );
        }



        public static BidirectionalGraph<TNewVertex, TNewEdge> Convert<TOldVertex, TOldEdge, TNewVertex, TNewEdge>(
            this IVertexAndEdgeListGraph<TOldVertex, TOldEdge> oldGraph,
            Func<TOldVertex, TNewVertex> vertexMapperFunc,
            Func<TOldEdge, TNewEdge> edgeMapperFunc )
            where TOldEdge : IEdge<TOldVertex>
            where TNewEdge : IEdge<TNewVertex>
        {
            Contract.Requires( oldGraph != null );

            return oldGraph.Convert(
                new BidirectionalGraph<TNewVertex, TNewEdge>( oldGraph.AllowParallelEdges, oldGraph.VertexCount ),
                vertexMapperFunc,
                edgeMapperFunc );
        }



        public static BidirectionalGraph<TOldVertex, TNewEdge> Convert<TOldVertex, TOldEdge, TNewEdge>(
            this IVertexAndEdgeListGraph<TOldVertex, TOldEdge> oldGraph,
            Func<TOldEdge, TNewEdge> edgeMapperFunc )
            where TOldEdge : IEdge<TOldVertex>
            where TNewEdge : IEdge<TOldVertex>
        {
            Contract.Requires( oldGraph != null );

            return oldGraph.Convert<TOldVertex, TOldEdge, TOldVertex, TNewEdge>( null, edgeMapperFunc );
        }



        public static TNewGraph Convert<TOldVertex, TOldEdge, TNewVertex, TNewEdge, TNewGraph>(
            this IVertexAndEdgeListGraph<TOldVertex, TOldEdge> oldGraph,
            TNewGraph newGraph,
            Func<TOldVertex, TNewVertex> vertexMapperFunc,
            Func<TOldEdge, TNewEdge> edgeMapperFunc )
            where TOldEdge : IEdge<TOldVertex>
            where TNewEdge : IEdge<TNewVertex>
            where TNewGraph : IMutableVertexAndEdgeListGraph<TNewVertex, TNewEdge>
        {
            Contract.Requires( oldGraph != null );

            //VERTICES
            if ( vertexMapperFunc != null )
                newGraph.AddVertexRange( oldGraph.Vertices.Select( vertexMapperFunc ) );
            else
                newGraph.AddVertexRange( oldGraph.Vertices.Cast<TNewVertex>() );

            //EDGES
            if ( edgeMapperFunc != null )
                newGraph.AddEdgeRange( oldGraph.Edges.Select( edgeMapperFunc ) );
            else
                newGraph.AddEdgeRange( oldGraph.Edges.Cast<TNewEdge>() );

            return newGraph;
        }



        public static TNewGraph Convert<TOldVertex, TOldEdge, TNewEdge, TNewGraph>(
            this IVertexAndEdgeListGraph<TOldVertex, TOldEdge> oldGraph,
            TNewGraph newGraph,
            Func<TOldEdge, TNewEdge> edgeMapperFunc )
            where TOldEdge : IEdge<TOldVertex>
            where TNewEdge : IEdge<TOldVertex>
            where TNewGraph : IMutableVertexAndEdgeListGraph<TOldVertex, TNewEdge>
        {
            Contract.Requires( oldGraph != null );

            return oldGraph.Convert<TOldVertex, TOldEdge, TOldVertex, TNewEdge, TNewGraph>( newGraph, null, edgeMapperFunc );
        }



        public static TNewGraph Convert<TOldVertex, TOldEdge, TNewGraph>(
            this IVertexAndEdgeListGraph<TOldVertex, TOldEdge> oldGraph,
            TNewGraph newGraph )
            where TOldEdge : IEdge<TOldVertex>
            where TNewGraph : IMutableVertexAndEdgeListGraph<TOldVertex, TOldEdge>
        {
            Contract.Requires( oldGraph != null );

            return oldGraph.Convert<TOldVertex, TOldEdge, TOldVertex, TOldEdge, TNewGraph>( newGraph, null, null );
        }


        public static BidirectionalGraph<TVertex, TEdge> CopyToBidirectionalGraph<TVertex, TEdge>(
            this IVertexAndEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            Contract.Requires(graph != null);
            Contract.Ensures(Contract.Result<BidirectionalGraph<TVertex,TEdge>>() != null);

            var newGraph = new BidirectionalGraph<TVertex, TEdge>();

            //copy the vertices
            newGraph.AddVerticesAndEdgeRange(graph.Edges);

            return newGraph;
        }
    }
}