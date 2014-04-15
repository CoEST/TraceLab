using System.Windows.Controls;
using TraceLabSDK;

namespace AdvSoftEng.Types
{
    /// <summary>
    /// Interaction logic for DocumentVectorCollection.xaml
    /// </summary>
    public partial class NormalizedVectorCollectionEditor : UserControl, IWorkspaceUnitEditor
    {
        public NormalizedVectorCollectionEditor()
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
