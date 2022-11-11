using SoapClient.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SoapClient
{
    public class SoapConsumeResponse
    {
        public string Content { get; set; } = string.Empty;
        public bool IsSuccessful { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        //public Dictionary<string, object> Headers { get; set; } = new Dictionary<string, object>();
        public string? StatusDescription { get;  set; }
        public string? Server { get;  set; }
        public Uri? ResponseUri { get;  set; }
        public string? ContentType { get;  set; }
        public long? ContentLength { get;  set; }
        public Version? Version { get;  set; }
        public ICollection<string> ContentEncoding { get;  set; }
        public Exception? ErrorException { get;  set; }
        public bool IsSuccessStatusCode { get;  set; }
        public HttpRequestMessage Request { get;  set; }
        public IReadOnlyCollection<HeaderParameter> Headers { get; internal set; }
        public IReadOnlyCollection<HeaderParameter> ContentHeaders { get; internal set; }
    }
}
