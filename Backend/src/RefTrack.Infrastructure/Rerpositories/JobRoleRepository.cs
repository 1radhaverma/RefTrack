using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RefTrack.Application.Interface;
using RefTrack.Domain.Entities;

namespace RefTrack.Infrastructure.Rerpositories
{
    public class JobRoleRepository : IJobRoleRepository
    {
        public Task AddAsync(JobRole entity, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(JobRole entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<JobRole>> GetAllAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<JobRole>> GetAppliedByUserAsync(Guid userId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<JobRole>> GetByCompanyAsync(Guid companyId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<JobRole> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<JobRole>> GetByUserAsync(Guid userId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public void Update(JobRole entity)
        {
            throw new NotImplementedException();
        }
    }
}
