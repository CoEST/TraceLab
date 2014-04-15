using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SEMERU.UI.BoxPlotGUI
{
    /// <summary>
    /// Interaction logic for EvaluationResultsView.xaml
    /// </summary>
    public partial class EvaluationResultsView : Window
    {
        public EvaluationResultsView(object data)
        {
            InitializeComponent();
            DataContext = data;
        }
    }
}
