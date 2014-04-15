using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEMERU.Types.Metrics
{
    [Serializable]
    public class Series
    {
        protected Series() { }

        public Series(string techniqueName, List<PrecisionRecallPoint> dataPoints)
        {
            m_techniqueName = techniqueName;

            DataPoints = dataPoints;
        }

        private string m_techniqueName;
        public string TechniqueName
        {
            get
            {
                return m_techniqueName;
            }
        }

        public List<PrecisionRecallPoint> DataPoints
        {
            get;
            private set;
        }
    }

    [Serializable]
    public class PrecisionRecallPoint
    {
        protected PrecisionRecallPoint() { }

        public PrecisionRecallPoint(double recall, double precision)
        {
            this.recall = recall;
            this.precision = precision;
        }

        public double recall
        {
            get;
            set;
        }

        public double precision
        {
            get;
            set;
        }
    }
}
