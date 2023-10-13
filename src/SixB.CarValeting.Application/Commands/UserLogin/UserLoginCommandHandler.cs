using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SixB.CarValeting.Data.Database;

namespace SixB.CarValeting.Application.Commands.UserLogin
{
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, UserLoginCommandResult>
    {
        private readonly CarValetingContext _carValetingContext;
        private readonly IValidator<UserLoginCommand> _validator;
        private readonly ILogger<UserLoginCommandHandler> _logger;

        public UserLoginCommandHandler(
            CarValetingContext carValetingContext,
            IValidator<UserLoginCommand> validator,
            ILogger<UserLoginCommandHandler> logger)
        {
            _carValetingContext = carValetingContext;
            _validator = validator;
            _logger = logger;
        }

        public async Task<UserLoginCommandResult> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _validator.ValidateAndThrowAsync(request, cancellationToken);

                var user = await _carValetingContext.Users.SingleOrDefaultAsync(u => u.Username == request.Username && u.Password == request.Password, CancellationToken.None);

                return new UserLoginCommandResult
                {
                    Username = user?.Username ?? "",
                    IsLoggedIn = user != null
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}
