using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Heisln.Car.Api.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Currency
    {
        USD = 0,
        JPY,
        BGN,
        CZK,
        DKK,
        GBP,
        HUF,
        PLN,
        RON,
        SEK,
        CHF,
        ISK,
        NOK,
        HRK,
        RUB,
        TRY,
        AUD,
        BRL,
        CAD,
        CNY,
        HKD,
        CIDR,
        ILS,
        INR,
        KRW,
        MXN,
        MYR,
        NZD,
        PHP,
        SGD,
        THB,
        ZAR
    }
}
