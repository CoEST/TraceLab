using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using TraceLabSDK;

namespace SEMERU.Types.Metrics
{
    /// <summary>
    /// DatasetResults can contain results for metrics for all techniques
    /// </summary>
    [Serializable]
    public class DatasetResults
    {
        protected DatasetResults() { }

        public DatasetResults(string datasetName, List<Metric> metrics)
        {
            m_datasetName = datasetName;
            m_metrics = metrics;
        }

        private string m_datasetName;
        /// <summary>
        /// Gets or sets the name of the dataset, the identifier for dataset for which results were computed.
        /// </summary>
        /// <value>
        /// The name of the dataset.
        /// </value>
        public string DatasetName
        {
            get
            {
                return m_datasetName;
            }
        }

        private List<Metric> m_metrics;
        public List<Metric> Metrics
        {
            get
            {
                return m_metrics;
            }
        }
    }
}
