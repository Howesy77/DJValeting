using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SixB.CarValeting.Data.Entities;

namespace SixB.CarValeting.Data.Database
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Name).IsRequired();
            builder.Property(b => b.PhoneNumber).IsRequired();
            builder.Property(b => b.Email).IsRequired();
            builder.Property(b => b.IsApproved).IsRequired();
            builder.Property(b => b.Date).IsRequired();
            builder.Property(b => b.Flexibility).IsRequired();
            builder.Property(b => b.VehicleSize).IsRequired();
        }
    }
}
