namespace ModuliX.SharedKernel.Contracts;

public interface IHasConcurrencyStamp
{
    string? ConcurrencyStamp { get; }
}
