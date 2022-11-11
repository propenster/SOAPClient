using Microsoft.Extensions.DependencyInjection;
using SoapClient.Exceptions;
using SoapClient.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SoapClient
{
    public class SoapClientFactory : ISoapClientFactory
    {
        public string? RequestBody { get; internal set; }
        public Uri RequestUri { get; internal set; }
        public string? SOAPAction { get; internal set; }
        public HttpMethod Method { get; internal set; }
        public HttpRequestMessage RequestMessage { get; internal set; }


        public SoapConsumeResponse Execute(SoapConsumeRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<SoapConsumeResponse> ExecuteAsync(SoapConsumeRequest request, CancellationToken token = default)
        {
            SoapConsumeResponse response = null;
            var serviceCollection = new ServiceCollection();
            try
            {
                //var client = serviceCollection.BuildServiceProvider().GetRequiredService<HttpClient>();
                //var client = clientFactory.CreateClient("SOAPClient");
                var spHandler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                    {
                        Console.WriteLine($"Sender: {sender}");
                        Console.WriteLine($"cert: {cert}");
                        Console.WriteLine($"chain: {chain}");
                        Console.WriteLine($"sslPolicyErrors: {sslPolicyErrors}");
                        return true;
                    },

                };

                var client = new HttpClient(spHandler);
                Method = GetMethodType(request.Method);
                if (string.IsNullOrWhiteSpace(request.RequestUri)) throw new ArgumentNullException(nameof(request.RequestUri));
                if (string.IsNullOrWhiteSpace(request.Body)) throw new ArgumentNullException(nameof(request.Body));
                RequestBody = request.Body;

                RequestUri = new Uri(request.RequestUri);

                RequestMessage = new HttpRequestMessage(Method ?? HttpMethod.Post, RequestUri);
                foreach(var item in request.Headers)
                {
                    client.DefaultRequestHeaders.Add(item.Key, Convert.ToString(item.Value));   

                }
                RequestMessage.Content = new StringContent(RequestBody, Encoding.UTF8, "text/xml");
                client.DefaultRequestHeaders.Add("SOAPAction", $"SOAPAction:{request.RequestUri}");
                
                //ServicePointManager.ServerCertificateValidationCallback = TrustAllCertificateCallback;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                       | SecurityProtocolType.Tls11
                       | SecurityProtocolType.Tls12;
                var res = await client.SendAsync(RequestMessage).ConfigureAwait(false);
                response = await FormatFinalResponseAsync(RequestMessage, res, token).ConfigureAwait(false);

                return response;

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private async Task<SoapConsumeResponse?> FormatFinalResponseAsync(HttpRequestMessage request, HttpResponseMessage res, CancellationToken token = default)
        {
            var content = await res.Content.ReadAsStringAsync(token).ConfigureAwait(false);
            return new SoapConsumeResponse
            {
                Content = content,
                IsSuccessful = res.IsSuccessStatusCode ? true : false,
                StatusCode = res.StatusCode,
                //RawBytes = bytes,
                ContentEncoding = res.Content.Headers.ContentEncoding,
                Version = res.RequestMessage?.Version,
                ContentLength = res.Content.Headers.ContentLength,
                ContentType = res.Content.Headers.ContentType?.MediaType,
                //ResponseStatus = calculateResponseStatus(res),
                ErrorException = MaybeException(),
                ResponseUri = res.RequestMessage!.RequestUri,
                Server = res.Headers.Server.ToString(),
                StatusDescription = res.ReasonPhrase,
                IsSuccessStatusCode = res.IsSuccessStatusCode,
                Request = request,
                Headers = res.Headers.GetHeaderParameters(),
                ContentHeaders = res.Content.Headers.GetHeaderParameters(),
                //Cookies = res.Content.Headers,
                //RootElement = request.RootElement

            };

            Exception? MaybeException()
                => res.IsSuccessStatusCode
                    ? null
            #if NETSTANDARD
                    : new HttpRequestException($"Request failed with status code {res.StatusCode}");
            #else
                    : new HttpRequestException($"Request failed with status code {res.StatusCode}", null, res.StatusCode);
            #endif


            //return new RestResponse
            //{
            //    Content = content,
            //    RawBytes = bytes,
            //    ContentEncoding = httpResponse.Content.Headers.ContentEncoding,
            //    Version = httpResponse.RequestMessage?.Version,
            //    ContentLength = httpResponse.Content.Headers.ContentLength,
            //    ContentType = httpResponse.Content.Headers.ContentType?.MediaType,
            //    ResponseStatus = calculateResponseStatus(httpResponse),
            //    ErrorException = MaybeException(),
            //    ResponseUri = httpResponse.RequestMessage!.RequestUri,
            //    Server = httpResponse.Headers.Server.ToString(),
            //    StatusCode = httpResponse.StatusCode,
            //    StatusDescription = httpResponse.ReasonPhrase,
            //    IsSuccessStatusCode = httpResponse.IsSuccessStatusCode,
            //    Request = request,
            //    Headers = httpResponse.Headers.GetHeaderParameters(),
            //    ContentHeaders = httpResponse.Content.Headers.GetHeaderParameters(),
            //    Cookies = cookieCollection,
            //    RootElement = request.RootElement
            //};


        }

        public static bool TrustAllCertificateCallback(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors)
        {

            return true;

        }
        private HttpMethod GetMethodType(string? method)
        {
            switch (method)
            {
                case "GET":
                    return HttpMethod.Get;
                case "POST":
                    return HttpMethod.Post;
                case "PUT":
                    return HttpMethod.Put;
                case "DELETE":
                    return HttpMethod.Delete;
                case "HEAD":
                    return HttpMethod.Head;
                case "PATCH":
                    return HttpMethod.Patch;
                default:
                    return null;
                    //return new ArgumentOutOfRangeException(nameof(method), new InvalidHttpMethodException(" invalid http method"));
            }
        }
    }
}
