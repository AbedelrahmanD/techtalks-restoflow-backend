using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RestoFlow.Dtos.Requests
{
    public class SettingRequestDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Required")]
        [Required(ErrorMessage = "Required")]
        public int CurrencyId { get; set; }

        [Required(ErrorMessage = "Required")]
        public string RestaurantName { get; set; }

        public IFormFile? Logo { get; set; }
    }
}
