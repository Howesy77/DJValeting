using FluentValidation;

namespace SixB.CarValeting.Application.Commands.EditBooking
{
    public class EditBookingCommandValidation : AbstractValidator<EditBookingCommand>
    {
        public EditBookingCommandValidation()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.Flexibility).IsInEnum();
            RuleFor(x => x.VehicleSize).IsInEnum();
            RuleFor(x => x.PhoneNumber).NotEmpty();
        }
    }
}
