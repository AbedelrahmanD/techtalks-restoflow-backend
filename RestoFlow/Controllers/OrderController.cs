using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RestoFlow.Dtos.Requests;
using RestoFlow.Dtos.Responses;
using RestoFlow.Hubs;
using RestoFlow.Models;
using RestoFlow.Services.Interfaces;
using Microsoft.Extensions.Localization;

namespace RestoFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class OrderController : ControllerBase
    {
        private static readonly string[] ValidStatuses = { "New", "InProgress", "Served" };

        private readonly IOrderService _service;
        private readonly IHubContext<OrderHub> _hub;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IDiningTableService _tableService;
        private readonly ISettingService _settingService;

        public OrderController(IOrderService service, IHubContext<OrderHub> hub, IStringLocalizer<SharedResource> localizer, IDiningTableService tableService, ISettingService settingService)
        {
            _service = service;
            _hub = hub;
            _localizer = localizer;
            _tableService = tableService;
            _settingService = settingService;
        }

        [HttpGet]
        [Authorize(Roles = "KitchenStaff,Admin")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _service.GetAllAsync();
            var dtos = orders.Select(MapToDto).ToList();
            return Ok(dtos);
        }

        [HttpGet("table/{qrCodeToken}")]
        public async Task<IActionResult> GetByTableQrCode(string qrCodeToken)
        {
            // resolve table from qr code token
            var table = await _tableService.GetByQrCodeTokenAsync(qrCodeToken);
            if (table == null)
            {
                return NotFound(new ErrorResponseDto
                {
                    Message = _localizer["table_not_found"],
                    Key = "table_not_found"
                });
            }

            // get the most recent active order (not Paid or Voided)
            var order = await _service.GetActiveByTableIdAsync(table.Id);

            if (order == null)
            {
                return NotFound(new ErrorResponseDto
                {
                    Message = _localizer["order_not_found"],
                    Key = "order_not_found"
                });
            }

            var dto = MapToDto(order);
            return Ok(dto);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "KitchenStaff")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusDto request)
        {
            if (!ValidStatuses.Contains(request.Status))
            {
                return BadRequest(new ErrorResponseDto
                {
                    Message = _localizer["invalid_order_status"],
                    Key = "invalid_order_status"
                });
            }

            var updated = await _service.UpdateStatusAsync(id, request.Status);
            if (!updated)
            {
                return NotFound(new ErrorResponseDto
                {
                    Message = _localizer["order_not_found"],
                    Key = "order_not_found"
                });
            }

            var order = await _service.GetByIdAsync(id);
            var dto = MapToDto(order!);

            var table = order!.Table;

            await _hub.Clients.Group($"table_{table.QrCodeToken}").SendAsync("OrderUpdated", dto);

            await _hub.Clients.Group("kitchen").SendAsync("OrderUpdated", dto);

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto request)
        {
            // resolve table from qr code token
            var table = await _tableService.GetByQrCodeTokenAsync(request.QrCodeToken);
            if (table == null)
            {
                return NotFound(new ErrorResponseDto { Message = _localizer["table_not_found"], Key = "table_not_found" });
            }

            // get currency from settings
            var settings = await _settingService.GetAsync();
            if (settings == null || settings.Currency == null)
            {
                return NotFound(new ErrorResponseDto { Message = _localizer["currency_not_found"], Key = "currency_not_found" });
            }

            var order = new Order
            {
                TableId = table.Id,
                CurrencyId = settings.Currency.Id,
                Items = request.Items?.Select(i => new OrderItem
                {
                    MenuItemId = i.MenuItemId,
                    Quantity = i.Quantity,
                    Note = i.Note
                }).ToList()
            };

            var created = await _service.CreateAsync(order);
            var dto = MapToDto(created);


            // Also send to kitchen staff for their display
            await _hub.Clients.Group("kitchen").SendAsync("OrderUpdated", dto);

            return Ok(dto);
        }

        private static OrderResponseDto MapToDto(Order order)
        {
            return new OrderResponseDto
            {
                Id = order.Id,
                TableId = order.TableId,
                TableNumber = order.Table?.TableNumber ?? string.Empty,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                Items = order.Items?.Select(i => new OrderItemResponseDto
                {
                    Id = i.Id,
                    MenuItemId = i.MenuItemId,
                    MenuItemName = i.MenuItem?.Name ?? string.Empty,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Note = i.Note,
                    Image = i?.MenuItem?.ImageUrl

                }).ToList() ?? new List<OrderItemResponseDto>()
            };
        }
    }
}
