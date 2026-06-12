using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RefTrack.Application.Interface;
using RefTrack.Domain.Entities;

namespace RefTrack.Infrastructure.Rerpositories
{
    public class ReferrerRepository : IReferrerRepository
    {
        public Task AddAsync(Referrer entity, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(Referrer entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Referrer>> GetAllAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Referrer> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Referrer>> GetByJobRoleAsync(Guid jobRoleId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Referrer>> GetByUserAsync(Guid userId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Referrer>> GetGhostedAfterDaysAsync(int days, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public void Update(Referrer entity)
        {
            throw new NotImplementedException();
        }
    }
}
