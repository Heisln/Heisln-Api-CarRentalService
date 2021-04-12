using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Heisln.Car.Application
{
    public class CurrencyConverterHandler : ICurrencyConverterHandler
    {
        public async Task<List<double>> Convert(string targetCurrency, List<double> values, string sourceCurrency = "USD")
        {
            var valuesAsXml = values.Select(x => $"<value>{x}</value>");
            var valuesAsString = string.Join("", valuesAsXml);
            using var client = new HttpClient(new HttpClientHandler());
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            var soapRequest = CreateMultipleValuesSoapMessage(sourceCurrency, targetCurrency, valuesAsString);
            var request = CreateHttpRequestMessage(soapRequest);

            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Something went wrong!");
            }
            var streamTask = response.Content.ReadAsStreamAsync();
            var stream = streamTask.Result;
            var sr = new StreamReader(stream);
            var soapResponse = XDocument.Load(sr);

            return soapResponse.Descendants("values").First().Elements().Select(w => System.Convert.ToDouble(w.Value)).ToList();
        }

        public async Task<double> Convert(string targetCurrency, double value, string sourceCurrency = "USD")
        {
            using var client = new HttpClient(new HttpClientHandler());
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            var soapRequest = CreateSingleValueSoapMessage(sourceCurrency, targetCurrency, value.ToString());
            var request = CreateHttpRequestMessage(soapRequest);

            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Something went wrong!");
            }
            var streamTask = response.Content.ReadAsStreamAsync();
            var stream = streamTask.Result;
            var sr = new StreamReader(stream);
            var soapResponse = XDocument.Load(sr);

            return System.Convert.ToDouble(soapResponse.Descendants("value").First().Value);
        }

        private static HttpRequestMessage CreateHttpRequestMessage(XDocument soapRequest)
        {
            var request = new HttpRequestMessage()
            {
                //todo config
                RequestUri = new Uri("http://heisln-currency-converter/currency-converter.php"),
                Method = HttpMethod.Post
            };
            request.Content = new StringContent(soapRequest.ToString(), Encoding.UTF8, "text/xml");
            request.Headers.Clear();
            //todo config
            var byteArray = Encoding.ASCII.GetBytes("heisl:salamibrot");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(byteArray));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
            //todo config
            request.Headers.Add("SOAPAction", request.RequestUri.ToString());
            return request;
        }

        private static XDocument CreateMultipleValuesSoapMessage(string sourceCurrency, string targetCurrency, string valuesAsString)
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

        private static XDocument CreateSingleValueSoapMessage(string sourceCurrency, string targetCurrency, string valueAsString)
        {
            var soapRequest = XDocument.Parse(
                "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ns=\"CurrencyConverter\"> " +
                "<soapenv:Header/>" +
                "<soapenv:Body>" +
                "<ns:convertCurrency>" +
                "<sourceCurrency>" + sourceCurrency + "</sourceCurrency>" +
                "<targetCurrency>" + targetCurrency + "</targetCurrency>" +
                "<value>" +
                valueAsString +
                "</value>" +
                "</ns:convertCurrency>" +
                "</soapenv:Body>" +
                "</soapenv:Envelope>"
            );
            return soapRequest;
        }
    }

}