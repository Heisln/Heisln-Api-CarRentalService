using Heisln.Car.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heisln.ApiTest.Mock
{
    class MockCurrencyConverterHandler : ICurrencyConverterHandler
    {
        readonly double factor;

        public MockCurrencyConverterHandler(double factor)
        {
            this.factor = factor;
        }

        public Task<List<double>> Convert(string targetCurrency, List<double> values, string sourceCurrency = "USD")
        {
            return Task.Run(() => {
                return values.Select(x => x * factor).ToList();
            });
        }

        public Task<double> Convert(string targetCurrency, double value, string sourceCurrency = "USD")
        {
            return Task.Run(() => {
                return value * factor;
            });
        }
    }
}
