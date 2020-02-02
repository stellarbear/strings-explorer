using Alphaleonis.Win32.Filesystem;
using StringsSharp;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace StringsExplorer.Infrustructure.Extractor
{
    public class ExtractedString
    {
        public string Value { get; set; }
        public string Encoding { get; set; }
    }

    public class StringExtractor : IStringExtractor
    {
        private StringFilter _stringFilter = null;
        private List<ExtractorSettings> _encodingList;

        public bool Filter(string value)
        {
            if (_stringFilter != null)
            {
                return _stringFilter.ScanQuick(value);
            }

            return false;
        }

        public void Init(IEnumerable<ExtractorSettings> encodingList)
        {
            _encodingList = new List<ExtractorSettings>(encodingList);
            _stringFilter = new StringFilter(Path.Combine(AuxiliarySharp.IO.General.GetCurrentDirectory(), "valuable"));
        }

        public IEnumerable<ExtractedString> Scan(string filename)
        {
            if (File.Exists(filename))
            {
                foreach (var match in ScanImplementation(filename))
                {
                    yield return match;
                }
            }
        }

        private IEnumerable<ExtractedString> ScanImplementation(string filename)
        {
            if (_encodingList != null)
            {
                foreach (var encodingEntry in _encodingList)
                {
                    using (StringsSharp.StringsSharp ss = new StringsSharp.StringsSharp(encodingEntry.Codepage, encodingEntry.CharRange, 3))
                    {
                        foreach (MatchCollection matches in ss.Scan(filename))
                        {
                            foreach (Match match in matches)
                            {
                                yield return new ExtractedString
                                {
                                    Value = match.Value,
                                    Encoding = encodingEntry.Name
                                };
                            }
                        }
                    }
                }
            }
        }
    }
}
