// RefTrack.Application/Common/DTOs/CompanyDto.cs
namespace RefTrack.Application.Common.DTOs;

// RefTrack.Application/Common/DTOs/JobRoleDto.cs
public record ApplicationDto(
    Guid Id,
    string Status,
    string? InterviewNotes,
    string? RejectionReason,
    Guid JobRoleId,
    string JobTitle,
    string CompanyName,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
