using System;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FileStream_Practice
{
    public static class TransactionParser
    {
        public static string ParseTransactionName(string str)
        {
            return str.Substring(str.IndexOf("\"") + 1, str.LastIndexOf("\"") - 1 - str.IndexOf("\""));
        }
        //TODO
        public static Request.RequestVerb ParseRequestVerb(string str)
        {
            if (str.Contains("Post")) return Request.RequestVerb.POST;
            else if (str.Contains("Delete")) return Request.RequestVerb.DELETE;
            else if (str.Contains("Put")) return Request.RequestVerb.PUT;
            else return Request.RequestVerb.GET;
        }
        //TOFIX - account for wlTemporary
        public static string ParseRequestParameters(string str)
        {
            char[] skippableChars = {'"',')','('};
            StringBuilder builder = new StringBuilder();

            for (int i = str.Length - 1; i >= 0 && CharacterIsValid(str[i], skippableChars); i--)
            {
                if (!skippableChars.Contains(str[i])) builder.Insert(0, str[i]);
            }
            return builder.ToString();
        }

        private static bool CharacterIsValid(char c, char[] skippableChars)
        {
            char[] validChars = {'.', '/' };
            return (Char.IsLetterOrDigit(c) || validChars.Contains(c) || skippableChars.Contains(c)) ? true : false;
        }

        public static void Parse(StreamReader reader, TransactionRepository repo)
        {
            Transaction trans = null;

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                if (trans == null && line.StartsWith("BeginTransaction") && !repo.Contains(ParseTransactionName(line)))
                {
                    trans = new Transaction(ParseTransactionName(line));
                }

                if (trans != null &&
                    line.StartsWith("wlHttp.Get(\"https://p", true, CultureInfo.InvariantCulture))
                {
                    trans.AddRequest(new Request(ParseRequestVerb(line), ParseRequestParameters(line)));
                }

                if (trans != null && line.StartsWith("EndTransaction"))
                {
                    if (trans.Name == ParseTransactionName(line))
                    {
                        repo.AddTransaction(trans);
                        trans = null;
                    }
                }
            }
        }
    }
}
