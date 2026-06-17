using MediatR;
using RefTrack.Application.Common.DTOs;
using RefTrack.Application.Interface;
using RefTrack.Application.Mappings;

namespace RefTrack.Application.Features.Referrers.Queries;
public record GetReferrersByJobRoleQuery(
 Guid JobRoleId, Guid UserId) : IRequest<List<ReferrerDto>>;
public class GetReferrersByJobRoleHandler
 : IRequestHandler<GetReferrersByJobRoleQuery, List<ReferrerDto>>
{
    private readonly IReferrerRepository _repo;
    public GetReferrersByJobRoleHandler(IReferrerRepository repo) => _repo = repo;
    public async Task<List<ReferrerDto>> Handle(
    GetReferrersByJobRoleQuery q, CancellationToken ct)
    {
        var referrers = await _repo.GetByJobRoleAsync(q.JobRoleId, ct);
        return referrers
        .Where(r => r.UserId == q.UserId)
        .Select(r => r.ToDto())
        .ToList();
    }
}
