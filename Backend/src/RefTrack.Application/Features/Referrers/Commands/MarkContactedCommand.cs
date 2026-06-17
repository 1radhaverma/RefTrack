using MediatR;
using RefTrack.Application.Interface;

namespace RefTrack.Application.Features.Referrers.Commands;
public record MarkContactedCommand(
 Guid ReferrerId, Guid UserId) : IRequest<bool>;
public class MarkContactedHandler
 : IRequestHandler<MarkContactedCommand, bool>
{
    private readonly IReferrerRepository _repo;
    public MarkContactedHandler(IReferrerRepository repo) => _repo = repo;
    public async Task<bool> Handle(
    MarkContactedCommand cmd, CancellationToken ct)
    {
        var referrer = await _repo.GetByIdAsync(cmd.ReferrerId, ct)
        ?? throw new KeyNotFoundException("Referrer not found.");
        if (referrer.UserId != cmd.UserId)
            throw new UnauthorizedAccessException("Not your referrer.");
        referrer.Contact(); // throws if already Referred
        _repo.Update(referrer);
        await _repo.SaveChangesAsync(ct);
        return true;
    }
}
