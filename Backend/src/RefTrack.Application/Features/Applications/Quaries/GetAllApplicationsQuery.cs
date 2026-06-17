using MediatR;
using RefTrack.Application.Common.DTOs;
using RefTrack.Application.Interface;

using RefTrack.Application.Mappings;
namespace RefTrack.Application.Features.Applications.Queries;
public record GetAllApplicationsQuery(
 Guid UserId) : IRequest<List<ApplicationDto>>;
public class GetAllApplicationsHandler
 : IRequestHandler<GetAllApplicationsQuery, List<ApplicationDto>>
{
    private readonly IApplicationRepository _repo;
    public GetAllApplicationsHandler(IApplicationRepository repo) => _repo = repo;
    public async Task<List<ApplicationDto>> Handle(
    GetAllApplicationsQuery q, CancellationToken ct)
    {
        var apps = await _repo.GetByUserAsync(q.UserId, ct);
        return apps.Select(a => a.ToDto()).ToList();
    }
}