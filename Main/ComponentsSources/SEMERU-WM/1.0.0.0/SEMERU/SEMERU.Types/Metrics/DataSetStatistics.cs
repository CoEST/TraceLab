using System;
// Located in c:\Program Files (x86)\COEST\TraceLab\lib\TraceLabSDK.dll
using System.Collections.Generic;
using System.Collections;
using TraceLabSDK;
using TraceLabSDK.Types;

namespace SEMERU.Types.Metrics
{
    [Serializable]
    public class DataSetStatistics
    {
        protected DataSetStatistics() { }

        public DataSetStatistics(string datasetName, int numQueries, int numDocuments, int numLinks, int numQueriesWithLinks, int numQueriesWithoutLinks,
                                 double averageNumLinksPerQuery, double averageNumLinksPerQueryWithLinks, double pcntLinksPerQueries, double pcntLinksPerQueriesWithLinks) 
        {
            DatasetName = datasetName;
            NumQueries = numQueries;
            NumDocuments = numDocuments;
            NumLinks = numLinks;
            NumQueriesWithLinks = numQueriesWithLinks;
            NumQueriesWithoutLinks = numQueriesWithoutLinks;
            AverageNumLinksPerQuery = averageNumLinksPerQuery;
            AverageNumLinksPerQueryWithLinks = averageNumLinksPerQueryWithLinks;
            PcntLinksPerQueries = pcntLinksPerQueries;
            PcntLinksPerQueriesWithLinks = pcntLinksPerQueriesWithLinks;
        }

        private string m_datasetName;

        public string DatasetName
        {
            get { return m_datasetName; }
            set { m_datasetName = value; }
        }

        private int m_numQueries;

        public int NumQueries
        {
            get { return m_numQueries; }
            set { m_numQueries = value; }
        }

        private int m_numDocuments;

        public int NumDocuments
        {
            get { return m_numDocuments; }
            set { m_numDocuments = value; }
        }

        private int m_numLinks;

        public int NumLinks
        {
            get { return m_numLinks; }
            set { m_numLinks = value; }
        }

        private int m_numQueriesWithLinks;

        public int NumQueriesWithLinks
        {
            get { return m_numQueriesWithLinks; }
            set { m_numQueriesWithLinks = value; }
        }

        private int m_numQueriesWithoutLinks;

        public int NumQueriesWithoutLinks
        {
            get { return m_numQueriesWithoutLinks; }
            set { m_numQueriesWithoutLinks = value; }
        }

        private double m_averageNumLinksPerQuery;

        public double AverageNumLinksPerQuery
        {
            get { return m_averageNumLinksPerQuery; }
            set { m_averageNumLinksPerQuery = value; }
        }

        private double m_averageNumLinksPerQueryWithLinks;

        public double AverageNumLinksPerQueryWithLinks
        {
            get { return m_averageNumLinksPerQueryWithLinks; }
            set { m_averageNumLinksPerQueryWithLinks = value; }
        }

        private double m_pcntLinksPerQueries;

        public double PcntLinksPerQueries
        {
            get { return m_pcntLinksPerQueries; }
            set { m_pcntLinksPerQueries = value; }
        }

        private double m_pcntLinksPerQueriesWithLinks;

        public double PcntLinksPerQueriesWithLinks
        {
            get { return m_pcntLinksPerQueriesWithLinks; }
            set { m_pcntLinksPerQueriesWithLinks = value; }
        }
    }
}