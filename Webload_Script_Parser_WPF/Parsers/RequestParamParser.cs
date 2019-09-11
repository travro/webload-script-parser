using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLScriptParser.Models;

namespace WLScriptParser.Parsers
{
    public static class RequestParamParser
    {
        public static Request.RequestVerb ParseRequestVerb(string line)
        {            
            if (line.StartsWith(" CONNECT")) return Request.RequestVerb.CONNECT;
            if (line.StartsWith(" DELETE")) return Request.RequestVerb.DELETE;
            if (line.StartsWith(" POST")) return Request.RequestVerb.POST;
            if (line.StartsWith(" PUT")) return Request.RequestVerb.PUT;
            if (line.StartsWith(" GET")) return Request.RequestVerb.GET;
            return Request.RequestVerb.GET;
        }

        public static string ParseRequestParamters(string line)
        {
            //string val = element.Attribute("Text").Value;
            string sumTotalSite = "sumtotaldevelopment.net";
            string domain = (line.Contains(sumTotalSite)) ? sumTotalSite : "https://";
            int domainLastIndex = line.IndexOf(domain) + domain.Length;
            //int paramIndex = line.IndexOf(' ', domainLastIndex);
            int paramIndex = line.IndexOfAny(new[] { '?', '%', ' ' }, domainLastIndex);
            return line.Substring(domainLastIndex, paramIndex - domainLastIndex);
        }
    }
}
