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
// along with this program. If not, see<http://www.gnu.org/licenses/>.

using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ResultsVisualization.Charts
{
    /// <summary>
    /// Data structure for chart settings
    /// </summary>
    public class ChartStyle
    {
        #region X-Axis Parameters

        private double xmin = -0.05d;
        public double Xmin
        {
            get { return this.xmin; }
            set { this.xmin = value; }
        }

        private double xmax = 1.05d;
        public double Xmax
        {
            get { return this.xmax; }
            set { this.xmax = value; }
        }

        private double xtickDelta = 0.1d;
        public double XtickDelta
        {
            get { return this.xtickDelta; }
            set { this.xtickDelta = value; }
        }

        private double xtickInitDelta = 0.05d;
        public double XtickInitDelta
        {
            get { return this.xtickInitDelta; }
            set { this.xtickInitDelta = value; }
        }

        private bool drawXGrid = true;
        public bool DrawXGrid
        {
            get { return this.drawXGrid; }
            set { this.drawXGrid = value; }
        }

        private bool drawXTicks = true;
        public bool DrawXTicks
        {
            get { return this.drawXTicks; }
            set { this.drawXTicks = value; }
        }

        #endregion

        #region Y-Axis Parameters

        private double ymin = -0.05d;
        public double Ymin
        {
            get { return this.ymin; }
            set { this.ymin = value; }
        }

        private double ymax = 1.05d;
        public double Ymax
        {
            get { return this.ymax; }
            set { this.ymax = value; }
        }

        private double ytickDelta = 0.1d;
        public double YtickDelta
        {
            get { return this.ytickDelta; }
            set { this.ytickDelta = value; }
        }

        private double ytickInitDelta = 0.05d;
        public double YtickInitDelta
        {
            get { return this.ytickInitDelta; }
            set { this.ytickInitDelta = value; }
        }

        private bool drawYGrid = true;
        public bool DrawYGrid
        {
            get { return this.drawYGrid; }
            set { this.drawYGrid = value; }
        }

        private bool drawYTicks = true;
        public bool DrawYTicks
        {
            get { return this.drawYTicks; }
            set { this.drawYTicks = value; }
        }

        #endregion

        #region Drawing Variables

        private Canvas chartCanvas;
        public Canvas ChartCanvas
        {
            get { return this.chartCanvas; }
            set { this.chartCanvas = value; }
        }

        private Canvas textCanvas;
        public Canvas TextCanvas
        {
            get { return this.textCanvas; }
            set { this.textCanvas = value; }
        }

        private Brush lineColor = Brushes.Black;
        public Brush LineColor
        {
            get { return this.lineColor; }
            set { this.lineColor = value; }
        }

        private double gridLineThickness = 1.0d;
        public double GridLineThickness
        {
            get { return this.gridLineThickness; }
            set { this.gridLineThickness = value; }
        }

        private LinePatternEnum gridLinePattern = LinePatternEnum.Dot;
        public LinePatternEnum GridLinePattern
        {
            get { return this.gridLinePattern; }
            set { this.gridLinePattern = value; }
        }

        private double tickSize = 5.0d;
        public double TickSize
        {
            get { return this.tickSize; }
            set { this.tickSize = value; }
        }

        private double tickLineThickness = 2.0d;
        public double TickLineThickness
        {
            get { return this.tickLineThickness; }
            set { this.tickLineThickness = value; }
        }

        private double leftOffset = 40.0d;
        private double bottomOffset = 25.0d;
        private double rightOffset = 5.0d;

        #endregion

        public ChartStyle()
        {
        }

        /// <summary>
        /// Converts given point to local chart coordinates
        /// </summary>
        public Point NormalizePoint(Point pt)
        {
            if (this.chartCanvas.Width.ToString() == "NaN")
            {
                this.chartCanvas.Width = 200.0d;
            }
            if (this.chartCanvas.Height.ToString() == "NaN")
            {
                this.chartCanvas.Height = 200.0d;
            }

            return new Point( (pt.X - this.xmin) * this.chartCanvas.Width / (this.xmax - this.xmin),
                                this.chartCanvas.Height - (pt.Y - this.ymin) * this.chartCanvas.Height / (this.ymax - this.ymin) );
        }

        /// <summary>
        /// Draws the chart without data, just the frame
        /// </summary>
        public void ApplyStyle()
        {
            double tolerance = 0.001d;
            Point pt;
            Line gridLine, tick;
            double dx, dy;
            TextBlock tb = new TextBlock();
            Size size = new Size();

            Canvas.SetLeft(this.chartCanvas, leftOffset);
            Canvas.SetBottom(this.chartCanvas, bottomOffset);
            this.chartCanvas.Width = Math.Abs(this.textCanvas.Width - leftOffset - rightOffset);
            this.chartCanvas.Height = Math.Abs(this.textCanvas.Height - bottomOffset);

            Rectangle chartRect = new Rectangle();
            chartRect.Stroke = Brushes.Black;
            chartRect.Fill = Brushes.White;
            chartRect.Width = this.chartCanvas.Width;
            chartRect.Height = this.chartCanvas.Height;
            this.chartCanvas.Children.Add(chartRect);

            // Create vertical gridlines: 
            if (this.drawXGrid)
            {
                for (dx = this.xmin + this.xtickInitDelta; dx + tolerance < this.xmax; dx += this.xtickDelta)
                {
                    gridLine = ChartsHelper.CreateLine(this.lineColor, this.gridLineThickness, this.gridLinePattern);
                    pt = this.NormalizePoint(new Point(dx, this.ymin));
                    gridLine.X1 = pt.X;
                    gridLine.Y1 = pt.Y;
                    pt = this.NormalizePoint(new Point(dx, this.ymax));
                    gridLine.X2 = pt.X;
                    gridLine.Y2 = pt.Y;
                    this.chartCanvas.Children.Add(gridLine);
                }
            }

            // Create horizontal gridlines: 
            if (this.drawYGrid)
            {
                for (dy = this.ymin + this.ytickInitDelta; dy + tolerance < this.ymax; dy += this.ytickDelta)
                {
                    gridLine = ChartsHelper.CreateLine(this.lineColor, this.gridLineThickness, this.gridLinePattern);
                    pt = this.NormalizePoint(new Point(this.xmin, dy));
                    gridLine.X1 = pt.X;
                    gridLine.Y1 = pt.Y;
                    pt = this.NormalizePoint(new Point(this.xmax, dy));
                    gridLine.X2 = pt.X;
                    gridLine.Y2 = pt.Y;
                    this.chartCanvas.Children.Add(gridLine);
                }
            }

            // Create x-axis tick marks
            if (this.drawXTicks)
            {
                for (dx = this.xmin + this.xtickInitDelta; dx + tolerance < this.xmax; dx += this.xtickDelta)
                {
                    pt = NormalizePoint(new Point(dx, this.ymin));
                    tick = ChartsHelper.CreateLine(this.lineColor, this.tickLineThickness, LinePatternEnum.Solid);
                    tick.X1 = pt.X;
                    tick.Y1 = pt.Y;
                    tick.X2 = pt.X;
                    tick.Y2 = pt.Y - this.tickSize;
                    this.chartCanvas.Children.Add(tick);

                    tb = new TextBlock();
                    tb.Text = Math.Round(dx, 3).ToString();
                    tb.Measure(new Size(Double.PositiveInfinity,
                                        Double.PositiveInfinity));
                    size = tb.DesiredSize;
                    this.textCanvas.Children.Add(tb);
                    Canvas.SetLeft(tb, this.leftOffset + pt.X - size.Width / 2.0d);
                    Canvas.SetTop(tb, pt.Y + size.Height / 2.0d);
                }
            }

            // Create y-axis tick marks
            if (this.drawYTicks)
            {
                for (dy = this.ymin + this.ytickInitDelta; dy + tolerance < this.ymax; dy += this.ytickDelta)
                {
                    pt = NormalizePoint(new Point(this.xmin, dy));
                    tick = ChartsHelper.CreateLine(this.lineColor, this.tickLineThickness, LinePatternEnum.Solid);
                    tick.X1 = pt.X;
                    tick.Y1 = pt.Y;
                    tick.X2 = pt.X + this.tickSize;
                    tick.Y2 = pt.Y;
                    this.chartCanvas.Children.Add(tick);

                    tb = new TextBlock();
                    tb.Text = Math.Round(dy, 3).ToString();
                    tb.Measure(new Size(Double.PositiveInfinity,
                                        Double.PositiveInfinity));
                    size = tb.DesiredSize;
                    this.textCanvas.Children.Add(tb);
                    Canvas.SetRight(tb, this.chartCanvas.Width + 10.0d);
                    Canvas.SetTop(tb, pt.Y - size.Height / 2.0d);
                }
            }
        }
    }
}
