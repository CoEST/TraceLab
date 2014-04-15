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

namespace ResultsVisualization.Charts
{
    /// <summary>
    /// Box Plot type of chart
    /// </summary>
    public class BoxPlot : Chart
    {
        private List<BoxData> boxes;

        public BoxPlot(string title, string description)
        {
            this.title = title;
            this.description = description;

            this.boxes = new List<BoxData>();

            this.chartStyle = new ChartStyle();
            this.chartStyle.DrawXGrid = false;
            this.chartStyle.DrawXTicks = false;
            this.chartStyle.Xmin = 0.0d;
            this.chartStyle.XtickInitDelta = 1.0d;
        }

        public bool AddBox(BoxSummaryData b, string tecnnique)
        {
            bool boxAdded = false;

            IEnumerable<BoxPlotPoint> points = b.Points;
            foreach (BoxPlotPoint p in points)
            {
                if (0.0d <= p.Min && p.Max <= 1.0d && p.Min <= p.Q1 && p.Q1 <= p.Median && 
                    p.Median <= p.Q3 && p.Q3 <= p.Max)
                {
                    BoxData box = new BoxData(p.Min, p.Q1, p.Median, p.Q3, p.Max);
                    box.Name = tecnnique;
                    box.Color = Chart.Colors[this.boxes.Count % Chart.Colors.Length];
                    this.boxes.Add(box);
                    boxAdded = true;
                }
            }

            return boxAdded;
        }

        public override void Draw(Canvas TextCanvas, Canvas ChartCanvas, Canvas LegendCanvas)
        {
            this.chartStyle.Xmax = this.boxes.Count + 1.0d;
            this.chartStyle.TextCanvas = TextCanvas;
            this.chartStyle.ChartCanvas = ChartCanvas;
            this.chartStyle.ApplyStyle();

            // Drawing each box 
            double width = (this.chartStyle.Xmax + this.chartStyle.Xmin) / (this.boxes.Count + 1);
            double x = width;

            foreach (BoxData b in this.boxes)
            {
                b.Draw(this.chartStyle, width, x);
                x += width;
            }

            this.DrawLegend(LegendCanvas);
        }

        public void DrawLegend(Canvas canvas)
        {
            TextBlock tb = new TextBlock();
            if (this.boxes.Count < 1)
            {
                return;
            }

            double legendWidth = 0.0d;
            Size size = new Size(0.0d, 0.0d);

            foreach (BoxData b in this.boxes)
            {
                tb = new TextBlock();
                tb.Text = b.Name;
                tb.Measure(new Size(Double.PositiveInfinity,
                                    Double.PositiveInfinity));
                size = tb.DesiredSize;
                if (legendWidth < size.Width)
                    legendWidth = size.Width;
            }

            legendWidth += 20.0d;
            canvas.Width = legendWidth + 5.0d;
            double legendHeight = 25.0d * this.boxes.Count;
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
            foreach (BoxData b in this.boxes)
            {
                double xText = 2.0d * sx + lineLength;
                double yText = n * sy + (2.0d * n - 1.0d) * textHeight / 2.0d;

                rect = new Rectangle();
                rect.Stroke = Brushes.Black;
                rect.StrokeThickness = this.strokeThickness;
                rect.Fill = b.Color;
                rect.Width = 10.0d;
                rect.Height = 10.0d;
                Canvas.SetLeft(rect, sx + lineLength / 2.0d - 15.0d);
                Canvas.SetTop(rect, yText - 2.0d);
                canvas.Children.Add(rect);

                tb = new TextBlock();
                tb.Text = b.Name;
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
