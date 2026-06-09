// Features/AtsScoring/Commands/CheckAtsScoreCommand.cs
using FluentValidation;
using MediatR;
using RefTrack.Application.Common.DTOs;
using RefTrack.Application.Common.Exceptions;
using RefTrack.Application.Interface;
using RefTrack.Application.Mappings;
using RefTrack.Application.Scorers;
using RefTrack.Domain.Entities;

namespace RefTrack.Application.Features.AtsScoring.Commands;

public record CheckAtsScoreCommand(
    string ResumeText,
    Guid JobRoleId,
    Guid UserId,
    string ScorerType = "keyword") : IRequest<AtsResultDto>;

public class CheckAtsScoreValidator
    : AbstractValidator<CheckAtsScoreCommand>
{
    public CheckAtsScoreValidator()
    {
        RuleFor(x => x.ResumeText)
            .NotEmpty()
            .MinimumLength(100)
            .WithMessage(
                "Paste your full resume text, not just a summary.");
        RuleFor(x => x.JobRoleId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}

public class CheckAtsScoreHandler
    : IRequestHandler<CheckAtsScoreCommand, AtsResultDto>
{
    private readonly IJobRoleRepository _jobRoleRepo;

    public CheckAtsScoreHandler(IJobRoleRepository jobRoleRepo)
        => _jobRoleRepo = jobRoleRepo;

    public async Task<AtsResultDto> Handle(
        CheckAtsScoreCommand cmd, CancellationToken ct)
    {
        // Load saved JD from job role
        var role = await _jobRoleRepo
            .GetByIdAsync(cmd.JobRoleId, ct)
            ?? throw new NotFoundException(
                nameof(JobRole), cmd.JobRoleId);

        // STRATEGY PATTERN — factory picks right scorer
        // cmd.ScorerType = "keyword" or "title"
        var scorer = AtsScorerFactory.Create(cmd.ScorerType);

        // Score resume against saved JD
        var result = scorer.Score(
            cmd.ResumeText, role.JobDescription);

        // Save ATS score on the job role entity
        role.SaveAtsScore(result.Score);
        _jobRoleRepo.Update(role);
        await _jobRoleRepo.SaveChangesAsync(ct);

        return result.ToDto();
    }
}