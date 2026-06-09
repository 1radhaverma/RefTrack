// RefTrack.Application/Mappings/MappingExtensions.cs
using RefTrack.Application.Common.DTOs;
using RefTrack.Application.Scorers;
using RefTrack.Domain.Entities;

namespace RefTrack.Application.Mappings;

// SRP — only handles entity → DTO conversion
// Extension methods keep entity classes clean
public static class MappingExtensions
{
    public static CompanyDto ToDto(this Company c) => new(
        c.Id, c.Name, c.CareerPageUrl,
        c.Tier.ToString(), c.IsBlacklisted, c.CreatedAt);

    public static JobRoleDto ToDto(
        this JobRole r, string companyName = "") => new(
        r.Id, r.Title, r.JobUrl, r.AtsScore,
        r.IsApplied, r.Tier.ToString(),
        r.CompanyId, companyName, r.CreatedAt);

    public static ReferrerDto ToDto(this Referrer r) => new(
        r.Id, r.Name, r.LinkedInUrl, r.Designation,
        r.Status.ToString(), r.LastContactedAt,
        r.JobRoleId, r.CreatedAt);

    public static ApplicationDto ToDto(
        this Domain.Entities.Application a,
        string jobTitle = "",
        string companyName = "") => new(
        a.Id, a.Status.ToString(),
        a.InterviewNotes, a.RejectionReason,
        a.JobRoleId, jobTitle, companyName,
        a.CreatedAt, a.UpdatedAt);

    public static AtsResultDto ToDto(this AtsResult r) => new(
        r.Score, r.MatchPercent,
        r.MissingKeywords, r.MatchedKeywords, r.Summary);
}