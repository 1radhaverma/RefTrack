using MediatR;
using RefTrack.Application.Interface;

namespace RefTrack.Application.Features.JobRoles.Commands;
public record ApplyToJobCommand(
 Guid JobRoleId, Guid UserId) : IRequest<bool>;
public class ApplyToJobHandler
 : IRequestHandler<ApplyToJobCommand, bool>
{
    private readonly IJobRoleRepository _repo;
    public ApplyToJobHandler(IJobRoleRepository repo) => _repo = repo;
    public async Task<bool> Handle(
    ApplyToJobCommand cmd, CancellationToken ct)
    {
        var role = await _repo.GetByIdAsync(cmd.JobRoleId, ct)
        ?? throw new KeyNotFoundException("Job role not found.");
        if (role.UserId != cmd.UserId)
            throw new UnauthorizedAccessException("Not your job role.");
        role.Apply(); // domain method
        _repo.Update(role);
        await _repo.SaveChangesAsync(ct);
        return true;
    }
}