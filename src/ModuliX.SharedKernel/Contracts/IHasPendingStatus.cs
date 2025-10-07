namespace ModuliX.SharedKernel.Contracts;

public interface IHasPendingStatus
{
    bool IsPending { get; }
    string? PendingReason { get; }
}
