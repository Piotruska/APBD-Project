using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RevenueRecodnition.Api.Repositories;
using RevenueRecodnition.DataBase.Context;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecognitionDatabaseUnitTests
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private RRConext _context;
        private UserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<RRConext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new RRConext(options);
            _userRepository = new UserRepository(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task AddNewUserAsync_Success()
        {
            // Arrange
            var user = new User 
            { 
                IdUser = 1, 
                Username = "testuser", 
                Password = "password", 
                Type = "Admin" 
            };

            // Act
            await _userRepository.AddNewUserAsync(user);
            var result = await _context.Users.FindAsync(user.IdUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user.IdUser, result.IdUser);
            Assert.AreEqual(user.Username, result.Username);
        }

        [Test]
        public async Task GetUserAsync_ByUsername_ReturnsUser()
        {
            // Arrange
            var user = new User 
            { 
                IdUser = 1, 
                Username = "testuser", 
                Password = "password", 
                Type = "Admin" 
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.GetUserAsync(user.Username);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user.IdUser, result.IdUser);
            Assert.AreEqual(user.Username, result.Username);
        }

        [Test]
        public async Task GetUserAsync_ByUsername_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var username = "nonexistentuser";

            // Act
            var result = await _userRepository.GetUserAsync(username);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetUserAsync_ById_ReturnsUser()
        {
            // Arrange
            var user = new User 
            { 
                IdUser = 1, 
                Username = "testuser", 
                Password = "password", 
                Type = "Admin" 
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.GetUserAsync(user.IdUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user.IdUser, result.IdUser);
            Assert.AreEqual(user.Username, result.Username);
        }

        [Test]
        public async Task GetUserAsync_ById_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 99;

            // Act
            var result = await _userRepository.GetUserAsync(userId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task DeleteUserAsync_Success()
        {
            // Arrange
            var user = new User 
            { 
                IdUser = 1, 
                Username = "testuser", 
                Password = "password", 
                Type = "Admin" 
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            await _userRepository.DeleteUserAsync(user);
            var result = await _context.Users.FindAsync(user.IdUser);

            // Assert
            Assert.IsNull(result);
        }
    }
}
