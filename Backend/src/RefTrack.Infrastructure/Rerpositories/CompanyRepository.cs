using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using RefTrack.Application.Interface;
using RefTrack.Domain.Entities;
using RefTrack.Domain.Enums;
using RefTrack.Infrastructure.Persistence;

namespace RefTrack.Infrastructure.Rerpositories
{
    public class CompanyRepository : ICompanyRepository
    {

        private readonly AppDBContext _db;
        protected readonly DbSet<T> _set;

        public CompanyRepository(AppDBContext db)=> _db = db;

        public Task AddAsync(Company entity, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(Company entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Company>> GetAllAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Company> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Company>> GetByTierAsync(Guid userId, CompanyTier tier, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Company>> GetByUserAsync(Guid userId, CancellationToken ct = default)
 => await _db.Set<Company>()
 .Where(c => c.UserId == userId)
 .OrderBy(c => c.Tier)
 .ToListAsync(ct);

        public Task<Company?> GetWithRolesAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public void Update(Company entity)
        {
            throw new NotImplementedException();
        }
    }
}
