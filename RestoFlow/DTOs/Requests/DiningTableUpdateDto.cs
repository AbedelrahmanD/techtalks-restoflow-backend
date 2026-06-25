using System.ComponentModel.DataAnnotations;

namespace RestoFlow.Dtos.Requests
{
    public class DiningTableUpdateDto
    {
        [Required]
        [StringLength(50)]
        public string TableNumber { get; set; }

        [Required]
        [Range(1, 100)]
        public int SeatingCapacity { get; set; }
    }
}
