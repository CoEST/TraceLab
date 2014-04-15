using System.Windows.Controls;
using TraceLabSDK;

namespace AdvSoftEng.Types
{
    /// <summary>
    /// Interaction logic for DocumentVectorCollection.xaml
    /// </summary>
    public partial class DocumentVectorCollectionEditor : UserControl, IWorkspaceUnitEditor
    {
        public DocumentVectorCollectionEditor()
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
