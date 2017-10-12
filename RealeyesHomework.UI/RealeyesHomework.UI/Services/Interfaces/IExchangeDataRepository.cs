using RealeyesHomework.UI.Models;
using System;
using System.Collections.Generic;

namespace RealeyesHomework.UI.Services.Interfaces
{
    public interface IExchangeDataRepository
    {
        Dictionary<DateTime, List<ExchangeRate>> Load();
        IEnumerable<string> LoadCurrencies();
    }
}