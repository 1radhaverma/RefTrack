// Features/Referrers/Commands/ContactReferrerCommand.cs
using MediatR;
using RefTrack.Application.Common.DTOs;
using RefTrack.Application.Common.Exceptions;
using RefTrack.Application.Interface;
using RefTrack.Application.Mappings;
using RefTrack.Domain.Entities;

namespace RefTrack.Application.Features.Referrers.Commands;

public record ContactReferrerCommand(
    Guid ReferrerId,
    Guid UserId) : IRequest<ReferrerDto>;

public class ContactReferrerHandler
    : IRequestHandler<ContactReferrerCommand, ReferrerDto>
{
    private readonly IReferrerRepository _repo;

    public ContactReferrerHandler(IReferrerRepository repo)
        => _repo = repo;

    public async Task<ReferrerDto> Handle(
        ContactReferrerCommand cmd, CancellationToken ct)
    {
        var referrer = await _repo.GetByIdAsync(
            cmd.ReferrerId, ct)
            ?? throw new NotFoundException(
                nameof(Referrer), cmd.ReferrerId);

        // Domain method — sets Status=Sent + LastContactedAt=Now
        // 5-day countdown starts here
        referrer.Contact();

        _repo.Update(referrer);
        await _repo.SaveChangesAsync(ct);

        return referrer.ToDto();
    }
}