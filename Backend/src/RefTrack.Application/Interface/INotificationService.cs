using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefTrack.Application.Interface
{
    public interface INotificationService
    {
        Task SendFollowUpReminderAsync(
        Guid userId,
        Guid referrerId,
        string referrerName,
        string linkedInUrl,
        Guid jobRoleId,
        int daysSince,
        CancellationToken ct = default);
    }
}
