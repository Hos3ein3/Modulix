using FluentValidation;
namespace ModuliX.Auth.API.Features.LoginByGoogle
{
    public class LoginByGoogleValidation : AbstractValidator<LoginByGoogleRequestDto>
    {
        public LoginByGoogleValidation()
        {

            RuleFor(x => x.GoogleId).NotEmpty().WithMessage("");
            RuleFor(x => x.FCMToken).NotEmpty().WithMessage("");
        }
    }
}
