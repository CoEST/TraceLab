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
using TraceLabSDK;
using TraceLabSDK.Component.Config;
using System.ComponentModel;
using System.Collections.Generic;
using TraceLabSDK.Types;
using System.Xml.XPath;
using TraceLabSDK.Types.Contests;

namespace CoestDatasetsLoader
{
    [Component(GuidIDString = "55ae54a0-6e3a-468d-97b1-2d2172daf117",
                Name = "MultipleCoestDatasetsImporter",
                DefaultLabel = "Coest Multiple Datasets Importer",
                Description = "The importer allows to import multiple datasets in coest format at once from the configuration file.",
                Author = "Re Lab",
                Version = "1.0",
                ConfigurationType = typeof(MultipleCoestDatasetsImporterConfig))]
    [IOSpec(IOSpecType.Output, "listOfDatasets", typeof(TLDatasetsList))]
    [IOSpec(IOSpecType.Output, "numberOfDatasets", typeof(int))]
    [Tag("Importers.TLDatasetsList.From XML")]
    [Tag("Contest utilities")]
    public class MultipleCoestDatasetsImporter : BaseComponent
    {
        public MultipleCoestDatasetsImporter(ComponentLogger log) : base(log) 
        {
            m_config = new MultipleCoestDatasetsImporterConfig();
            Configuration = m_config;
        }

        private MultipleCoestDatasetsImporterConfig m_config;

        public override void Compute()
        {
            var datasetsLocations = ReadDatasetsLocations();

            TLDatasetsList listOfDatasets = new TLDatasetsList();

            foreach (DatasetLocation locations in datasetsLocations)
            {
                TLDataset dataset = new TLDataset(locations.DatasetName);

                string error;

                //do validation
                if (CoestDatasetImporterHelper.ValidatePath(locations.SourceArtifactsLocation, "Source Artifacts File", out error))
                {
                    dataset.SourceArtifacts = CoestDatasetImporterHelper.ImportArtifacts(locations.SourceArtifactsLocation, m_config.TrimElementValues);
                    Logger.Info(String.Format("Source artifacts imported from {0}.", locations.SourceArtifactsLocation));
                }
                else
                {
                    throw new ComponentException(error);
                }

                //do validation
                if (CoestDatasetImporterHelper.ValidatePath(locations.SourceArtifactsLocation, "Target Artifacts File", out error))
                {
                    dataset.TargetArtifacts = CoestDatasetImporterHelper.ImportArtifacts(locations.TargetArtifactsLocation, m_config.TrimElementValues);
                    Logger.Info(String.Format("Target artifacts imported from {0}.", locations.TargetArtifactsLocation));
                }
                else
                {
                    throw new ComponentException(error);
                }

                //do validation
                if (CoestDatasetImporterHelper.ValidatePath(locations.SourceArtifactsLocation, "Target Artifacts File", out error))
                {
                    dataset.AnswerSet = CoestDatasetImporterHelper.ImportAnswerSet(locations.AnswerSetLocation, dataset.SourceArtifacts, locations.SourceArtifactsLocation, dataset.TargetArtifacts, locations.TargetArtifactsLocation, Logger, m_config.TrimElementValues);
                    Logger.Info(String.Format("Answer set imported from {0}.", locations.AnswerSetLocation));
                }
                else
                {
                    throw new ComponentException(error);
                }

                listOfDatasets.Add(dataset);
            }

            Workspace.Store("listOfDatasets", listOfDatasets);
            Workspace.Store("numberOfDatasets", listOfDatasets.Count);
        }

        private List<DatasetLocation> ReadDatasetsLocations()
        {
            XPathDocument doc = new XPathDocument(m_config.ConfigurationFile);

            string rootDir = System.IO.Path.GetDirectoryName(m_config.ConfigurationFile.Absolute);

            XPathNavigator nav = doc.CreateNavigator();

            XPathNodeIterator datasetsIterator = nav.Select("/load_datasets_config/datasets/dataset");

            var datasetsLocations = new List<DatasetLocation>();
            XPathNavigator iter;
            while (datasetsIterator.MoveNext())
            {
                var datasetLocation = new DatasetLocation();
                iter = datasetsIterator.Current.SelectSingleNode("dataset_name");
                datasetLocation.DatasetName = iter.Value;

                iter = datasetsIterator.Current.SelectSingleNode("source_artifacts_location");
                datasetLocation.SourceArtifactsLocation = System.IO.Path.Combine(rootDir, iter.Value);

                iter = datasetsIterator.Current.SelectSingleNode("target_artifacts_location");
                datasetLocation.TargetArtifactsLocation = System.IO.Path.Combine(rootDir, iter.Value);

                iter = datasetsIterator.Current.SelectSingleNode("answer_set_location");
                datasetLocation.AnswerSetLocation = System.IO.Path.Combine(rootDir, iter.Value);
                datasetsLocations.Add(datasetLocation);
            }

            return datasetsLocations;
        }

        class DatasetLocation
        {
            public string DatasetName
            {
                get;
                set;
            }

            public string SourceArtifactsLocation
            {
                get;
                set;
            }
            public string TargetArtifactsLocation
            {
                get;
                set;
            }
            public string AnswerSetLocation
            {
                get;
                set;
            }
        }
    }

    

    public class MultipleCoestDatasetsImporterConfig
    {
        [DisplayName("Configuration file")]
        public FilePath ConfigurationFile
        {
            get;
            set;
        }

        [DisplayName("Trim elements values")]
        public bool TrimElementValues
        {
            get;
            set;
        }
    }
}