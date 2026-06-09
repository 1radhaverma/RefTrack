using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RefTrack.Domain.Entities;

namespace RefTrack.Application.Interface
{
    public interface IRepository<T> where T:BaseEntity
    {
        // READ
        Task<T> GetByIdAsync(Guid id , CancellationToken ct=default);
        Task<List<T>> GetAllAsync(CancellationToken ct = default);

        // WRITE
        Task AddAsync(T entity, CancellationToken ct = default);
        void Update(T entity);
        void Delete(T entity);

        // COMMIT
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
