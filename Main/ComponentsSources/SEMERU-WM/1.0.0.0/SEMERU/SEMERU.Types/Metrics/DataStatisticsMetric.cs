using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SEMERU.Types.Metrics
{
    [Serializable]
    public class DataStatisticsMetric : Metric
    {
        protected DataStatisticsMetric() { }

        public DataStatisticsMetric(List<DataSetStatistics> statisticsPerDataset)
        {
            m_statisticsPerDataset = statisticsPerDataset;
        }

        public override string MetricName
        {
            get { return "Data Statistics"; }
        }

        public override string Description
        {
            get { return "Useful data statistics"; }
        }

        private List<DataSetStatistics> m_statisticsPerDataset;

        public List<DataSetStatistics> StatisticsPerDataset
        {
            get { return m_statisticsPerDataset; }
        }

        public override void WriteCSV(System.IO.TextWriter writer)
        {
            //base.WriteCSV(writer);

            writer.WriteLine("DatasetName,NumQueries,NumDocuments,NumLinks,NumQueriesWithLinks,"
                            +"NumQueriesWithoutLinks,AverageNumLinksPerQuery,"
                            +"AverageNumLinksPerQueryWithLinks,PcntLinksPerQueries,PcntLinksPerQueriesWithLinks");
            
            foreach (DataSetStatistics stats in StatisticsPerDataset)
            {
                writer.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8}", stats.DatasetName, 
                                                                        stats.NumQueries, 
                                                                        stats.NumDocuments, 
                                                                        stats.NumLinks, 
                                                                        stats.NumQueriesWithLinks, 
                                                                        stats.NumQueriesWithoutLinks, 
                                                                        stats.AverageNumLinksPerQuery, 
                                                                        stats.AverageNumLinksPerQueryWithLinks, 
                                                                        stats.PcntLinksPerQueries, 
                                                                        stats.PcntLinksPerQueriesWithLinks);
            }
        }
    }
}
