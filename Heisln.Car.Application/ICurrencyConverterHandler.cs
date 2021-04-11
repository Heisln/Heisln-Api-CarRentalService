using System.Collections.Generic;

namespace Heisln.Car.Application
{
    public interface ICurrencyConverterHandler
    {
        List<int> Convert(string sourceCurrency, string targetCurrency, List<int> values);
    }
}