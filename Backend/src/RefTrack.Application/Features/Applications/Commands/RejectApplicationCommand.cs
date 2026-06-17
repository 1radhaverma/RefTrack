using MediatR;
using RefTrack.Application.Interface;

namespace RefTrack.Application.Features.Applications.Commands;
public record DeleteApplicationCommand(
 Guid ApplicationId, Guid UserId) : IRequest<bool>;
public class DeleteApplicationHandler
 : IRequestHandler<DeleteApplicationCommand, bool>
{
    private readonly IApplicationRepository _repo;
    public DeleteApplicationHandler(IApplicationRepository repo) => _repo = repo;
    public async Task<bool> Handle(
    DeleteApplicationCommand cmd, CancellationToken ct)
    {
        var app = await _repo.GetByIdAsync(cmd.ApplicationId, ct)
        ?? throw new KeyNotFoundException("Application not found.");
        if (app.UserId != cmd.UserId)
            throw new UnauthorizedAccessException("Not your application.");
        _repo.Delete(app);
        await _repo.SaveChangesAsync(ct);
        return true;
    }
}
