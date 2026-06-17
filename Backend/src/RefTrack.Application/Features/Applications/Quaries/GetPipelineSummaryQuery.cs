using MediatR;
using RefTrack.Application.Interface;

namespace RefTrack.Application.Features.Applications.Queries;
public record GetPipelineSummaryQuery(
 Guid UserId) : IRequest<Dictionary<string, int>>;
public class GetPipelineSummaryHandler
 : IRequestHandler<GetPipelineSummaryQuery, Dictionary<string, int>>
{
    private readonly IApplicationRepository _repo;
    public GetPipelineSummaryHandler(IApplicationRepository repo) => _repo = repo;
    public async Task<Dictionary<string, int>> Handle(
    GetPipelineSummaryQuery q, CancellationToken ct)
    {
        var summary = await _repo.GetPipelineSummaryAsync(q.UserId, ct);
        return summary.ToDictionary(
        kvp => kvp.Key.ToString(),
        kvp => kvp.Value);
    }
}