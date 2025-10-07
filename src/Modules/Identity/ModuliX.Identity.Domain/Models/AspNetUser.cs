
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using ModuliX.SharedKernel;
using ModuliX.SharedKernel.Contracts;

namespace ModuliX.Identity.Domain.Models;

public class AspNetUser : IdentityUser<Guid>, IBaseEntity<Guid>
{
    private readonly BaseEntity<Guid> _entity = new InternalEntity();
    
    public string GoogleId { get; set; }
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _entity.DomainEvents;

    [NotMapped]
    public DateTime CreatedAt => throw new NotImplementedException();
    [NotMapped]
    public Guid? CreatedByUserId => throw new NotImplementedException();

    [NotMapped] public DateTime? LastUpdatedAt => throw new NotImplementedException();
    [NotMapped]
    public Guid? LastUpdatedByUserId => throw new NotImplementedException();

    [NotMapped]
    public bool IsDeleted => throw new NotImplementedException();
    [NotMapped]
    public DateTime? DeletedAt => throw new NotImplementedException();

    [NotMapped] public string? DeletedByUserId => throw new NotImplementedException();



    public void ClearDomainEvents() => _entity.ClearDomainEvents();
    public void RegenerateConcurrencyStamp() => _entity.RegenerateConcurrencyStamp();
    public void MarkCreated(Guid userId) => _entity.MarkCreated(userId);
    public void MarkUpdated(Guid userId) => _entity.MarkUpdated(userId);
    public void MarkDeleted(string userId) => _entity.MarkDeleted(userId);
    public void MarkPending(string? reason = null) => _entity.MarkPending(reason);
    public void ClearPending() => _entity.ClearPending();
    private sealed class InternalEntity : BaseEntity<Guid> { }
}

