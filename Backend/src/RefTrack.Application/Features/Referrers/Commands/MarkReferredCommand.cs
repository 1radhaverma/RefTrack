using MediatR;
using RefTrack.Application.Interface;

namespace RefTrack.Application.Features.Referrers.Commands;
public record MarkReferredCommand(
 Guid ReferrerId, Guid UserId) : IRequest<bool>;
public class MarkReferredHandler
 : IRequestHandler<MarkReferredCommand, bool>
{
    private readonly IReferrerRepository _repo;
    public MarkReferredHandler(IReferrerRepository repo) => _repo = repo;
    public async Task<bool> Handle(
    MarkReferredCommand cmd, CancellationToken ct)
    {
        var referrer = await _repo.GetByIdAsync(cmd.ReferrerId, ct)
        ?? throw new KeyNotFoundException("Referrer not found.");
        if (referrer.UserId != cmd.UserId)
            throw new UnauthorizedAccessException("Not your referrer.");
        referrer.Refer(); // entity method name is Refer(), not MarkReferred()
        _repo.Update(referrer);
        await _repo.SaveChangesAsync(ct);
        return true;
    }
}

