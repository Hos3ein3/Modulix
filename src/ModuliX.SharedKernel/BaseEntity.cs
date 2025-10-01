
namespace ModuliX.SharedKernel;

public abstract class BaseEntity<TId>
{
    public TId Id { get; protected set; } = default!;

    // 🔹 Audit Fields
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? LastUpdatedAt { get; private set; }
    public string? CreatedByUserId { get; private set; }
    public string? LastUpdatedByUserId { get; private set; }

    // 🔹 Soft delete (optional, but often useful)
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public string? DeletedByUserId { get; private set; }

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();

    // 🔹 Helpers for updating audit fields
    public void MarkCreated(string userId)
    {
        CreatedByUserId = userId;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkUpdated(string userId)
    {
        LastUpdatedByUserId = userId;
        LastUpdatedAt = DateTime.UtcNow;
    }

    public void MarkDeleted(string userId)
    {
        IsDeleted = true;
        DeletedByUserId = userId;
        DeletedAt = DateTime.UtcNow;
    }
}

