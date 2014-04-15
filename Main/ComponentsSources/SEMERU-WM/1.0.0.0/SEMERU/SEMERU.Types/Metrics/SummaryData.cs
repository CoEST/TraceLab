using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEMERU.Types.Metrics
{
    [Serializable]
    public class SummaryData
    {
        protected SummaryData() { }

        private string m_seriesName;
        public string SeriesName
        {
            get
            {
                return m_seriesName;
            }
        }

        public SummaryData(string techniqueName, double[] dataPoints)
        {
            m_seriesName = techniqueName;

            Array.Sort(dataPoints);
            DataPoints = dataPoints;

            int numElements = dataPoints.Count<double>();

            //set Min
            Min = dataPoints.Min();

            //set Median
            if ((numElements % 2) == 0)
            {
                double median = Math.Round((dataPoints[((numElements - 1) / 2)] + dataPoints[(numElements + 1) / 2]) / 2, 2);
                Median = median;
            }
            else
            {
                double median = Math.Round(dataPoints[(numElements / 2)], 2);
                Median = median;
            }
            double checkMedian = percentile(dataPoints, 50);
            
            //set Max
            Max = dataPoints.Max();

            //Mean
            Mean = dataPoints.Sum() / numElements;

            //Standard deviation
            double someSum = 0;
            foreach (double x in dataPoints)
            {
                someSum += Math.Pow((Mean - x), 2);
            }
            double stdevValue = Math.Round(Math.Sqrt(someSum / numElements), 2);
            StdDev = stdevValue;

            //Q1
            Q1 = percentile(dataPoints, 25);

            //Q3
            Q3 = percentile(dataPoints, 75);
        }

        public double Min
        {
            get;
            private set;
        }

        public double Q1
        {
            get;
            private set;
        }

        public double Median
        {
            get;
            private set;
        }

        public double Mean
        {
            get;
            private set;
        }

        public double Q3
        {
            get;
            private set;
        }

        public double Max
        {
            get;
            private set;
        }

        public double StdDev
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the Number of Datapoints.
        /// </summary>
        /// <value>
        /// The N.
        /// </value>
        public double N
        {
            get
            {
                return DataPoints.Length;
            }
        }

        public double[] DataPoints
        {
            get;
            private set;
        }

        // this method assumes the data have already been sorted in ascending order of values
        private double percentile(double[] data, int percentile)
        {
            //Array.Sort(data);
            int numberOfValues = data.Length;
            double i = 0.5 + ((numberOfValues * (percentile * 1.0)) / 100);
            int whole = (int)Math.Floor(i);
            double frac = i - whole;
            if (frac == 0)
            {
                return data[whole - 1];
            }
            else
            {
                return data[whole - 1] * (1 - frac) + frac * data[whole];
            }
        }
    }
}
