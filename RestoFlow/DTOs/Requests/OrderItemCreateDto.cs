namespace RestoFlow.Dtos.Requests
{
    public class OrderItemCreateDto
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; } = 1;
        public string? Note { get; set; }
    }
}
