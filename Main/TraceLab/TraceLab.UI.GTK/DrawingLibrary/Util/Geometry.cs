// MonoHotDraw. Diagramming Framework
//
// Authors:
//	Manuel Cerón <ceronman@gmail.com>
//
// Copyright (C) 2006, 2007, 2008, 2009 MonoUML Team (http://www.monouml.org)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using Cairo;
using System;
using System.Collections.Generic;

namespace MonoHotDraw.Util {

	public sealed class Geometry {
	
		private Geometry () {
		}
	
		public static double AngleFromPoint (RectangleD r, PointD point) {
			double rx = point.X - r.Center.X;
			double ry = point.Y - r.Center.Y;
			return Math.Atan2 (ry * r.Width, rx * r.Height);
		}

		public static PointD EdgePointFromAngle (RectangleD r, double angle) {
			double sin = Math.Sin (angle);
			double cos = Math.Cos (angle);
			double e = 0.0001;
			double x = 0;
			double y = 0;

			if (Math.Abs (sin) > e) {
				x = (1.0 + cos / Math.Abs (sin)) / 2.0 * r.Width;
				x = Range (0.0, r.Width, x);
			}
			else if (cos >= 0.0) {
				x = r.Width;
			}

			if (Math.Abs (cos) > e) {
				y = (1.0 + sin / Math.Abs (cos)) / 2.0 * r.Height;
				y = Range (0.0, r.Height, y);
			}
			else if (sin >= 0.0) {
				y = r.Height;
			}

			return new PointD (r.X + x, r.Y + y);
		}

		public static double Range (double min, double max, double num)	{
			return num < min ? min : (num > max ? max: num);
		}
	
		public static bool LineContainsPoint (double x1, double y1, double x2, double y2, double px, double py) {
			RectangleD r = new RectangleD (new PointD (x1, y1));
			r.Add (x2, y2);
			r.Inflate (2.0, 2.0);
			if (!r.Contains (px, py)) {
				return false;
			}

			double a, b, x, y;

			if (x1 == x2) {
				return (Math.Abs (px - x1) < 3.0);
			}

			if (y1 == y2) {
				return (Math.Abs (py - y1) < 3.0);
			}
			a = (y1 - y2) / (x1 - x2);
			b = y1 - a * x1;
			x = (py - b) / a;
			y = a * px + b;

			return (Math.Min (Math.Abs (x - px), Math.Abs (y - py)) < 4.0);
		}
		
		public static double LineSize (PointD point1, PointD point2) {
			double w = point1.X - point2.X;
			double h = point1.Y - point2.Y;
			
			return Math.Sqrt (w*w + h*h);
		}
		
		public static double PolyLineSize (List <PointD> points) {
			double len = 0.0;
			for (int i=0; i<points.Count-1; i++) {
				len += Geometry.LineSize (points [i], points [i+1]);
			}
			return len;
		}
		
		// ------ ARROW METHODS
		//
		//                                   .(p2)
		//            pointDistance ----\
		//                               \
		//                             |__\__|
		//                             |     |
		//
		//                     ___  (a)._____.____________________.(b)
		//                      |       \   L|(m) 
		//                      |        \   | <--- (normal)
		//      lineDistance ---|         \  |
		//                      |          \ V
		//                      |           \
		//                     _|_           .(p)
		//
		//
		// Having a rect [ab] from (a) to (b), the arrow point is (p).
		// (p2) is the simetrical point to (p)
		//
		// lineDistance is the minimum distance between the rect [ab] and (p)
		//
		// (m) is the point when the line that passes throught (p) and is perpendicular
		// to [ab] cuts it.
		//
		// pointDistance is the distance between (a) and (p).
		//
		// (normal) is the unitary vector that is normal to [ab]
		//
		// Calculating (p) in terms of pointDistance and lineDistance:
		//
		//
		// (p) =  (m) + (normal) * lineDistance
		// 
		// (m) = (a) + (b-a) * pointDistance / length(ab)
		// 
		// (normal) = (-(b-a).Y, (b-a).X)
		// 
		// When (p) is moved (throught the handle), lineDistance and pointDistance must
		// be recalculated using (p).
		// 
		// Theory at: http://local.wasp.uwa.edu.au/~pbourke/geometry/pointline/
		// 
		// 
		//                  (p.X-a.X) * (b.X-a.X) + (p.Y-a.Y) * (b.Y-a.Y)
		// pointDistance =   ---------------------------------------------
		//                                 length(ab)
		//                                 
		// lineDistance  =   (a.X-p.X) * (b.Y-a.Y) - (a.Y-p.Y) * (b.X-a.X)
		//                  ---------------------------------------------
		//                                 length(ab)
		
