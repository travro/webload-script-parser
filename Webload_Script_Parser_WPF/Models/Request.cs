using System.Collections.Generic;
using System.Xml.Linq;

namespace Webload_Script_Parser_WPF.Models
{
    public class Request
    {
        private List<Correlation> _correlations;
        public Request.RequestVerb Verb { get; }
        public string Parameters { get; }
        public Correlation[] Correlations => _correlations.ToArray();

        public Request(RequestVerb verb, string parameters)
        {
            Verb = verb; Parameters = parameters;
            _correlations = new List<Correlation>();
        }
        public Request(XElement element)
        {
            Verb = ParseRequestVerb(element);
            Parameters = ParseRequestParams(element);
            _correlations = new List<Correlation>();
        }
        public Request(XElement httpHeaderElement, XElement nodeScriptElement)
        {
            Verb = ParseRequestVerb(httpHeaderElement);
            Parameters = ParseRequestParams(httpHeaderElement);
            if (nodeScriptElement != null && nodeScriptElement.Value.Contains("setCorrelation") && Verb == RequestVerb.GET)
            {
                _correlations = CorrelationFactory.GetCorrelations(nodeScriptElement);
            }
        }
        public enum RequestVerb
        {
            GET = 0,
            POST = 1,
            PUT = 2,
            DELETE = 3,
            CONNECT = 4
        }
        private Request.RequestVerb ParseRequestVerb(XElement element)
        {
            string val = element.Attribute("Text").Value;
            if (val.StartsWith(" CONNECT")) return Request.RequestVerb.CONNECT;
            if (val.StartsWith(" DELETE")) return Request.RequestVerb.DELETE;
            if (val.StartsWith(" POST")) return Request.RequestVerb.POST;
            if (val.StartsWith(" PUT")) return Request.RequestVerb.PUT;
            if (val.StartsWith(" GET")) return Request.RequestVerb.GET;
            return Request.RequestVerb.GET;
        }
        private string ParseRequestParams(XElement element)
        {
            string val = element.Attribute("Text").Value;
            string sumTotalSite = "sumtotaldevelopment.net/";
            string domain = (val.Contains(sumTotalSite)) ? sumTotalSite : "https://";
            int domainIndex = val.IndexOf(domain) + domain.Length;
            int paramIndex = val.IndexOf(' ', domainIndex);

            return val.Substring(domainIndex/*, paramIndex - domainIndex*/);
        }
    }
}
