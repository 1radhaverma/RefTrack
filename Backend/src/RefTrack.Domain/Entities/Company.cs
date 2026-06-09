// RefTrack.Domain/Entities/Company.cs
using RefTrack.Domain.Enums;

namespace RefTrack.Domain.Entities;

public class Company : BaseEntity  // Inheritance — gets Id, CreatedAt free
{
    // Encapsulation — all private setters
    public string Name { get; private set; } = string.Empty;
    public string CareerPageUrl { get; private set; } = string.Empty;
    public CompanyTier Tier { get; private set; } = CompanyTier.Stretch;
    public bool IsBlacklisted { get; private set; }
    public Guid UserId { get; private set; }

    // EF Core needs this — keep private
    private Company() { }

    // Factory Method (OOP) — controlled creation
    // SRP: creation logic in one place
    public static Company Create(
        string name, string careerPageUrl,
        CompanyTier tier, Guid userId)
    {
        // Validation inside domain — not in controller
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return new Company
        {
            Name = name,
            CareerPageUrl = careerPageUrl,
            Tier = tier,
            UserId = userId
        };
    }

    // Domain method — business logic belongs here (SRP)
    public void Blacklist()
    {
        if (IsBlacklisted)
            throw new InvalidOperationException(
                $"{Name} is already blacklisted.");
        IsBlacklisted = true;
        SetUpdated(); // inherited from BaseEntity
    }

    public void UpdateTier(CompanyTier newTier)
    {
        Tier = newTier;
        SetUpdated();
    }
}