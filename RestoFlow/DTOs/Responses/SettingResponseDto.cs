namespace RestoFlow.Dtos.Responses
{
    public class SettingResponseDto
    {
        public int Id { get; set; }
        public int? CurrencyId { get; set; }
        public string? RestaurantName { get; set; }
        public string? LogoUrl { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
