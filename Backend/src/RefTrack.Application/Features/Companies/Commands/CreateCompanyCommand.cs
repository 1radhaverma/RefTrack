using FluentValidation;
using MediatR;
using RefTrack.Application.Common.DTOs;
using RefTrack.Application.Interface;
using RefTrack.Application.Mappings;
using RefTrack.Domain.Entities;
using RefTrack.Domain.Enums;

namespace RefTrack.Application.Features.Companies.Commands;
public record CreateCompanyCommand(
 string Name,
 string CareerPageUrl,
 string Tier,
 Guid UserId) : IRequest<CompanyDto>;
public class CreateCompanyValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyValidator()
    {
        RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Company name is required.")
        .MaximumLength(200);
        RuleFor(x => x.CareerPageUrl)
        .NotEmpty().WithMessage("Career page URL is required.")
        .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
        .WithMessage("Must be a valid URL.");
        RuleFor(x => x.Tier)
        .Must(t => Enum.TryParse<CompanyTier>(t, true, out _))
        .WithMessage("Tier must be Dream, Stretch, or Safe.");
    }
}
public class CreateCompanyHandler
 : IRequestHandler<CreateCompanyCommand, CompanyDto>
{
    private readonly ICompanyRepository _repo;
    public CreateCompanyHandler(ICompanyRepository repo) => _repo = repo;
    public async Task<CompanyDto> Handle(
    CreateCompanyCommand cmd, CancellationToken ct)
    {
        var tier = Enum.Parse<CompanyTier>(cmd.Tier, true);
        var company = Company.Create(
        cmd.Name, cmd.CareerPageUrl, tier, cmd.UserId);
        await _repo.AddAsync(company, ct);
        await _repo.SaveChangesAsync(ct);
        return company.ToDto();
    }
}
