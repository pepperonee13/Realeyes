using RealeyesHomework.UI.Models;
using RealeyesHomework.UI.Services;
using System;
using System.Web.Http;

namespace RealeyesHomework.UI.ApiControllers
{
    [RoutePrefix("api/exchange")]
    public class ExchangeController : ApiController
    {
        private readonly ExchangeService _exchangeService;

        public ExchangeController()
        {
            _exchangeService = new ExchangeService();
        }

        [HttpGet]
        [Route("GetCurrencies")]
        public IHttpActionResult GetCurrencies()
        {
            try
            {
                return Ok(_exchangeService.LoadCurrencies());
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

            return Ok(_exchangeService.GetExchangeRate(request.FromCurrency, request.ToCurrency));
        }
    }
}
