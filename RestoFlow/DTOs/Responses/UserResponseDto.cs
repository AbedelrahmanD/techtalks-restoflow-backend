using RestoFlow.Enums;

namespace RestoFlow.Dtos.Responses
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Role Role { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
