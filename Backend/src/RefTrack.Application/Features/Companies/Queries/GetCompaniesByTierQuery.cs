using MediatR;
using RefTrack.Application.Common.DTOs;
using RefTrack.Application.Interface;
using RefTrack.Application.Mappings;

namespace RefTrack.Application.Features.Companies.Queries;
public record GetCompanyByIdQuery(
 Guid CompanyId, Guid UserId) : IRequest<CompanyDto>;
public class GetCompanyByIdHandler
 : IRequestHandler<GetCompanyByIdQuery, CompanyDto>
{
    private readonly ICompanyRepository _repo;
    public GetCompanyByIdHandler(ICompanyRepository repo) => _repo = repo;
    public async Task<CompanyDto> Handle(
    GetCompanyByIdQuery q, CancellationToken ct)
    {
        var company = await _repo.GetByIdAsync(q.CompanyId, ct)
        ?? throw new KeyNotFoundException("Company not found.");
        if (company.UserId != q.UserId)
            throw new UnauthorizedAccessException("Not your company.");
        return company.ToDto();
    }
}
