using FluentValidation;

namespace SixB.CarValeting.Application.Queries.GetBookingById
{
    public class GetBookingByIdQueryValidation : AbstractValidator<GetBookingByIdQuery>
    {
        public GetBookingByIdQueryValidation()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
