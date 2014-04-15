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
using System.Windows.Controls;
using System.Windows.Media;

namespace TraceLab.UI.WPF.Controls
{
    public class AngleBorder : Decorator
    {
        public static readonly DependencyProperty AngleAmountProperty;
        public static readonly DependencyProperty BorderBrushProperty;
        public static readonly DependencyProperty BackgroundProperty;
        public static readonly DependencyProperty BorderStrokeThicknessProperty;


        static AngleBorder()
        {
            const FrameworkPropertyMetadataOptions frameworkOptions = FrameworkPropertyMetadataOptions.AffectsRender | 
                                                                      FrameworkPropertyMetadataOptions.AffectsMeasure | 
                                                                      FrameworkPropertyMetadataOptions.AffectsArrange;

            AngleAmountProperty = DependencyProperty.Register("AngleAmount", typeof(Thickness),
                                                                             typeof(AngleBorder),
                                                                             new FrameworkPropertyMetadata(default(Thickness),
                                                                             frameworkOptions));

            BorderBrushProperty = DependencyProperty.Register("BorderBrush", typeof(Brush),
                                                                             typeof(AngleBorder),
                                                                             new FrameworkPropertyMetadata(System.Windows.Media.Brushes.Black,
                                                                             FrameworkPropertyMetadataOptions.AffectsRender));

            BackgroundProperty = DependencyProperty.Register("Background",   typeof(Brush),
                                                                             typeof(AngleBorder),
                                                                             new FrameworkPropertyMetadata(System.Windows.Media.Brushes.Transparent,
                                                                             FrameworkPropertyMetadataOptions.AffectsRender));

            BorderStrokeThicknessProperty = DependencyProperty.Register("BorderStrokeThickness", typeof(double),
                                                                                       typeof(AngleBorder),
                                                                                       new FrameworkPropertyMetadata(default(double),
                                                                                       frameworkOptions));
        }

        public Thickness AngleAmount
        {
            get { return (Thickness)GetValue(AngleAmountProperty); }
            set { SetValue(AngleAmountProperty, value); }
        }

        
        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public double BorderStrokeThickness
        {
            get { return (double)GetValue(BorderStrokeThicknessProperty); }
            set { SetValue(BorderStrokeThicknessProperty, value); }
        }

        private Rect GetChildRect(Size arrangeSize)
        {
            var extraSize = GetExtraSize();

            var childSize = new Size(Math.Round(arrangeSize.Width - extraSize.Width), 
                                     Math.Round(arrangeSize.Height - extraSize.Height));
            double left = Math.Round(AngleAmount.Left + BorderStrokeThickness);
            double top = Math.Round(AngleAmount.Top + BorderStrokeThickness);
            var childRect = new Rect(new Point(left, top), childSize);

            return childRect;
        }

        private Rect GetChildRect()
        {
            return GetChildRect(new Size(ActualWidth, ActualHeight));
        }

        private Size GetExtraSize()
        {
            double extraHeight = AngleAmount.Top + AngleAmount.Bottom + ((BorderStrokeThickness + BorderStrokeThickness));
            double extraWidth = AngleAmount.Left + AngleAmount.Right + ((BorderStrokeThickness + BorderStrokeThickness));

            return new Size(extraWidth, extraHeight);
        }

        protected override Size MeasureOverride(System.Windows.Size constraint)
        {
            Size childSize = base.MeasureOverride(constraint);
            if(Child != null)
                Child.Measure(constraint);

            var extraSize = GetExtraSize();
            childSize = new Size(childSize.Width + extraSize.Width, childSize.Height + extraSize.Height);

            return childSize;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            if(Child != null)
                Child.Arrange(GetChildRect(arrangeSize));
            return arrangeSize;
        }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var childRect = GetChildRect();
            double childMiddleY = childRect.Top + (childRect.Height / 2);
            double childMiddleX = childRect.Left + (childRect.Width / 2);

            var strokePen = new Pen(BorderBrush, BorderStrokeThickness);
            
            drawingContext.DrawGeometry(Background, strokePen, GetCombinedSides(childMiddleX, childMiddleY));
        }

        //private Geometry GetLeftSide(double childMiddleX, double childMiddleY)
        //{
        //    // Bottom-left corner
        //    var bottomLeftCorner = new Point(AngleAmount.Left + BorderStrokeThickness,
        //                                     ActualHeight - AngleAmount.Bottom - BorderStrokeThickness);

        //    var leftCenter = new Point(0 + BorderStrokeThickness, childMiddleY);

        //    var topLeftCorner = new Point(AngleAmount.Left + BorderStrokeThickness,
        //                                  AngleAmount.Top + BorderStrokeThickness);

        //    var leftSide = new PathFigure { StartPoint = bottomLeftCorner, IsClosed = false };
        //    leftSide.Segments.Add(new LineSegment(leftCenter, true));
        //    leftSide.Segments.Add(new LineSegment(topLeftCorner, true));
        //    var collection = new PathFigureCollection {leftSide};

        //    return new PathGeometry(collection);
        //}

