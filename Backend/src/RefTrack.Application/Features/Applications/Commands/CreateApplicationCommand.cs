// Features/Companies/Commands/CreateCompanyCommand.cs
using FluentValidation;
using MediatR;
using RefTrack.Application.Common.DTOs;
using RefTrack.Application.Interface;
using RefTrack.Application.Mappings;
using RefTrack.Domain.Entities;
using RefTrack.Domain.Enums;

namespace RefTrack.Application.Features.Companies.Commands;

// SRP — record holds data, Handler holds logic, Validator holds rules
// DIP — Handler injects ICompanyRepository, not concrete class
public record CreateCompanyCommand(
    string Name,
    string CareerPageUrl,
    string Tier,
    Guid UserId) : IRequest<CompanyDto>;

public class CreateCompanyValidator
    : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Company name is required.")
            .MaximumLength(200);
        RuleFor(x => x.CareerPageUrl)
            .NotEmpty()
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("Must be a valid URL.");
        RuleFor(x => x.UserId).NotEmpty();
    }
}

public class CreateCompanyHandler
    : IRequestHandler<CreateCompanyCommand, CompanyDto>
{
    // DIP — depends on interface, not concrete repo
    private readonly ICompanyRepository _repo;

    public CreateCompanyHandler(ICompanyRepository repo)
        => _repo = repo;

    public async Task<CompanyDto> Handle(
        CreateCompanyCommand cmd, CancellationToken ct)
    {
        // Parse tier from string
        var tier = Enum.Parse<CompanyTier>(cmd.Tier, true);

        // Factory method creates entity — OOP
        var company = Company.Create(
            cmd.Name, cmd.CareerPageUrl, tier, cmd.UserId);

        await _repo.AddAsync(company, ct);
        await _repo.SaveChangesAsync(ct);

        return company.ToDto();
    }
}