

using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ModuliX.BuildingBlocks.Middlewares
{
    /// <summary>
    /// Handles unhandled exceptions from CQRS handlers (MediatR).
    /// </summary>
    public class CQRSExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CQRSExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var problem = new
                {
                    Title = "Unhandled Exception",
                    Status = context.Response.StatusCode,
                    Message = ex.Message,
                    Source = ex.Source
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
            }
        }
    }
}
