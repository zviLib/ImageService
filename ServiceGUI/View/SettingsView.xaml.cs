using ServiceGUI.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ServiceGUI.View
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView
    {
        private ISettingsViewModel svm;

        public SettingsView()
        {
            InitializeComponent();
            svm = new SettingsViewModel();
            svm.UpdateAppConfig();
            this.DataContext = svm;
        }

        /// <summary>
        /// remove selected handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string s = (string)list.SelectedItem;
            svm.CloseDirectory(s);
        }
    }
}
