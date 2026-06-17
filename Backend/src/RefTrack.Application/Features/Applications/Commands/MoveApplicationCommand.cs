using MediatR;
using RefTrack.Application.Interface;
using RefTrack.Domain.Enums;

namespace RefTrack.Application.Features.Applications.Commands;
public record MoveApplicationCommand(
 Guid ApplicationId,
 string NewStatus,
 Guid UserId) : IRequest<bool>;
public class MoveApplicationHandler
 : IRequestHandler<MoveApplicationCommand, bool>
{
    private readonly IApplicationRepository _repo;
    public MoveApplicationHandler(IApplicationRepository repo) => _repo = repo;
    public async Task<bool> Handle(
    MoveApplicationCommand cmd, CancellationToken ct)
    {
        var app = await _repo.GetByIdAsync(cmd.ApplicationId, ct)
        ?? throw new KeyNotFoundException("Application not found.");
        if (app.UserId != cmd.UserId)
            throw new UnauthorizedAccessException("Not your application.");
        if (!Enum.TryParse<ApplicationStatus>(
        cmd.NewStatus, true, out var newStatus))
            throw new ArgumentException(
            $"Invalid status: {cmd.NewStatus}");
        app.MoveTo(newStatus); // throws if transition not allowed
        _repo.Update(app);
        await _repo.SaveChangesAsync(ct);
        return true;
    }
}
