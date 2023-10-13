using MediatR;
using System.ComponentModel.DataAnnotations;

namespace SixB.CarValeting.Application.Commands.UserLogin
{
    public class UserLoginCommand : IRequest<UserLoginCommandResult>
    {
        [Required(ErrorMessage = "Please enter your email address"), EmailAddress(ErrorMessage = "Please enter a valid email address"), Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your password"), Display(Name = "Password")]
        public string Password { get; set; }
    }
}