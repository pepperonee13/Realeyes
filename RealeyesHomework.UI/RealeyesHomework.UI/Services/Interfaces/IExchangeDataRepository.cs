using RealeyesHomework.UI.Models;
using System;
using System.Collections.Generic;

namespace RealeyesHomework.UI.Services.Interfaces
{
    public interface IExchangeDataRepository
    {
        IList<string> GetCurrencies();

        IDictionary<DateTime, IList<ExchangeRate>> GetExchangeRates(DateTime from, DateTime to);

        void SaveData(IDictionary<DateTime, IList<ExchangeRate>> data);
    }
}