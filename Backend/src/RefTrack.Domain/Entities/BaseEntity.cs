// RefTrack.Domain/Entities/BaseEntity.cs
namespace RefTrack.Domain.Entities;

/// <summary>
/// Abstract base — ALL entities inherit this.
/// SRP: only manages Id + timestamps.
/// Cannot be instantiated directly (abstract).
/// </summary>
public abstract class BaseEntity
{
    // Private setters = Encapsulation (OOP)
    // Nobody outside can set Id directly
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    // Protected = only child classes can call this
    // This is Encapsulation + Inheritance together
    protected void SetUpdated() => UpdatedAt = DateTime.UtcNow;
}