using MediatR;
using RefTrack.Application.Interface;

namespace RefTrack.Application.Features.Companies.Commands;
public record DeleteCompanyCommand(
 Guid CompanyId, Guid UserId) : IRequest<bool>;
public class DeleteCompanyHandler
 : IRequestHandler<DeleteCompanyCommand, bool>
{
    private readonly ICompanyRepository _repo;
    public DeleteCompanyHandler(ICompanyRepository repo) => _repo = repo;
    public async Task<bool> Handle(
    DeleteCompanyCommand cmd, CancellationToken ct)
    {
        var company = await _repo.GetByIdAsync(cmd.CompanyId, ct)
        ?? throw new KeyNotFoundException("Company not found.");
        if (company.UserId != cmd.UserId)
            throw new UnauthorizedAccessException("Not your company.");
        _repo.Delete(company);
        await _repo.SaveChangesAsync(ct);
        return true;
    }
}
