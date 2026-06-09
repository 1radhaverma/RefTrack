// RefTrack.Application/Common/DTOs/CompanyDto.cs
namespace RefTrack.Application.Common.DTOs;

// RefTrack.Application/Common/DTOs/JobRoleDto.cs
public record JobRoleDto(
    Guid Id,
    string Title,
    string JobUrl,
    int AtsScore,
    bool IsApplied,
    string Tier,
    Guid CompanyId,
    string CompanyName,
    DateTime CreatedAt
);
