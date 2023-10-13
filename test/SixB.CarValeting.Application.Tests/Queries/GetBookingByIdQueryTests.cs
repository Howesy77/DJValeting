using AutoFixture;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SixB.CarValeting.Application.Queries.GetBookingById;
using SixB.CarValeting.Data.Database;
using SixB.CarValeting.Data.Entities;

namespace SixB.CarValeting.Application.Tests.Queries
{
    [TestFixture]
    public class GetBookingByIdQueryTests
    {
        private GetBookingByIdQueryHandler _handler;
        private GetBookingByIdQueryValidation _validator;
        private CarValetingContext _carValetingContext;

        [SetUp]
        public void Setup()
        {
            var perTestDatabaseName = Guid.NewGuid();

            _validator = new GetBookingByIdQueryValidation();

            _carValetingContext = new CarValetingContext(
                new DbContextOptionsBuilder<CarValetingContext>().
                    UseInMemoryDatabase(perTestDatabaseName.ToString()).Options);

            _handler = new GetBookingByIdQueryHandler(_carValetingContext, _validator, Mock.Of<ILogger<GetBookingByIdQueryHandler>>());
        }

        [TearDown]
        public void Teardown()
        {
            _carValetingContext.Dispose();
        }

        [Test]
        public async Task Handle_WithValidBookingId_ReturnsCorrectBooking()
        {
            // Arrange
            var fixture = new Fixture();

            var bookings = fixture.Build<Booking>().CreateMany(10);
            var expected = bookings.Skip(4).First();
         
            await _carValetingContext.Bookings.AddRangeAsync(bookings, CancellationToken.None);
            await _carValetingContext.SaveChangesAsync(CancellationToken.None);

            var query = new GetBookingByIdQuery
            {
                Id = expected.Id
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Booking, Is.Not.Null);
                Assert.That(result.Booking.Name, Is.EqualTo(expected.Name));
                Assert.That(result.Booking.Email, Is.EqualTo(expected.Email));
                Assert.That(result.Booking.Date, Is.EqualTo(expected.Date));
                Assert.That(result.Booking.Flexibility, Is.EqualTo(expected.Flexibility));
                Assert.That(result.Booking.VehicleSize, Is.EqualTo(expected.VehicleSize));
                Assert.That(result.Booking.PhoneNumber, Is.EqualTo(expected.PhoneNumber));
                Assert.That(result.Booking.IsApproved, Is.EqualTo(expected.IsApproved));
            });
        }

        [Test]
        public async Task Handle_WithInValidBookingId_ThrowsException()
        {
            // Arrange
            var fixture = new Fixture();

            var bookings = fixture.Build<Booking>().CreateMany(10);
        
            await _carValetingContext.Bookings.AddRangeAsync(bookings, CancellationToken.None);
            await _carValetingContext.SaveChangesAsync(CancellationToken.None);

            var query = new GetBookingByIdQuery();

            // Act + Assert
            Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(query, CancellationToken.None));
        }

        [Test]
        public async Task Handle_WithBookingIdThatDoesntExist_ThrowsException()
        {
            // Arrange
            var fixture = new Fixture();

            var bookings = fixture.Build<Booking>().CreateMany(10);

            await _carValetingContext.Bookings.AddRangeAsync(bookings, CancellationToken.None);
            await _carValetingContext.SaveChangesAsync(CancellationToken.None);

            var query = new GetBookingByIdQuery
            {
                Id = int.MaxValue
            };

            // Act + Assert
            var result = Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(query, CancellationToken.None));
        }
    }
}
