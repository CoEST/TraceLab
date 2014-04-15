using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SEMERU.Types.Metrics;
using ZedGraph;

namespace SEMERU.UI.BoxPlotGUI.BoxPlotUserControl
{
    public partial class MultiCurveGraph : UserControl
    {
        public MultiCurveGraph()
        {
            InitializeComponent();
        }

        private void MultiCurveGraph_Load(object sender, EventArgs e)
        {
            //Create a new instance of the random class
            random = new Random();
        }
        
        public void RefreshData(PrecisionRecallCurveMetric data)
        {
            //Background color
            this.BackColor = Color.FromArgb(173, 255, 47);

            GraphPane pane = zedGraphControl1.GraphPane;
            
            pane.XAxis.Title.Text = "Recall";
            pane.YAxis.Title.Text = "Precision";
            pane.Title.Text = "Precision vs. Recall";

            List<string> titles = new List<string>();

            //reset the color index
            currentColorIndex = 0;

            //iterate over all given techniques
            foreach (Series series in data.Series)
            {
                titles.Add(series.TechniqueName);

                List<PrecisionRecallPoint> points = series.DataPoints;

                //Put in data
                PointPairList ppl = new PointPairList();
                foreach (PrecisionRecallPoint point in points)
                {
                    ppl.Add(point.recall, point.precision);
                }

                SymbolType st = SymbolType.None;
                //if (currentColorIndex == 0)
                //{
                //    st = SymbolType.Circle;
                //}
                //else
                //{
                //    st = SymbolType.Diamond;
                //}

                Color color = GetNextColor();

                LineItem curve = pane.AddCurve(series.TechniqueName, ppl, color, st);
                curve.Line.Width = 2.0F;
                //curve.Symbol.Fill = new Fill(Color.White);
                
            }
            

            this.zedGraphControl1.AxisChange();
        }

        #region CurveColors

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
