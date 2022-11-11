// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SoapClient;
using System.Text;


Console.WriteLine("Hello, World!");

string XMLresponse = String.Empty;
string MessageTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");

//construct XML request body here...
StringBuilder sb = new StringBuilder();

XMLresponse = sb.ToString();

var req = new SoapConsumeRequest()
{
    SOAPAction = $"SOAPAction:https://www.example.org/mywebservice?wsdl",
    Body = XMLresponse,
    Method = "POST",
    RequestUri = "https://www.example.org/mywebservice"
};

var client = new SoapClientFactory();

var response = await client.ExecuteAsync(req);

Console.WriteLine(response.Content);

////await host.RunAsync();


