
using MediatR;

namespace ModuliX.BuildingBlocks.CQRS;
/// <summary>
/// Marker interface for commands that modify state.
/// </summary>
public interface ICommand<out TResponse> : IRequest<TResponse> { }

/// <summary>
/// Marker interface for commands with no response.
/// </summary>
public interface ICommand : IRequest<Unit> { }

/// <summary>
/// Marker interface for queries that return data.
/// </summary>
public interface IQuery<out TResponse> : IRequest<TResponse> { }

/// <summary>
/// Command handler interface.
/// </summary>
public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{ }

/// <summary>
/// Query handler interface.
/// </summary>
public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{ }
