using ServiceGUI.ViewModel;

namespace ServiceGUI.View
{
    /// <summary>
    /// Interaction logic for LogView.xaml
    /// </summary>
    public partial class LogView
    {
        
        public LogView()
        {
            InitializeComponent();
            LogViewModel ViewModel = new LogViewModel();
            this.DataContext = ViewModel;
        }
    }
}
