using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SixB.CarValeting.Application.Commands.EditBooking;
using SixB.CarValeting.Data.Database;
using SixB.CarValeting.Data.Entities;
using SixB.CarValeting.Infrastructure.Enums;

namespace SixB.CarValeting.Application.Tests.Commands
{
    [TestFixture]
    public class EditBookingCommandTests
    {
        private IValidator<EditBookingCommand> _validator;
        private EditBookingCommandHandler _handler;
        private CarValetingContext _carValetingContext;

        [SetUp]
        public void Setup()
        {
            var perTestDatabaseName = Guid.NewGuid();

            _validator = new EditBookingCommandValidation();

            _carValetingContext = new CarValetingContext(
                new DbContextOptionsBuilder<CarValetingContext>().
                    UseInMemoryDatabase(perTestDatabaseName.ToString()).Options);

            _handler = new EditBookingCommandHandler(_carValetingContext, _validator,
                Mock.Of<ILogger<EditBookingCommandHandler>>());
        }

        [TearDown]
        public void Teardown()
        {
            _carValetingContext.Dispose();
        }

        [Test]
        public async Task Handle_WithValidBooking_EditsBooking()
        {
            // Arrange
            var command = new EditBookingCommand
            {
                Id = 1,
                Name = "Edit Name",
                Email = "Edit@test.com",
                Date = DateTime.Now.AddDays(1),
                Flexibility = Flexibility.TwoDays,
                VehicleSize = VehicleSize.Medium,
                PhoneNumber = "234",
                IsApproved = true
            };

            var existing = new Booking
            {
                Id = 1,
                Name = "Test Name",
                Email = "Test@test.com",
                Date = DateTime.Now,
                Flexibility = Flexibility.OneDay,
                VehicleSize = VehicleSize.Large,
                PhoneNumber = "01212121",
                IsApproved = false
            };

            await _carValetingContext.Bookings.AddAsync(existing, CancellationToken.None);
            await _carValetingContext.SaveChangesAsync(CancellationToken.None);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            var booking = await _carValetingContext.Bookings.SingleAsync(CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(booking, Is.Not.Null);
                Assert.That(booking.Name, Is.EqualTo(command.Name));
                Assert.That(booking.Email, Is.EqualTo(command.Email));
                Assert.That(booking.Date, Is.EqualTo(command.Date));
                Assert.That(booking.Flexibility, Is.EqualTo(command.Flexibility));
                Assert.That(booking.VehicleSize, Is.EqualTo(command.VehicleSize));
                Assert.That(booking.PhoneNumber, Is.EqualTo(command.PhoneNumber));
                Assert.That(booking.IsApproved, Is.EqualTo(command.IsApproved));
            });
        }

        [Test]
        public void Handle_WithInvalidBookingPhoneNumber_ThrowsException()
        {
            // Assign
            var command = new EditBookingCommand
            {
                Name = "Test Name",
                Date = DateTime.Now,
                Email = "email@email.com",
                Flexibility = Flexibility.OneDay,
                VehicleSize = VehicleSize.Large,
                IsApproved = false
            };

            // Act
            var ex = Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));

            //Assert
            Assert.That(ex.Errors.Where(e => e.PropertyName == "PhoneNumber"), Is.Not.Null);
        }
    }
}
