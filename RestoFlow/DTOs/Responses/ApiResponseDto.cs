namespace RestoFlow.Dtos.Responses
{
    public class ApiResponseDto
    {
        public string Key { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public Object? Data { get; set; }
    }
}
