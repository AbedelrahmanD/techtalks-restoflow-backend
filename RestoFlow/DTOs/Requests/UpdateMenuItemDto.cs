using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RestoFlow.Dtos.Requests
{
    public class UpdateMenuItemDto
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public IFormFile? Image { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
