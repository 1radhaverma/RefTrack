namespace RefTrack.API.DTOs
{
    public record CreateJobRoleRequest(string Title, string? JobUrl, string? JobDescription, string Tier, Guid CompanyId);
   
}
