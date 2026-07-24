namespace RestoFlow.Dtos.Responses
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public string TableNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<OrderItemResponseDto> Items { get; set; } = new();
    }
}
