using MediatR;

namespace SixB.CarValeting.Application.Commands.DeleteBooking
{
    public class DeleteBookingCommand : IRequest
    {
        public int Id { get; set; }
    }
}