        //private Geometry GetTopSide(double childMiddleX, double childMiddleY)
        //{
        //    var topLeftCorner = new Point(AngleAmount.Left + BorderStrokeThickness,
        //                                  AngleAmount.Top + BorderStrokeThickness);

        //    // Top-middle
        //    var topMiddle = new Point(childMiddleX, 0 + BorderStrokeThickness);

        //    // Top-right corner
        //    var topRightCorner = new Point(ActualWidth - AngleAmount.Right - BorderStrokeThickness,
        //                                   AngleAmount.Top + BorderStrokeThickness);

        //    var topSide = new PathFigure { StartPoint = topLeftCorner, IsClosed = false };
        //    topSide.Segments.Add(new LineSegment(topMiddle, true));
        //    topSide.Segments.Add(new LineSegment(topRightCorner, true));
        //    var collection = new PathFigureCollection {topSide};

        //    return new PathGeometry(collection);
        //}

        //private Geometry GetRightSide(double childMiddleX, double childMiddleY)
        //{
        //    // Top-right corner
        //    var topRightCorner = new Point(ActualWidth - AngleAmount.Right - BorderStrokeThickness,
        //                                   AngleAmount.Top + BorderStrokeThickness);

        //    // Middle, right side
        //    var middleRight = new Point(ActualWidth - BorderStrokeThickness, childMiddleY);

        //    // Bottom right corner
        //    var bottomRight = new Point(ActualWidth - AngleAmount.Right - BorderStrokeThickness,
        //                                ActualHeight - AngleAmount.Bottom - BorderStrokeThickness);

        //    var rightSide = new PathFigure { StartPoint = topRightCorner, IsClosed = false };
        //    rightSide.Segments.Add(new LineSegment(middleRight, true));
        //    rightSide.Segments.Add(new LineSegment(bottomRight, true));
        //    var collection = new PathFigureCollection { rightSide };

        //    return new PathGeometry(collection);
        //}

        //private Geometry GetBottomSide(double childMiddleX, double childMiddleY)
        //{
        //    // Bottom right corner
        //    var bottomRight = new Point(ActualWidth - AngleAmount.Right - BorderStrokeThickness,
        //                                ActualHeight - AngleAmount.Bottom - BorderStrokeThickness);

        //    // Bottom middle
        //    var bottomMiddle = new Point(childMiddleX,
        //                       ActualHeight - AngleAmount.Bottom - BorderStrokeThickness);

        //    // Bottom-left corner
        //    var bottomLeftCorner = new Point(AngleAmount.Left + BorderStrokeThickness,
        //                                     ActualHeight - AngleAmount.Bottom - BorderStrokeThickness);

        //    var bottomSide = new PathFigure { StartPoint = bottomRight, IsClosed = false };
        //    bottomSide.Segments.Add(new LineSegment(bottomMiddle, true));
        //    bottomSide.Segments.Add(new LineSegment(bottomLeftCorner, true));
        //    var collection = new PathFigureCollection { bottomSide };

        //    return new PathGeometry(collection);
        //}

        private Geometry GetCombinedSides(double childMiddleX, double childMiddleY)
        {
            // Bottom-left corner
            var bottomLeftCorner = new Point(AngleAmount.Left + BorderStrokeThickness,
                                             ActualHeight - AngleAmount.Bottom - BorderStrokeThickness);

            var leftCenter = new Point(0 + BorderStrokeThickness, childMiddleY);

            var topLeftCorner = new Point(AngleAmount.Left + BorderStrokeThickness,
                                          AngleAmount.Top + BorderStrokeThickness);

            // Top-middle
            var topMiddle = new Point(childMiddleX, 0 + BorderStrokeThickness);

            // Top-right corner
            var topRightCorner = new Point(ActualWidth - AngleAmount.Right - BorderStrokeThickness,
                                           AngleAmount.Top + BorderStrokeThickness);

            // Middle, right side
            var middleRight = new Point(ActualWidth - BorderStrokeThickness, childMiddleY);

            // Bottom right corner
            var bottomRight = new Point(ActualWidth - AngleAmount.Right - BorderStrokeThickness,
                                        ActualHeight - AngleAmount.Bottom - BorderStrokeThickness);

            // Bottom middle
            var bottomMiddle = new Point(childMiddleX,
                               ActualHeight - AngleAmount.Bottom - BorderStrokeThickness);

            var path = new PathFigure { StartPoint = bottomLeftCorner, IsClosed = true };
            path.Segments.Add(new LineSegment(leftCenter, true));
            path.Segments.Add(new LineSegment(topLeftCorner, true));
            path.Segments.Add(new LineSegment(topMiddle, true));
            path.Segments.Add(new LineSegment(topRightCorner, true));
            path.Segments.Add(new LineSegment(middleRight, true));
            path.Segments.Add(new LineSegment(bottomRight, true));
            path.Segments.Add(new LineSegment(bottomMiddle, true));

            var collection = new PathFigureCollection { path };
            return new PathGeometry(collection);
        }

    }
}
