using RealeyesHomework.UI.Models;
using RealeyesHomework.UI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RealeyesHomework.UI.Services.Implementations
{
    public class ExchangeRateCalculator
    {
        private readonly IExchangeDataRepository _exchangeDataRepository;
        public static readonly ExchangeRate LeadCurrency = new ExchangeRate
        {
            Currency = "EUR",
            Rate = 1
        };
        private IList<ExchangeRate> _exchangeRates;

        public ExchangeRateCalculator(IExchangeDataRepository exchangeDataRepository)
        {
            _exchangeDataRepository = exchangeDataRepository;
        }

        public IDictionary<long, decimal> GetExchangeRate(string fromCurrency, string toCurrency)
        {
            var result = new Dictionary<long, decimal>();
            //TODO: pass from and to dates as parameters
            var data = _exchangeDataRepository.GetExchangeRates(DateTime.MinValue, DateTime.Today).OrderBy(x => x.Key);
            foreach (var dateAndRates in data)
            {
                var exchangeRates = AddLeadCurrency(dateAndRates.Value);
                var exchangeRate = GetExchangeRate(exchangeRates, fromCurrency, toCurrency);
                var timestamp = GetTimestamp(dateAndRates.Key);
                result.Add(timestamp, exchangeRate);
            }
            return result;
        }

        private static IList<ExchangeRate> AddLeadCurrency(IList<ExchangeRate> exchangeRates)
        {
            exchangeRates.Add(LeadCurrency);
            return exchangeRates;
        }

        private static long GetTimestamp(DateTime date)
        {
            var dateTimeOffset = new DateTimeOffset(date);
            return dateTimeOffset.ToUnixTimeSeconds();
        }

        public decimal GetExchangeRate(IList<ExchangeRate> exchangeRates, string fromCurrency, string toCurrency)
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