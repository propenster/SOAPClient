using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoapClient
{
    public class SoapConsumeRequest 
    {
        public string Body { get; set; } = string.Empty;
        public string RequestUri { get; set; } = string.Empty;
        public string SOAPAction { get; set; } = string.Empty;
        public string? Method { get; set; } = "POST";
        //public Authenticator Authenticator { get; set; }
        public Dictionary<string, object> Headers { get; set; } = new Dictionary<string, object>(); 


    }
}
