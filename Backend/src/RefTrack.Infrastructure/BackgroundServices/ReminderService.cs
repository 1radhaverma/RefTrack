using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RefTrack.Application.Interface;
using RefTrack.Infrastructure.Hubs;

namespace RefTrack.Infrastructure.BackgroundServices
{
    public class ReminderService : BackgroundService
    {
        private readonly IServiceProvider _sp;
        private readonly IHubContext<ReminderHub> _hub;

        public ReminderService(
            IServiceProvider sp,
            IHubContext<ReminderHub> hub)
        {
            _sp = sp;
            _hub = hub;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckGhostedReferrers(stoppingToken);
                // Wait 24 hours before next check
                await Task.Delay(
                    TimeSpan.FromHours(24), stoppingToken);
            }
        }

        private async Task CheckGhostedReferrers(
            CancellationToken ct)
        {
            // BackgroundService is singleton — must create
            // a scope to use scoped DbContext/Repositories
            using var scope = _sp.CreateScope();
            var repo = scope.ServiceProvider
                .GetRequiredService<IReferrerRepository>();

            // Find referrers messaged 5+ days ago with no reply
            var ghosted = await repo
                .GetGhostedAfterDaysAsync(5, ct);

            foreach (var r in ghosted)
            {
                // Push notification to that user's SignalR group
                await _hub.Clients
                    .Group(r.UserId.ToString())
                    .SendAsync("ReminderAlert", new
                    {
                        Message = $"No reply from {r.Name}",
                        ReferrerId = r.Id,
                        JobRoleId = r.JobRoleId
                    }, ct);
            }
        }
    }
}
