using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SixB.CarValeting.Data.Database;

namespace SixB.CarValeting.Application.Commands.DeleteBooking
{
    public class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommand>
    {
        private readonly CarValetingContext _carValetingContext;
        private readonly IValidator<DeleteBookingCommand> _validator;
        private readonly ILogger<DeleteBookingCommandHandler> _logger;

        public DeleteBookingCommandHandler(
            CarValetingContext carValetingContext,
            IValidator<DeleteBookingCommand> validator,
            ILogger<DeleteBookingCommandHandler> logger)
        {
            _carValetingContext = carValetingContext;
            _validator = validator;
            _logger = logger;
        }

        public async Task Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _validator.ValidateAndThrowAsync(request, cancellationToken);

                var booking = await _carValetingContext.Bookings.SingleAsync(b => b.Id == request.Id, CancellationToken.None);
               
                _carValetingContext.Bookings.Remove(booking);
                await _carValetingContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}
