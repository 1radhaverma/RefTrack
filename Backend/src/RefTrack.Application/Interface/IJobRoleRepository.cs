using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RefTrack.Domain.Entities;

namespace RefTrack.Application.Interface
{
    public interface IJobRoleRepository : IRepository<JobRole>
    {
        Task<List<JobRole>> GetByCompanyAsync(
            Guid companyId, CancellationToken ct = default);

        Task<List<JobRole>> GetByUserAsync(
            Guid userId, CancellationToken ct = default);

        Task<List<JobRole>> GetAppliedByUserAsync(
            Guid userId, CancellationToken ct = default);
    }
}
