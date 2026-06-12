using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace RefTrack.Infrastructure.Hubs
{
    [Authorize]  // only authenticated users can connect
    public class ReminderHub : Hub
    {
        // Angular calls this to join their personal group
        // so reminders only go to the right user
        public async Task JoinUserGroup(string userId)
            => await Groups.AddToGroupAsync(
                   Context.ConnectionId, userId);
    }
}
