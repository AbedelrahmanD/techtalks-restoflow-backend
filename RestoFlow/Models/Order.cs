namespace RestoFlow.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public DiningTable? Table { get; set; }
        public string Status { get; set; } = "New";//New,InProgress,Served,Paid,Voided
        public decimal TotalAmount { get; set; } = 0m;
        public int CurrencyId { get; set; }
        public Currency? Currency { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<OrderItem>? Items { get; set; }
    }
}
