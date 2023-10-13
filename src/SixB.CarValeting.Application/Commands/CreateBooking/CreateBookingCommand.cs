using MediatR;
using SixB.CarValeting.Infrastructure.Enums;

namespace SixB.CarValeting.Application.Commands.CreateBooking
{
    public class CreateBookingCommand : IRequest
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public Flexibility Flexibility { get; set; }
        public VehicleSize VehicleSize { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
