// RefTrack.Domain/Entities/Referrer.cs
using RefTrack.Domain.Enums;

namespace RefTrack.Domain.Entities;

public class Referrer : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string LinkedInUrl { get; private set; } = string.Empty;
    public string Designation { get; private set; } = string.Empty;
    public OutreachStatus Status { get; private set; }
        = OutreachStatus.NotContacted;
    public DateTime? LastContactedAt { get; private set; }
    public Guid JobRoleId { get; private set; }
    public Guid UserId { get; private set; }

    private Referrer() { }

    public static Referrer Create(
        string name, string linkedInUrl,
        string designation, Guid jobRoleId, Guid userId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(linkedInUrl);
        return new Referrer
        {
            Name = name,
            LinkedInUrl = linkedInUrl,
            Designation = designation,
            JobRoleId = jobRoleId,
            UserId = userId
        };
    }

    // Each status transition is a domain method — SRP
    public void Contact()
    {
        if (Status == OutreachStatus.Referred)
            throw new InvalidOperationException("Already referred.");
        Status = OutreachStatus.Sent;
        LastContactedAt = DateTime.UtcNow; // 5-day countdown starts
        SetUpdated();
    }

    public void MarkSeen() { Status = OutreachStatus.Seen; SetUpdated(); }
    public void MarkReplied() { Status = OutreachStatus.Replied; SetUpdated(); }
    public void MarkGhosted() { Status = OutreachStatus.Ghosted; SetUpdated(); }
    public void Refer() { Status = OutreachStatus.Referred; SetUpdated(); }
}