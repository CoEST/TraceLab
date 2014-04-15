using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SEMERU.Types.Metrics
{
    [Serializable]
    public abstract class SummaryDataMetric : Metric
    {
        protected SummaryDataMetric() { }

        public SummaryDataMetric(List<SummaryData> summaryData, double pvalue, string nameOfPValueTest)
        {
            m_summaryData = summaryData;
            m_pvalue = pvalue;
            m_nameOfPValueTest = nameOfPValueTest;
        }

        private List<SummaryData> m_summaryData;
        public List<SummaryData> SummaryData
        {
            get
            {
                return m_summaryData;
            }
        }

        private double m_pvalue;
        public double PValue
        {
            get
            {
                return m_pvalue;
            }
        }

        private string m_nameOfPValueTest;
        public string NameOfPValueTest
        {
            get
            {
                return m_nameOfPValueTest;
            }
        }

        public override void WriteCSV(System.IO.TextWriter writer)
        {
            // TODO: investigate this
            //base.WriteCSV(writer);
            writer.WriteLine("P-Value Test Name,{0}", NameOfPValueTest);
            writer.WriteLine("P-Value,{0}", PValue);

            writer.WriteLine("Technique Name, Min, Q1, Mean, Median, Q3, Max, StdDev, N");
            foreach (SummaryData summary in SummaryData)
            {
                writer.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8}", summary.SeriesName, summary.Min, summary.Q1, summary.Mean, summary.Median, summary.Q3, summary.Max, summary.StdDev, summary.N);
            }
        }
    }
}
