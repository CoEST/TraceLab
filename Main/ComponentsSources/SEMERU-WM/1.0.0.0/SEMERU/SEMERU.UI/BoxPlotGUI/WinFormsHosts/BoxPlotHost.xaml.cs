using System.Windows;
using System.Windows.Controls;
using SEMERU.Types.Metrics;

namespace SEMERU.UI.BoxPlotGUI.WinFormsHosts
{
    /// <summary>
    /// Interaction logic for BoxPlotHost.xaml
    /// </summary>
    public partial class BoxPlotHost : UserControl
    {
        public BoxPlotHost()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty BoxPlotDataProperty = DependencyProperty.Register("BoxPlotData", typeof(SummaryDataMetric), typeof(BoxPlotHost), new FrameworkPropertyMetadata(OnBoxPlotDataChanged));

        private static void OnBoxPlotDataChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var boxPlotHost = (BoxPlotHost)sender;
            boxPlotHost.RefreshBoxPlot();
        }

        public SummaryDataMetric BoxPlotData
        {
            get { return (SummaryDataMetric)GetValue(BoxPlotDataProperty); }
            set { SetValue(BoxPlotDataProperty, value); }
        }
        
        private void wfh_Loaded(object sender, RoutedEventArgs e)
        {
            var host = (System.Windows.Forms.Integration.WindowsFormsHost)sender;
            host.Child = m_boxPlot;
        }

        private BoxPlotUserControl.SummaryDataMetricBoxPlot m_boxPlot = new BoxPlotUserControl.SummaryDataMetricBoxPlot();
        
        public void RefreshBoxPlot() 
        {
            if (BoxPlotData != null)
            {
                m_boxPlot.RefreshData(BoxPlotData);
            }
        }
    }
}
