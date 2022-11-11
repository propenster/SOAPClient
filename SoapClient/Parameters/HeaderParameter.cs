using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoapClient.Parameters
{
    public class HeaderParameter
    {
        public string? Key { get; set; } = string.Empty;
        public object Value { get; set; } = new();
        public HeaderParameter(string key, object value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}
