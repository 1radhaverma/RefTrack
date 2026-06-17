using MediatR;
using RefTrack.Application.Interface;

namespace RefTrack.Application.Features.JobRoles.Commands;
public record DeleteJobRoleCommand(
 Guid JobRoleId, Guid UserId) : IRequest<bool>;
public class DeleteJobRoleHandler
 : IRequestHandler<DeleteJobRoleCommand, bool>
{
    private readonly IJobRoleRepository _repo;
    public DeleteJobRoleHandler(IJobRoleRepository repo) => _repo = repo;
    public async Task<bool> Handle(
    DeleteJobRoleCommand cmd, CancellationToken ct)
    {
        var role = await _repo.GetByIdAsync(cmd.JobRoleId, ct)
        ?? throw new KeyNotFoundException("Job role not found.");
        if (role.UserId != cmd.UserId)
            throw new UnauthorizedAccessException("Not your job role.");
        _repo.Delete(role);
        await _repo.SaveChangesAsync(ct);
        return true;
    }
}
