using System.ComponentModel.DataAnnotations;

namespace RestoFlow.Dtos.Requests
{
    public class DiningTableCreateDto
    {
        [Required(ErrorMessage = "Required")]
        [StringLength(50)]
        public string TableNumber { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(1, 100, ErrorMessage = "SeatingCapacityRange")]
        public int SeatingCapacity { get; set; }
    }
}
