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
            StringBuilder buildingString = new StringBuilder();  


            //get the first correlation argument
            for (int i = 0; i < line.Length; i++)
            {
                if(line[i] == '\"')
                {
                    if (quoteStack.Count == 0) 
                    {
                        quoteStack.Push(line[i]);
                        continue;
                    }
                    else{
                        quoteStack.Pop();
                        secondParamStartingIndex = i;
                        argList.Add(buildingString.ToString());
                        buildingString = new StringBuilder();
                        break;
                    }
                }
                else
                {
                    if(quoteStack.Count > 0)
                    {
                        buildingString.Append(line[i]);
                    }
                }
            }

            //get the third
            for(int j = line.Length - 1; j >=0; j--)
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
                        argList.Add(buildingString.ToString());
                        buildingString = new StringBuilder();
                        break;
                    }
                }
                else
                {
                    if (quoteStack.Count > 0)
                    {
                        buildingString.Append(line[j]);
                    }
                }
            }

            argList.Insert(1,line.Substring(secondParamStartingIndex, secondParamEndingIndex - secondParamStartingIndex));




            string error = "--no argument captured--";



            switch (ord)
            {
                case Ordinal.First: return (argList[0] ?? error);
                case Ordinal.Second: return (argList[1] ?? error);
                case Ordinal.Third: return (argList[2] ?? error);
                default: return error;
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
