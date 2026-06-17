using MediatR;
using RefTrack.Application.Common.DTOs;
using RefTrack.Application.Interface;
using RefTrack.Application.Mappings;

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
        var app = Application.Create(cmd.JobRoleId, cmd.UserId);
        await _repo.AddAsync(app, ct);
        await _repo.SaveChangesAsync(ct);
        return app.ToDto();
    }
}