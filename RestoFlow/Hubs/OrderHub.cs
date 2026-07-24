using Microsoft.AspNetCore.SignalR;

namespace RestoFlow.Hubs
{
    public class OrderHub : Hub
    {
        // Clients can join a table group by providing their QR code token
        public async Task JoinTableGroup(string qrCodeToken)
        {
            if (!string.IsNullOrEmpty(qrCodeToken))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"table_{qrCodeToken}");
            }
        }

        // Clients can leave a table group
        public async Task LeaveTableGroup(string qrCodeToken)
        {
            if (!string.IsNullOrEmpty(qrCodeToken))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"table_{qrCodeToken}");
            }
        }

        // Kitchen staff can join the kitchen group to receive all order updates
        public async Task JoinKitchenGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "kitchen");
        }

        // Kitchen staff can leave the kitchen group
        public async Task LeaveKitchenGroup()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "kitchen");
        }
    }
}
