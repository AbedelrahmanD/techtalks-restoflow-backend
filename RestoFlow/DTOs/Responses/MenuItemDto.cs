namespace RestoFlow.Dtos.Responses
{
    public class MenuItemDto
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; }
    }
}
