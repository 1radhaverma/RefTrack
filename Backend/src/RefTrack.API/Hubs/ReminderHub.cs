using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace RefTrack.API.Hubs
{
    [Authorize]  // only authenticated users can connect
    public class ReminderHub : Hub
    {
        // Angular calls this to join their personal group
        // so reminders only go to the right user
        public override async Task OnConnectedAsync()
        => await base.OnConnectedAsync();

        public override async Task OnDisconnectedAsync(
            Exception? exception)
            => await base.OnDisconnectedAsync(exception);
    }
}
