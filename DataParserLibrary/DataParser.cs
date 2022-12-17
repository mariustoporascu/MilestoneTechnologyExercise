using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParserLibrary
{
    public abstract class DataParser<T>
    {
        public abstract T MapData(Dictionary<int, string> indexAndHeaderNames, string[] lineDataArray);
        public string[] SplitLineData(char delimiter, string lineData) => lineData.Split(delimiter);
        public bool IsLineDataValid(string lineData) => !string.IsNullOrWhiteSpace(lineData);
        public bool IsDelimiterValid(char delimiter) => delimiter != '\0';
    }
}
