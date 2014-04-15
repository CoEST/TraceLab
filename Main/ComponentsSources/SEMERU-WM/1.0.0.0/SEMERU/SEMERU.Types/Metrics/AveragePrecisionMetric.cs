using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEMERU.Types.Metrics
{
    [Serializable]
    public class AveragePrecisionMetric : SummaryDataMetric
    {
        protected AveragePrecisionMetric() { }

        public AveragePrecisionMetric(List<SummaryData> summaryData, double pValue, string nameOfPValueTest)
            : base(summaryData, pValue, nameOfPValueTest)
        {
        }

        public override string MetricName
        {
            get { return "Average Precision"; }
        }

        public override string Description
        {
            get
            {
                String description = "Average precision measures how well a traceability technique retrieves relevant documents at the top of the retrieved document list when the documents are listed in descending order of relevance score." +
                                     " Average precision is defined as the sum of the precision divided by the number of the total relevant documents, where precision is computed after correct retrieval of each document.";
                return description;
            }
        }
    }
}
