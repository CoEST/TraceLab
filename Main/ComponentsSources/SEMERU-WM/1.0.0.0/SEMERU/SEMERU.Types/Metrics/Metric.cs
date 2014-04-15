using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEMERU.Types.Metrics
{
    /// <summary>
    /// Represents the results across many techniques. 
    /// </summary>
    [Serializable]
    public abstract class Metric
    {
        protected Metric() { }

        /// <summary>
        /// Gets the name of the metric.
        /// </summary>
        /// <value>
        /// The name of the metric.
        /// </value>
        public abstract string MetricName
        {
            get;
        }

        public abstract string Description
        {
            get;
        }

        public virtual void WriteCSV(System.IO.TextWriter writer)
        {
            writer.WriteLine("Metric Name,\"{0}\"", MetricName);
            writer.WriteLine("Description,\"{0}\"", Description);
        }
    }
}
