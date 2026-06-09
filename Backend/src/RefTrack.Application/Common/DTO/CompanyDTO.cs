// RefTrack.Application/Common/DTOs/CompanyDto.cs
namespace RefTrack.Application.Common.DTOs;

public record CompanyDto(
    Guid Id,
    string Name,
    string CareerPageUrl,
    string Tier,
    bool IsBlacklisted,
    DateTime CreatedAt
);