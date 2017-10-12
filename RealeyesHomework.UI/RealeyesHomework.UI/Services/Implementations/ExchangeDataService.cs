using RealeyesHomework.UI.Models;
using RealeyesHomework.UI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace RealeyesHomework.UI.Services.Implementations
{
    public class ExchangeDataService : IExchangeDataRepository
    {
        private const string DataUrl = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml";
        private const string CubeNodeName = "Cube";
        private const string DateFormat = "yyyy-MM-dd";
        private const string TimeAttributeName = "time";
        private const string CurrencyAttributeName = "currency";
        private const string RateAttributeName = "rate";
        private const string NumberDecimalSeparator = ".";
        private static IEnumerable<string> _currencies;
        private static Dictionary<DateTime, List<ExchangeRate>> _data;


        private static readonly IFormatProvider FormatProvider = new NumberFormatInfo
        {
            NumberDecimalSeparator = NumberDecimalSeparator
        };

        public IEnumerable<string> LoadCurrencies()
        {
            return _currencies ?? (_currencies = Load().First()
                                                       .Value
                                                       .Select(x => x.Currency)
                                                       .Distinct());
        }

        public Dictionary<DateTime, List<ExchangeRate>> Load()
        {
            return _data ?? (_data = XDocument.Load(DataUrl)
                                              .Root
                                              .Elements().Single(x => x.Name.LocalName == CubeNodeName)
                                              .Elements()
                                              .Where(x => x.Name.LocalName == CubeNodeName)
                                              .ToDictionary(ParseDate, ParseExchangeRates));
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
    }
}