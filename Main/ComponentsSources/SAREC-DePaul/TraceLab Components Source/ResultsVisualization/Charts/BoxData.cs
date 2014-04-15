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
using System.Windows.Shapes;

namespace ResultsVisualization.Charts
{
    /// <summary>
    /// Data structure for each box in a box plot
    /// </summary>
    public class BoxData
    {
        #region Members

        protected string name;
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        protected Brush color = Brushes.Black;
        public Brush Color
        {
            get { return this.color; }
            set { this.color = value; }
        }

        private double minimum;
        public double Minimum
        {
            get { return this.minimum; }
        }

        private double lowerQuartile;
        public double LowerQuartile
        {
            get { return this.lowerQuartile; }
        }

        private double median;
        public double Median
        {
            get { return this.median; }
        }

        private double upperQuartile;
        public double UpperQuartile
        {
            get { return this.upperQuartile; }
        }

        private double maximum;
        public double Maximum
        {
            get { return this.maximum; }
        }

        #endregion

        public BoxData(double min, double lq, double med, double uq, double max)
        {
            this.minimum = min;
            this.lowerQuartile = lq;
            this.median = med;
            this.upperQuartile = uq;
            this.maximum = max;
        }

        public void Draw(ChartStyle chart, double width, double x)
        {
            double thirdWidth = width * 0.34d;
            double quarterWidth = width * 0.25d;
            double strokeThickness = 2.0d;

            Point pMinL = chart.NormalizePoint(new Point(x - quarterWidth, this.minimum));
            Point pMinR = chart.NormalizePoint(new Point(x + quarterWidth, this.minimum));
            Point pLQ = chart.NormalizePoint(new Point(x, this.lowerQuartile));
            Point pMedL = chart.NormalizePoint(new Point(x - thirdWidth, this.median));
            Point pMedR = chart.NormalizePoint(new Point(x + thirdWidth, this.median));
            Point pUQ = chart.NormalizePoint(new Point(x, this.upperQuartile));
            Point pMax = chart.NormalizePoint(new Point(x, this.maximum));

            // Box
            Polygon plg = new Polygon();
            plg.Fill = this.color;
            plg.Stroke = this.color;
            plg.StrokeThickness = strokeThickness;
            plg.Points.Add(new Point(pMedL.X, pLQ.Y));
            plg.Points.Add(new Point(pMedR.X, pLQ.Y));
            plg.Points.Add(new Point(pMedR.X, pUQ.Y));
            plg.Points.Add(new Point(pMedL.X, pUQ.Y));


            TextBlock tBlock = new TextBlock();
            tBlock.TextWrapping = TextWrapping.Wrap;
            tBlock.Width = 80.0d;
            tBlock.Text = "Max: " + Math.Round(this.maximum, 3).ToString() + "\n" +
                            "Q3: " + Math.Round(this.upperQuartile, 3).ToString() + "\n" +
                            "Median: " + Math.Round(this.median, 3).ToString() + "\n" +
                            "Q1: " + Math.Round(this.lowerQuartile, 3).ToString() + "\n" +
                            "Min: " + Math.Round(this.minimum, 3).ToString();

            ToolTip tTip = new ToolTip();
            tTip.Content = tBlock;
            plg.ToolTip = tTip;


            // Bottom Whiskers
            Line lBottomH = new Line();
            lBottomH.Stroke = this.color;
            lBottomH.StrokeThickness = strokeThickness;
            lBottomH.X1 = pMinL.X;
            lBottomH.Y1 = pMinL.Y;
            lBottomH.X2 = pMinR.X;
            lBottomH.Y2 = pMinR.Y;
            lBottomH.ToolTip = tTip;

            Line lBottomV = new Line();
            lBottomV.Stroke = this.color;
            lBottomV.StrokeThickness = strokeThickness;
            lBottomV.X1 = pLQ.X;
            lBottomV.Y1 = pMinL.Y;
            lBottomV.X2 = pLQ.X;
            lBottomV.Y2 = pLQ.Y;
            lBottomV.ToolTip = tTip;

            // Top Whiskers
            Line lTopH = new Line();
            lTopH.Stroke = this.color;
            lTopH.StrokeThickness = strokeThickness;
            lTopH.X1 = pMinL.X;
            lTopH.Y1 = pMax.Y;
            lTopH.X2 = pMinR.X;
            lTopH.Y2 = pMax.Y;
            lTopH.ToolTip = tTip;

            Line lTopV = new Line();
            lTopV.Stroke = this.color;
            lTopV.StrokeThickness = strokeThickness;
            lTopV.X1 = pMax.X;
            lTopV.Y1 = pMax.Y;
            lTopV.X2 = pUQ.X;
            lTopV.Y2 = pUQ.Y;
            lTopV.ToolTip = tTip;

            // Median line
            Line lMedian = new Line();
            lMedian.Stroke = Brushes.Black;
            lMedian.StrokeThickness = strokeThickness;
            lMedian.X1 = pMedL.X;
            lMedian.Y1 = pMedL.Y;
            lMedian.X2 = pMedR.X;
            lMedian.Y2 = pMedR.Y;

            chart.ChartCanvas.Children.Add(plg);
            chart.ChartCanvas.Children.Add(lBottomV);
            chart.ChartCanvas.Children.Add(lTopV);
            chart.ChartCanvas.Children.Add(lBottomH);
            chart.ChartCanvas.Children.Add(lTopH);
            chart.ChartCanvas.Children.Add(lMedian);
        }
    }
}
