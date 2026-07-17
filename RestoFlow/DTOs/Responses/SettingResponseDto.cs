using RestoFlow.Models;

namespace RestoFlow.Dtos.Responses
{
    public class SettingResponseDto
    {

       required public Currency Currency { get; set; }
        public string? RestaurantName { get; set; }
        public string? LogoUrl { get; set; }

    }
}
