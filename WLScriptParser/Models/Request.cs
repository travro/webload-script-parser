using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using WLScriptParser.Parsers;

namespace WLScriptParser.Models
{
    public class Request { 
        private Correlation[] _correlations;
        public bool Visible { get; }
        public bool Matched { get; set; }
        public int MatchingId { get; set; }
        public Request.RequestVerb Verb { get; }
        public string Parameters { get; }
        public Correlation[] Correlations => _correlations ?? new Correlation[] {}; /*new Correlation("", "")*/

        public Request(RequestVerb verb, string parameters)
        {
            Verb = verb; Parameters = parameters;
            Matched = false;
            MatchingId = -1;
        }
        public Request(XElement element, bool visible)
        {
            Verb = RequestParamParser.ParseRequestVerb(element.Attribute("Text").Value);
            Parameters = RequestParamParser.ParseRequestParamters(element.Attribute("Text").Value);
            Visible = visible;
            Matched = false;
            MatchingId = -1;
        }
        public Request(XElement httpHeaderElement, XElement nodeScriptElement, bool visible)
        {
            Verb = RequestParamParser.ParseRequestVerb(httpHeaderElement.Attribute("Text").Value);
            Parameters = RequestParamParser.ParseRequestParamters(httpHeaderElement.Attribute("Text").Value);
            Visible = visible;
            _correlations = CorrelationFactory.GetCorrelations(nodeScriptElement);
            Matched = false;
            MatchingId = -1;
        }
        public enum RequestVerb
        {
            GET = 0,
            POST = 1,
            PUT = 2,
            DELETE = 3,
            CONNECT = 4,
            MISSING = 5
        }
        public string GetRequestString()
        {
            return Verb.ToString() + " " + Parameters;
        }
        public bool Equals(Request request)
        {
            return (Verb == request.Verb && Parameters.Equals(request.Parameters, System.StringComparison.OrdinalIgnoreCase));
        }
    }
}
