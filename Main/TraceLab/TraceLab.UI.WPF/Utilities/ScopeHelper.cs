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
using System.Windows;

namespace TraceLab.UI.WPF.Utilities
{
    class ScopeHelper
    {
        /// <summary>
        /// Gets the intersection point between the border of scope and line from scope node center to given 'toNode'.
        /// Padding allows controling how far inside from the border is the intersection computed.
        /// </summary>
        /// <param name="scopeNode">The scope node.</param>
        /// <param name="toNode">To node.</param>
        /// <param name="paddingLeft">The padding left.</param>
        /// <param name="paddingTop">The padding top.</param>
        /// <param name="paddingRight">The padding right.</param>
        /// <param name="paddingBottom">The padding bottom.</param>
        /// <param name="topCanvasOffset">The top canvas offset - represents the offset from top border of the scope to start of the inside canvas. 
        /// Needed because of label above the canvas.</param>
        /// <returns>
        /// Intersection point between scope's border and line from scope center to given 'toNode'. Returns null point if lines do not intersect.
        /// </returns>
        public static Point? GetIntersection(TraceLab.Core.Experiments.ScopeNode scopeNode, TraceLab.Core.Experiments.ExperimentNode toNode, 
                                             double paddingLeft, double paddingTop, double paddingRight, double paddingBottom, double topCanvasOffset)
        {
            //calculate optimal x of intersection between scope node center to its 
            Point scopeCenter = new Point(scopeNode.Data.X, scopeNode.Data.Y);

            Point endNodeCenter = new Point(toNode.Data.X, toNode.Data.Y);

            //corners including padding
            Point topLeftCorner = new Point(scopeCenter.X - scopeNode.DataWithSize.Width / 2 + paddingLeft, scopeCenter.Y - scopeNode.DataWithSize.Height / 2 + paddingTop + topCanvasOffset);
            Point topRightCorner = new Point(scopeCenter.X + scopeNode.DataWithSize.Width / 2 - paddingRight, scopeCenter.Y - scopeNode.DataWithSize.Height / 2 + paddingTop + topCanvasOffset);
            Point bottomLeftCorner = new Point(scopeCenter.X - scopeNode.DataWithSize.Width / 2 + paddingLeft, scopeCenter.Y + scopeNode.DataWithSize.Height / 2 - paddingBottom);
            Point bottomRightCorner = new Point(scopeCenter.X + scopeNode.DataWithSize.Width / 2 - paddingRight, scopeCenter.Y + scopeNode.DataWithSize.Height / 2 - paddingBottom);

            Point? intersectionOnMainCanvas;

            if (TryFindIntersection(scopeCenter, endNodeCenter, bottomLeftCorner, bottomRightCorner, out intersectionOnMainCanvas)) { } /* top */
            else if (TryFindIntersection(scopeCenter, endNodeCenter, bottomRightCorner, topRightCorner, out intersectionOnMainCanvas)) { } /* right */
            else if (TryFindIntersection(scopeCenter, endNodeCenter, bottomLeftCorner, topLeftCorner, out intersectionOnMainCanvas)) { } /* left */
            else if (TryFindIntersection(scopeCenter, endNodeCenter, topLeftCorner, topRightCorner, out intersectionOnMainCanvas)) { } /* top */

            //scope has it's own canvas with its own origin point, which is located in top left corner of the scope
            //therefore the computed point must be adjusted relatively to the scope canvas origin
            Point? adjustedIntersection = null;

            if (intersectionOnMainCanvas != null)
            {
                adjustedIntersection = AdjustPointToScopeCanvasOrigin(intersectionOnMainCanvas.Value, scopeNode, topCanvasOffset, ref scopeCenter);
            }

            return adjustedIntersection;
        }

        /// <summary>
        /// Tries to find intersection between two lines. Each line is defined by two points.
        /// If intersection is found method returns true. If lines do not intersect method returns false.
        /// 
        /// The math is explained at this blog post: http://thirdpartyninjas.com/blog/2008/10/07/line-segment-intersection/
        /// </summary>
        /// <param name="point1">The point1 - start of first line.</param>
        /// <param name="point2">The point2 - end of first line.</param>
        /// <param name="point3">The point3 - start of second line.</param>
        /// <param name="point4">The point4 - end of second line.</param>
        /// <param name="intersectionPoint">The intersection point if lines intersects, otherwise null.</param>
        /// <returns>true if intersection is found</returns>
        private static bool TryFindIntersection(Point point1, Point point2, Point point3, Point point4, out Point? intersectionPoint)
        {
            bool intersects = false;
            intersectionPoint = null;

            double ua = (point4.X - point3.X) * (point1.Y - point3.Y) - (point4.Y - point3.Y) * (point1.X - point3.X);
            double ub = (point2.X - point1.X) * (point1.Y - point3.Y) - (point2.Y - point1.Y) * (point1.X - point3.X);
            double denominator = (point4.Y - point3.Y) * (point2.X - point1.X) - (point4.X - point3.X) * (point2.Y - point1.Y);

            if (Math.Abs(denominator) <= 0.00001f)
            {
                if (Math.Abs(ua) <= 0.00001f && Math.Abs(ub) <= 0.00001f)
                {
                    intersects = true;
                    intersectionPoint = new Point((point1.X + point2.X) / 2, (point1.Y + point2.Y) / 2);
                }
            }
            else
            {
                ua /= denominator;
                ub /= denominator;

                if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
                {
                    intersects = true;
                    double x = point1.X + ua * (point2.X - point1.X);
                    double y = point1.Y + ua * (point2.Y - point1.Y);
                    intersectionPoint = new Point(x, y);
                }
            }

            return intersects;
        }

