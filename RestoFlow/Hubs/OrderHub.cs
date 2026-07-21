using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace RestoFlow.Hubs
{
    [Authorize(Roles = "KitchenStaff")]
    public class OrderHub : Hub
    {
    }
}
