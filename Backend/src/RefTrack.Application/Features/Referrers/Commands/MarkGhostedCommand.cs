using MediatR;
using RefTrack.Application.Interface;

namespace RefTrack.Application.Features.Referrers.Commands;
public record MarkGhostedCommand(
 Guid ReferrerId, Guid UserId) : IRequest<bool>;
public class MarkGhostedHandler
 : IRequestHandler<MarkGhostedCommand, bool>
{
    private readonly IReferrerRepository _repo;
    public MarkGhostedHandler(IReferrerRepository repo) => _repo = repo;
    public async Task<bool> Handle(
    MarkGhostedCommand cmd, CancellationToken ct)
    {
        var referrer = await _repo.GetByIdAsync(cmd.ReferrerId, ct)
        ?? throw new KeyNotFoundException("Referrer not found.");
        if (referrer.UserId != cmd.UserId)
            throw new UnauthorizedAccessException("Not your referrer.");
        referrer.MarkGhosted();
        _repo.Update(referrer);
        await _repo.SaveChangesAsync(ct);
        return true;
    }
}
