using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEMERU.Types.Metrics
{
    [Serializable]
    public class AverageRankMetric : SummaryDataMetric
    {
        protected AverageRankMetric() { }

        public AverageRankMetric(List<SummaryData> summaryData, double pValue, string nameOfPValueTest, string metricName)
            : base(summaryData, pValue, nameOfPValueTest)
        {
            m_metricName = metricName;
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
                return "Mean rank measures how well a traceability technique performs compared with other traceability techniques by computing the ranks of metric values among the compared traceability techniques, where the metric can be any kind of metric such as average precision." +
                            " For each query, the ranks of traceability techniques are computed. Then the average of rank across queries is computed for each traceability technique." +
                            " To obtain robust estimates, the current implementation performs resampling of a data set using 1000 repetitions of bootstrapping." +
                            " In each bootstrap, the same number of queries as the original data set is randomly selected with replacement and the mean rank is computed." +
                            " The same procedure is repeated for all the data sets. Therefore, for five data sets, 5000 bootstraps are made." +
                            " Because mean rank ignores the magnitudes of metric values, each data set contributes to the final results with the same weight." +
                            " Therefore, mean rank is more robust than actual metric values." +
                            " A low rank value indicates a high metric value. For example, if average precision from technique 1 is point 0.7 and average precision from technique 2 is point 0.5 technique 1 will have rank 1 and technique 2 will have rank 2.";
            }
        }
    }
}
