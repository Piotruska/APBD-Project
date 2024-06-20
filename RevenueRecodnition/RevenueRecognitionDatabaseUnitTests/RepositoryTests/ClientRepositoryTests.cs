using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Modls;
using RevenueRecodnition.Api.Repositories;
using RevenueRecodnition.DataBase.Context;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecognitionDatabaseUnitTests
{
    [TestFixture]
    public class ClientRepositoryTests
    {
        private RRConext _context;
        private ClientRepository _clientRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<RRConext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new RRConext(options);
            _clientRepository = new ClientRepository(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task AddClientAsync_Success()
        {
            // Arrange
            var client = new Client { Address = "123 Street", Email = "test@example.com", PhoneNumber = "123-456-7890" };

            // Act
            await _clientRepository.AddClientAsync(client);
            var result = await _context.Clients.FindAsync(client.IdClient);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(client.Address, result.Address);
            Assert.AreEqual(client.Email, result.Email);
            Assert.AreEqual(client.PhoneNumber, result.PhoneNumber);
        }

        [Test]
        public async Task GetClientWithoutSoftDeletedAsync_ReturnsClient()
        {
            // Arrange
            var client = new Client { Address = "123 Street", Email = "test@example.com", PhoneNumber = "123-456-7890", IsDeleted = false };
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();

            // Act
            var result = await _clientRepository.GetClientWithoutSoftDeletedAsync(client.IdClient);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(client.IdClient, result.IdClient);
        }

        [Test]
        public async Task GetClientWithoutSoftDeletedAsync_ReturnsNull_WhenClientIsDeleted()
        {
            // Arrange
            var client = new Client { Address = "123 Street", Email = "test@example.com", PhoneNumber = "123-456-7890", IsDeleted = true };
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();

            // Act
            var result = await _clientRepository.GetClientWithoutSoftDeletedAsync(client.IdClient);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task SoftDeleteIndividualClientAsync_Success()
        {
            // Arrange
            var client = new Client { Address = "123 Street", Email = "test@example.com", PhoneNumber = "123-456-7890" };
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();

            // Act
            await _clientRepository.SoftDeleteIndividualCLientAsync(client.IdClient);
            var result = await _context.Clients.FindAsync(client.IdClient);

            // Assert
            Assert.IsTrue(result.IsDeleted);
        }

        [Test]
        public async Task UpdateIndividualClientAsync_Success()
        {
            // Arrange
            var client = new Client
            {
                Address = "123 Street",
                Email = "test@example.com",
                PhoneNumber = "123-456-7890",
                IndividualClient = new IndividualClient { FirstName = "John", LastName = "Doe", PESEL = "1234567890" }
            };
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();

            var updateDto = new UpdateIndividualClientDTO
            {
                Address = "456 Avenue",
                Email = "updated@example.com",
                PhoneNumber = "987-654-3210",
                FirstName = "Jane",
                LastName = "Smith"
            };

            // Act
            await _clientRepository.UpdateIndividualCLientAsync(updateDto, client.IdClient);
            var result = await _context.Clients
                .Include(c => c.IndividualClient)
                .FirstOrDefaultAsync(c => c.IdClient == client.IdClient);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updateDto.Address, result.Address);
            Assert.AreEqual(updateDto.Email, result.Email);
            Assert.AreEqual(updateDto.PhoneNumber, result.PhoneNumber);
            Assert.AreEqual(updateDto.FirstName, result.IndividualClient.FirstName);
            Assert.AreEqual(updateDto.LastName, result.IndividualClient.LastName);
        }

        [Test]
        public async Task UpdateCompanyClientAsync_Success()
        {
            // Arrange
            var client = new Client
            {
                Address = "123 Street",
                Email = "test@example.com",
                PhoneNumber = "123-456-7890",
                CompanyClient = new CompanyClient { ComapnyName = "Test Company", KRS = "123456789" }
            };
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();

            var updateDto = new UpdateCompanyClientDto
            {
                Address = "456 Avenue",
                Email = "updated@example.com",
                PhoneNumber = "987-654-3210",
                CompanyName = "Updated Company"
            };

            // Act
            await _clientRepository.UpdateCompanyClientAsync(updateDto, client.IdClient);
            var result = await _context.Clients
                .Include(c => c.CompanyClient)
                .FirstOrDefaultAsync(c => c.IdClient == client.IdClient);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updateDto.Address, result.Address);
            Assert.AreEqual(updateDto.Email, result.Email);
            Assert.AreEqual(updateDto.PhoneNumber, result.PhoneNumber);
            Assert.AreEqual(updateDto.CompanyName, result.CompanyClient.ComapnyName);
        }
    }
}
