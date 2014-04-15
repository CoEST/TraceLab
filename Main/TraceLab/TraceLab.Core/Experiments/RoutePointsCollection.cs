using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace TraceLab.Core.Experiments
{
    /// <summary>
    /// Class is a collection of route points belonging to experiment node connection.
    /// The collection send simple notification about all changes in the collection and its route points changes.
    /// </summary>
    public class RoutePointsCollection : IEnumerable<RoutePoint>, INotifyCollectionChanged, IXmlSerializable
    {
        /// <summary>
        /// Actual container for route points
        /// </summary>
        private LinkedList<RoutePoint> m_routePoints;

        /// <summary>
        /// Owner connection is needed to know starting and ending point of connection
        /// to compute in each place the new route point should be added
        /// </summary>
        private ExperimentNodeConnection m_ownerConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutePointsCollection"/> class.
        /// </summary>
        private RoutePointsCollection()
        {
            m_routePoints = new LinkedList<RoutePoint>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutePointsCollection"/> class.
        /// </summary>
        /// <param name="ownerConnection">The owner connection.</param>
        public RoutePointsCollection(ExperimentNodeConnection ownerConnection) : this()
        {
            m_ownerConnection = ownerConnection;
        }

        #region Add

        /// <summary>
        /// Adds the specified route point
        /// </summary>
        /// <param name="routePoint">The final point.</param>
        public void Add(RoutePoint routePoint)
        {
            bool added = false;
            //if it first point just add it to the list
            if (m_routePoints.Count == 0)
            {
                m_routePoints.AddLast(routePoint);
                added = true;
            }
            else
            {
                //otherwise find the correct location in the linked list for it to be added
                added = AddInPlace(routePoint);
            }

            //if route point has been added successfully raise a notification and attach listener
            if (added)
            {
                AttachPropertyChangedListener(routePoint);

                //notify about collection change (note, that it also rises PointsChange event)
                var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, routePoint);
                NotifyCollectionChanged(args);
            }
        }

        /// <summary>
        /// Attaches the property changed listener, so that anytime user moves point
        /// it fires proper notification, so the line can be redrawn.
        /// </summary>
        /// <param name="routePoint">The route point.</param>
        private void AttachPropertyChangedListener(RoutePoint routePoint)
        {
            //attach listener to the route point change
            routePoint.PropertyChanged += OnRoutePointChanged;
        }

        /// <summary>
        /// Adds the new route point in the correct place in the linked list.
        /// It must be added in correct location as the drawing of the graph processes route points
        /// in order they are in the list, so if user adds the point between two existing route points,
        /// it must be added in that place.
        /// </summary>
        /// <param name="newPoint">The new point.</param>
        /// <returns></returns>
        private bool AddInPlace(RoutePoint newPoint)
        {
            //compute where to put new route point    
            //note that since source start and target end connection are not within route points collection
            //do check these line segments first with first and last route point accordingly

            //1. check if new route point is on the segment between start point and first route point.
            Point start = new Point(m_ownerConnection.Source.Data.X, m_ownerConnection.Source.Data.Y);
            RoutePoint firstPoint = m_routePoints.First.Value;
            if (IsPointOnLineSegment(start.X, start.Y, firstPoint.X, firstPoint.Y, newPoint.X, newPoint.Y))
            {
                m_routePoints.AddFirst(newPoint);
                return true;
            }

            //2. check if new route point is on the segment between end point and last route point.
            Point end = new Point(m_ownerConnection.Target.Data.X, m_ownerConnection.Target.Data.Y);
            RoutePoint endPoint = m_routePoints.Last.Value;
            if (IsPointOnLineSegment(endPoint.X, endPoint.Y, end.X, end.Y, newPoint.X, newPoint.Y))
            {
                m_routePoints.AddLast(newPoint);
                return true;
            }

            //3. finally check between which two route points is the new point
            LinkedListNode<RoutePoint> currentStart = m_routePoints.First;
            while (currentStart != null && currentStart.Next != null)
            {
                LinkedListNode<RoutePoint> currentEnd = currentStart.Next;
                if (IsPointOnLineSegment(currentStart.Value.X, currentStart.Value.Y, currentEnd.Value.X, currentEnd.Value.Y, newPoint.X, newPoint.Y))
                {
                    m_routePoints.AddAfter(currentStart, newPoint);
                    return true;
                }

                currentStart = currentStart.Next;
            }

            return false;
        }

        #endregion

        #region Geometry Is Point On the Line

        /// <summary>
        /// Determines whether the point given by (x,y) is on the line segment between given two points (x1, y1) and (x2, y2)
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>
        ///   <c>true</c> if given point is on the line segment; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsPointOnLineSegment(double x1, double y1, double x2, double y2, double x, double y)
        {
            //user mouse input can be a bit imprecise, 
            //practice showed that moving mouse just a little little bit makes difference of even 5 to 6 in double 
            //so tolerance 8 should be enough
            double tolerance = 8;
            return IsPointOnLineSegment(x1, y1, x2, y2, x, y, tolerance);
        }

        /// <summary>
        /// Determines whether the point given by (x,y) is on the line segment between given two points (x1, y1) and (x2, y2)
        /// with a given tolerance of imprecision. 
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns>
        ///   <c>true</c> if given point is on the line segment; otherwise, <c>false </c>.
        /// </returns>
        private static bool IsPointOnLineSegment(double x1, double y1, double x2, double y2, double x, double y, double tolerance)
        {
            // First checking if (x, y) is in the range of the line segment's end points.
            if (IsBetween (x, x1, x2, tolerance) && IsBetween (y, y1, y2, tolerance))
            {
                if (Math.Abs(x2 - x1) <= tolerance) // Vertical line.
                {
                    return true;
                }
                
                //line equation: y = m*x + b
                
                //first compute the 'm' slope
                double m = (y2 - y1) / (x2 - x1); 

                //secondly compute the 'b' Y intercept
                double b = y1 - (m * x1); 

                //finally plug in (x,y) to line equation and check if they are equal 
                //within tolerance
                bool result = (Math.Abs(y - (m * x + b)) <= tolerance);
                return result;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified x value is between specified bound values within given tolerance
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="bound1">The bound1.</param>
        /// <param name="bound2">The bound2.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns>
        ///   <c>true</c> if the specified x is between bound values; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsBetween(double x, double bound1, double bound2, double tolerance)
        {
           // Handles cases when 'bound1' is greater than 'bound2' and when
           // 'bound2' is greater than 'bound1'.
           return (((x >= (bound1 - tolerance)) && (x <= (bound2 + tolerance))) ||
              ((x >= (bound2 - tolerance)) && (x <= (bound1 + tolerance))));
        }

        #endregion Geometry Is Point On the Line

        /// <summary>
        /// Called when route point properties changed. Method propagates the change notification
        /// so that view is notified to redraw the path
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnRoutePointChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPointsChange();
        }

        /// <summary>
        ///  Gets the number of route points contained in the collection
        /// </summary>
        public int Count
        {
            get { return m_routePoints.Count; }
        }

        /// <summary>
        /// Copies the points from the given collection of points
        /// in same order as they are in the given list
        /// into this collection.
        /// (Note, it does work differently than adding points using Add method one by one)
        /// </summary>
        /// <param name="routePoints">The route points.</param>
        internal void CopyPointsFrom(IEnumerable<RoutePoint> routePoints)
        {
            foreach (RoutePoint point in routePoints)
            {
                RoutePoint clonedPoint = new RoutePoint(point);
                m_routePoints.AddLast(clonedPoint);
                AttachPropertyChangedListener(clonedPoint);
            }
        }

        #region IEnumerable<RoutePoint> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection of route points.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<RoutePoint> GetEnumerator()
        {
            return m_routePoints.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_routePoints.GetEnumerator();
        }

        #endregion

        #region Points Changed

        /// <summary>
        /// Occurs when anything in the collection has been changed, including properties 
        /// of route points contained in the collection. 
        /// In other words it "stronger" event than CollectionChanged.
        /// It not only notifies if any route point has been added or removed, but also
        /// if any route point coordinates changed.
        /// </summary>
        public event EventHandler PointsChanged;

        /// <summary>
        /// Notifies the change.
        /// </summary>
        private void NotifyPointsChange()
        {
            if (PointsChanged != null)
                PointsChanged(this, new EventArgs());
        }

        #endregion

        #region INotifyCollectionChanged Members

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Notifies of the collection changed.
        /// If route points has been added or removed it notifies about these changes.
        /// Note, that this is needed for binding to ItemsSource.
        /// </summary>
        /// <param name="args">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void NotifyCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (args == null)
                throw new ArgumentNullException("args");

            if (CollectionChanged != null)
                CollectionChanged(this, args);

            //fire also PointsChanged event
            NotifyPointsChange();
        }

        #endregion

        #region IXmlSerializable Members

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.
        /// </returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            XPathDocument doc = new XPathDocument(reader);
            XPathNavigator nav = doc.CreateNavigator();

            XPathNodeIterator routePointIter = nav.Select("//RoutePointsCollection/RoutePoint");

            while (routePointIter.MoveNext())
            {
                double x, y;
                if (Double.TryParse(routePointIter.Current.GetAttribute("X", String.Empty), out x)
                   && Double.TryParse(routePointIter.Current.GetAttribute("Y", String.Empty), out y))
                {
                    RoutePoint point = new RoutePoint(x, y);
                    m_routePoints.AddLast(point);
                    AttachPropertyChangedListener(point);
                }
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            foreach (RoutePoint routePoint in m_routePoints)
            {
                writer.WriteStartElement("RoutePoint");
                writer.WriteAttributeString("X", routePoint.X.ToString());
                writer.WriteAttributeString("Y", routePoint.Y.ToString());
                writer.WriteEndElement();
            }
        }

        #endregion
    }
}
