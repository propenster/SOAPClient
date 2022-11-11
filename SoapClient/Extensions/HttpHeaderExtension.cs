using SoapClient.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SoapClient.Extensions
{
    static class HttpHeadersExtensions
    {
        public static IReadOnlyCollection<HeaderParameter> GetHeaderParameters(this HttpHeaders httpHeaders)
            => httpHeaders
                .SelectMany(x => x.Value.Select(y => (x.Key, y)))
                .Select(x => new HeaderParameter(x.Key, x.y))
                .ToList();
    }
}
