﻿using System.Collections;
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
        //private Request.RequestVerb ParseRequestVerb(XElement element)
        //{
        //    string val = element.Attribute("Text").Value;
        //    if (val.StartsWith(" CONNECT")) return Request.RequestVerb.CONNECT;
        //    if (val.StartsWith(" DELETE")) return Request.RequestVerb.DELETE;
        //    if (val.StartsWith(" POST")) return Request.RequestVerb.POST;
        //    if (val.StartsWith(" PUT")) return Request.RequestVerb.PUT;
        //    if (val.StartsWith(" GET")) return Request.RequestVerb.GET;
        //    return Request.RequestVerb.GET;
        //}
        //private string ParseRequestParams(XElement element)
        //{
        //    string val = element.Attribute("Text").Value;
        //    string sumTotalSite = "sumtotaldevelopment.net";
        //    string domain = (val.Contains(sumTotalSite)) ? sumTotalSite : "https://";
        //    int domainLastIndex = val.IndexOf(domain) + domain.Length;
        //    int paramIndex = val.IndexOf(' ', domainLastIndex);
        //    //int paramIndex = val.IndexOfAny(new[] { '?', ' ' }, domainLastIndex);

        //    return val.Substring(domainLastIndex, paramIndex - domainLastIndex);
        //}
        public string GetRequestString()
        {
            return Verb.ToString() + " " + Parameters;
        }
        public bool Equals(Request request)
        {
            return (Verb == request.Verb && Parameters == request.Parameters);
        }
    }
}
