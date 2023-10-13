using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SixB.CarValeting.Data.Database;

namespace SixB.CarValeting.Application.Queries.GetAllBookings
{
    public class GetAllBookingsQueryHandler : IRequestHandler<GetAllBookingsQuery, GetAllBookingsQueryResult>
    {
        private readonly CarValetingContext _carValetingContext;
        private readonly ILogger<GetAllBookingsQueryHandler> _logger;

        public GetAllBookingsQueryHandler(
            CarValetingContext carValetingContext,
            ILogger<GetAllBookingsQueryHandler> logger)
        {
            _carValetingContext = carValetingContext;
            _logger = logger;
        }

        public async Task<GetAllBookingsQueryResult> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return new GetAllBookingsQueryResult
                {
                    Bookings = await _carValetingContext.Bookings.OrderBy(b => b.Date).ToListAsync(cancellationToken)
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
