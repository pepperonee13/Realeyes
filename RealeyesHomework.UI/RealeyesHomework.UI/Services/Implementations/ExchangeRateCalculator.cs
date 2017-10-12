using RealeyesHomework.UI.Models;
using RealeyesHomework.UI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RealeyesHomework.UI.Services.Implementations
{
    public class ExchangeRateCalculator
    {
        private readonly IExchangeDataRepository _exchangeService;
        private static readonly ExchangeRate LeadCurrency = new ExchangeRate
        {
            Currency = "EUR",
            Rate = 1
        };
        private List<ExchangeRate> _exchangeRates;

        public ExchangeRateCalculator(IExchangeDataRepository exchangeService)
        {
            _exchangeService = exchangeService;
        }

        public IEnumerable<string> LoadCurrencies()
        {
            return _exchangeService.LoadCurrencies()
                                   .Union(new[] { LeadCurrency.Currency })
                                   .OrderBy(x => x)
                                   .ToList();
        }

        public IDictionary<int, decimal> GetExchangeRate(string fromCurrency, string toCurrency)
        {
            var result = new Dictionary<int, decimal>();
            var data = _exchangeService.Load().OrderBy(x => x.Key);
            foreach (var dateAndRates in data)
            {
                var exchangeRates = AddLeadCurrency(dateAndRates.Value);
                var exchangeRate = GetExchangeRate(exchangeRates, fromCurrency, toCurrency);
                var timestamp = GetTimestamp(dateAndRates.Key);
                result.Add(timestamp, exchangeRate);
            }
            return result;
        }

        private static List<ExchangeRate> AddLeadCurrency(List<ExchangeRate> exchangeRates)
        {
            exchangeRates.Add(LeadCurrency);
            return exchangeRates;
        }

        private static int GetTimestamp(DateTime date)
        {
            return (int)date.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public decimal GetExchangeRate(List<ExchangeRate> exchangeRates, string fromCurrency, string toCurrency)
        {
            _exchangeRates = exchangeRates;
            // Preconditions
            if (fromCurrency.ToLower() == toCurrency.ToLower())
            {
                return 1;
            }
            if (!CurrencyExists(fromCurrency) || !CurrencyExists(toCurrency))
            {
                throw new Exception($"CurrencyExchangeFailed {fromCurrency} - {toCurrency}, LeadCurrency: {LeadCurrency}");
            }

            decimal exchangeRate = 1;

            // If from currency is lead currency
            if (IsLeadCurrency(fromCurrency))
            {
                var exchangeRateEntity = GetExchangeData(toCurrency);
                if (exchangeRateEntity != null)
                {
                    exchangeRate = exchangeRateEntity.Rate;
                }
            }
            // If to currency is lead currency
            else if (IsLeadCurrency(toCurrency))
            {
                var exchangeRateEntity = GetExchangeData(fromCurrency);
                if (exchangeRateEntity != null)
                {
                    exchangeRate = 1 / exchangeRateEntity.Rate;
                }
            }
            // if both currencies are not lead currency
            else
            {
                exchangeRate = GetExchangeRate(_exchangeRates, fromCurrency, LeadCurrency.Currency);
                exchangeRate *= GetExchangeRate(_exchangeRates, LeadCurrency.Currency, toCurrency);
            }
            return exchangeRate;
        }

        private bool IsLeadCurrency(string currencyCode)
        {
            return LeadCurrency.Currency.ToLower() == currencyCode.ToLower();
        }

        private ExchangeRate GetExchangeData(string currencyCode)
        {
            return _exchangeRates.FirstOrDefault(x => x.Currency.ToLower() == currencyCode.ToLower());
        }

        private bool CurrencyExists(string currencyCode)
        {
            if (_exchangeRates == null)
            {
                return false;
            }
            return GetExchangeData(currencyCode) != null;
        }
    }
}