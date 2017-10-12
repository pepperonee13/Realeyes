using RealeyesHomework.UI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace RealeyesHomework.UI.Services
{
    public class ExchangeService
    {
        private const string DataUrl = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml";
        private const string CubeNodeName = "Cube";
        private const string DateFormat = "yyyy-MM-dd";
        private const string TimeAttributeName = "time";
        private const string CurrencyAttributeName = "currency";
        private const string RateAttributeName = "rate";
        private const string NumberDecimalSeparator = ".";
        private const string LeadCurrency = "EUR";

        private static Random Random = new Random();

        private static readonly IFormatProvider FormatProvider = new NumberFormatInfo
        {
            NumberDecimalSeparator = NumberDecimalSeparator
        };

        public List<string> LoadCurrencies()
        {
            return GetData().First()
                            .Value
                            .Select(x => x.Currency)
                            .Distinct()
                            .Union(new[] { LeadCurrency })
                            .OrderBy(x => x)
                            .ToList();
        }

        private static Dictionary<DateTime, List<ExchangeRate>> GetData()
        {
            return XDocument.Load(DataUrl)
                            .Root
                            .Elements().Single(x => x.Name.LocalName == CubeNodeName)
                            .Elements()
                            .Where(x => x.Name.LocalName == CubeNodeName)
                            .ToDictionary(ParseDate, ParseExchangeRates);
        }

        private static List<ExchangeRate> ParseExchangeRates(XElement x)
        {
            return x.Elements().Select(ParseExchangeRate).ToList();
        }

        private static ExchangeRate ParseExchangeRate(XElement e)
        {
            return new ExchangeRate
            {
                Currency = e.Attribute(CurrencyAttributeName).Value,
                Rate = ParseDecimal(e)
            };
        }

        private static decimal ParseDecimal(XElement e)
        {
            return decimal.Parse(e.Attribute(RateAttributeName).Value, FormatProvider);
        }

        private static DateTime ParseDate(XElement x)
        {
            return DateTime.ParseExact(x.Attribute(TimeAttributeName).Value, DateFormat, CultureInfo.CurrentCulture);
        }

        public IDictionary<string, decimal> GetExchangeRate(string fromCurrency, string toCurrency)
        {
            return new Dictionary<string, decimal>
            {
                {"2017-10-10", Random.Next(1000,1500)/1000.0M},
                {"2017-10-11", Random.Next(1000,1500)/1000.0M},
                {"2017-10-12", Random.Next(1000,1500)/1000.0M},
                {"2017-10-13", Random.Next(1000,1500)/1000.0M},
                {"2017-10-14", Random.Next(1000,1500)/1000.0M},
                {"2017-10-15", Random.Next(1000,1500)/1000.0M},
                {"2017-10-16", Random.Next(1000,1500)/1000.0M},
                {"2017-10-17", Random.Next(1000,1500)/1000.0M}
            };
        }
    }
}