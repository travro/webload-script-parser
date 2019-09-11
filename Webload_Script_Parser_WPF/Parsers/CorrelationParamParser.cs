using System.Collections.Generic;
using System.Text;

namespace WLScriptParser.Parsers
{
    public static class CorrelationParamParser
    {
        public static string Parse(string line, Ordinal ord)
        {
            //Capture outermost parenthesis
            Stack<char> paramStack = new Stack<char>();
            List<string> argList = new List<string>();
            StringBuilder buildingString = new StringBuilder();

            foreach(char c in line)
            {
                switch (c)
                {
                    case ' ': continue;
                    case '(':
                        paramStack.Push(c);
                        if (paramStack.Count > 1)
                        {
                            buildingString.Append(c);
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    case ')':
                        paramStack.Pop();
                        if (paramStack.Count >= 1)
                        {
                            buildingString.Append(c);
                            continue;
                        }
                        else if(paramStack.Count == 0)
                        {
                            argList.Add(buildingString.ToString());
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    case ',':
                        if (paramStack.Count > 1)
                        {
                            buildingString.Append(c);
                            continue;
                        }
                        else
                        {
                            argList.Add(buildingString.ToString());
                            buildingString = new StringBuilder();
                            continue;
                        }
                    default:
                        if (paramStack.Count >= 1)
                        {
                            buildingString.Append(c);
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                } 
            }

            for(int i = 0; i < line.Length)


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
