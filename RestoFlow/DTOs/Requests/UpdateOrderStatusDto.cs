using System.ComponentModel.DataAnnotations;

namespace RestoFlow.Dtos.Requests
{
    public class UpdateOrderStatusDto
    {
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}
