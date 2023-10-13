using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SixB.CarValeting.Data.Database;

namespace SixB.CarValeting.Application.Queries.GetBookingById
{
    public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, GetBookingByIdQueryResult>
    {
        private readonly CarValetingContext _carValetingContext;
        private readonly IValidator<GetBookingByIdQuery> _validator;
        private readonly ILogger<GetBookingByIdQueryHandler> _logger;

        public GetBookingByIdQueryHandler(
            CarValetingContext carValetingContext,
            IValidator<GetBookingByIdQuery> validator,
            ILogger<GetBookingByIdQueryHandler> logger)
        {
            _carValetingContext = carValetingContext;
            _validator = validator;
            _logger = logger;
        }

        public async Task<GetBookingByIdQueryResult> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                await _validator.ValidateAndThrowAsync(request, cancellationToken);

                return new GetBookingByIdQueryResult
                {
                    Booking = await _carValetingContext.Bookings.SingleAsync(b => b.Id == request.Id, cancellationToken)
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
