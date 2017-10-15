using RealeyesHomework.UI.Models;
using RealeyesHomework.UI.Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RealeyesHomework.UI.Services.Implementations
{
    public class InMemoryExchangeDataRepository : IExchangeDataRepository
    {
        private static readonly IDictionary<DateTime, IList<ExchangeRate>> Data = new ConcurrentDictionary<DateTime, IList<ExchangeRate>>();

        public IList<string> GetCurrencies()
        {
            var currencies = new List<string>();
            if (Data.Any())
            {
                currencies = Data.First().Value.Select(x => x.Currency).ToList();
            }
            return currencies;
        }

        public IDictionary<DateTime, IList<ExchangeRate>> GetExchangeRates(DateTime from, DateTime to)
        {
            return Data.Where(x => x.Key >= from && x.Key <= to).ToDictionary(x => x.Key, x => x.Value);
        }

        public void SaveData(IDictionary<DateTime, IList<ExchangeRate>> data)
        {
            foreach (var newData in data.Where(x => !Data.ContainsKey(x.Key)))
            {
                Data[newData.Key] = newData.Value;
            }
        }
    }
}