using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SixB.CarValeting.Data.Database;

namespace SixB.CarValeting.Application.Commands.EditBooking
{
    public class EditBookingCommandHandler : IRequestHandler<EditBookingCommand>
    {
        private readonly CarValetingContext _carValetingContext;
        private readonly IValidator<EditBookingCommand> _validator;
        private readonly ILogger<EditBookingCommandHandler> _logger;

        public EditBookingCommandHandler(
            CarValetingContext carValetingContext,
            IValidator<EditBookingCommand> validator,
            ILogger<EditBookingCommandHandler> logger)
        {
            _carValetingContext = carValetingContext;
            _validator = validator;
            _logger = logger;
        }

        public async Task Handle(EditBookingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _validator.ValidateAndThrowAsync(request, cancellationToken);

                var booking = await _carValetingContext.Bookings.SingleAsync(b => b.Id == request.Id, cancellationToken);

                booking.Name = request.Name;
                booking.Email = request.Email;
                booking.Date = request.Date;
                booking.Flexibility = request.Flexibility;
                booking.VehicleSize = request.VehicleSize;
                booking.IsApproved = request.IsApproved;
                booking.PhoneNumber = request.PhoneNumber;

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
