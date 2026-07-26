using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RestoFlow.Dtos.Requests
{
    public class UpdateMenuItemDto
    {
        [Required(ErrorMessage = "Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Required")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Required")]
        public string Name { get; set; }

        [StringLength(2000, ErrorMessage = "StringMaxLength")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Required")]
        public decimal Price { get; set; }

        public IFormFile? Image { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
