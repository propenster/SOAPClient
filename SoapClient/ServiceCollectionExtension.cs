using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SoapClient
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSoapClient(this IServiceCollection services)
        {
            var spHandler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                {
                   
                    return true;
                }
            };
            services.AddHttpClient("SOAPClient", client =>
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                client.Timeout = TimeSpan.FromSeconds(180); // Overall timeout across all tries
            }).ConfigurePrimaryHttpMessageHandler(x => spHandler);

            services.AddHttpClient();

            return services;
        }



        

    }
}
