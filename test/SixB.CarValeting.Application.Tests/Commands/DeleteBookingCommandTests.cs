using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SixB.CarValeting.Application.Commands.DeleteBooking;
using SixB.CarValeting.Data.Database;
using SixB.CarValeting.Data.Entities;
using SixB.CarValeting.Infrastructure.Enums;

namespace SixB.CarValeting.Application.Tests.Commands
{
    [TestFixture]
    public class DeleteBookingCommandTests
    {
        private IValidator<DeleteBookingCommand> _validator;
        private DeleteBookingCommandHandler _handler;
        private CarValetingContext _carValetingContext;

        [SetUp]
        public void Setup()
        {
            var perTestDatabaseName = Guid.NewGuid();

            _validator = new DeleteBookingCommandValidation();

            _carValetingContext = new CarValetingContext(
                new DbContextOptionsBuilder<CarValetingContext>().
                    UseInMemoryDatabase(perTestDatabaseName.ToString()).Options);

            _handler = new DeleteBookingCommandHandler(_carValetingContext, _validator,
                Mock.Of<ILogger<DeleteBookingCommandHandler>>());
        }

        [TearDown]
        public void Teardown()
        {
            _carValetingContext.Dispose();
        }

        [Test]
        public async Task Handle_WithValidBooking_DeletesBooking()
        {
            // Arrange
            var command = new DeleteBookingCommand
            {
                Id = 1
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
            var booking = await _carValetingContext.Bookings.SingleOrDefaultAsync(b => b.Id == command.Id, CancellationToken.None);
            Assert.That(booking, Is.Null);
        }

        [Test]
        public async Task Handle_WithInvalidBookingEmail_ThrowsException()
        {
            // Arrange
            var command = new DeleteBookingCommand
            {
                Id = 2
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

            // Act + Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
