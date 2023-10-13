using FluentValidation;

namespace SixB.CarValeting.Application.Commands.UserLogin
{
    public class UserLoginCommandValidation : AbstractValidator<UserLoginCommand>
    {
        public UserLoginCommandValidation()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
