using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SixB.CarValeting.Data.Database;
using SixB.CarValeting.Data.Entities;

namespace SixB.CarValeting.Application.Commands.CreateBooking
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand>
    {
        private readonly CarValetingContext _carValetingContext;
        private readonly IValidator<CreateBookingCommand> _validator;
        private readonly ILogger<CreateBookingCommandHandler> _logger;

        public CreateBookingCommandHandler(
            CarValetingContext carValetingContext,
            IValidator<CreateBookingCommand> validator,
            ILogger<CreateBookingCommandHandler> logger)
        {
            _carValetingContext = carValetingContext;
            _validator = validator;
            _logger = logger;
        }

        public async Task Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _validator.ValidateAndThrowAsync(request, cancellationToken);

                var booking = new Booking
                {
                    Name = request.Name,
                    Date = request.Date,
                    Email = request.Email,
                    Flexibility = request.Flexibility,
                    PhoneNumber = request.PhoneNumber,
                    VehicleSize = request.VehicleSize,
                    IsApproved = false
                };

                await _carValetingContext.Bookings.AddAsync(booking, cancellationToken);
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
