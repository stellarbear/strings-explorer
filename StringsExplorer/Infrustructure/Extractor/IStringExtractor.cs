using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringsExplorer.Infrustructure.Extractor
{

    public interface IStringExtractor
    {
        void Init(IEnumerable<ExtractorSettings> encodings);
        IEnumerable<ExtractedString> Scan(string filename);
        bool Filter(string value);
    }
}
