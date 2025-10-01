using ModuliX.Auth.API.Features.LoginByGoogle;
namespace ModuliX.Auth.API;

public static class Router
{
    public static void ModuliXAuthApiRouter(this WebApplication app)
    {
        app.MapGet("/login-by-google", LoginByGoogle.Handle);
    }

}
