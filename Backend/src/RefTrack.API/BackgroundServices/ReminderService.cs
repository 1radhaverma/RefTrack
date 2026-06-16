using RefTrack.Application.Interface;

namespace RefTrack.API.BackgroundServices
{
    public class ReminderService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly INotificationService _notifier;
        private readonly ILogger<ReminderService> _logger;
        private const int GhostedAfterDays = 5;

        public ReminderService(
            IServiceScopeFactory scopeFactory,
            INotificationService notifier,
            ILogger<ReminderService> logger)
        {
            _scopeFactory = scopeFactory;
            _notifier = notifier;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken ct)
        {
            _logger.LogInformation(
                "ReminderService started.");

            while (!ct.IsCancellationRequested)
            {
                await CheckAndNotify(ct);
                await Task.Delay(
                    TimeSpan.FromHours(24), ct);
            }
        }

        private async Task CheckAndNotify(
            CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();

            var repo = scope.ServiceProvider
                .GetRequiredService<IReferrerRepository>();

            var ghosted = await repo
                .GetGhostedAfterDaysAsync(
                    GhostedAfterDays, ct);

            _logger.LogInformation(
                "Found {Count} ghosted referrers.",
                ghosted.Count);

            foreach (var referrer in ghosted)
            {
                var daysSince = (int)(
                    DateTime.UtcNow
                    - referrer.LastContactedAt!.Value)
                    .TotalDays;

                await _notifier.SendFollowUpReminderAsync(
                    referrer.UserId,
                    referrer.Id,
                    referrer.Name,
                    referrer.LinkedInUrl,
                    referrer.JobRoleId,
                    daysSince,
                    ct);
            }
        }
    }
}
