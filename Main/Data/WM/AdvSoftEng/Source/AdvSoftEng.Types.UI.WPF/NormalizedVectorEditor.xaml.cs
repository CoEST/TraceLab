using System.Windows.Controls;
using TraceLabSDK;

namespace AdvSoftEng.Types
{
    /// <summary>
    /// Interaction logic for DocumentVectorEditor.xaml
    /// </summary>
    public partial class NormalizedVectorEditor : UserControl, IWorkspaceUnitEditor
    {
        public NormalizedVectorEditor()
        {
            InitializeComponent();
        }

        public object Data
        {
            get
            {
                return DataContext;
            }
            set
            {
                DataContext = value;
            }
        }
    }
}