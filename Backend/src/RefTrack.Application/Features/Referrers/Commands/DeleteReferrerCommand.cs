using MediatR;
using RefTrack.Application.Interface;

namespace RefTrack.Application.Features.Referrers.Commands;
public record DeleteReferrerCommand(
 Guid ReferrerId, Guid UserId) : IRequest<bool>;
public class DeleteReferrerHandler
 : IRequestHandler<DeleteReferrerCommand, bool>
{
    private readonly IReferrerRepository _repo;
    public DeleteReferrerHandler(IReferrerRepository repo) => _repo = repo;
    public async Task<bool> Handle(
    DeleteReferrerCommand cmd, CancellationToken ct)
    {
        var referrer = await _repo.GetByIdAsync(cmd.ReferrerId, ct)
        ?? throw new KeyNotFoundException("Referrer not found.");
        if (referrer.UserId != cmd.UserId)
            throw new UnauthorizedAccessException("Not your referrer.");
        _repo.Delete(referrer);
        await _repo.SaveChangesAsync(ct);
        return true;
    }
}
