using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RefTrack.Domain.Enums;

namespace RefTrack.Application.Interface
{
    public interface IApplicationRepository : IRepository<Domain.Entities.Application>
    {
        Task<List<Domain.Entities.Application>> GetByUserAsync(
            Guid userId, CancellationToken ct = default);

        Task<List<Domain.Entities.Application>> GetByStatusAsync(
            Guid userId, ApplicationStatus status,
            CancellationToken ct = default);

        // Dashboard — count of apps per status
        Task<Dictionary<ApplicationStatus, int>> GetPipelineSummaryAsync(
            Guid userId, CancellationToken ct = default);
    }
}
