using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringsExplorer.Infrustructure.Strings
{

    public class StringInfo
    {
        public int Length { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public int OrderNumber { get; set; }

        public StringInfo(string value, string type, int orderNumber)
        {
            Type = type;
            Value = value;
            Length = value.Length;
            OrderNumber = orderNumber;
        }
    }
}
