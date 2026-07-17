namespace RestoFlow.Models
{
    public class Setting
    {
        public int Id { get; set; }
        public int? CurrencyId { get; set; } = 0;
        public Currency? Currency { get; set; }
        public string? RestaurantName { get; set; } = string.Empty;
        public string? LogoUrl { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
