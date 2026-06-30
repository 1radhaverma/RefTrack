using Microsoft.EntityFrameworkCore;
using RefTrack.Application.Interface;
using RefTrack.Domain.Entities;
using RefTrack.Domain.Enums;
using RefTrack.Infrastructure.Persistence;

public class JobRoleRepository : IJobRoleRepository
{
    private readonly AppDBContext _context;

    public JobRoleRepository(AppDBContext context)
    {
        _context = context;
    }

    public async Task AddAsync(JobRole entity, CancellationToken ct = default)
    {
        await _context.JobRoles.AddAsync(entity, ct);
    }

    public void Delete(JobRole entity)
    {
        _context.JobRoles.Remove(entity);
    }

    public async Task<List<JobRole>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.JobRoles.ToListAsync(ct);
    }

    public async Task<List<JobRole>> GetAppliedByUserAsync(Guid userId, CancellationToken ct = default)
    {
        return await _context.JobRoles
            .Where(j => j.UserId == userId && j.IsApplied == true)
            .ToListAsync(ct);
    }

    public async Task<List<JobRole>> GetByCompanyAsync(Guid companyId, CancellationToken ct = default)
    {
        return await _context.JobRoles
            .Where(j => j.CompanyId == companyId)
            .ToListAsync(ct);
    }

    public async Task<JobRole> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.JobRoles
            .FirstOrDefaultAsync(j => j.Id == id, ct);
    }

    public async Task<List<JobRole>> GetByUserAsync(Guid userId, CancellationToken ct = default)
    {
        return await _context.JobRoles
            .Where(j => j.UserId == userId)
            .ToListAsync(ct);
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }

    public void Update(JobRole entity)
    {
        _context.JobRoles.Update(entity);
    }
}