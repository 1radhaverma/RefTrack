using MediatR;
using RefTrack.Domain.Entities;
using RefTrack.Application.Common.DTOs;
using RefTrack.Application.Interface;
using RefTrack.Application.Mappings;
using AppEntity = RefTrack.Domain.Entities.Application;


namespace RefTrack.Application.Features.Applications.Commands;
public record CreateApplicationCommand(
 Guid JobRoleId, Guid UserId) : IRequest<ApplicationDto>;
public class CreateApplicationHandler
 : IRequestHandler<CreateApplicationCommand, ApplicationDto>
{
    private readonly IApplicationRepository _repo;
    public CreateApplicationHandler(IApplicationRepository repo) => _repo = repo;
    public async Task<ApplicationDto> Handle(
    CreateApplicationCommand cmd, CancellationToken ct)
    {
        // FIX: Replace the tuple with your actual Domain Entity creation
        // Note: Change 'JobApplication' to match your exact entity class name if it differs
        var app = AppEntity.Create(cmd.JobRoleId, cmd.UserId);

        // Safely pass the entity to your repository layers
        await _repo.AddAsync(app, ct);
        await _repo.SaveChangesAsync(ct);

        // The extension method .ToDto() will now work perfectly on the real entity
        return app.ToDto();
    }
}
