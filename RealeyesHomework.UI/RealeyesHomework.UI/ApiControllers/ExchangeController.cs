using RealeyesHomework.UI.Models;
using RealeyesHomework.UI.Services.Implementations;
using System;
using System.Web.Http;

namespace RealeyesHomework.UI.ApiControllers
{
    [RoutePrefix("api/exchange")]
    public class ExchangeController : ApiController
    {
        private readonly ExchangeRateCalculator _exchangeRateCalculator;

        public ExchangeController()
        {
            //TODO: DI
            _exchangeRateCalculator = new ExchangeRateCalculator(new ExchangeDataService());
        }

        [HttpGet]
        [Route("GetCurrencies")]
        public IHttpActionResult GetCurrencies()
        {
            try
            {
                return Ok(_exchangeRateCalculator.LoadCurrencies());
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
