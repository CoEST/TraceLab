using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace TraceLab.Core.Experiments
{
    /// <summary>
    /// Route point represent elbow point on the node connection edge.
    /// </summary>
    public class RoutePoint : INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoutePoint"/> class.
        /// </summary>
        public RoutePoint()
        {
            m_x = 0.0;
            m_y = 0.0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutePoint"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public RoutePoint(double x, double y)
        {
            m_x = x;
            m_y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutePoint"/> class.
        /// Cloning constructor.
        /// </summary>
        /// <param name="p">The point.</param>
        public RoutePoint(RoutePoint p)
        {
            m_x = p.X;
            m_y = p.Y;
        }

        private double m_x;

        /// <summary>
        /// Gets or sets the X.
        /// </summary>
        /// <value>
        /// The X.
        /// </value>
        public double X
        {
            get { return m_x; }
            set
            {
                m_x = value;
                NotifyPropertyChanged("X");
            }
        }

        private double m_y;

        /// <summary>
        /// Gets or sets the Y.
        /// </summary>
        /// <value>
        /// The Y.
        /// </value>
        public double Y
        {
            get { return m_y; }
            set
            {
                m_y = value;
                NotifyPropertyChanged("Y");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private RoutePoint p;

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="property">The property.</param>
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}
