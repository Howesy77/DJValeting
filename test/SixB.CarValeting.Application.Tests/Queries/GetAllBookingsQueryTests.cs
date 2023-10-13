using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SixB.CarValeting.Application.Queries.GetAllBookings;
using SixB.CarValeting.Data.Database;
using SixB.CarValeting.Data.Entities;

namespace SixB.CarValeting.Application.Tests.Queries
{
    [TestFixture]
    public class GetAllBookingsQueryTests
    {
        private GetAllBookingsQueryHandler _handler;
        private CarValetingContext _carValetingContext;

        [SetUp]
        public void Setup()
        {
            var perTestDatabaseName = Guid.NewGuid();

            _carValetingContext = new CarValetingContext(
                new DbContextOptionsBuilder<CarValetingContext>().
                    UseInMemoryDatabase(perTestDatabaseName.ToString()).Options);

            _handler = new GetAllBookingsQueryHandler(_carValetingContext, Mock.Of<ILogger<GetAllBookingsQueryHandler>>());
        }

        [TearDown]
        public void Teardown()
        {
            _carValetingContext.Dispose();
        }

        [Test]
        public async Task Handle_ReturnsAllBookings()
        {
            // Arrange
            var fixture = new Fixture();

            var bookings = fixture.Build<Booking>().CreateMany(10);
            await _carValetingContext.Bookings.AddRangeAsync(bookings, CancellationToken.None);
            await _carValetingContext.SaveChangesAsync(CancellationToken.None);

            var query = new GetAllBookingsQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Bookings.Count, Is.EqualTo(bookings.Count()));

                foreach (var booking in result.Bookings)
                {
                    Assert.That(bookings.Any(b => b.Id == booking.Id), Is.True);
                }
            });
        }
    }
}
