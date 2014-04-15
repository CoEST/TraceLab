using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetricsTypes;

namespace SEMERU.ICPC12.Metrics
{
    public class EffectivenessMetric : SummaryDataMetric
    {
        private string _modifier;

        protected EffectivenessMetric() { }

        public EffectivenessMetric(List<SummaryData> summaryData, double pValue, string nameOfPValueTest)
            : base(summaryData, pValue, nameOfPValueTest)
        {
            _modifier = "";
        }

        public EffectivenessMetric(List<SummaryData> summaryData, double pValue, string nameOfPValueTest, string modifier)
            : base(summaryData, pValue, nameOfPValueTest)
        {
            _modifier = " " + modifier;
        }

        public override string Description
        {
            get { return "Location in rank list of results."; }
        }

        public override string MetricName
        {
            get { return "Effectiveness Measure" + _modifier; }
        }

    }
}
