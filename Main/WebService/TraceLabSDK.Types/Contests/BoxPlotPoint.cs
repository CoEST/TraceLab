// TraceLab - Software Traceability Instrument to Facilitate and Empower Traceability Research
// Copyright (C) 2012-2013 CoEST - National Science Foundation MRI-R2 Grant # CNS: 0959924
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see<http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TraceLabSDK.Types.Contests
{
    /// <summary>
    /// Represents the box plot point in BoxSummaryData series
    /// </summary>
    [Serializable]
    public class BoxPlotPoint : IRawSerializable
    {
        #region Members

        private double m_min;
        /// <summary>
        /// Gets or sets the min.
        /// </summary>
        /// <value>
        /// The min.
        /// </value>
        public double Min
        {
            get { return m_min; }
            set
            {
                if (Double.IsNaN(value))
                    throw new ArgumentException("The box plot point Min value cannot be set to Nan (Not a number) double value. ");
                m_min = value;
            }
        }

        private double m_q1;
        /// <summary>
        /// Gets or sets the q1.
        /// </summary>
        /// <value>
        /// The q1.
        /// </value>
        public double Q1
        {
            get { return m_q1; }
            set
            {
                if (Double.IsNaN(value))
                    throw new ArgumentException("The box plot point Q1 value cannot be set to Nan (Not a number) double value. ");
                m_q1 = value;
            }
        }

        private double m_median;
        /// <summary>
        /// Gets or sets the median.
        /// </summary>
        /// <value>
        /// The median.
        /// </value>
        public double Median
        {
            get { return m_median; }
            set
            {
                if (Double.IsNaN(value))
                    throw new ArgumentException("The box plot point Median value cannot be set to Nan (Not a number) double value. ");

                m_median = value;
            }
        }

        private double m_mean;
        /// <summary>
        /// Gets or sets the mean.
        /// </summary>
        /// <value>
        /// The mean.
        /// </value>
        public double Mean
        {
            get { return m_mean; }
            set
            {
                if (Double.IsNaN(value))
                    throw new ArgumentException("The box plot point Mean value cannot be set to Nan (Not a number) double value. ");
                m_mean = value;
            }
        }

        private double m_q3;
        /// <summary>
        /// Gets or sets the q3.
        /// </summary>
        /// <value>
        /// The q3.
        /// </value>
        public double Q3
        {
            get { return m_q3; }
            set
            {
                if (Double.IsNaN(value))
                    throw new ArgumentException("The box plot point Q3 value cannot be set to Nan (Not a number) double value. ");
                m_q3 = value;
            }
        }

        private double m_max;
        /// <summary>
        /// Gets or sets the max.
        /// </summary>
        /// <value>
        /// The max.
        /// </value>
        public double Max
        {
            get { return m_max; }
            set
            {
                if (Double.IsNaN(value))
                    throw new ArgumentException("The box plot point Max value cannot be set to Nan (Not a number) double value. ");
                m_max = value;
            }
        }

        private double m_stdDev;
        /// <summary>
        /// Gets or sets the STD dev.
        /// </summary>
        /// <value>
        /// The STD dev.
        /// </value>
        public double StdDev
        {
            get { return m_stdDev; }
            set
            {
                if (Double.IsNaN(value))
                    throw new ArgumentException("The box plot point StdDev value cannot be set to Nan (Not a number) double value. ");
                m_stdDev = value;
            }
        }

        private double m_n;
        /// <summary>
        /// Gets or sets the Number of Datapoints.
        /// </summary>
        /// <value>
        /// The N.
        /// </value>
        public double N
        {
            get { return m_n; }
            set
            {
                if (Double.IsNaN(value))
                    throw new ArgumentException("The box plot point N (Number of Datapoints) value cannot be set to Nan (Not a number) double value. ");
                m_n = value;
            }
        }

        #endregion Members

        #region Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxPlotPoint"/> class.
        /// </summary>
        internal BoxPlotPoint()
        {
            this.m_min = 0.0d;
            this.m_q1 = 0.0d;
            this.m_median = 0.0d;
            this.m_mean = 0.0d;
            this.m_q3 = 0.0d;
            this.m_max = 0.0d;
            this.m_stdDev = 0.0d;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxPlotPoint"/> class.
        /// </summary>
        /// <param name="min">The min.</param>
        /// <param name="q1">The q1.</param>
        /// <param name="median">The median.</param>
        /// <param name="mean">The mean.</param>
        /// <param name="q3">The q3.</param>
        /// <param name="max">The max.</param>
        /// <param name="stdDev">The STD dev.</param>
        /// <param name="n">The n - number of points.</param>
        public BoxPlotPoint(double min, double q1, double median, double mean, double q3, double max, double stdDev, double n)
        {
            Min = min;
            Q1 = q1;
            Median = median;
            Mean = mean;
            Q3 = q3;
            Max = max;
            StdDev = stdDev;
            N = n;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxPlotPoint"/> class.
        /// This constructor will compute Min, Q1, Median, Mean, Q3, Max StdDev, N based on the given array of points
        /// </summary>
        /// <param name="dataPoints">The data points.</param>
        public BoxPlotPoint(double[] dataPoints)
        {
            Array.Sort(dataPoints);

            N = dataPoints.Length;

            if (N == 0)
            {
                Min = 0;
                Q1 = 0;
                Median = 0;
                Mean = 0;
                Q3 = 0;
                Max = 0;
                StdDev = 0;
            }
            else if (N == 1)
            {
                Min = dataPoints[0];
                Q1 = dataPoints[0];
                Median = dataPoints[0];
                Mean = dataPoints[0];
                Q3 = dataPoints[0];
                Max = dataPoints[0];
                StdDev = 0;
            }
            else
            {
                int numElements = dataPoints.Count<double>();

                //set Min
                Min = dataPoints.Min();

                //set Median
                if ((numElements % 2) == 0)
                {
                    //double median = Math.Round((dataPoints[((numElements - 1) / 2)] + dataPoints[(numElements + 1) / 2]) / 2, 2);
                    double median = (dataPoints[((numElements - 1) / 2)] + dataPoints[(numElements + 1) / 2]) / 2;
                    Median = median;
                }
                else
                {
                    double median = dataPoints[(numElements / 2)];
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
                double stdevValue = Math.Sqrt(someSum / numElements);
                StdDev = stdevValue;

                //Q1
                Q1 = percentile(dataPoints, 25);

                //Q3
                Q3 = percentile(dataPoints, 75);
            }
        }

        /// <summary>
        /// Percentiles the specified data.
        /// This method assumes the data have already been sorted in ascending order of values
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="percentile">The percentile.</param>
        /// <returns></returns>
        private static double percentile(double[] data, int percentile)
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

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        ///   </exception>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            BoxPlotPoint p = obj as BoxPlotPoint;
            if ((System.Object)p == null)
            {
                return false;
            }

            return (Min == p.Min && Q1 == p.Q1 && Median == p.Median && Mean == p.Mean && Q3 == p.Q3 && Max == p.Max && StdDev == p.StdDev && N == p.N);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Min.GetHashCode() ^
                   Q1.GetHashCode() ^
                   Median.GetHashCode() ^
                   Mean.GetHashCode() ^
                   Q3.GetHashCode() ^
                   Max.GetHashCode() ^
                   StdDev.GetHashCode() ^
                   N.GetHashCode();
        }

        #endregion Methods

        #region IRawSerializable Members

        /// <summary>
        /// Reads the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void ReadData(System.IO.BinaryReader reader)
        {
            this.m_min = reader.ReadDouble();
            this.m_q1 = reader.ReadDouble();
            this.m_median = reader.ReadDouble();
            this.m_mean = reader.ReadDouble();
            this.m_q3 = reader.ReadDouble();
            this.m_max = reader.ReadDouble();
            this.m_stdDev = reader.ReadDouble();
            this.m_n = reader.ReadDouble();
        }

        /// <summary>
        /// Writes the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void WriteData(System.IO.BinaryWriter writer)
        {
            writer.Write(this.m_min);
            writer.Write(this.m_q1);
            writer.Write(this.m_median);
            writer.Write(this.m_mean);
            writer.Write(this.m_q3);
            writer.Write(this.m_max);
            writer.Write(this.m_stdDev);
            writer.Write(this.m_n);
        }

        #endregion IRawSerializable Members
    }
}
