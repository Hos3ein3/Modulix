using ModuliX.BuildingBlocks.Context;
using ModuliX.Identity.API;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddIdentityAppSettings();
builder.Services.AddIdentityModule(builder.Configuration);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseIdentityModule();

await app.RunAsync();

