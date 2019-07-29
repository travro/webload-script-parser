using System.Collections.Generic;
using System.Xml.Linq;
using Webload_Script_Parser_WPF.Parsers;

namespace Webload_Script_Parser_WPF.Models
{
    public static class CorrelationFactory
    {
        public static Correlation[] GetCorrelations(XElement element)
        {
            List<Correlation> _listCorrelations = new List<Correlation>();
            string correlationLine = "setCorrelation";
            string correlationName = "[CorrelationName]";
            string correlationValue = "[OriginalValue]";

            using (System.IO.StringReader reader = new System.IO.StringReader(element.Value))
            {
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null) break;
                    if (line.Contains(correlationLine))
                    {
                        correlationName = CorrelationParamParser.Parse(line, CorrelationParamParser.Ordinal.First);
                        correlationValue = CorrelationParamParser.Parse(line, CorrelationParamParser.Ordinal.Third);
                        _listCorrelations.Add(new Correlation(correlationName, correlationValue));
                    }
                }
            }
            return _listCorrelations.ToArray();
        }
    }
}
