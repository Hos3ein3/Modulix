namespace ModuliX.SharedKernel.Contracts;

public interface ISoftDeletable
{
    bool IsDeleted { get; }
    DateTime? DeletedAt { get; }
    string? DeletedByUserId { get; }
}
