using StringsExplorer.Infrustructure.Extractor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace StringsExplorer.Infrustructure.Strings
{

    public class StringList
    {
        public HashSet<string> Chache { get; set; }
        public ObservableCollection<StringInfo> AllStrings { get; set; }
        public ObservableCollection<StringInfo> Strings { get; set; }
        public StringList()
        {
            Chache = new HashSet<string>();
            Strings = new ObservableCollection<StringInfo>();
            AllStrings = new ObservableCollection<StringInfo>();
        }
        public void AppendString(StringInfo value)
        {
            AllStrings.Add(value);
        }
        public void AppendString(ExtractedString extractedString)
        {
            if (!Chache.Contains(extractedString.Value))
            {
                Chache.Add(extractedString.Value);

                AllStrings.Add(new StringInfo(extractedString.Value, extractedString.Encoding, Chache.Count));
            }
        }
        public void Clear()
        {
            Chache.Clear();
            Strings.Clear();
            AllStrings.Clear();
        }
        public void UpdateStrings(int length)
        {
            Strings.Clear();
            foreach (StringInfo stringInfo in AllStrings.Where(x => x.Length >= length))
                Strings.Add(stringInfo);
        }

        public string Serialize(string delimiter = ";")
        {
            string test = String.Join("\r\n", Strings.Select(x => $"{x.OrderNumber}{delimiter}{x.Type}{delimiter}{x.Length}{delimiter}{x.Value}"));
            return String.Join(Environment.NewLine, Strings.Select(x => $"{x.OrderNumber}{delimiter}{x.Type}{delimiter}{x.Length}{delimiter}{x.Value}"));
        }
    }
}
