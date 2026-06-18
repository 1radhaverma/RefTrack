namespace RefTrack.API.DTOs
{
    public record CreateReferrerRequest(string Name, string? LinkedInUrl, string? Designation, Guid JobRoleId);

}
