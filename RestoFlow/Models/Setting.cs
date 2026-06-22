namespace RestoFlow.Models
{
    public class Setting
    {
        public int Id { get; set; }
        public int? CurrencyId { get; set; }
        public Currency? Currency { get; set; }
        public string? RestaurantName { get; set; }
        public string? LogoUrl { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
