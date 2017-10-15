using RealeyesHomework.UI.Models;
using RealeyesHomework.UI.Services.Adapters;
using RealeyesHomework.UI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace RealeyesHomework.UI.Services.Implementations
{
    public class ExchangeDataService : IExchangeDataService
    {
        private const string DataUrl = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml";

        public IDictionary<DateTime, IList<ExchangeRate>> Load()
        {
            return ExchangeDataAdapter.Adapt(XDocument.Load(DataUrl));
        }
    }
}