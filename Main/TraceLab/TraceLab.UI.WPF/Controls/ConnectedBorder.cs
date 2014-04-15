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

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TraceLab.UI.WPF.Utilities;

namespace TraceLab.UI.WPF.Controls
{
    class ConnectedBorder : Decorator
    {
        public static readonly DependencyProperty ForegroundLinesProperty;
        public static readonly DependencyProperty BackgroundLinesProperty;
        public static readonly DependencyProperty LineStrokeThicknessProperty;
        public static readonly DependencyProperty OriginElementProperty;

        static ConnectedBorder()
        {
            const FrameworkPropertyMetadataOptions frameworkOptions = FrameworkPropertyMetadataOptions.AffectsRender |
                                                          FrameworkPropertyMetadataOptions.AffectsMeasure |
                                                          FrameworkPropertyMetadataOptions.AffectsArrange;

            ForegroundLinesProperty = DependencyProperty.Register("ForegroundLines", typeof(Brush),
                                                                typeof(ConnectedBorder),
                                                                new FrameworkPropertyMetadata(System.Windows.Media.Brushes.Black,
                                                                FrameworkPropertyMetadataOptions.AffectsRender));

            BackgroundLinesProperty = DependencyProperty.Register("BackgroundLines", typeof(Brush),
                                                                typeof(ConnectedBorder),
                                                                new FrameworkPropertyMetadata(System.Windows.Media.Brushes.Black,
                                                                FrameworkPropertyMetadataOptions.AffectsRender));

            LineStrokeThicknessProperty = DependencyProperty.Register("LineStrokeThickness", typeof(double),
                                                                typeof(ConnectedBorder),
                                                                new FrameworkPropertyMetadata(default(double),
                                                                frameworkOptions));

            OriginElementProperty = DependencyProperty.Register("OriginElement", typeof(UIElement),
                                                                typeof(ConnectedBorder),
                                                                new FrameworkPropertyMetadata(default(UIElement),
                                                                frameworkOptions));
        }

        public Brush ForegroundLines
        {
            get { return (Brush)GetValue(ForegroundLinesProperty); }
            set { SetValue(ForegroundLinesProperty, value); }
        }

        public Brush BackgroundLines
        {
            get { return (Brush)GetValue(BackgroundLinesProperty); }
            set { SetValue(BackgroundLinesProperty, value); }
        }

        public double LineStrokeThickness
        {
            get { return (double)GetValue(LineStrokeThicknessProperty); }
            set { SetValue(LineStrokeThicknessProperty, value); }
        }

        public UIElement OriginElement
        {
            get { return (UIElement)GetValue(OriginElementProperty); }
            set { SetValue(OriginElementProperty, value); }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var element = OriginElement;
            if (element != null)
            {
                var mainWindow = this.GetParent<TraceLab.UI.WPF.Views.DockableGraph>(null);

                var gt = element.TransformToVisual(this);
                var other = gt.Transform(new Point(0, 0));

                var oriElementLocation = element.TranslatePoint(new Point(0, 0), mainWindow);
                var originElementLocation = mainWindow.TranslatePoint(oriElementLocation, this);
                var targetLocation = new Point(0,0);// this.TranslatePoint(new Point(0, 0), mainWindow);

                Rect originRect = new Rect(originElementLocation, element.RenderSize);
                Rect targetRect = new Rect(targetLocation, RenderSize);

                Pen solidPen = new Pen(ForegroundLines, LineStrokeThickness);
                Pen dashedPen = new Pen(BackgroundLines, LineStrokeThickness);
                dashedPen.DashStyle = new DashStyle(new List<double>() { 2.0, 1.0 }, 1.0);

                drawingContext.DrawLine(solidPen, targetRect.TopLeft, originRect.TopLeft);
                drawingContext.DrawLine(solidPen, targetRect.TopRight, originRect.TopRight);
                drawingContext.DrawLine(dashedPen, targetRect.BottomLeft, originRect.BottomLeft);
                drawingContext.DrawLine(dashedPen, targetRect.BottomRight, originRect.BottomRight);
            }
        }
    }
}
