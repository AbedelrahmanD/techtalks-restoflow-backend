namespace RestoFlow.Dtos.Responses
{
    public class MenuItemSimpleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Image { get; set; }
    }
}
