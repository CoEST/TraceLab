using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SEMERU.Types.Metrics;
using ZedGraph;

namespace SEMERU.UI.BoxPlotGUI.BoxPlotUserControl
{
    public partial class SummaryDataMetricBoxPlot : UserControl
    {
        public List<List<PointPairList>> masterList;
        
        public Color[] masterColorList = new Color[4] { Color.LightSkyBlue, Color.Crimson, Color.Chartreuse, Color.LightGray };

        public SummaryDataMetricBoxPlot()
        {
            //Create a new instance of the random class
            random = new Random();
            InitializeComponent();
        }

        private SummaryDataMetric m_data;

        public void RefreshData(SummaryDataMetric data)
        {
            //reset color
            currentColorIndex = 0;
            GraphPane myPane = zedGraphControl1.GraphPane;

            //the data is the dictionary of techniques names and their SummaryData 
            m_data = data;

            //GRAPH
            myPane.CurveList.Clear();
            
            PrepareTableData();

            BoxPlotGraph();

            this.testLabel.Visible = true;
            this.pvaluelabel.Visible = true;
            this.pvalueTextBox.Visible = true;
            this.testLabel.Text = m_data.NameOfPValueTest;
            this.pvalueTextBox.Text = String.Format("{0:0.0000}", m_data.PValue);
            
            //Set up the chart
            myPane.BarSettings.Type = BarType.Overlay;
            myPane.XAxis.IsVisible = false;
            myPane.XAxis.MinSpace = 10;
            myPane.XAxis.Scale.Min = -1;
            myPane.XAxis.Scale.Max = data.SummaryData.Count;
            //p.XAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.Scale.FontSpec.Size = 20;
            //myPane.YAxis.Scale.MinGrace = 0.1;
            //myPane.YAxis.Scale.MaxGrace = 0.1;
            myPane.YAxis.Title.Text = "";

            myPane.Legend.IsVisible = true;
            myPane.Legend.FontSpec.Size = 20; 
            myPane.Title.Text = data.MetricName;
            myPane.Title.FontSpec.Size = 20;
            //myPane.BarSettings.MinBarGap = 1.0f;
            myPane.BarSettings.ClusterScaleWidthAuto = true;


            zedGraphControl1.GraphPane.AxisChange();

            zedGraphControl1.Refresh();
        }

        #region Box Plot Graph
        
        private void BoxPlotGraph()
        {
            masterList = new List<List<PointPairList>>();

            int i = 0;
            foreach(SummaryData summary in m_data.SummaryData)
            {
                double[] dataPoints = summary.DataPoints;

                masterList.Add(new List<PointPairList>());
                //median of each array
                masterList[i].Add(new PointPairList());
                //75th and 25th percentile, defines the box
                masterList[i].Add(new PointPairList());
                //+/- 1.5*Interquartile range, extentent of wiskers
                masterList[i].Add(new PointPairList());
                //outliers
                masterList[i].Add(new PointPairList());
                //Add the values
                masterList[i][0].Add(i, percentile(dataPoints, 50));
                masterList[i][1].Add(i, percentile(dataPoints, 75), percentile(dataPoints, 25));
                double iqr = 1.5 * (percentile(dataPoints, 75) - percentile(dataPoints, 25));
                double upperLimit = percentile(dataPoints, 75) + iqr;
                double lowerLimit = percentile(dataPoints, 25) - iqr;
                //The wiskers must end on an actual data point
                masterList[i][2].Add(i, ValueNearestButGreater(dataPoints, lowerLimit), ValueNearestButLess(dataPoints, upperLimit));
                //Sort out the outliers
                foreach (double aValue in dataPoints)
                {
                    if (aValue > upperLimit)
                    {
                        masterList[i][3].Add(i, aValue);
                    }
                    if (aValue < lowerLimit)
                    {
                        masterList[i][3].Add(i, aValue);
                    }
                }

                i++;
            }
            plotBox(-1);
        }

        public void plotBox(int colorIndex)
        {
            GraphPane myPane = zedGraphControl1.GraphPane;

            int index = 0;
            foreach (SummaryData summary in m_data.SummaryData) 
            {
                //Plot the items, first the median values
                CurveItem meadian = myPane.AddCurve("", masterList[index][0], Color.Black, SymbolType.HDash);
                LineItem myLine = (LineItem)meadian;
                myLine.Line.IsVisible = false;
                myLine.Symbol.Size = 25;
                myLine.Symbol.Fill.Type = FillType.Solid;
               
                //Box
                HiLowBarItem myCurve = myPane.AddHiLowBar(summary.SeriesName, masterList[index][1], GetNextColor());
                myCurve.Bar.Fill.Type = FillType.Solid;
                
                //Wiskers
                ErrorBarItem myerror = myPane.AddErrorBar("", masterList[index][2], Color.Black);
                //Outliers
                CurveItem upper = myPane.AddCurve("", masterList[index][3], Color.Black, SymbolType.Circle);
                LineItem bLine = (LineItem)upper;
                bLine.Symbol.Size = 3;
                bLine.Line.IsVisible = false;

                index++;
            }
        }

