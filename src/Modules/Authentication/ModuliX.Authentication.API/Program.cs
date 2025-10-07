using Microsoft.Extensions.DependencyInjection;
using ModuliX.Authentication.API;
using ModuliX.Authentication.Infrastructure;
using ModuliX.BuildingBlocks.Configurations;
using ModuliX.BuildingBlocks.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAuthenticationAppSettings();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddAuthenticationModule(builder.Configuration);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthenticationModule();

await app.RunAsync();


