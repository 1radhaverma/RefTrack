using MediatR;
using RefTrack.Application.Interface;

namespace RefTrack.Application.Features.Referrers.Commands;
public record MarkRepliedCommand(
 Guid ReferrerId, Guid UserId) : IRequest<bool>;
public class MarkRepliedHandler
 : IRequestHandler<MarkRepliedCommand, bool>
{
    private readonly IReferrerRepository _repo;
    public MarkRepliedHandler(IReferrerRepository repo) => _repo = repo;
    public async Task<bool> Handle(
    MarkRepliedCommand cmd, CancellationToken ct)
    {
        var referrer = await _repo.GetByIdAsync(cmd.ReferrerId, ct)
        ?? throw new KeyNotFoundException("Referrer not found.");
        if (referrer.UserId != cmd.UserId)
            throw new UnauthorizedAccessException("Not your referrer.");
        referrer.MarkReplied();
        _repo.Update(referrer);
        await _repo.SaveChangesAsync(ct);
        return true;
    }
}
