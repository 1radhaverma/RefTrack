namespace RefTrack.API.DTOs
{
    public record CreateCompanyRequest(string Name, string? Domain, string Tier);
}
