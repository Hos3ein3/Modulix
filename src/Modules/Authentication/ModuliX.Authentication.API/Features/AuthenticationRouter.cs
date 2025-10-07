
namespace ModuliX.Authentication.API.Features;

public static class AuthenticationRouter
{
    public static IEndpointRouteBuilder MapAuthenticationRouter(this IEndpointRouteBuilder app)
    {
        app.MapGet("/auth", () => "ModuliX.Authentication.API");

        var group = app.MapGroup("/auth").WithTags("Authentication");

        group.MapGet("/signup/google", SignUpByGoogle.SignUpByGoogle.Handle)
             .WithName("SignUpByGoogle")
             .Produces<string>(StatusCodes.Status200OK)
             .WithSummary("Sign up using Google account")
             .WithDescription("This endpoint allows users to sign up using their Google account.");

        return app;
    }
}
