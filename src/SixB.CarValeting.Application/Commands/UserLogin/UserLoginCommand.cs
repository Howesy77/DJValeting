using MediatR;

namespace SixB.CarValeting.Application.Commands.UserLogin
{
    public class UserLoginCommand : IRequest<UserLoginCommandResult>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
