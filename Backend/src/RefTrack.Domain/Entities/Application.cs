using RefTrack.Domain.Enums;

namespace RefTrack.Domain.Entities;

// STATE PATTERN — valid transitions enforced at domain level
// OCP — adding a new status = add to enum + add to dictionary
// SRP — transition logic lives here, not in any controller
public class Application : BaseEntity
{
    public ApplicationStatus Status { get; private set; }
        = ApplicationStatus.Applied;
    public Guid JobRoleId { get; private set; }
    public Guid UserId { get; private set; }
    public string? InterviewNotes { get; private set; }
    public string? RejectionReason { get; private set; }

    // STATE MACHINE — dictionary of valid moves
    // OCP: extend this dictionary when new status added
    private static readonly Dictionary<ApplicationStatus,
        ApplicationStatus[]> ValidTransitions = new()
        {
            [ApplicationStatus.Applied] = [ApplicationStatus.HRScreen,
                                           ApplicationStatus.Rejected],
            [ApplicationStatus.HRScreen] = [ApplicationStatus.TechRound1,
                                           ApplicationStatus.Rejected],
            [ApplicationStatus.TechRound1] = [ApplicationStatus.TechRound2,
                                           ApplicationStatus.Rejected],
            [ApplicationStatus.TechRound2] = [ApplicationStatus.Final,
                                           ApplicationStatus.Rejected],
            [ApplicationStatus.Final] = [ApplicationStatus.Offered,
                                           ApplicationStatus.Rejected],
            [ApplicationStatus.Offered] = [],
            [ApplicationStatus.Rejected] = []
        };

    private Application() { }

    public static Application Create(Guid jobRoleId, Guid userId)
        => new() { JobRoleId = jobRoleId, UserId = userId };

    // STATE PATTERN — this is the core method
    public void MoveTo(ApplicationStatus newStatus)
    {
        // Validate transition — cannot skip stages
        if (!ValidTransitions[Status].Contains(newStatus))
            throw new InvalidOperationException(
                $"Cannot move from {Status} to {newStatus}. " +
                $"Valid moves: {string.Join(", ", ValidTransitions[Status])}");

        Status = newStatus;
        SetUpdated();
    }

    public void AddNote(string note)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(note);
        InterviewNotes = note;
        SetUpdated();
    }

    public void Reject(string reason)
    {
        MoveTo(ApplicationStatus.Rejected);
        RejectionReason = reason;
    }
}