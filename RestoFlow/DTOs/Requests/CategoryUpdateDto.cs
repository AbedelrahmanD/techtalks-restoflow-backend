using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RestoFlow.Dtos.Requests
{
    public class CategoryUpdateDto
    {
        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string Name { get; set; }

        public IFormFile? Image { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
