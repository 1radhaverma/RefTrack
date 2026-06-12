using Microsoft.EntityFrameworkCore;
using RefTrack.Application.Interface;
using RefTrack.Domain.Entities;
using RefTrack.Infrastructure.Persistence;          // IEntity lives here


namespace RefTrack.Infrastructure.Rerpositories
{
    public class GenericRerpository<T> : IRepository<T>
    where T : BaseEntity               // constrained to BaseEntity
    {
        protected readonly AppDBContext _db;
        protected readonly DbSet<T> _set;  // PROTECTED — child classes need this

        public GenericRerpository(AppDBContext db)
        {
            _db = db;
            _set = db.Set<T>();
        }

        // ── CRUD ──────────────────────────────────────────
        public async Task AddAsync(
            T entity, CancellationToken ct = default)
            => await _set.AddAsync(entity, ct);   // was missing — caused compile errors

        public async Task<T?> GetByIdAsync(
            Guid id, CancellationToken ct = default)
            => await _set.FindAsync(
                   new object[] { id }, ct);

        public async Task<List<T>> GetAllAsync(
            CancellationToken ct = default)
            => await _set.ToListAsync(ct);

        public void Update(T entity)
            => _db.Entry(entity).State = EntityState.Modified;

        public void Delete(T entity)
            => _set.Remove(entity);

        public async Task SaveChangesAsync(
            CancellationToken ct = default)
            => await _db.SaveChangesAsync(ct);

        Task<int> IRepository<T>.SaveChangesAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
