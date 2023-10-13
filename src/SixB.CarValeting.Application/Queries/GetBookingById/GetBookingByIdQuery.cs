using MediatR;

namespace SixB.CarValeting.Application.Queries.GetBookingById
{
    public class GetBookingByIdQuery : IRequest<GetBookingByIdQueryResult>
    {
        public int Id { get; set; }
    }
}
