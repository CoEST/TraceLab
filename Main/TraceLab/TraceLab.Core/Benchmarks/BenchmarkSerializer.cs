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
using TraceLab.Core.Experiments;
using System.Xml;
using QuickGraph.Serialization;
using TraceLabSDK.Types.Contests;
using System.Xml.XPath;

namespace TraceLab.Core.Benchmarks
{
    class BenchmarkSerializer
    {
                /// <summary>
        /// Initializes a new instance of the <see cref="ExperimentSerializer"/> class.
        /// </summary>
        /// <param name="library">The library.</param>
        /// <param name="workspaceTypeDirectories">The workspace type directories.</param>
        /// <param name="dataRoot">The data root.</param>
        /// <param name="settings">The settings.</param>
        public BenchmarkSerializer(TraceLab.Core.Components.IComponentsLibrary library, string dataRoot)
        {
            m_library = library;
            m_dataRoot = dataRoot;
        }

        private TraceLab.Core.Components.IComponentsLibrary m_library;
        private string m_dataRoot;

        /// <summary>
        /// Serializes the experiment.
        /// </summary>
        /// <param name="experiment">The experiment to be serialized.</param>
        /// <param name="writer">The writer.</param>
        /// <param name="library">The library.</param>
        /// <param name="workspaceTypeDirectories">The workspace type directories.</param>
        /// <param name="dataRoot">The data root.</param>
        public void SerializeExperiment(IExperiment experiment, TLExperimentResults baseline, XmlWriter writer)
        {
            //use standard factory to serialize the experiment
            var factory = new BenchmarkExperimentFactory(baseline);

            experiment.SerializeToXml(writer,
                (QuickGraph.VertexIdentity<ExperimentNode>)(v => v.ID),
                (QuickGraph.EdgeIdentity<ExperimentNode, ExperimentNodeConnection>)(e => e.ID),
                "graph", "node", "edge", "",
                factory.WriteGraphAttributes,
                factory.WriteNodeAttributes,
                factory.WriteEdgeAttributes);
        }
    }
}
