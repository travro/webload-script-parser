using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

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
            int _leftBound;
            int _rightBound;
            int substringLength;    

            using (System.IO.StringReader reader = new System.IO.StringReader(element.Value))
            {
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null) break;

                    if (line.Contains(correlationLine))
                    {
                        _leftBound = IndexOfBound(line, '\"', 1);
                        _rightBound = IndexOfBound(line, '\"', 2);
                        substringLength = _rightBound - _leftBound + 1;

                        correlationName = line.Substring(_leftBound, substringLength);

                        _listCorrelations.Add(new Correlation(correlationName, correlationValue));
                    }            
                }
            }
            return _listCorrelations.ToArray();
        }
        private static int IndexOfBound(string line, char c, int ordinal)
        {
            if (ordinal < 1) ordinal = 1;
            if (ordinal > line.Length - 1) ordinal = line.Length - 1;

            for(int i = 0; i < line.Length; i++)
            {
                if (line[i] != c) continue;
                if (line[i] == c && ordinal == 1)
                {
                    return i;
                }
                else
                {
                    ordinal--;
                }

            }
            return 0;
        }
        private static int LengthBetweenBounds(int l, int r)
        {
            return r - l;
        }
    }
}
