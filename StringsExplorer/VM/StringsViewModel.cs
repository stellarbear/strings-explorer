using Alphaleonis.Win32.Filesystem;
using AuxiliarySharp.IO;
using StringsExplorer.Infrustructure;
using StringsExplorer.Infrustructure.Extractor;
using StringsExplorer.Infrustructure.Strings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
namespace StringsExplorer
{
    public class StringsViewModel : INotifyPropertyChanged
    {

        private int minValue;
        private string _filename;
        private string _errorMessage;
        private bool _stringsIsActive;
        private bool _valuableStringsIsActive;

        public int MinLength
        {
            get => minValue; set
            {
                minValue = value;
                OnPropertyChanged();
            }
        }
        public bool StringsIsActive
        {
            get => _stringsIsActive; set
            {
                _stringsIsActive = value; OnPropertyChanged();
            }
        }
        public bool ValuableStringsIsActive
        {
            get => _valuableStringsIsActive; set
            {
                _valuableStringsIsActive = value; OnPropertyChanged();
            }
        }
        public string Filename
        {
            get => _filename; set
            {
                _filename = value; OnPropertyChanged();
            }
        }
        public string ErrorMessage
        {
            get => _errorMessage; set
            {
                _errorMessage = value; OnPropertyChanged();
            }
        }

        private IStringExtractor _extractor;
        public StringList ExtractedStrings { get; set; }
        public StringList ExtractedValuableStrings { get; set; }

        public RelayCommand SaveResults { get; set; }
        public IEnumerable<ExtractorSettings> Encodings { get; set; }
        public StringsViewModel(IEnumerable<ExtractorSettings> encodings, string filePathInput = default(string))
        {
            minValue = 5;
            Encodings = encodings;
            _extractor = new StringExtractor();
            ExtractedStrings = new StringList();
            ExtractedValuableStrings = new StringList();

            SaveResults = new RelayCommand(null, async (p) =>
            {
                string filepath = Path.Combine(General.GetCurrentDirectory(), "results", General.GetCurrentTime());
                Processing.Serialize(Path.Combine(filepath, "valuable"), ExtractedValuableStrings.Serialize());
                Processing.WriteFile(Path.Combine(filepath, "full"), ExtractedStrings.Serialize());
                General.OpenDirectory(filepath);
            });

            ScanFile(filePathInput);
        }

        public async void ScanFile(string filename)
        {
            if (StringsIsActive || ValuableStringsIsActive)
                return;

            if (filename != default(string))
            {
                try
                {
                    _extractor.Init(Encodings);
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }

                if (General.CheckIfFileIsAccessible(filename))
                {
                    Filename = filename;
                    StringsIsActive = true;
                    ExtractedStrings.Clear();
                    ExtractedValuableStrings.Clear();

                    await TaskEx.Run(() =>
                    {
                        foreach (var scanEntry in _extractor.Scan(filename))
                        {
                            ExtractedStrings.AppendString(scanEntry);
                        }
                    });

                    ExtractedStrings.UpdateStrings(MinLength);
                    OnPropertyChanged("ExtractedStrings");
                    ValuableStringsIsActive = true;

                    StringsIsActive = false;
                    await TaskEx.Run(() =>
                    {
                        Parallel.ForEach(
                        ExtractedStrings.AllStrings,
                        new ParallelOptions
                        {
                            MaxDegreeOfParallelism = Environment.ProcessorCount > 1 ?
                                Environment.ProcessorCount - 1 : 1
                        },
                        (scanEntry, loopState) =>
                        {
                            if (_extractor.Filter(scanEntry.Value))
                            {
                                Application.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    ExtractedValuableStrings.AppendString(scanEntry);
                                });
                            }
                        });
                    });

                    ExtractedValuableStrings.UpdateStrings(MinLength);
                    OnPropertyChanged("ExtractedValuableStrings");
                    ValuableStringsIsActive = false;
                }
                else
                {
                    ErrorMessage = "Выбранный файл недостyпен для чтения";
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string PropertyName = default(string)) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
    }
}
