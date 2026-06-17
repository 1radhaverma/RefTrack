// RefTrack.Domain/Entities/AppUser.cs
using RefTrack.Domain.Enums;

namespace RefTrack.Domain.Entities;

public class AppUser : BaseEntity
{
    public string Email { get; private set; } = string.Empty;
    public string DisplayName { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public UserRole Role { get; private set; } = UserRole.Member;

    private AppUser() { }

    public static AppUser Create(string email, string displayName, string passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        return new AppUser
        {
            Email = email.ToLower().Trim(),
            DisplayName = displayName,
            PasswordHash = passwordHash
        };
    }

    public bool IsAdmin() => Role == UserRole.Admin;

    public void PromoteToAdmin()
    {
        if (IsAdmin())
            throw new InvalidOperationException("Already Admin.");
        Role = UserRole.Admin;
        SetUpdated();
    }
}