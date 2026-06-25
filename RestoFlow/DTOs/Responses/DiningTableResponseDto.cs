namespace RestoFlow.Dtos.Responses
{
    public class DiningTableResponseDto
    {
        public int Id { get; set; }
        public string TableNumber { get; set; }
        public int SeatingCapacity { get; set; }
        public string QrCodeToken { get; set; }
    }
}
