namespace RefTrack.API.DTOs
{
    public record CreateCompanyRequest(string Name, string? CareerPageUrl, string Tier);
}
