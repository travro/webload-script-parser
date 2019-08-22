using System.Collections.Generic;
using System.Xml.Linq;
using WLScriptParser.Parsers;

namespace WLScriptParser.Models
{
    public static class CorrelationFactory
    {
        public static Correlation[] GetCorrelations(XElement element)
        {
            List<Correlation> _listCorrelations = new List<Correlation>();

            using (System.IO.StringReader reader = new System.IO.StringReader(element.Value))
            {
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null) break;
                    if (line.Contains("setCorrelation"))
                    {
                        string name = CorrelationParamParser.Parse(line, CorrelationParamParser.Ordinal.First)?? "[Name]";
                        string extractionLogic = CorrelationParamParser.Parse(line, CorrelationParamParser.Ordinal.Second)?? "[ExtractionLogic]";
                        string originalValue = CorrelationParamParser.Parse(line, CorrelationParamParser.Ordinal.Third)?? "[OriginalValue]";
                        _listCorrelations.Add(new Correlation(name, extractionLogic, originalValue));
                    }
                }
            }
            return _listCorrelations.ToArray();
        }
    }
}
