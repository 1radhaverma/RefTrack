using FluentValidation;
using MediatR;
using RefTrack.Application.Common.DTOs;
using RefTrack.Application.Interface;
using RefTrack.Application.Mappings;
using RefTrack.Domain.Entities;

namespace RefTrack.Application.Features.Referrers.Commands;
public record CreateReferrerCommand(
 string Name,
 string LinkedInUrl,
 string Designation,
 Guid JobRoleId,
 Guid UserId) : IRequest<ReferrerDto>;
public class CreateReferrerValidator : AbstractValidator<CreateReferrerCommand>
{
    public CreateReferrerValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.LinkedInUrl)
        .NotEmpty()
        .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
        .WithMessage("Must be a valid LinkedIn URL.");
    }
}
public class CreateReferrerHandler
 : IRequestHandler<CreateReferrerCommand, ReferrerDto>
{
    private readonly IReferrerRepository _repo;
    public CreateReferrerHandler(IReferrerRepository repo) => _repo = repo;
    public async Task<ReferrerDto> Handle(
    CreateReferrerCommand cmd, CancellationToken ct)
    {
        var referrer = Referrer.Create(
        cmd.Name, cmd.LinkedInUrl, cmd.Designation,
        cmd.JobRoleId, cmd.UserId);
        await _repo.AddAsync(referrer, ct);
        await _repo.SaveChangesAsync(ct);
        return referrer.ToDto();
    }
}

