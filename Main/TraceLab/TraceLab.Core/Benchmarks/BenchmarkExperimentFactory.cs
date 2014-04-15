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
using TraceLabSDK.Types.Contests;
using System.Xml.XPath;
using TraceLab.Core.Workspaces;

namespace TraceLab.Core.Benchmarks
{
    class BenchmarkExperimentFactory : ExperimentFactoryWriter
    {
        public BenchmarkExperimentFactory(TLExperimentResults baselineResults) : base()
        {
            m_baseline = baselineResults;
        }

        private TLExperimentResults m_baseline;

        public override void WriteGraphAttributes(System.Xml.XmlWriter writer, IExperiment flow)
        {
            base.WriteGraphAttributes(writer, flow);

            //and also serialize the baseline results
            if (m_baseline != null)
            {
                var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(TLExperimentResults), null);
                serializer.Serialize(writer, m_baseline);
            }
        }
    }
}
