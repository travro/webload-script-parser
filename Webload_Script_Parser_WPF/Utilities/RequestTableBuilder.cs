using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLScriptParser.Models;

namespace WLScriptParser.Utilities
{
    public static class RequestTableBuilder
    {
        public static Request[,] GetRequestTable(IEnumerable<Request> r1, IEnumerable<Request> r2)
        {
            //Clone the request IEnumerables into new lists, ensuring pure functunality
            List<Request> r1Clone = new List<Request>();
            foreach (var r in r1) { r1Clone.Add(r); }

            List<Request> r2Clone = new List<Request>();
            foreach (var r in r2) { r2Clone.Add(r); }

            //Pointers to the lists that are longer and shorter
            List<Request> longerList = (r1Clone.Count >= r2Clone.Count) ? r1Clone : r2Clone;
            List<Request> shorterList = (longerList == r1Clone) ? r2Clone : r1Clone;
            int listLengthDiff = Math.Abs(longerList.Count - shorterList.Count);

            string blankRequestParams = "--Blank--";

            //Prime the shorter list with blank Requests to equal the length of the longer
            while (listLengthDiff > 0)
            {
                shorterList.Add(new Request(Request.RequestVerb.GET, blankRequestParams));
                listLengthDiff--;
            }

            //Logic for building the list results after comparisons
            for (int i = 0; i < longerList.Count; i++)
            {
                //if both blank
                if (longerList[i].Parameters == blankRequestParams && shorterList[i].Parameters == blankRequestParams)
                {
                    longerList.RemoveAt(i);
                    shorterList.RemoveAt(i);
                    continue;
                }
                //if equality or only one is blank
                if (
                        longerList[i].Parameters == blankRequestParams && shorterList[i].Parameters != blankRequestParams
                        ||
                        longerList[i].Parameters != blankRequestParams && shorterList[i].Parameters == blankRequestParams
                        ||
                        longerList[i] == shorterList[i])
                {
                    continue;
                }

                //if neither are blank and neither are equal, check that the requests of each list exists in equal amounts within the opposing list
                //using the defined List<> extension method "ContainSameAmount" defined below
                if (longerList[i] != shorterList[i])
                {
                    var blankRequest = new Request(Request.RequestVerb.GET, blankRequestParams);

                    if (!ListExtension.ContainsSameAmount(shorterList, longerList[i], longerList))
                    {
                        shorterList.Insert(i, blankRequest);
                        longerList.Add(blankRequest);
                        continue;
                    }
                    if (!ListExtension.ContainsSameAmount(longerList, shorterList[i], shorterList))
                    {
                        longerList.Insert(i, blankRequest);
                        shorterList.Add(blankRequest);
                        continue;
                    }
                }
            }

            //Check that both clone lists' lengths are euqal and return Request[,] containing both lists' values, else a blank Request[,]
            if (r1Clone.Count == r2Clone.Count)
            {
                Request[,] arr = new Request[longerList.Count, 2];

                for (int i = 0; i < arr.GetLength(0); i++)
                {
                    arr[i, 0] = r1Clone[i];
                    arr[i, 1] = r2Clone[i];
                }
                return arr;
            }
            else
            {
                return new Request[,] { { new Request(Request.RequestVerb.POST, "ERROR"), new Request(Request.RequestVerb.POST, "ERROR") } };
            }
        }
    }
    public static class ListExtension
    {
        
        //Extension method for checking the occurence of the request in the list compared to its occurence in listToCompare
        public static bool ContainsSameAmount(this List<Request> list, Request request, List<Request> listToCompare)
        {
            int listAmt = list.Count(item => item.GetRequestString() == request.GetRequestString());
            int listToCompareAmt = listToCompare.Count(item => item.GetRequestString() == request.GetRequestString());
            return listAmt == listToCompareAmt;
        }
    }
}
