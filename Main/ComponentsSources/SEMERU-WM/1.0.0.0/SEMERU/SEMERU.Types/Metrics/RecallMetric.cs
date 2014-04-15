using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEMERU.Types.Metrics
{
    [Serializable]
    public class RecallMetric : SummaryDataMetric
    {
        protected RecallMetric() { }

        public RecallMetric(List<SummaryData> summaryData, double pValue, string nameOfPValueTest)
            : base(summaryData, pValue, nameOfPValueTest)
        {

        }

        public override string MetricName
        {
            get { return "Recall"; }
        }

        public override string Description
        {
            get
            {
                return "Recall measures the fraction of correctly retrieved documents among the total correct documents." +
                            " Recall is measured at a certain threshold such as a certain relevance score or the number of retrieved documents given as a parameter.";
            }
        }
    }
}
