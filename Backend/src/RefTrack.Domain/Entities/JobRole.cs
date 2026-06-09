// RefTrack.Domain/Entities/JobRole.cs
using RefTrack.Domain.Enums;

namespace RefTrack.Domain.Entities;

public class JobRole : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public string JobUrl { get; private set; } = string.Empty;
    public string JobDescription { get; private set; } = string.Empty;
    public int AtsScore { get; private set; }
    public bool IsApplied { get; private set; }
    public CompanyTier Tier { get; private set; }
    public Guid CompanyId { get; private set; }
    public Guid UserId { get; private set; }

    private JobRole() { }

    public static JobRole Create(
        string title, string jobUrl,
        string jobDescription, CompanyTier tier,
        Guid companyId, Guid userId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        return new JobRole
        {
            Title = title,
            JobUrl = jobUrl,
            JobDescription = jobDescription,
            Tier = tier,
            CompanyId = companyId,
            UserId = userId
        };
    }

    // SRP — score saving is this entity's responsibility
    public void SaveAtsScore(int score)
    {
        if (score < 0 || score > 100)
            throw new ArgumentOutOfRangeException(
                nameof(score), "Score must be 0-100.");
        AtsScore = score;
        SetUpdated();
    }

    public void Apply()
    {
        if (IsApplied)
            throw new InvalidOperationException(
                "Already applied to this role.");
        IsApplied = true;
        SetUpdated();
    }
}