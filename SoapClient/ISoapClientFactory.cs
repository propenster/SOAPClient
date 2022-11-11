using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoapClient
{
    public interface ISoapClientFactory
    {
        Task<SoapConsumeResponse> ExecuteAsync(SoapConsumeRequest request, CancellationToken token);
        SoapConsumeResponse Execute(SoapConsumeRequest request);

    }
}
