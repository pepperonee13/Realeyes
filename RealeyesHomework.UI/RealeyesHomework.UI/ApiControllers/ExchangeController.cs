using RealeyesHomework.UI.Models;
using RealeyesHomework.UI.Services.Implementations;
using RealeyesHomework.UI.Services.Interfaces;
using System;
using System.Linq;
using System.Web.Http;

namespace RealeyesHomework.UI.ApiControllers
{
    [RoutePrefix("api/exchange")]
    public class ExchangeController : ApiController
    {
        private readonly ExchangeRateCalculator _exchangeRateCalculator;
        private readonly IExchangeDataRepository _exchangeDataRepository;
        private readonly IExchangeDataService _exchangeDataService;

        public ExchangeController()
        {
            //TODO: DI
            _exchangeDataService = new ExchangeDataService();
            _exchangeDataRepository = new InMemoryExchangeDataRepository();
            _exchangeRateCalculator = new ExchangeRateCalculator(_exchangeDataRepository);
        }

        [HttpPost]
        [Route("LoadData")]
        public IHttpActionResult LoadData()
        {
            var data = _exchangeDataService.Load();
            _exchangeDataRepository.SaveData(data);
            return Ok("Exchange data loaded and saved");
        }

        [HttpGet]
        [Route("GetCurrencies")]
        public IHttpActionResult GetCurrencies()
        {
            try
            {
                return Ok(_exchangeDataRepository.GetCurrencies()
                                                 .Union(new[] { ExchangeRateCalculator.LeadCurrency.Currency })
                                                 .OrderBy(x => x)
                                                 .ToList());
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpGet]
        [Route("GetExchangeRate")]
        public IHttpActionResult GetExchangeRate([FromUri]GetExchangeRateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_exchangeRateCalculator.GetExchangeRate(request.FromCurrency, request.ToCurrency));
        }
    }
}
