using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEMERU.Types.Metrics
{
    [Serializable]
    public class PrecisionAtRecall100Metric : SummaryDataMetric
    {
        protected PrecisionAtRecall100Metric() { }

        public PrecisionAtRecall100Metric(List<SummaryData> summaryData, double pValue, string nameOfPValueTest, string name)
            : base(summaryData, pValue, nameOfPValueTest)
        {
            m_metricName = name;
        }

        private string m_metricName;
        public override string MetricName
        {
            get { return m_metricName; }
        }

        public override string Description
        {
            get 
            {
                return "Precision at recall 100% measures precision at the earliest point that recall reaches 1 when the retrieved documents are listed in descending order of relevance score."; 
            }
        }
    }
}
