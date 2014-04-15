using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SEMERU.Types.Metrics
{
    [Serializable]
    public class PrecisionRecallCurveMetric : Metric
    {
        protected PrecisionRecallCurveMetric() { }

        public PrecisionRecallCurveMetric(List<Series> series)
        {
            m_series = series;
        }

        public override string MetricName
        {
            get { return "Precision Recall Curve"; }
        }

        public override string Description
        {
            get
            {
                return "A precision-recall curve shows a curve that consists of points of precision and recall measured after retrieval of each documents.";
            }
        }

        private List<Series> m_series;
        public List<Series> Series
        {
            get
            {
                return m_series;
            }
        }

        public override void WriteCSV(System.IO.TextWriter writer)
        {
            //base.WriteCSV(writer);

            writer.WriteLine("DatasetName,NumQueries,NumDocuments,NumLinks,NumQueriesWithLinks,"
                            + "NumQueriesWithoutLinks,AverageNumLinksPerQuery,"
                            + "AverageNumLinksPerQueryWithLinks,PcntLinksPerQueries,PcntLinksPerQueriesWithLinks");

            foreach (Series series in Series)
            {
                writer.WriteLine("Technique Name,\"{0}\"", series.TechniqueName);
                writer.WriteLine("Datapoints");
                writer.WriteLine("Precision,Recall");
                foreach (PrecisionRecallPoint point in series.DataPoints)
                {
                    writer.WriteLine("{0},{1}", point.precision, point.recall);
                }
            }
        }
    }
}
