using ModuliX.SharedKernel.Contracts;

namespace ModuliX.SharedKernel;

public abstract class BaseEntity<TId> : IBaseEntity<TId>
{
    public TId Id { get; protected set; } = default!;
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();

    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public string? DeletedByUserId { get; private set; }

    public string ConcurrencyStamp { get; private set; } = Guid.NewGuid().ToString();
    public Guid TrackId { get; private set; } = Guid.NewGuid();

    public bool IsPending { get; private set; }
    public string? PendingReason { get; private set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public Guid? CreatedByUserId { get; private set; }
    public DateTime? LastUpdatedAt { get; private set; }
    public Guid? LastUpdatedByUserId { get; private set; }
    public void RegenerateConcurrencyStamp() => ConcurrencyStamp = Guid.NewGuid().ToString();
    public void MarkCreated(Guid userId)
    {
        CreatedByUserId = userId;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkUpdated(Guid userId)
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

    public void MarkPending(string? reason = null)
    {
        IsPending = true;
        PendingReason = reason;
    }

    public void ClearPending() => IsPending = false;
}

