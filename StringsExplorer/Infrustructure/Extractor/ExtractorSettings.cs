namespace StringsExplorer.Infrustructure.Extractor
{

    public class ExtractorSettings
    {
        public string Name { get; set; }
        public int Codepage { get; set; }
        public string CharRange { get; set; }
        public ExtractorSettings(string name, int codepage, string charRange)
        {
            Name = name;
            Codepage = codepage;
            CharRange = charRange;
        }
    }
}