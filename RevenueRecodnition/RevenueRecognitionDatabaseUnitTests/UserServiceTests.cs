using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RevenueRecodnition.Api.Exeptions;
using RevenueRecodnition.Api.Helpers;
using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Repositories;
using RevenueRecodnition.Api.Services;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecognitionDatabaseUnitTests
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object);
        }

        [Test]
        public async Task AddNewUserAsync_Success()
        {
            // Arrange
            var dto = new AddUserDTO
            {
                Username = "newuser",
                Password = "password123",
                Type = "User"
            };

            _mockUserRepository.Setup(r => r.GetUserAsync(dto.Username)).ReturnsAsync((User)null);
            _mockUserRepository.Setup(r => r.AddNewUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            await _userService.AddNewUserAsync(dto);

            // Assert
            _mockUserRepository.Verify(r => r.GetUserAsync(dto.Username), Times.Once);
            _mockUserRepository.Verify(r => r.AddNewUserAsync(It.Is<User>(u =>
                u.Username == dto.Username &&
                u.Type == dto.Type)), Times.Once);
        }

        [Test]
        public void AddNewUserAsync_ThrowsBadRequestExeption_WhenUsernameAlreadyExists()
        {
            // Arrange
            var dto = new AddUserDTO
            {
                Username = "existinguser",
                Password = "password123",
                Type = "User"
            };

            var existingUser = new User { Username = dto.Username };

            _mockUserRepository.Setup(r => r.GetUserAsync(dto.Username)).ReturnsAsync(existingUser);

            // Act & Assert
            var ex = Assert.ThrowsAsync<BadRequestExeption>(async () => await _userService.AddNewUserAsync(dto));
            Assert.AreEqual($"User with username '{dto.Username}' already exists.", ex.Message);
        }

        [Test]
        public void AddNewUserAsync_ThrowsBadRequestExeption_WhenUserTypeIsInvalid()
        {
            // Arrange
            var dto = new AddUserDTO
            {
                Username = "newuser",
                Password = "password123",
                Type = "InvalidType"
            };

            _mockUserRepository.Setup(r => r.GetUserAsync(dto.Username)).ReturnsAsync((User)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<BadRequestExeption>(async () => await _userService.AddNewUserAsync(dto));
            Assert.AreEqual("Wrong user type. Type : [Admin , User]", ex.Message);
        }
    }
}
