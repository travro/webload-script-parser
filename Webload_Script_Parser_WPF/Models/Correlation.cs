using System.Xml.Linq;

namespace Webload_Script_Parser_WPF.Models
{
    public class Correlation
    {
        public string Rule { get; set; }
        public string OriginalValue { get; set; }
        public string ExtractionLogic { get; set; }

        public Correlation( string rule, string extractionLogic, string originalValue)
        {
            Rule = rule; ExtractionLogic = extractionLogic; OriginalValue = originalValue;
        }
    }
}
