// RefTrack.Application/Common/DTOs/CompanyDto.cs
namespace RefTrack.Application.Common.DTOs;

// RefTrack.Application/Common/DTOs/JobRoleDto.cs
public record ReferrerDto(
    Guid Id,
    string Name,
    string LinkedInUrl,
    string Designation,
    string Status,
    DateTime? LastContactedAt,
    Guid JobRoleId,
    DateTime CreatedAt
);
