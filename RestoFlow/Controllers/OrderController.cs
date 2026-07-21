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
    [Authorize(Roles = "KitchenStaff")]
    public class OrderController : ControllerBase
    {
        private static readonly string[] ValidStatuses = { "New", "InProgress", "Served" };

        private readonly IOrderService _service;
        private readonly IHubContext<OrderHub> _hub;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public OrderController(IOrderService service, IHubContext<OrderHub> hub, IStringLocalizer<SharedResource> localizer)
        {
            _service = service;
            _hub = hub;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _service.GetAllAsync();
            var dtos = orders.Select(MapToDto).ToList();
            return Ok(dtos);
        }

        [HttpPut("{id}/status")]
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

            await _hub.Clients.All.SendAsync("OrderUpdated", dto);

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
                    Note = i.Note
                }).ToList() ?? new List<OrderItemResponseDto>()
            };
        }
    }
}
