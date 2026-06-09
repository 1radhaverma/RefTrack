// Features/Applications/Commands/MoveApplicationCommand.cs
using MediatR;
using RefTrack.Application.Common.DTOs;
using RefTrack.Application.Common.Exceptions;
using RefTrack.Application.Interface;
using RefTrack.Application.Mappings;
using RefTrack.Domain.Enums;

namespace RefTrack.Application.Features.Applications.Commands;

public record MoveApplicationCommand(
    Guid ApplicationId,
    string NewStatus,
    string? Note,
    Guid UserId) : IRequest<ApplicationDto>;

public class MoveApplicationHandler
    : IRequestHandler<MoveApplicationCommand, ApplicationDto>
{
    private readonly IApplicationRepository _repo;
    private readonly IJobRoleRepository _jobRoleRepo;

    public MoveApplicationHandler(
        IApplicationRepository repo,
        IJobRoleRepository jobRoleRepo)
    {
        _repo = repo;
        _jobRoleRepo = jobRoleRepo;
    }

    public async Task<ApplicationDto> Handle(
        MoveApplicationCommand cmd, CancellationToken ct)
    {
        var app = await _repo.GetByIdAsync(
            cmd.ApplicationId, ct)
            ?? throw new NotFoundException(
                nameof(Application), cmd.ApplicationId);

        var newStatus = Enum.Parse<ApplicationStatus>(
            cmd.NewStatus, true);

        // STATE PATTERN — validates move, throws if invalid
        app.MoveTo(newStatus);

        // Add interview note if provided
        if (!string.IsNullOrWhiteSpace(cmd.Note))
            app.AddNote(cmd.Note);

        _repo.Update(app);
        await _repo.SaveChangesAsync(ct);

        // Get job title for DTO
        var role = await _jobRoleRepo
            .GetByIdAsync(app.JobRoleId, ct);

        return app.ToDto(role?.Title ?? "", "");
    }
}