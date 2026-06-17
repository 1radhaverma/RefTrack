using MediatR;
using RefTrack.Application.Common.DTOs;
using RefTrack.Application.Interface;
using RefTrack.Application.Mappings;


namespace RefTrack.Application.Features.Companies.Queries;
public record GetAllCompaniesQuery(
 Guid UserId) : IRequest<List<CompanyDto>>;
public class GetAllCompaniesHandler
 : IRequestHandler<GetAllCompaniesQuery, List<CompanyDto>>
{
    private readonly ICompanyRepository _repo;
    public GetAllCompaniesHandler(ICompanyRepository repo) => _repo = repo;
    public async Task<List<CompanyDto>> Handle(
    GetAllCompaniesQuery q, CancellationToken ct)
    {
        var companies = await _repo.GetByUserAsync(q.UserId, ct);
        return companies.Select(c => c.ToDto()).ToList();
    }
}