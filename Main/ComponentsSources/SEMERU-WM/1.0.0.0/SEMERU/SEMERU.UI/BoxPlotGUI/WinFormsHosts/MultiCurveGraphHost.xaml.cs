using System.Windows;
using System.Windows.Controls;
using SEMERU.Types.Metrics;

namespace SEMERU.UI.BoxPlotGUI.WinFormsHosts
{
    /// <summary>
    /// Interaction logic for MultiCurveGraphHost.xaml
    /// </summary>
    public partial class MultiCurveGraphHost : UserControl
    {
        public MultiCurveGraphHost()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(PrecisionRecallCurveMetric), typeof(MultiCurveGraphHost), new FrameworkPropertyMetadata(OnDataChanged));

        private static void OnDataChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var plotHost = (MultiCurveGraphHost)sender;
            plotHost.RefreshPlot();
        }

        public PrecisionRecallCurveMetric Data
        {
            get { return (PrecisionRecallCurveMetric)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        private void wfh_Loaded(object sender, RoutedEventArgs e)
        {
            var host = (System.Windows.Forms.Integration.WindowsFormsHost)sender;
            host.Child = m_plot;
        }

        private SEMERU.UI.BoxPlotGUI.BoxPlotUserControl.MultiCurveGraph m_plot = new SEMERU.UI.BoxPlotGUI.BoxPlotUserControl.MultiCurveGraph();

        public void RefreshPlot()
        {
            if (Data != null)
            {
                m_plot.RefreshData(Data);
            }
        }
    }
}
