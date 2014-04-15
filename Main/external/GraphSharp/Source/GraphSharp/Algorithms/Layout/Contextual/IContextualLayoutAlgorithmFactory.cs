using QuickGraph;
using System.Diagnostics.Contracts;
using GraphSharp.Contracts;

namespace GraphSharp.Algorithms.Layout.Contextual
{

    [ContractClass( typeof( IContextualLayoutAlgorithmFactoryContract<,,> ) )]
    public interface IContextualLayoutAlgorithmFactory<TVertex, TEdge, TGraph> : ILayoutAlgorithmFactory<TVertex, TEdge, TGraph>
        where TVertex : class
        where TEdge : IEdge<TVertex>
        where TGraph : class, IBidirectionalGraph<TVertex, TEdge>
    {
    }
}
