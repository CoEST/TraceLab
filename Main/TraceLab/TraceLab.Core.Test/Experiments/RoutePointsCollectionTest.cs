using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TraceLab.Core.Experiments;
using System.Windows;

namespace TraceLab.Core.Test.Experiments
{
    [TestClass]
    public class RoutePointsCollectionTest
    {
        private const double tolerance = 0.1;

        [TestMethod]
        public void IsOnLineSegmentVerticalLineTest()
        {
            // test 1 - vertical line - point beetween
            Point p1 = new Point(5, 5);
            Point p2 = new Point(5, 10);
            Point p = new Point(5, 7);
            bool result = RoutePointsCollection_Accessor.IsPointOnLineSegment(p1.X, p1.Y, p2.X, p2.Y, p.X, p.Y, tolerance);
            Assert.IsTrue(result);
            
            // test 2 - vertical line - point not beetween
            p1 = new Point(5, 5);
            p2 = new Point(5, 10);
            p = new Point(6, 7);
            result = RoutePointsCollection_Accessor.IsPointOnLineSegment(p1.X, p1.Y, p2.X, p2.Y, p.X, p.Y, tolerance);
            Assert.IsFalse(result);

            //test 3 - vertical line within tolerance - point beetween
            p1 = new Point(5, 5);
            p2 = new Point(5 + tolerance, 10);
            p = new Point(5, 7);
            result = RoutePointsCollection_Accessor.IsPointOnLineSegment(p1.X, p1.Y, p2.X, p2.Y, p.X, p.Y, tolerance);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsOnLineSegmentTest()
        {
            // test 1 - slope - point beetween
            Point p1 = new Point(140, 290);
            Point p2 = new Point(92, 480);
            Point p = new Point(116, 380);
            bool result = RoutePointsCollection_Accessor.IsPointOnLineSegment(p1.X, p1.Y, p2.X, p2.Y, p.X, p.Y);
            Assert.IsTrue(result);
        }
    }
}
