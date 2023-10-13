using SixB.CarValeting.Infrastructure.Enums;

namespace SixB.CarValeting.Data.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public Flexibility Flexibility { get; set; }
        public VehicleSize VehicleSize { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsApproved { get; set; }
    }
}
