using System.Collections.Generic;
using System.Threading.Tasks;

namespace Heisln.Car.Application
{
    public interface ICurrencyConverterHandler
    {
        Task<List<double>> Convert(string targetCurrency, List<double> values, string sourceCurrency = "USD");

        Task<double> Convert(string targetCurrency, double value, string sourceCurrency = "USD");
    }
}