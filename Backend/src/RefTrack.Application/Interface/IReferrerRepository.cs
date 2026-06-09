using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RefTrack.Domain.Entities;

namespace RefTrack.Application.Interface
{
    public interface IReferrerRepository : IRepository<Referrer>
    {
        // Key query — used by ReminderService to find
        // referrers that haven't replied in 5+ days
        Task<List<Referrer>> GetGhostedAfterDaysAsync(
            int days, CancellationToken ct = default);

        Task<List<Referrer>> GetByJobRoleAsync(
            Guid jobRoleId, CancellationToken ct = default);

        Task<List<Referrer>> GetByUserAsync(
            Guid userId, CancellationToken ct = default);
    }
}
