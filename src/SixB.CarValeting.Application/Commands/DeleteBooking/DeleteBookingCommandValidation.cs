using FluentValidation;

namespace SixB.CarValeting.Application.Commands.DeleteBooking
{
    public class DeleteBookingCommandValidation : AbstractValidator<DeleteBookingCommand>
    {
        public DeleteBookingCommandValidation()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
