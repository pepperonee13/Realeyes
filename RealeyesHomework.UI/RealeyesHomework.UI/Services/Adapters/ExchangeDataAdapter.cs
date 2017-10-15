using RealeyesHomework.UI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace RealeyesHomework.UI.Services.Adapters
{
    public class ExchangeDataAdapter
    {
        private const string DateFormat = "yyyy-MM-dd";
        private const string TimeAttributeName = "time";
        private const string CurrencyAttributeName = "currency";
        private const string RateAttributeName = "rate";
        private const string NumberDecimalSeparator = ".";
        private const string CubeNodeName = "Cube";

        private static readonly IFormatProvider FormatProvider = new NumberFormatInfo
        {
            NumberDecimalSeparator = NumberDecimalSeparator
        };

        public static Dictionary<DateTime, IList<ExchangeRate>> Adapt(XDocument document)
        {
            //TODO: error handling
            return document.Root
                           .Elements().Single(x => x.Name.LocalName == CubeNodeName)
                           .Elements()
                           .Where(x => x.Name.LocalName == CubeNodeName)
                           .ToDictionary(ParseDate, ParseExchangeRates);
        }

        private static IList<ExchangeRate> ParseExchangeRates(XElement x)
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