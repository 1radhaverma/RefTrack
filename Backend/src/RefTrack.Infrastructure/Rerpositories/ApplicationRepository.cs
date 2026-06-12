using Microsoft.EntityFrameworkCore;
using AppEntity = RefTrack.Domain.Entities.Application; // alias
using RefTrack.Application.Interface;
using RefTrack.Domain.Enums;
using RefTrack.Infrastructure.Persistence;

namespace RefTrack.Infrastructure.Rerpositories
{
    public class ApplicationRepository
    : GenericRerpository<AppEntity>, IApplicationRepository
    {
        public ApplicationRepository(AppDBContext db) : base(db) { }

        public async Task<List<AppEntity>> GetByUserAsync(
            Guid userId, CancellationToken ct = default)
            => await _set                           // _set now works — it's protected
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.UpdatedAt)
                .ToListAsync(ct);

        public async Task<List<AppEntity>> GetByStatusAsync(
            Guid userId, ApplicationStatus status,
            CancellationToken ct = default)
            => await _set
                .Where(a => a.UserId == userId
                         && a.Status == status)
                .ToListAsync(ct);

        public async Task<Dictionary<ApplicationStatus, int>>
            GetPipelineSummaryAsync(
            Guid userId, CancellationToken ct = default)
            => await _set
                .Where(a => a.UserId == userId)
                .GroupBy(a => a.Status)
                .ToDictionaryAsync(
                    g => g.Key,
                    g => g.Count(), ct);

        public Task<AppEntity> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<AppEntity>> GetAllAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(AppEntity entity, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public void Update(AppEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(AppEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
