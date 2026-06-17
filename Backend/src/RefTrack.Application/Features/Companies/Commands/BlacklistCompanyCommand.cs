using MediatR;
using RefTrack.Application.Interface;

namespace RefTrack.Application.Features.Companies.Commands;
public record BlacklistCompanyCommand(
 Guid CompanyId, Guid UserId) : IRequest<bool>;
public class BlacklistCompanyHandler
 : IRequestHandler<BlacklistCompanyCommand, bool>
{
    private readonly ICompanyRepository _repo;
    public BlacklistCompanyHandler(ICompanyRepository repo) => _repo = repo;
    public async Task<bool> Handle(
    BlacklistCompanyCommand cmd, CancellationToken ct)
    {
        var company = await _repo.GetByIdAsync(cmd.CompanyId, ct)
        ?? throw new KeyNotFoundException("Company not found.");
        if (company.UserId != cmd.UserId)
            throw new UnauthorizedAccessException("Not your company.");
        company.Blacklist(); // domain method — throws if already blacklisted
        _repo.Update(company);
        await _repo.SaveChangesAsync(ct);
        return true;
    }
}
