using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Heisln.Car.Application
{
    public class CurrencyConverterHandler : ICurrencyConverterHandler
    {
        public List<int> Convert(string sourceCurrency, string targetCurrency, List<int> values)
        {
            var valuesAsXml = values.Select(x => $"<value>{x}</value>");
            var valuesAsString = string.Join("", valuesAsXml);
            using var client = new HttpClient(new HttpClientHandler());
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            var soapRequest = CreateSoapRequest(sourceCurrency, targetCurrency, valuesAsString);
            var request = CreateHttpRequestMessage(soapRequest);

            var response = client.SendAsync(request).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Something went wrong!");
            }
            var streamTask = response.Content.ReadAsStreamAsync();
            var stream = streamTask.Result;
            var sr = new StreamReader(stream);
            var soapResponse = XDocument.Load(sr);

            return soapResponse.Descendants("values").First().Elements().Select(w => System.Convert.ToInt32(w.Value)).ToList();
        }

        private static HttpRequestMessage CreateHttpRequestMessage(XDocument soapRequest)
        {
            var request = new HttpRequestMessage()
            {
                //todo config
                RequestUri = new Uri("http://127.0.0.1:9000/currency-converter.php"),
                Method = HttpMethod.Post
            };
            request.Content = new StringContent(soapRequest.ToString(), Encoding.UTF8, "text/xml");
            request.Headers.Clear();
            //todo config
            var byteArray = Encoding.ASCII.GetBytes("heisl:salamibrot");
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(byteArray));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
            //todo config
            request.Headers.Add("SOAPAction", "http://127.0.0.1:9000/currency-converter.php");
            return request;
        }

        private static XDocument CreateSoapRequest(string sourceCurrency, string targetCurrency, string valuesAsString)
        {
            var soapRequest = XDocument.Parse(
                "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ns=\"CurrencyConverter\"> " +
                "<soapenv:Header/>" +
                "<soapenv:Body>" +
                "<ns:convertCurrencies>" +
                "<sourceCurrency>" + sourceCurrency + "</sourceCurrency>" +
                "<targetCurrency>" + targetCurrency + "</targetCurrency>" +
                "<values>" +
                valuesAsString +
                "</values>" +
                "</ns:convertCurrencies>" +
                "</soapenv:Body>" +
                "</soapenv:Envelope>"
            );
            return soapRequest;
        }
    }

}