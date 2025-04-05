using Microsoft.AspNetCore.SignalR;

namespace PRN222.Assignment.FUHotelBookingSystem.Service.helper
{
    public class FUHub : Hub
    {
        public async Task SendUpdate()
        {
            await Clients.All.SendAsync("ReceiveUpdate");
        }
    }
}
