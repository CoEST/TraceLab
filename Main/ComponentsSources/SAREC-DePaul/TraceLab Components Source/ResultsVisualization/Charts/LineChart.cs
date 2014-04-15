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
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;

using TraceLabSDK.Types.Contests;

using TLPoint = TraceLabSDK.Types.Contests.Point;
using Point = System.Windows.Point;

namespace ResultsVisualization.Charts
{
    /// <summary>
    /// Line Series type of chart
    /// </summary>
    public class LineChart : Chart
    {
        private List<LineData> lines;

        public LineChart(string title, string description)
        {
            this.title = title;
            this.description = description;

            this.lines = new List<LineData>();

            this.chartStyle = new ChartStyle();
        }

        public bool AddLine(LineSeries l, string tecnnique)
        {
            LineData line = new LineData();
            line.Name = tecnnique;
            line.Color = Chart.Colors[this.lines.Count % Chart.Colors.Length];

            IEnumerable<TLPoint> points = l.Points;
            foreach (TLPoint p in points)
            {
                if (this.chartStyle.Xmin <= p.X && p.X <= this.chartStyle.Xmax &&
                    this.chartStyle.Ymin <= p.Y && p.Y <= this.chartStyle.Ymax)
                {
                    line.AddPoint(new Point(p.X, p.Y));
                }
            }

            bool lineAdded = line.Points.Count > 0;
            if (lineAdded)
            {
                this.lines.Add(line);
            }
            return lineAdded;
        }

        public override void Draw(Canvas TextCanvas, Canvas ChartCanvas, Canvas LegendCanvas)
        {
            this.chartStyle.TextCanvas = TextCanvas;
            this.chartStyle.ChartCanvas = ChartCanvas;
            this.chartStyle.ApplyStyle();

            // Drawing each line
            foreach (LineData l in this.lines)
            {
                l.Draw(this.chartStyle);
            }

            this.DrawLegend(LegendCanvas);
        }

        public void DrawLegend(Canvas canvas)
        {
            TextBlock tb = new TextBlock();
            if (this.lines.Count < 1)
                return;

            double legendWidth = 0.0d;
            Size size = new Size(0.0d, 0.0d);

            foreach (LineData l in this.lines)
            {
                tb = new TextBlock();
                tb.Text = l.Name;
                tb.Measure(new Size(Double.PositiveInfinity,
                                    Double.PositiveInfinity));
                size = tb.DesiredSize;
                if (legendWidth < size.Width)
                    legendWidth = size.Width;
            }

            legendWidth += 20.0d;
            canvas.Width = legendWidth + 5.0d;
            double legendHeight = 25.0d * this.lines.Count;
            double sx = 5.0d;
            double sy = 0.0d;
            double textHeight = size.Height;
            double lineLength = 34.0d;
            Rectangle legendRect = new Rectangle();
            legendRect.Stroke = Brushes.Black;
            legendRect.Fill = Brushes.White;
            legendRect.Width = legendWidth + 18.0d;
            legendRect.Height = legendHeight;

            canvas.Children.Add(legendRect);

            Rectangle rect;
            int n = 1;
            foreach (LineData l in this.lines)
            {
                double xText = 2.0d * sx + lineLength;
                double yText = n * sy + (2.0d * n - 1.0d) * textHeight / 2.0d;

                rect = new Rectangle();
                rect.Stroke = Brushes.Black;
                rect.StrokeThickness = this.strokeThickness;
                rect.Fill = l.Color;
                rect.Width = 10.0d;
                rect.Height = 10.0d;
                Canvas.SetLeft(rect, sx + lineLength / 2.0d - 15.0d);
                Canvas.SetTop(rect, yText - 2.0d);
                canvas.Children.Add(rect);

                tb = new TextBlock();
                tb.Text = l.Name;
                canvas.Children.Add(tb);
                Canvas.SetTop(tb, yText - size.Height / 2.0d + 3.0d);
                Canvas.SetLeft(tb, xText - 20.0d);
                n++;
            }

            canvas.Width = legendRect.Width;
            canvas.Height = legendRect.Height;
        }
    }
}