        private double ValueNearestButLess(double[] data, double number)
        {
            var lowNums = from n in data where n <= number select n;
            return lowNums.Max();
        }

        private double ValueNearestButGreater(double[] data, double number)
        {
            var lowNums = from n in data where n >= number select n;
            return lowNums.Min();
        }
        public double percentile(double[] data, int percentile)
        {
            Array.Sort(data);
            int numberOfValues = data.Length;
            double i = 0.5 + ((numberOfValues * (percentile * 1.0)) / 100);
            int whole = (int)Math.Floor(i);
            double frac = i - whole;
            if (frac == 0)
            {
                return data[whole - 1];
            }
            else
            {
                return data[whole - 1] * (1 - frac) + frac * data[whole];
            }
        }

        #endregion

        #region Data Grid 

        /// <summary>
        /// Methods formats data into grid table.
        /// </summary>
        private void PrepareTableData()
        {
            //TABLE
            DataTable table = new DataTable("Summary Data");

            object[] minima = new object[m_data.SummaryData.Count + 1]; // + 1 for header column
            object[] q1 = new object[m_data.SummaryData.Count + 1];
            object[] medians = new object[m_data.SummaryData.Count + 1];
            object[] means = new object[m_data.SummaryData.Count + 1];
            object[] q3 = new object[m_data.SummaryData.Count + 1];
            object[] maxima = new object[m_data.SummaryData.Count + 1];
            object[] stdev = new object[m_data.SummaryData.Count + 1];
            object[] n = new object[m_data.SummaryData.Count + 1];

            //row headers
            table.Columns.Add(" ", typeof(string));
            minima[0] = "Min";
            q1[0] = "Q1";
            medians[0] = "Median";
            means[0] = "Mean";
            q3[0] = "Q3";
            maxima[0] = "Max";
            stdev[0] = "Std.Dev";
            n[0] = "Data points";

            //add data columns
            int i = 1;
            foreach (SummaryData summary in m_data.SummaryData)
            {
                //create column for the technique with the given name
                DataColumn column = new DataColumn(summary.SeriesName, typeof(double));
                table.Columns.Add(column);
                
                minima[i] = String.Format("{0:0.00}", summary.Min);
                q1[i] = String.Format("{0:0.00}", summary.Q1);
                medians[i] = String.Format("{0:0.00}", summary.Median);
                means[i] = String.Format("{0:0.00}", summary.Mean);
                q3[i] = String.Format("{0:0.00}", summary.Q3);
                maxima[i] = String.Format("{0:0.00}", summary.Max);
                stdev[i] = String.Format("{0:0.00}", summary.StdDev);
                n[i] = String.Format("{0:0.00}", summary.N);

                i++;
            }

            var row = table.NewRow();
            row.ItemArray = minima;
            table.Rows.Add(row);

            row = table.NewRow();
            row.ItemArray = q1;
            table.Rows.Add(row);

            row = table.NewRow();
            row.ItemArray = medians;
            table.Rows.Add(row);

            row = table.NewRow();
            row.ItemArray = means;
            table.Rows.Add(row);

            row = table.NewRow();
            row.ItemArray = q3;
            table.Rows.Add(row);

            row = table.NewRow();
            row.ItemArray = maxima;
            table.Rows.Add(row);

            row = table.NewRow();
            row.ItemArray = stdev;
            table.Rows.Add(row);

            row = table.NewRow();
            row.ItemArray = n;
            table.Rows.Add(row);
           
            this.dataGridView1.DataSource = table.DefaultView;
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;

        }

        #endregion

        #region Colors

        private Random random;

        //private Color[] colors = new Color[] { Color.FromArgb(100, 100, 100), Color.FromArgb(180, 180, 180), Color.PaleVioletRed, Color.AliceBlue, Color.Azure };
        private Color[] colors = new Color[] { Color.LimeGreen, Color.Tomato, Color.PaleVioletRed, Color.AliceBlue, Color.Azure };
        private int currentColorIndex = 0;
        private Color GetNextColor()
        {
            Color color;
            if (currentColorIndex < colors.Length)
            {
                //get defined color
                color = colors[currentColorIndex];
                currentColorIndex++;
            }
            else
            {
                //if there are more curves than defined colors, generate random color
                color = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
            }
            return color;
        }

        #endregion
    }
}
