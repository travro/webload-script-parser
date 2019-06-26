namespace Webload_Script_Parser_WPF.Models
{
    public class Request
    {
        public Request.RequestVerb Verb { get; set; }
        public string Parameters { get; set; }

        public Request(RequestVerb verb, string parameters)
        {
            Verb = verb; Parameters = parameters;
        }
        public enum RequestVerb
        {
            GET = 0,
            POST = 1,
            PUT = 2,
            DELETE = 3,
            CONNECT = 4
        }
    }
}
