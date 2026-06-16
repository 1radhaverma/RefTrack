using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RefTrack.Application.Interface;
using RefTrack.Domain.Entities;
using RefTrack.Domain.Enums;
using RefTrack.Infrastructure.Persistence;

namespace RefTrack.Infrastructure.Rerpositories
{
        public class ReferrerRepository
            : GenericRerpository<Referrer>, IReferrerRepository
        {
            public ReferrerRepository(AppDBContext db) : base(db) { }

            public async Task<List<Referrer>> GetGhostedAfterDaysAsync(
                int days, CancellationToken ct = default)
                => await _set
                    .Where(r =>
                        r.Status == OutreachStatus.Sent
                        && r.LastContactedAt.HasValue
                        && r.LastContactedAt.Value <= DateTime.UtcNow.AddDays(-days))
                    .ToListAsync(ct);

            public async Task<List<Referrer>> GetByJobRoleAsync(
                Guid jobRoleId, CancellationToken ct = default)
                => await _set
                    .Where(r => r.JobRoleId == jobRoleId)
                    .OrderBy(r => r.Status.ToString())
                    .ToListAsync(ct);

            public async Task<List<Referrer>> GetByUserAsync(
                Guid userId, CancellationToken ct = default)
                => await _set
                    .Where(r => r.UserId == userId)
                    .OrderByDescending(r => r.CreatedAt)
                    .ToListAsync(ct);
        }
}
