
namespace ModuliX.SharedKernel.Contracts;

public interface IBaseEntity<TId> : IAuditable, ISoftDeletable, IHasConcurrencyStamp
{
    TId Id { get; }
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    void ClearDomainEvents();
    void RegenerateConcurrencyStamp();

    void MarkCreated(Guid userId);
    void MarkUpdated(Guid userId);
    void MarkDeleted(string userId);

    void MarkPending(string? reason = null);
    void ClearPending();
}
