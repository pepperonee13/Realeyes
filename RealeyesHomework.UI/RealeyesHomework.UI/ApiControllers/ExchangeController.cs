using RealeyesHomework.UI.Services;
using System.Web.Http;

namespace RealeyesHomework.UI.ApiControllers
{
    public class ExchangeController : ApiController
    {
        private readonly ExchangeService _exchangeService;

        public ExchangeController()
        {
            _exchangeService = new ExchangeService();
        }

        [HttpGet]
        public IHttpActionResult GetCurrencies()
        {
            return Ok(_exchangeService.LoadCurrencies());
        }
    }
}
