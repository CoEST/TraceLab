using System.Linq;
using GraphSharp.Sample.Model;

namespace GraphSharp.Sample.ViewModel
{
	public partial class LayoutAnalyzerViewModel
	{
		partial void CreateSampleGraphs()
        {
            #region SimpleTree
            int i = 0;
            {
                var graph = new PocGraph();

                for (int j = 0; j < 11; i++, j++)
                {
                    var v = new PocVertex(i.ToString());
                    graph.AddVertex(v);
                    v.Desc = "test" + i.ToString();
                }

                graph.AddEdge(new PocEdge("StartToImporter", graph.Vertices.ElementAt(0), graph.Vertices.ElementAt(1)));
                graph.AddEdge(new PocEdge("ImporterToTarget", graph.Vertices.ElementAt(1), graph.Vertices.ElementAt(2)));
                graph.AddEdge(new PocEdge("ImporterToSource", graph.Vertices.ElementAt(1), graph.Vertices.ElementAt(3)));

                // Target Side
                graph.AddEdge(new PocEdge("TargetCleanupToStopword", graph.Vertices.ElementAt(2), graph.Vertices.ElementAt(4)));
                graph.AddEdge(new PocEdge("TargetStopwordToStemmer", graph.Vertices.ElementAt(4), graph.Vertices.ElementAt(6)));
                graph.AddEdge(new PocEdge("TargetStemmerToDictionary", graph.Vertices.ElementAt(6), graph.Vertices.ElementAt(8)));
                graph.AddEdge(new PocEdge("TargetDictionaryToTracer", graph.Vertices.ElementAt(8), graph.Vertices.ElementAt(9)));

                // Source side
                graph.AddEdge(new PocEdge("SourceCleanupToStopword", graph.Vertices.ElementAt(3), graph.Vertices.ElementAt(5)));
                graph.AddEdge(new PocEdge("SourceStopwordToStemmer", graph.Vertices.ElementAt(5), graph.Vertices.ElementAt(7)));
                graph.AddEdge(new PocEdge("SourceStemmerToTracer", graph.Vertices.ElementAt(7), graph.Vertices.ElementAt(9)));


                graph.AddEdge(new PocEdge("DecisionToImporter", graph.Vertices.ElementAt(9), graph.Vertices.ElementAt(1)));
                graph.AddEdge(new PocEdge("DecisionToEnd", graph.Vertices.ElementAt(9), graph.Vertices.ElementAt(10)));

                GraphModels.Add(new GraphModel("Fa", graph));
            }
            {
                var graph = new PocGraph();

                for (int j = 0; j < 2; i++, j++)
                {
                    var v = new PocVertex(i.ToString());
                    graph.AddVertex(v);
                    v.Desc = "test" + i.ToString();
                }
                graph.AddEdge(new PocEdge("StartToImporter", graph.Vertices.ElementAt(0), graph.Vertices.ElementAt(1)));

                GraphModels.Add(new GraphModel("Fb", graph));
            }
            #endregion
        }
	}
}