using MediatR;
using RefTrack.Application.Common.DTOs;
using RefTrack.Application.Interface;
using RefTrack.Application.Mappings;

namespace RefTrack.Application.Features.JobRoles.Queries;
public record GetJobsByCompanyQuery(
 Guid CompanyId, Guid UserId) : IRequest<List<JobRoleDto>>;
public class GetJobsByCompanyHandler
 : IRequestHandler<GetJobsByCompanyQuery, List<JobRoleDto>>
{
    private readonly IJobRoleRepository _repo;
    public GetJobsByCompanyHandler(IJobRoleRepository repo) => _repo = repo;
    public async Task<List<JobRoleDto>> Handle(
    GetJobsByCompanyQuery q, CancellationToken ct)
    {
        var roles = await _repo.GetByCompanyAsync(q.CompanyId, ct);
        return roles
        .Where(r => r.UserId == q.UserId)
        .Select(r => r.ToDto())
        .ToList();
    }
}