        #region LoopScope Helper methods

        /// <summary>
        /// Gets the point of bottom border center with a given padding to border
        /// </summary>
        /// <param name="scopeNode">The scope node.</param>
        /// <param name="paddingBottom">The padding from bottom border.</param>
        /// <param name="topCanvasOffset">The top canvas offset - scope may have label, thus graph canvas is off by some amount from top.</param>
        /// <returns>bottom border center</returns>
        public static Point GetBottomBorderCenter(TraceLab.Core.Experiments.ScopeNodeBase scopeNode, double paddingBottom, double topCanvasOffset)
        {
            Point scopeCenter = new Point(scopeNode.Data.X, scopeNode.Data.Y);
            Point bottomBorderCenter = new Point(scopeCenter.X, scopeCenter.Y + scopeNode.DataWithSize.Height / 2 - paddingBottom);
            Point adjustedPoint = AdjustPointToScopeCanvasOrigin(bottomBorderCenter, scopeNode, topCanvasOffset, ref scopeCenter);
            return adjustedPoint;
        }

        /// <summary>
        /// Gets the point of top border center with a given padding to border
        /// </summary>
        /// <param name="scopeNode">The scope node.</param>
        /// <param name="paddingBottom">The padding from top border.</param>
        /// <param name="topCanvasOffset">The top canvas offset - scope may have label, thus graph canvas is off by some amount from top.</param>
        /// <returns>top border center</returns>
        public static Point GetTopBorderCenter(TraceLab.Core.Experiments.ScopeNodeBase scopeNode, double paddingTop, double topCanvasOffset)
        {
            Point scopeCenter = new Point(scopeNode.Data.X, scopeNode.Data.Y);
            Point topBorderCenter = new Point(scopeCenter.X, scopeCenter.Y - scopeNode.DataWithSize.Height / 2 + paddingTop + topCanvasOffset);
            Point adjustedPoint = AdjustPointToScopeCanvasOrigin(topBorderCenter, scopeNode, topCanvasOffset, ref scopeCenter);
            return adjustedPoint;
        }

        #endregion

        #region Small methods

        /// <summary>
        /// Adjusts the point to scope canvas origin of the scope node
        /// </summary>
        /// <param name="pointToAdjust">The point to adjust.</param>
        /// <param name="scopeNode">The scope node.</param>
        /// <param name="topCanvasOffset">The top canvas offset - represents the offset from top border of the scope to start of the inside canvas. </param>
        /// <param name="scopeCenter">The scope center.</param>
        /// <returns></returns>
        private static Point AdjustPointToScopeCanvasOrigin(Point pointToAdjust, TraceLab.Core.Experiments.ScopeNodeBase scopeNode, double topCanvasOffset, ref Point scopeCenter)
        {
            Point originCanvasPoint = GetOriginCanvasPoint(scopeNode, topCanvasOffset, ref scopeCenter);
            Point adjustedPoint = AdjustPointToGivenOriginPoint(pointToAdjust, ref originCanvasPoint);
            return adjustedPoint;
        }

        /// <summary>
        /// Adjusts the point to given origin.
        /// </summary>
        /// <param name="pointToAdjust">The point to adjust.</param>
        /// <param name="originPoint">The origin canvas point.</param>
        /// <returns>adjusted point</returns>
        private static Point AdjustPointToGivenOriginPoint(Point pointToAdjust, ref Point originPoint)
        {
            double x = pointToAdjust.X - originPoint.X;
            double y = pointToAdjust.Y - originPoint.Y;
            Point adjustedPoint = new Point(x, y);

            return adjustedPoint;
        }

        /// <summary>
        /// Gets the origin canvas point of the scope
        /// </summary>
        /// <param name="scopeNode">The scope node.</param>
        /// <param name="topCanvasOffset">The top canvas offset.</param>
        /// <param name="scopeCenter">The scope center.</param>
        /// <returns></returns>
        private static Point GetOriginCanvasPoint(TraceLab.Core.Experiments.ScopeNodeBase scopeNode, double topCanvasOffset, ref Point scopeCenter)
        {
            Point originCanvasPoint = new Point(scopeCenter.X - scopeNode.DataWithSize.Width / 2, scopeCenter.Y - scopeNode.DataWithSize.Height / 2 + topCanvasOffset);
            return originCanvasPoint;
        }

        #endregion
    }
}
