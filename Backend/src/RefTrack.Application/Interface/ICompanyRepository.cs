using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RefTrack.Domain.Entities;
using RefTrack.Domain.Enums;

namespace RefTrack.Application.Interface
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<List<Company>> GetByUserAsync(
            Guid userId, CancellationToken ct = default);

        Task<List<Company>> GetByTierAsync(
            Guid userId, CompanyTier tier,
            CancellationToken ct = default);

        Task<Company?> GetWithRolesAsync(
            Guid id, CancellationToken ct = default);
    }
}
