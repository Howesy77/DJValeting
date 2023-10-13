using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SixB.CarValeting.Application.Commands.CreateBooking;
using SixB.CarValeting.Data.Database;
using SixB.CarValeting.Infrastructure.Enums;

namespace SixB.CarValeting.Application.Tests.Commands
{
    [TestFixture]
    public class CreateBookingCommandTests
    {
        private IValidator<CreateBookingCommand> _validator;
        private CreateBookingCommandHandler _handler;
        private CarValetingContext _carValetingContext;

        [SetUp]
        public void Setup()
        {
            var perTestDatabaseName = Guid.NewGuid();

            _validator = new CreateBookingCommandValidation();

            _carValetingContext = new CarValetingContext(
                new DbContextOptionsBuilder<CarValetingContext>().
                    UseInMemoryDatabase(perTestDatabaseName.ToString()).Options);

            _handler = new CreateBookingCommandHandler(_carValetingContext, _validator,
                Mock.Of<ILogger<CreateBookingCommandHandler>>());
        }

        [TearDown]
        public void Teardown()
        {
            _carValetingContext.Dispose();
        }

        [Test]
        public async Task Handle_WithValidBooking_CreatesBooking()
        {
            // Arrange
            var command = new CreateBookingCommand
            {
                Name = "Test Name",
                Email = "Test@test.com",
                Date = DateTime.Now,
                Flexibility = Flexibility.OneDay,
                VehicleSize = VehicleSize.Large,
                PhoneNumber = "01212121"
            };

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
            });
        }

        [Test]
        public void Handle_WithInvalidBookingEmail_ThrowsException()
        {
            // Assign
            var command = new CreateBookingCommand
            {
                Name = "Test Name",
                Date = DateTime.Now,
                Flexibility = Flexibility.OneDay,
                VehicleSize = VehicleSize.Large,
                PhoneNumber = "01212121"
            };

            // Act
            var ex = Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));

            //Assert
            Assert.That(ex.Errors.Where(e => e.PropertyName == "Email"), Is.Not.Null);
        }
    }
}
