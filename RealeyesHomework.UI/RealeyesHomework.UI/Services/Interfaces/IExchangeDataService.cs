using RealeyesHomework.UI.Models;
using System;
using System.Collections.Generic;

namespace RealeyesHomework.UI.Services.Interfaces
{
    public interface IExchangeDataService
    {
        IDictionary<DateTime, IList<ExchangeRate>> Load();
    }
}