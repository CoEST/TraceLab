using System;
using System.Collections.Generic;
using System.Text;
using QuickGraph;
using System.IO;
using System.Xml;
using QuickGraph.Algorithms.Search;
using QuickGraph.Glee;
using Microsoft.Glee.GraphViewerGdi;
using QuickGraph.Algorithms;
using QuickGraph.Algorithms.Condensation;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace QuickGraph.Heap
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// http://www.cs.utexas.edu/users/speedway/DaCapo/papers/cork-popl-2007.pdf
    /// </remarks>
    public sealed class GcTypeHeap
    {
        private readonly BidirectionalGraph<GcType, GcTypeEdge> graph;
        private int size = -1;
        private Form viewerForm;
        private GViewer viewer;

        internal GcTypeHeap(BidirectionalGraph<GcType, GcTypeEdge> graph)
        {
            if (graph == null)
                throw new ArgumentNullException("graph");

            this.graph = graph;
        }

        public int Size
        {
            get 
            {
                if (this.size < 0)
                {
                    int s = 0;
                    foreach (GcType type in this.graph.Vertices)
                        s += type.Size;
                    this.size = s;
                }
                return this.size; 
            }
        }

        /// <summary>
        /// Loads the specified heap XML file name.
        /// </summary>
        /// <param name="heapXmlFileName">Name of the heap XML file.</param>
        /// <returns></returns>
        public static GcTypeHeap Load(string heapXmlFileName)
        {
            return Load(heapXmlFileName, null);
        }

        private readonly static Regex regex = new Regex(
              "name=\"(?<Type>[^\"]*)\"",
            RegexOptions.IgnoreCase
            | RegexOptions.CultureInvariant
            | RegexOptions.IgnorePatternWhitespace
            | RegexOptions.Compiled
            );

        private static string EscapeXmlAttribute(Match m)
        {
            string value = m.Groups["Type"].Value;
            return String.Format("name=\"{0}\"",
                        value.Replace("<", "&lt;")
                            .Replace(">", "&gt;")
                        );
        }

        /// <summary>
        /// Loads the specified heap XML file name.
        /// </summary>
        /// <param name="heapXmlFileName">Name of the heap XML file.</param>
        /// <param name="dumpFileName">Name of the dump file.</param>
        /// <returns></returns>
        public static GcTypeHeap Load(string heapXmlFileName, string dumpFileName)
        {
            if (String.IsNullOrEmpty(heapXmlFileName))
                throw new ArgumentNullException("heapXmlFileName");
            if (!File.Exists(heapXmlFileName))
                throw new FileNotFoundException("could not find heap file", heapXmlFileName);

            Console.WriteLine("loading {0}", heapXmlFileName);
            GcTypeGraphReader greader = new GcTypeGraphReader();

            try
            {
                using (XmlReader reader = XmlReader.Create(heapXmlFileName))
                    greader.Read(reader);
            }
            catch (XmlException)
            {
                Console.WriteLine("fixing up xml file (incorrect xml with generics)");
                // fix the xml and save back to disk.
                File.WriteAllText(
                    heapXmlFileName,
                    regex.Replace(
                        File.ReadAllText(heapXmlFileName),
                        EscapeXmlAttribute)
                    );
                // try again
                greader = new GcTypeGraphReader();
                using (XmlReader reader = XmlReader.Create(heapXmlFileName))
                    greader.Read(reader);
            }

            if (!String.IsNullOrEmpty(dumpFileName))
            {
                if (!File.Exists(dumpFileName))
                    throw new FileNotFoundException("could not find dump file", dumpFileName);
                using (StreamReader reader = new StreamReader(File.OpenRead(dumpFileName)))
                    greader.ParseDump(reader);
            }

            GcTypeHeap heap = new GcTypeHeap(greader.Graph).RemoveDefault(); ;
            Console.WriteLine("heap: {0}", heap);
            return heap;
        }

        public override string ToString()
        {
            return String.Format("{0} types, {1} edges, {2}", 
                this.graph.VertexCount,
                this.graph.EdgeCount,
                FormatHelper.ToSize(this.Size));
        }

        /// <summary>
        /// Gets the list of types in the graph
        /// </summary>
        public GcTypeCollection Types
        {
            get
            {
                return new GcTypeCollection(this.graph.Vertices).SortSize();
            }
        }

        /// <summary>
        /// Gets the list of root types
        /// </summary>
        public GcTypeCollection Roots
        {
            get
            {
                GcTypeCollection roots = new GcTypeCollection(this.graph.Vertices);
                foreach (GcType type in this.graph.Vertices)
                    if (type.Root)
                        roots.Add(type);
                return roots;
            }
        }

        public GcTypeHeap Clone()
        {
            return new GcTypeHeap(this.graph.Clone());
        }

        #region Rendering
        public GcTypeHeap Render()
        {
            Console.WriteLine("rendering...");
            if (this.graph.VertexCount > 500)
            {
                Console.WriteLine("too many vertices ");
                return this;
            }

            GleeGraphPopulator<GcType, GcTypeEdge> populator = GleeGraphUtility.Create(this.graph);
            try
            {
                populator.NodeAdded += new GleeVertexNodeEventHandler<GcType>(populator_NodeAdded);
                populator.EdgeAdded += new GleeEdgeEventHandler<GcType, GcTypeEdge>(populator_EdgeAdded);
                populator.Compute();

                if (viewer == null)
                {
                    viewerForm = new Form();
                    viewer = new GViewer();
                    viewer.Dock = DockStyle.Fill;
                    viewerForm.Controls.Add(viewer);
                }
                viewer.Graph = populator.GleeGraph;
                viewerForm.ShowDialog();
            }
            finally
            {
                populator.NodeAdded -= new GleeVertexNodeEventHandler<GcType>(populator_NodeAdded);
                populator.EdgeAdded -= new GleeEdgeEventHandler<GcType, GcTypeEdge>(populator_EdgeAdded);
            }

            return this;
        }

        void populator_EdgeAdded(object sender, GleeEdgeEventArgs<GcType, GcTypeEdge> e)
        {
            e.GEdge.Attr.Label = e.Edge.Count.ToString();
        }

        void populator_NodeAdded(object sender, GleeVertexEventArgs<GcType> args)
        {
            args.Node.Attr.Shape = Microsoft.Glee.Drawing.Shape.Box;
            double gen = args.Vertex.Gen;
            if (gen > 2)
                args.Node.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.LightBlue;
            else if (gen > 1)
                args.Node.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.LightGoldenrodYellow;
            else if (gen >= 0)
                args.Node.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.LightPink;
            else
                args.Node.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.LightSalmon;

            // root -> other color
            if (args.Vertex.Root)
                args.Node.Attr.AddStyle(Microsoft.Glee.Drawing.Style.Dashed);

            // label
            args.Node.Attr.Label =
                String.Format("{0}\nc:{1} g:{2:0.0} s:{3}",
                    args.Vertex.Name,
                    args.Vertex.Count,
                    args.Vertex.Gen,
                    FormatHelper.ToSize(args.Vertex.Size)
                    );
        }
        #endregion

        #region Filtering
        private GcTypeHeap RemoveDefault()
        {
            // remove default types
            this.graph.RemoveVertexIf(
                delegate(GcType type)
                {
                    switch (type.Name)
                    {
                        case "System.OutOfMemoryException":
                        case "System.StackOverflowException":
                        case "System.ExecutionEngineException":
                        case "System.RuntimeType":
                        case "System.Reflection.Missing":
                        case "System.IO.TextReader/SyncTextReader":
                        case "System.AppDomain":
                        case "System.AppDomainSetup":
                        case "System.IO.Stream/NullStream":
                        case "System.Text.UTF8Encoding":
                        case "System.__Filters":
                        case "System.Reflection.MemberFilter":
                        case "System.Text.CodePageEncoding":
                        case "System.IO.TextWriter/NullTextWriter":
                        case "System.Text.UTF8Encoding/UTF8Encoder":
                        case "System.IO.TextReader/NullTextReader":
                        case "System.IO.StreamReader/NullStreamReader":
                        case "System.Text.CodePageEncoding/Decoder":
                        case "System.IO.__ConsoleStream":
                        case "System.Text.Encoding/DefaultEncoder":
                        case "System.IO.TextWriter/SyncTextWriter":
                            return true;
                        default:
                            return false;
                    }
                });
            return this;
        }

        public GcTypeHeap Touching(string typeNames)
        {
            if (String.IsNullOrEmpty(typeNames))
                throw new ArgumentNullException("typeNames");

            return this.Clone().TouchingInPlace(typeNames);
        }

        private GcTypeHeap TouchingInPlace(string typeNames)
        {
            if (String.IsNullOrEmpty(typeNames))
                throw new ArgumentNullException("typeNames");

            var filter = FilterHelper.ToFilter(typeNames);
            Console.WriteLine("filtering nodes not connected to type matching '{0}'", filter);
            var colors = new Dictionary<GcType, GraphColor>(this.graph.VertexCount);
            foreach (var type in this.graph.Vertices)
                colors.Add(type, GraphColor.White);

            var rgraph = new ReversedBidirectionalGraph<GcType, GcTypeEdge>(graph);
            foreach (var type in this.graph.Vertices)
            {
                if (filter.Match(type.Name))
                {
                    { // parents
                        var dfs =
                            new DepthFirstSearchAlgorithm<GcType, ReversedEdge<GcType, GcTypeEdge>>(rgraph, colors);
                        dfs.Visit(type, -1);
                    }
                    { // children
                        var dfs = new DepthFirstSearchAlgorithm<GcType, GcTypeEdge>(graph, colors);
                        dfs.Visit(type, -1);
                    }
                }
            }
            // remove all white vertices
            this.graph.RemoveVertexIf(t => colors[t] == GraphColor.White);
            Console.WriteLine("resulting {0} types, {1} edges", graph.VertexCount, graph.EdgeCount);
            return this;
        }

        public GcTypeHeap Merge(int minimumSize)
        {
            var merged = new BidirectionalGraph<GcType,MergedEdge<GcType,GcTypeEdge>>(false, this.graph.VertexCount);
            var merger = new EdgeMergeCondensationGraphAlgorithm<GcType, GcTypeEdge>(
                this.graph,
                merged,
                delegate(GcType type)
                {
                    return type.Size >= minimumSize;
                });
            merger.Compute();
            var clone = new BidirectionalGraph<GcType, GcTypeEdge>(
                false,
                merged.VertexCount);
            foreach (var type in merged.Vertices)
                clone.AddVertex(type);
            foreach (var medge in merged.Edges)
            {
                GcTypeEdge edge = new GcTypeEdge(medge.Source, medge.Target);
                foreach (GcTypeEdge e in medge.Edges)
                    edge.Count += e.Count;
                clone.AddEdge(edge);
            }

            Console.WriteLine("resulting {0} types, {1} edges", clone.VertexCount, clone.EdgeCount);
            return new GcTypeHeap(clone);
        }
        #endregion
    }
}
