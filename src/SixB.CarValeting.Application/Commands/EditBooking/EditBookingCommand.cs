using SixB.CarValeting.Application.Commands.CreateBooking;

namespace SixB.CarValeting.Application.Commands.EditBooking
{
    public class EditBookingCommand : CreateBookingCommand
    {
        public int Id { get; set; }
        public bool IsApproved { get; set; }
    }
}
