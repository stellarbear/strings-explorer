using Alphaleonis.Win32.Filesystem;
using ObjectExplorerWPF;
using StringsExplorer.Infrustructure;
using StringsExplorer.Infrustructure.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace StringsExplorer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class WExtractor : Window
    {
        private WSettings settings;
        public WExtractor()
        {
            InitializeComponent();
            this.MouseDown += MouseDown_Event;
            string[] args = Environment.GetCommandLineArgs();

            MAppVersion.Content = $"Strings Explorer: {Assembly.GetEntryAssembly().GetName().Version}";
            MStringseVersion.Content = $"stringssharp: {StringsSharp.StringsSharp.Version}";
            MMaterialVersion.Content = $"materialsharp: {MaterialSharp.MaterialSharp.Version}";
            MAuxiliaryVersion.Content = $"auxiliarysharp: {AuxiliarySharp.AuxiliarySharp.Version}";

            DataContext = new StringsViewModel((new SettingsViewModel()).Encodings, args.Count() >= 2 ? args[1] : default(string));
        }

        private void LoadSettings()
        {
            (DataContext as StringsViewModel).Encodings = (settings.DataContext as SettingsViewModel).Encodings;
        }

        private void Slider_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            (DataContext as StringsViewModel).ExtractedValuableStrings.UpdateStrings(Convert.ToInt32((sender as Slider).Value));
            (DataContext as StringsViewModel).ExtractedStrings.UpdateStrings(Convert.ToInt32((sender as Slider).Value));
        }

        private string _selectedValue;
        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid TypedSender = (sender as DataGrid);
            try
            {
                if (TypedSender.SelectedCells.Count > 0)
                {
                    _selectedValue = ((sender as DataGrid).SelectedCells[0].Item as StringInfo).Value;
                }
            }
            catch { }
        }
        public void MouseDown_Event(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                DragMove();
        }

        private void TriggerCommand(object sender, RoutedEventArgs e)
        {
            (sender as Button).Command.Execute((sender as Button).CommandParameter);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            AuxiliarySharp.IO.General.CopyToClipboard(_selectedValue);
        }

        private void DGStrings_DragEnter(object sender, DragEventArgs e)
        {
            /*if ((DataContext as StringsViewModel).StringsIsActive ||
                (DataContext as StringsViewModel).ValuableStringsIsActive)
                e.Effects = DragDropEffects.Link;
            else*/
            e.Effects = DragDropEffects.Copy;
        }

        private void DGStrings_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files =
                    (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files.Count() > 0)
                    (DataContext as StringsViewModel).ScanFile(files[0]);
            }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            UpdateWindowState();
        }
        private void UpdateWindowState()
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as StringsViewModel).ScanFile(OpenDialog());
        }
        private string OpenDialog()
        {
            List<string> ObservableDirs = new List<string>();

            ObservableDirs.AddRange(DriveInfo.GetDrives().
                Select(x => x.Name));

            WPFObjectExplorerWPF explorer = new WPFObjectExplorerWPF(ObservableDirs, ESelectionRestrictions.FileOnlySingle);
            explorer.Owner = this;
            explorer.ShowDialog();

            return explorer.SelectedObject;
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            settings = new WSettings();
            settings.Owner = this;
            settings.ShowDialog();

            LoadSettings();
        }

        private void PanelClick_Event(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if ((e.LeftButton == System.Windows.Input.MouseButtonState.Pressed) && (e.ClickCount >= 2))
                UpdateWindowState();
        }
    }
}
