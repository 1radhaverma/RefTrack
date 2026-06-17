using MediatR;
using RefTrack.Application.Common.DTOs;
using RefTrack.Application.Interface;
using RefTrack.Application.Mappings;

namespace RefTrack.Application.Features.JobRoles.Queries;
public record GetAllJobRolesQuery(
 Guid UserId) : IRequest<List<JobRoleDto>>;
public class GetAllJobRolesHandler
 : IRequestHandler<GetAllJobRolesQuery, List<JobRoleDto>>
{
    private readonly IJobRoleRepository _repo;
    public GetAllJobRolesHandler(IJobRoleRepository repo) => _repo = repo;
    public async Task<List<JobRoleDto>> Handle(
    GetAllJobRolesQuery q, CancellationToken ct)
    {
        var roles = await _repo.GetByUserAsync(q.UserId, ct);
        return roles.Select(r => r.ToDto()).ToList();
    }
}
