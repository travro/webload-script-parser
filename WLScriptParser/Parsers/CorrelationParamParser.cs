using System.Collections.Generic;
using System.Text;

namespace WLScriptParser.Parsers
{
    public static class CorrelationParamParser
    {
        public static string Parse(string line, Ordinal ord)
        {
            //Capture outermost parenthesis
            Stack<char> quoteStack = new Stack<char>();
            int secondParamStartingIndex = 0;
            int secondParamEndingIndex = 0;
            List<string> argList = new List<string>();
            string[] argArray = new string[] { "--no argument captured--", "--no argument captured--", "--no argument captured--" };
            StringBuilder buildingString = new StringBuilder();


            //get the first correlation argument
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '\"')
                {
                    if (quoteStack.Count == 0)
                    {
                        quoteStack.Push(line[i]);
                        continue;
                    }
                    else
                    {
                        quoteStack.Pop();
                        secondParamStartingIndex = i;
                        //argList.Add(buildingString.ToString());
                        argArray[0] = buildingString.ToString();
                        buildingString.Clear();
                        break;
                    }
                }
                else
                {
                    if (quoteStack.Count > 0)
                    {
                        buildingString.Append(line[i]);
                    }
                }
            }

            //get the third
            for (int j = line.Length - 1; j >= 0; j--)
            {
                if (line[j] == '\"')
                {
                    if (quoteStack.Count == 0)
                    {
                        quoteStack.Push(line[j]);
                        continue;
                    }
                    else
                    {
                        quoteStack.Pop();
                        secondParamEndingIndex = j;
                        //argList.Add(buildingString.ToString());
                        argArray[2] = buildingString.ToString();
                        buildingString.Clear();
                        break;
                    }
                }
                else
                {
                    if (quoteStack.Count > 0)
                    {
                        buildingString.Insert(0, line[j]);
                    }
                }
            }

            //argList.Insert(1, line.Substring(secondParamStartingIndex, secondParamEndingIndex - secondParamStartingIndex));

            argArray[1] = line.Substring(secondParamStartingIndex, secondParamEndingIndex - secondParamStartingIndex);

            switch (ord)
            {
                case Ordinal.First: return (argArray[0]);
                case Ordinal.Second: return (argArray[1]);
                case Ordinal.Third: return (argArray[2]);
                default: return "--error--";
            }
        }
        public enum Ordinal
        {
            First = 1,
            Second = 2,
            Third = 3
        };
    }
}
