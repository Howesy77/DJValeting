using MediatR;
using SixB.CarValeting.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;

namespace SixB.CarValeting.Application.Commands.CreateBooking
{
    public class CreateBookingCommand : IRequest
    {
        [Required(ErrorMessage = "Please enter a booking name"), Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a booking date"), Display(Name = "Date"), DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please enter the booking flexibility"), Display(Name = "Flexibility")]
        public Flexibility Flexibility { get; set; }

        [Required(ErrorMessage = "Please enter the booking vehicle size"), Display(Name = "Vehicle Size")]
        public VehicleSize VehicleSize { get; set; }

        [Required(ErrorMessage = "Please enter a booking phone number"), Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter a booking email address"), EmailAddress(ErrorMessage = "Please enter a valid email address"), Display(Name = "Email")]
        public string Email { get; set; }
    }
}