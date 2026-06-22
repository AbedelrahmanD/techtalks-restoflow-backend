using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RestoFlow.Dtos.Requests
{
    public class SettingRequestDto
    {
        public int? CurrencyId { get; set; }

        [StringLength(200)]
        public string? RestaurantName { get; set; }

        public IFormFile? Logo { get; set; }
    }
}
