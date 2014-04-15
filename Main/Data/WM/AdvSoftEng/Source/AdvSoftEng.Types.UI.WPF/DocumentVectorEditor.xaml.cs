using System.Windows.Controls;
using TraceLabSDK;

namespace AdvSoftEng.Types
{
    /// <summary>
    /// Interaction logic for DocumentVectorEditor.xaml
    /// </summary>
    public partial class DocumentVectorEditor : UserControl, IWorkspaceUnitEditor
    {
        public DocumentVectorEditor()
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
