// Features/Referrers/Commands/AddReferrerCommand.cs
using FluentValidation;
using MediatR;
using RefTrack.Application.Common.DTOs;
using RefTrack.Application.Common.Exceptions;
using RefTrack.Application.Interface;
using RefTrack.Application.Mappings;
using RefTrack.Domain.Entities;

namespace RefTrack.Application.Features.Referrers.Commands;

public record AddReferrerCommand(
    string Name,
    string LinkedInUrl,
    string Designation,
    Guid JobRoleId,
    Guid UserId) : IRequest<ReferrerDto>;

public class AddReferrerValidator
    : AbstractValidator<AddReferrerCommand>
{
    public AddReferrerValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LinkedInUrl)
            .NotEmpty()
            .Must(url => url.Contains("linkedin.com"))
            .WithMessage("Must be a LinkedIn URL.");
        RuleFor(x => x.JobRoleId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}

public class AddReferrerHandler
    : IRequestHandler<AddReferrerCommand, ReferrerDto>
{
    private readonly IReferrerRepository _repo;
    private readonly IJobRoleRepository _jobRoleRepo;

    public AddReferrerHandler(
        IReferrerRepository repo,
        IJobRoleRepository jobRoleRepo)
    {
        _repo = repo;
        _jobRoleRepo = jobRoleRepo;
    }

    public async Task<ReferrerDto> Handle(
        AddReferrerCommand cmd, CancellationToken ct)
    {
        // Verify job role exists before adding referrer
        _ = await _jobRoleRepo.GetByIdAsync(cmd.JobRoleId, ct)
            ?? throw new NotFoundException(
                nameof(JobRole), cmd.JobRoleId);

        var referrer = Referrer.Create(
            cmd.Name, cmd.LinkedInUrl,
            cmd.Designation, cmd.JobRoleId, cmd.UserId);

        await _repo.AddAsync(referrer, ct);
        await _repo.SaveChangesAsync(ct);

        return referrer.ToDto();
    }
}