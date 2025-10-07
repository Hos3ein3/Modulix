
using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace ModuliX.BuildingBlocks.Middlewares;
/// <summary>
/// Middleware to handle FluentValidation exceptions globally.
/// </summary>
public class FluentValidationMiddleware
{
    private readonly RequestDelegate _next;

    public FluentValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            var problem = new
            {
                Title = "Validation Failed",
                Status = 400,
                Errors = errors
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
}
