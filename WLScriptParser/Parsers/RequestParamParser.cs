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

            //initially filter out domain and session data
            string sumTotalSite = "sumtotaldevelopment.net";
            string domain = (line.Contains(sumTotalSite)) ? sumTotalSite : "https://";
            int domainLastIndex = line.IndexOf(domain) + domain.Length;
            int paramIndex = line.IndexOfAny(new[] { '?', '%', ' ' }, domainLastIndex);

            string filteredUrl = line.Substring(domainLastIndex, paramIndex - domainLastIndex);

            filteredUrl = filteredUrl.ReplaceInBounds("[TenantKey]", "/learning/DataStore/", "/Learning/");
            filteredUrl = filteredUrl.ReplaceInBounds("[TenantKey]", "/learning/DataStore/", "/Common/");
            filteredUrl = filteredUrl.ReplaceInBounds("[TimeStamp]", "blank___", ".");
            filteredUrl = filteredUrl.ReplaceInBounds("[UserId]", "gamification/summaries/");
            filteredUrl = filteredUrl.ReplaceInBounds("[BrokerSessionId]", "api/sumtSocial/communities/");
            filteredUrl = filteredUrl.ReplaceInBounds("[BrokerSessionId]", "api/sumtSocial/communities/", "/");
            filteredUrl = filteredUrl.ReplaceInBounds("[BrokerSessionId]", "api/social/discuss");
            //filteredUrl = filteredUrl.ReplaceInBounds("[BrokerSessionID]", "api/sumtSocial/communities/", "ownerCount");

            return filteredUrl;
        }
    }
    //Custom extension methods for checking for different boundaries and replacing internal values if checks are truthy
    internal static class StringExtension
    {
        public static string ReplaceInBounds(this string url, string replacementVal, string leftBound, string rightBound = null)
        {
            if (!url.Contains(leftBound)) return url;
            if (rightBound != null && !url.Substring(url.IndexOf(leftBound) + leftBound.Length).Contains(rightBound)) return url;


            int leftBoundIndex = url.IndexOf(leftBound) + leftBound.Length;

            if (leftBoundIndex >= url.Length - 1) return url;

            int rightBoundIndex;
            string originalValue;

            if (rightBound != null)
            {
                rightBoundIndex = url.IndexOf(rightBound, leftBoundIndex);
                originalValue = url.Substring(leftBoundIndex, rightBoundIndex - leftBoundIndex);
            }
            else
            {
                originalValue = url.Substring(leftBoundIndex);
            }

            var strBuilder = new StringBuilder(url);
            strBuilder.Replace(originalValue, replacementVal);
            url = strBuilder.ToString();
            return url;
        }
    }
}

