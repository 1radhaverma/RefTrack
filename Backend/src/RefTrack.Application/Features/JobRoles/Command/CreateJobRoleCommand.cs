//// Features/JobRoles/Commands/CreateJobRoleCommand.cs
//using FluentValidation;
//using MediatR;
//using RefTrack.Application.Common.DTOs;
//using RefTrack.Application.Common.Exceptions;
//using RefTrack.Application.Interface;
//using RefTrack.Application.Mappings;
//using RefTrack.Domain.Entities;
//using RefTrack.Domain.Enums;

//namespace RefTrack.Application.Features.JobRoles.Commands;

//public record CreateJobRoleCommand(
//    string Title,
//    string JobUrl,
//    string JobDescription,
//    string Tier,
//    Guid CompanyId,
//    Guid UserId) : IRequest<JobRoleDto>;

//public class CreateJobRoleValidator
//    : AbstractValidator<CreateJobRoleCommand>
//{
//    public CreateJobRoleValidator()
//    {
//        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
//        RuleFor(x => x.JobUrl).NotEmpty();
//        RuleFor(x => x.JobDescription)
//            .NotEmpty()
//            .MinimumLength(50)
//            .WithMessage("Paste the full JD — needed for ATS scoring.");
//        RuleFor(x => x.CompanyId).NotEmpty();
//        RuleFor(x => x.UserId).NotEmpty();
//    }
//}

//public class CreateJobRoleHandler
//    : IRequestHandler<CreateJobRoleCommand, JobRoleDto>
//{
//    private readonly IJobRoleRepository _repo;
//    private readonly ICompanyRepository _companyRepo;

//    public CreateJobRoleHandler(
//        IJobRoleRepository repo,
//        ICompanyRepository companyRepo)
//    {
//        _repo = repo;
//        _companyRepo = companyRepo;
//    }

//    public async Task<JobRoleDto> Handle(
//        CreateJobRoleCommand cmd, CancellationToken ct)
//    {
//        // Verify company exists
//        var company = await _companyRepo.GetByIdAsync(
//            cmd.CompanyId, ct)
//            ?? throw new NotFoundException(
//                nameof(Company), cmd.CompanyId);

//        var tier = Enum.Parse<CompanyTier>(cmd.Tier, true);

//        var role = JobRole.Create(
//            cmd.Title, cmd.JobUrl, cmd.JobDescription,
//            tier, cmd.CompanyId, cmd.UserId);

//        await _repo.AddAsync(role, ct);
//        await _repo.SaveChangesAsync(ct);

//        return role.ToDto(company.Name);
//    }
//}
using FluentValidation;
using MediatR;
using RefTrack.Application.Common.DTOs;
using RefTrack.Application.Interface;
using RefTrack.Application.Mappings;
using RefTrack.Domain.Entities;
using RefTrack.Domain.Enums;
namespace RefTrack.Application.Features.JobRoles.Commands;
public record CreateJobRoleCommand(
 string Title,
 string JobUrl,
 string JobDescription,
 string Tier,
 Guid CompanyId,
 Guid UserId) : IRequest<JobRoleDto>;
public class CreateJobRoleValidator : AbstractValidator<CreateJobRoleCommand>
{
    public CreateJobRoleValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.JobUrl).NotEmpty();
        RuleFor(x => x.Tier)
        .Must(t => Enum.TryParse<CompanyTier>(t, true, out _))
        .WithMessage("Tier must be Dream, Stretch, or Safe.");
    }
}
public class CreateJobRoleHandler
 : IRequestHandler<CreateJobRoleCommand, JobRoleDto>
{
    private readonly IJobRoleRepository _repo;
    public CreateJobRoleHandler(IJobRoleRepository repo) => _repo = repo;
    public async Task<JobRoleDto> Handle(
    CreateJobRoleCommand cmd, CancellationToken ct)
    {
        var tier = Enum.Parse<CompanyTier>(cmd.Tier, true);
        var role = JobRole.Create(
        cmd.Title, cmd.JobUrl, cmd.JobDescription,
        tier, cmd.CompanyId, cmd.UserId);
        await _repo.AddAsync(role, ct);
        await _repo.SaveChangesAsync(ct);
        return role.ToDto();
    }
}
