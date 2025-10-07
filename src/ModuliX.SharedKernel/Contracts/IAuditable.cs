namespace ModuliX.SharedKernel.Contracts;

public interface IAuditable
{
    DateTime CreatedAt { get; }
    Guid? CreatedByUserId { get; }
    DateTime? LastUpdatedAt { get; }
    Guid? LastUpdatedByUserId { get; }
}
