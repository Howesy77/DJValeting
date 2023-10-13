using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SixB.CarValeting.Data.Entities;

namespace SixB.CarValeting.Data.Database
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Username).IsRequired();
            builder.Property(u => u.Password).IsRequired();

            builder.HasData(new User
            {
                Id = 1,
                Username = "test@email.com",
                Password = "password"
            });
        }
    }
}
