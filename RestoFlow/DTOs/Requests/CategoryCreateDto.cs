using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RestoFlow.Dtos.Requests
{
    public class CategoryCreateDto
    {
        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
