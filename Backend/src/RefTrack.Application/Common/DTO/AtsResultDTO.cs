// RefTrack.Application/Common/DTOs/CompanyDto.cs
namespace RefTrack.Application.Common.DTOs;

// RefTrack.Application/Common/DTOs/JobRoleDto.cs
public record AtsResultDto(
    int Score,
    int MatchPercent,
    List<string> MissingKeywords,
    List<string> MatchedKeywords,
    string Summary
);
