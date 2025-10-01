

namespace ModuliX.Auth.API.Features.LoginByGoogle;

public record LoginByGoogleRequestDto
{
    public string GoogleId { get; set; }
    public string FCMToken { get; set; }

}
public record LoginByGoogleResponseDto
{
    public string UserId { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string Name { get; set; }


}

