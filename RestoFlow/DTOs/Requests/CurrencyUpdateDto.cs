using System.ComponentModel.DataAnnotations;

namespace RestoFlow.Dtos.Requests
{
    public class CurrencyUpdateDto
    {

        [Required(ErrorMessage = "Required")]
        public string Code { get; set; } = string.Empty;


        [Required(ErrorMessage = "Required")]
        public string Symbol { get; set; } = string.Empty;

        [Required(ErrorMessage = "Required")]
        public string Name { get; set; } = string.Empty;
    }
}