		// This method gets (p) and (m) from (a), (b), lineDistance and pointDistance
		public static void GetArrowPoints (PointD a, PointD b, 
											double lineDistance, double pointDistance,
											out PointD p, out PointD p2,
											out PointD m) {
											
			PointD ab_vector = new PointD (b.X - a.X, b.Y - a.Y);
			
			double length = Geometry.LineSize (a, b);
			
			if (length == 0) {
				m = p = a;
			}
				
			double proportion = pointDistance / length;
			
			PointD normal = new PointD (0.0, 0.0);
			normal.X = -ab_vector.Y / length;
			normal.Y = ab_vector.X / length;
			
			m = new PointD ();
			m.X = a.X + proportion * ab_vector.X;
			m.Y = a.Y + proportion * ab_vector.Y;
			
			p = new PointD ();
			p.X = m.X + normal.X * lineDistance;
			p.Y = m.Y + normal.Y * lineDistance;
			
			p2 = new PointD ();
			p2.X = m.X + normal.X * lineDistance * -1.0;
			p2.Y = m.Y + normal.Y * lineDistance * -1.0;
		}
		
		// This method does the oposite to GetArrowPoint it gets
		// lineDistence and pointDistance from (a), (b) and (p)
		public static void GetArrowDistances (PointD a, PointD b, PointD p, 
											out double lineDistance, out double pointDistance) {
											
			double length = Geometry.LineSize (a, b);
			
			if (length == 0) {
				pointDistance = 0.0;
				lineDistance = 0.0;
			}
			pointDistance = ((p.X - a.X) * (b.X - a.X) + (p.Y - a.Y) * (b.Y - a.Y)) / length;
			lineDistance =  ((a.X - p.X) * (b.Y - a.Y) - (a.Y - p.Y) * (b.X - a.X)) / length;
		}
		
		public static PointD? LineIntersection (PointD l1p1, PointD l1p2, PointD l2p1, PointD l2p2) {
			return LineIntersection (l1p1.X, l1p1.Y, l1p2.X, l1p2.Y, l2p1.X, l2p1.Y, l2p2.X, l2p2.Y);
		}
		
		public static PointD? LineIntersection ( double xa,   // line 1 point 1 x
												double ya,   // line 1 point 1 y
												double xb,   // line 1 point 2 x
												double yb,   // line 1 point 2 y
												double xc,   // line 2 point 1 x
												double yc,   // line 2 point 1 y
												double xd,   // line 2 point 2 x
												double yd) { // line 2 point 2 y
												
			// source: http://vision.dai.ed.ac.uk/andrewfg/c-g-a-faq.html
			// eq: for lines AB and CD
			//     (YA-YC)(XD-XC)-(XA-XC)(YD-YC)
			// r = -----------------------------  (eqn 1)
			//     (XB-XA)(YD-YC)-(YB-YA)(XD-XC)
			//
			//     (YA-YC)(XB-XA)-(XA-XC)(YB-YA)
			// s = -----------------------------  (eqn 2)
			//     (XB-XA)(YD-YC)-(YB-YA)(XD-XC)
			//  XI = XA + r(XB-XA)
			//  YI = YA + r(YB-YA)
		
		    double denom = ((xb - xa) * (yd - yc) - (yb - ya) * (xd - xc));
		    
		    double rnum = ((ya - yc) * (xd - xc) - (xa - xc) * (yd - yc));
		    
			if (denom == 0.0) { // parallel
				if (rnum == 0.0) { // coincident; pick one end of first line
					if ((xa < xb && (xb < xc || xb < xd)) ||
						(xa > xb && (xb > xc || xb > xd))) {
							return new PointD (xb, yb);
					}
					else {
						return new PointD (xa, ya);
					}
				}
				else {
					return null;
				}
			}

			double r = rnum / denom;
			double snum = ((ya - yc) * (xb - xa) - (xa - xc) * (yb - ya));
			double s = snum / denom;
			
			if (0.0 <= r && r <= 1.0 && 0.0 <= s && s <= 1.0) {
				double px = (xa + (xb - xa) * r);
				double py = (ya + (yb - ya) * r);
				return new PointD (px, py);
			}
			else {
				return null;
			}
		}
	}
}
