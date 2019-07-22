using System.Xml.Linq;

namespace Webload_Script_Parser_WPF.Models
{
    public class Correlation
    {
        public string Name { get; set; }
        public string OriginalValue { get; set; }

        public Correlation( string name, string originalValue)
        {
            Name = name; OriginalValue = originalValue;
        }
    }
}
