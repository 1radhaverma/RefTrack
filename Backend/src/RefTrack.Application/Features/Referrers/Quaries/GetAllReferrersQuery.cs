using MediatR;
using RefTrack.Application.Common.DTOs;
using RefTrack.Application.Interface;
using RefTrack.Application.Mappings;

namespace RefTrack.Application.Features.Referrers.Queries;
public record GetAllReferrersQuery(
 Guid UserId) : IRequest<List<ReferrerDto>>;
public class GetAllReferrersHandler
 : IRequestHandler<GetAllReferrersQuery, List<ReferrerDto>>
{
    private readonly IReferrerRepository _repo;
    public GetAllReferrersHandler(IReferrerRepository repo) => _repo = repo;
    public async Task<List<ReferrerDto>> Handle(
    GetAllReferrersQuery q, CancellationToken ct)
    {
        var referrers = await _repo.GetByUserAsync(q.UserId, ct);
        return referrers.Select(r => r.ToDto()).ToList();
    }
}