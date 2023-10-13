using AutoFixture;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SixB.CarValeting.Application.Commands.UserLogin;
using SixB.CarValeting.Data.Database;
using SixB.CarValeting.Data.Entities;

namespace SixB.CarValeting.Application.Tests.Commands
{
    [TestFixture]
    public class UserLoginCommandTests
    {
        private IValidator<UserLoginCommand> _validator;
        private UserLoginCommandHandler _handler;
        private CarValetingContext _carValetingContext;

        [SetUp]
        public void Setup()
        {
            var perTestDatabaseName = Guid.NewGuid();

            _validator = new UserLoginCommandValidation();

            _carValetingContext = new CarValetingContext(
                new DbContextOptionsBuilder<CarValetingContext>().
                    UseInMemoryDatabase(perTestDatabaseName.ToString()).Options);

            _handler = new UserLoginCommandHandler(_carValetingContext, _validator,
                Mock.Of<ILogger<UserLoginCommandHandler>>());
        }

        [TearDown]
        public void Teardown()
        {
            _carValetingContext.Dispose();
        }

        [Test]
        public async Task Handle_WithValidUser_ReturnsSuccessfulLogin()
        {
            // Arrange
            var fixture = new Fixture();

            var users = fixture.Build<User>().CreateMany(10);
            var expected = users.Skip(4).First();
            await _carValetingContext.Users.AddRangeAsync(users, CancellationToken.None);
            await _carValetingContext.SaveChangesAsync(CancellationToken.None);

            var command = new UserLoginCommand
            {
                Username = expected.Username,
                Password = expected.Password
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.Username, Is.EqualTo(expected.Username));
            Assert.That(result.IsLoggedIn, Is.True);
        }

        [Test]
        public async Task Handle_WithInvalidUser_ReturnsFailedLogin()
        {
            // Arrange
            var fixture = new Fixture();

            var users = fixture.Build<User>().CreateMany(10);
            var expected = users.Skip(4).First();
            await _carValetingContext.Users.AddRangeAsync(users, CancellationToken.None);
            await _carValetingContext.SaveChangesAsync(CancellationToken.None);

            var command = new UserLoginCommand
            {
                Username = expected.Username,
                Password = "NOT_THE_PASSWORD"
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.Username, Is.EqualTo(""));
            Assert.That(result.IsLoggedIn, Is.False);
        }
    }
}
