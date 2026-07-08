namespace RestoFlow.Dtos.Responses
{
    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public string? Message { get; set; }
        public string? Key { get; set; }
    }
}
