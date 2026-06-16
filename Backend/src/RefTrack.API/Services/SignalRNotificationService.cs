using Microsoft.AspNetCore.SignalR;
using RefTrack.Application.Interface;
using RefTrack.API.Hubs;

namespace RefTrack.API.Services
{
    public class SignalRNotificationService
    : INotificationService
    {
        private readonly IHubContext<ReminderHub> _hub;
        private readonly ILogger<SignalRNotificationService>
            _logger;

        public SignalRNotificationService(
            IHubContext<ReminderHub> hub,
            ILogger<SignalRNotificationService> logger)
        {
            _hub = hub;
            _logger = logger;
        }

        public async Task SendFollowUpReminderAsync(
            Guid userId,
            Guid referrerId,
            string referrerName,
            string linkedInUrl,
            Guid jobRoleId,
            int daysSince,
            CancellationToken ct = default)
        {
            await _hub.Clients
                .User(userId.ToString())
                .SendAsync("FollowUpReminder", new
                {
                    referrerId,
                    name = referrerName,
                    linkedInUrl,
                    jobRoleId,
                    daysSince,
                    message = $"Follow up with {referrerName}"
                            + $" — {daysSince} days no reply"
                }, ct);

            _logger.LogInformation(
                "Reminder pushed → {Name}", referrerName);
        }
    }
}
