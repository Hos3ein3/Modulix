// using ModuliX.Auth.API;

using Microsoft.Extensions.DependencyInjection;
using ModuliX.Authentication.API;
using ModuliX.BuildingBlocks.Configurations;
using ModuliX.BuildingBlocks.Context;
using ModuliX.Identity.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Configuration.AddAuthenticationAppSettings().AddIdentityAppSettings();
builder.Services.AddOpenApi();
builder.Services.AddAuthenticationModule(builder.Configuration).AddIdentityModule(builder.Configuration);


builder.Services.RegisterAllDbContexts(builder.Configuration,
    DeploymentConfigurations.GetDeploymentConfigurations(builder.Configuration));


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthenticationModule().UseIdentityModule();



app.MapGet("/x", () => "ModuliX API is running 🚀");
//app.ModuliXAuthApiRouter();
await app.RunAsync();


