using System.Collections.Generic;

namespace RestoFlow.Dtos.Requests
{
    public class CreateOrderDto
    {
        public string QrCodeToken { get; set; } = string.Empty;
        public List<OrderItemCreateDto>? Items { get; set; }
    }
}
