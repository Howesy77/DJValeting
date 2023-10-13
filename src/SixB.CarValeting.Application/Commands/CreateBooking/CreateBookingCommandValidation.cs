using FluentValidation;

namespace SixB.CarValeting.Application.Commands.CreateBooking
{
    public class CreateBookingCommandValidation : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.Flexibility).IsInEnum();
            RuleFor(x => x.VehicleSize).IsInEnum();
            RuleFor(x => x.PhoneNumber).NotEmpty();
        }
    }
}
