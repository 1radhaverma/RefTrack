using Microsoft.EntityFrameworkCore;
using RefTrack.Application.Interface;
using RefTrack.Domain.Entities;
using RefTrack.Domain.Enums;
using RefTrack.Infrastructure.Persistence;

namespace RefTrack.Infrastructure.Rerpositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDBContext _db;

        public CompanyRepository(AppDBContext db) => _db = db;

        public async Task AddAsync(Company entity, CancellationToken ct = default)
        {
            await _db.Companies.AddAsync(entity, ct);
        }

        public void Delete(Company entity)
        {
            _db.Companies.Remove(entity);
        }

        public async Task<List<Company>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.Companies.ToListAsync(ct);
        }

        public async Task<Company> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.Companies.FirstOrDefaultAsync(c => c.Id == id, ct);
        }

        public async Task<List<Company>> GetByTierAsync(Guid userId, CompanyTier tier, CancellationToken ct = default)
        {
            return await _db.Companies
                .Where(c => c.UserId == userId && c.Tier == tier)
                .ToListAsync(ct);
        }

        public async Task<List<Company>> GetByUserAsync(Guid userId, CancellationToken ct = default)
            => await _db.Companies
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.Tier)
                .ToListAsync(ct);

        public async Task<Company?> GetWithRolesAsync(Guid id, CancellationToken ct = default)
        {
            return null;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _db.SaveChangesAsync(ct);
        }

        public void Update(Company entity)
        {
            _db.Companies.Update(entity);
        }
    }
}