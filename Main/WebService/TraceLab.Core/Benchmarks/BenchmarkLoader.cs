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
using System.IO;
using System.Xml;
using TraceLab.Core.Experiments;
using System.Xml.XPath;
using System.Xml.Serialization;
using TraceLab.Core.Components;
using TraceLabSDK.Types.Contests;

namespace TraceLab.Core.Benchmarks
{
    class BenchmarkLoader
    {
        public static List<Benchmark> LoadBenchmarksInfo(string benchmarkDirectory)
        {
            if (benchmarkDirectory == null)
                throw new ArgumentNullException("benchmarkDirectory", "Benchmark directory path cannot be null");
            if (!System.IO.Path.IsPathRooted(benchmarkDirectory))
                throw new ArgumentException("Benchmark directory must be fully qualified.", benchmarkDirectory);
            if (Directory.Exists(benchmarkDirectory) == false)
                throw new ArgumentException("Benchmark directory does not exist: ", benchmarkDirectory);

            var benchmarkFiles = System.IO.Directory.GetFiles(benchmarkDirectory, "*.tbml");

            List<Benchmark> benchmarkCollection = new List<Benchmark>();

            foreach (var benchmarkFile in benchmarkFiles)
            {
                Benchmark benchmark = ReadBenchmark(benchmarkFile);
                if (benchmark != null)
                {
                    benchmarkCollection.Add(benchmark);
                }
            }

            return benchmarkCollection;
        }

        /// <summary>
        /// Searches the provided file and finds the template component metadata.
        /// </summary>
        /// <param name="benchmarkFile">The file path to the benchmark file.</param>
        /// <exception cref="System.Xml.XmlException">Throws if file could not have properly read by xml reader</exception>
        /// <returns>Component template metadata</returns>
        public static ComponentTemplateMetadata FindTemplateComponentMetadata(string benchmarkFile)
        {
            XmlReader reader = XmlReader.Create(benchmarkFile);
            XPathDocument doc = new XPathDocument(reader);
            XPathNavigator nav = doc.CreateNavigator();
            return FindTemplateComponentMetadata(nav);
        }

        private static Benchmark ReadBenchmark(string benchmarkFile)
        {
            XmlReader reader = XmlReader.Create(benchmarkFile);
            XPathDocument doc = new XPathDocument(reader);
            XPathNavigator nav = doc.CreateNavigator();

            //new version
            XPathNavigator benchmarkInfoXmlNode = nav.SelectSingleNode("/graph/BenchmarkInfo");

            BenchmarkInfo benchmarkInfo = null;
            ComponentTemplateMetadata template = null;
            
            //read the benchmark info 
            if (benchmarkInfoXmlNode != null)
            {
                XmlSerializer m_experimentInfoSerializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(BenchmarkInfo), null);
                benchmarkInfo = (BenchmarkInfo)m_experimentInfoSerializer.Deserialize(benchmarkInfoXmlNode.ReadSubtree());
            }

            //find the template
            if (benchmarkInfo != null)
            {
                template = FindTemplateComponentMetadata(nav);
            }

            Benchmark benchmark = null;
            if (benchmarkInfo != null && template != null)
            {
                //update benchmark filepath
                benchmarkInfo.FilePath = benchmarkFile;
                benchmark = new Benchmark(benchmarkInfo, template);
            }

            return benchmark;
        }

        public static string ReadExperimentResultsUnitname(string benchmarkFile) 
        {
            XmlReader reader = XmlReader.Create(benchmarkFile);
            XPathDocument doc = new XPathDocument(reader);
            XPathNavigator nav = doc.CreateNavigator();

            //new version
            XPathNavigator unitnameXmlNode = nav.SelectSingleNode("/graph/BenchmarkInfo/ExperimentResultsUnitname");

            if (unitnameXmlNode != null)
            {
                return unitnameXmlNode.Value;
            }
            else
            {
                return null;
            }
        }

        private static ComponentTemplateMetadata FindTemplateComponentMetadata(XPathNavigator nav)
        {
            ComponentTemplateMetadata template = null;

            //search for all references for template components metadata
            string templateType = typeof(ComponentTemplateMetadata).ToString();
            XPathExpression expression = nav.Compile("//Metadata[starts-with(@type, '" + templateType + "')]");
            XPathNavigator templateXmlNode = nav.SelectSingleNode(expression);

            if (templateXmlNode != null)
            {
                string label = templateXmlNode.GetAttribute("Label", String.Empty);
                IOSpec ioSpec = new IOSpec();
                XPathNavigator ioSpecNav = templateXmlNode.SelectSingleNode("IOSpec");
                ioSpec.ReadXml(ioSpecNav.ReadSubtree());

                template = new ComponentTemplateMetadata(ioSpec, label);
            }
            return template;
        }

        public static TraceLabSDK.Types.Contests.TLExperimentResults ReadBaseline(string benchmarkFile)
        {
            TLExperimentResults baseline = null;
             
            using (XmlReader reader = XmlReader.Create(benchmarkFile))
            {
                XPathDocument doc = new XPathDocument(reader);
                XPathNavigator nav = doc.CreateNavigator();
                XPathNavigator baselineXmlNode = nav.SelectSingleNode("/graph/TLExperimentResults");

                if (baselineXmlNode != null)
                {
                    var experimentInfoSerializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(typeof(TLExperimentResults), null);
                    baseline = (TLExperimentResults)experimentInfoSerializer.Deserialize(baselineXmlNode.ReadSubtree());
                }
            }

            return baseline;
        }
    }
}
