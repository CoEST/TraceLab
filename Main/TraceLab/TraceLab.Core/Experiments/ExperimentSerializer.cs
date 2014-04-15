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
using System.Xml;
using QuickGraph.Serialization;
using System.Xml.XPath;
using TraceLab.Core.Workspaces;
using TraceLabSDK.PackageSystem;
using System.Collections.ObjectModel;
using TraceLab.Core.Exceptions;

namespace TraceLab.Core.Experiments
{
    public class ExperimentSerializer
    {
        private ExperimentSerializer() { }

        /// <summary>
        /// Serializes the experiment.
        /// </summary>
        /// <param name="experiment">The experiment to be serialized.</param>
        /// <param name="writer">The writer.</param>
        /// <param name="library">The library - needed for post XML processing to determine if components has been located in library, and needed to show their definition and description.</param>
        public static void SerializeExperiment(XmlWriter writer, IExperiment experiment)
        {
            var factory = new ExperimentFactoryWriter();

            experiment.SerializeToXml(writer,
                (QuickGraph.VertexIdentity<ExperimentNode>)(v => v.ID),
                (QuickGraph.EdgeIdentity<ExperimentNode, ExperimentNodeConnection>)(e => e.ID),
                "graph", "node", "edge", "",
                factory.WriteGraphAttributes,
                factory.WriteNodeAttributes,
                factory.WriteEdgeAttributes);
        }

        /// <summary>
        /// Deserializes the experiment. The relative config paths are not set. 
        /// It is valid in case of graphs that belong to composite components metadefinitions
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public static Experiment DeserializeExperiment(XmlReader reader, TraceLab.Core.Components.IComponentsLibrary library)
        {
            //as default the experiment location is empty - it is valid in case of graphs that belong to composite components metadefinitions
            //before they are added to the experiment
            return DeserializeExperiment(reader, library, null);
        }

        /// <summary>
        /// Deserializes the experiment.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="experimentFilename">The experiment filename - needed so that experiment can set all relative config paths in relation to experiment location.</param>
        /// <returns></returns>
        internal static Experiment DeserializeExperiment(XmlReader reader, TraceLab.Core.Components.IComponentsLibrary library, string experimentFilename)
        {
            Experiment loadedFlow = null;

            if (reader != null)
            {
                IXPathNavigable doc = new XPathDocument(reader);

                var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(PackageSystem.PackageReference), Type.EmptyTypes);

                var references = new ObservableCollection<IPackageReference>();
                // Get the references:
                var nav = doc.CreateNavigator();
                var referenceIter = nav.Select("/graph/References/PackageReference");
                if (referenceIter != null)
                {
                    while (referenceIter.MoveNext())
                    {
                        var reference= (IPackageReference)serializer.Deserialize(referenceIter.Current.ReadSubtree());
                        references.Add(reference);
                    }
                }

                var graphFactory = new ExperimentFactoryReader(library, references, System.IO.Path.GetDirectoryName(experimentFilename));

                loadedFlow = QuickGraph.Serialization.SerializationExtensions.DeserializeFromXml<ExperimentNode, ExperimentNodeConnection, Experiment>(doc,
                    "/graph", "/graph/node", "/graph/edge",
                    graphFactory.GraphFactory,
                    graphFactory.NodeFactory,
                    graphFactory.EdgeFactory
                );

                loadedFlow.References = references;

                //Update the loaded graph and sets its start and end node
                loadedFlow.ReloadStartAndEndNode();

                loadedFlow.ExperimentInfo.FilePath = experimentFilename;
            }
            
            return loadedFlow;
        }
    }
}
