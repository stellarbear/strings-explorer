using StringsExplorer.Infrustructure;
using System.Windows;
using System.Windows.Controls;

namespace StringsExplorer
{
    /// <summary>
    /// Логика взаимодействия для WSettings.xaml
    /// </summary>
    public partial class WSettings : Window
    {
        public WSettings()
        {
            InitializeComponent();

            DataContext = new SettingsViewModel();
        }

        private void ForceSelectEncodingItem_Event(object sender, RoutedEventArgs e)
        {
            ((ListBoxItem)EncodingListBox.ItemContainerGenerator.ContainerFromItem((e.OriginalSource as FrameworkElement)?.DataContext)).IsSelected = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (DataContext as SettingsViewModel).Save();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
