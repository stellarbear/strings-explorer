using Alphaleonis.Win32.Filesystem;
using StringsExplorer.Infrustructure.Extractor;
using System.Collections.ObjectModel;

namespace StringsExplorer.Infrustructure
{
    public class SettingsViewModel
    {
        private string configurationFile = Path.Combine(AuxiliarySharp.IO.General.GetCurrentDirectory(), "configuration");
        public RelayCommand AddEncodingCommand { get; set; }
        public RelayCommand DeleteEncodingCommand { get; set; }
        public RelayCommand RestoreEncodingCommand { get; set; }
        public ExtractorSettings SelectedEncoding { get; set; }
        public ObservableCollection<ExtractorSettings> Encodings { get; set; }
        public SettingsViewModel()
        {
            ReadConfiguration();

            AddEncodingCommand = new RelayCommand(null, (param) =>
            {
                Encodings.Add(new ExtractorSettings("Кодировка", 0, "[ -~]"));
            });

            RestoreEncodingCommand = new RelayCommand(null, (param) =>
            {
                Encodings.Clear();
                DefaultSeed();
            });

            DeleteEncodingCommand = new RelayCommand(null, (param) =>
            {
                if (SelectedEncoding != null)
                {
                    if (Encodings.Contains(SelectedEncoding))
                    {
                        Encodings.Remove(SelectedEncoding);
                    }
                }
            });
        }

        public void ReadConfiguration()
        {
            try
            {
                Encodings = AuxiliarySharp.IO.Processing.DeSerialize<ObservableCollection<ExtractorSettings>>(configurationFile);
            }
            catch
            {
                Encodings = new ObservableCollection<ExtractorSettings>();
                DefaultSeed();
            }
        }

        private void DefaultSeed()
        {
            Encodings.Add(new ExtractorSettings("Unicode", 1200, @"[\u0020-\u007E]"));
            Encodings.Add(new ExtractorSettings("Ascii", 1251, @"[\x20-\x7E]"));

            Save();
        }

        public void Save()
        {
            try
            {
                AuxiliarySharp.IO.Processing.Serialize(configurationFile, Encodings);
            }
            catch { }
        }
    }
}
