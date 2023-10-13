using System.ComponentModel.DataAnnotations;

namespace SixB.CarValeting.Infrastructure.Enums
{
    public enum Flexibility
    {
        [Display(Name = "+/- 1 Day")]
        OneDay,
        [Display(Name = "+/- 2 Days")]
        TwoDays,
        [Display(Name = "+/- 3 Days")]
        ThreeDays
    }
}