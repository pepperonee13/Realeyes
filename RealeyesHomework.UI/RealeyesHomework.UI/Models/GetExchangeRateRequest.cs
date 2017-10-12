using System.ComponentModel.DataAnnotations;

namespace RealeyesHomework.UI.Models
{
    public class GetExchangeRateRequest
    {
        [Required]
        public string FromCurrency { get; set; }
        [Required]
        public string ToCurrency { get; set; }
    }
}