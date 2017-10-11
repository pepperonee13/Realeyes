using System.Collections.Generic;
using System.Web.Http;

namespace RealeyesHomework.UI.ApiControllers
{
    public class ExchangeController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetCurrencies()
        {
            return Ok(new List<string>
            {
                "EUR",
                "HUF"
            });
        }
    }
}
