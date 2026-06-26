using System.ComponentModel.DataAnnotations;

namespace RestoFlow.Dtos.Requests
{
    public class CategoryUpdateDto
    {
        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
