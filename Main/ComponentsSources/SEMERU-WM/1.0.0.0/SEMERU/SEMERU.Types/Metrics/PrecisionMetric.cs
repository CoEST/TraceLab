using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEMERU.Types.Metrics
{
    [Serializable]
    public class PrecisionMetric : SummaryDataMetric
    {
        protected PrecisionMetric() { }

        public PrecisionMetric(List<SummaryData> summaryData, double pValue, string nameOfPValueTest)
            : base(summaryData, pValue, nameOfPValueTest)
        {
        }

        public override string MetricName
        {
            get { return "Precision"; }
        }
        
        public override string Description
        {
            get
            {
                return "Precision measures the fraction of correctly retrieved documents among the retrieved documents." +
                            " Precision is measured at a certain threshold such as a certain relevance score or the number of retrieved documents given as a parameter.";
            }
        }
    }
}
