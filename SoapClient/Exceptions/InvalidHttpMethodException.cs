using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoapClient.Exceptions
{
    public class InvalidHttpMethodException : Exception
    {
        public InvalidHttpMethodException(string message): base(message) { }
        public InvalidHttpMethodException(string message, Exception innerException): base(message, innerException) { }
        public InvalidHttpMethodException(Exception innerException) : base("an exception occurred while parsing the HTTP Method", innerException) { }
    }
}
