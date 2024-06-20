using System;
using Moq;
using RevenueRecodnition.Api.Exeptions;
using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Modls;
using RevenueRecodnition.Api.Repositories.Interfaces;
using RevenueRecodnition.Api.Services;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecognitionDatabaseUnitTests
{
    [TestFixture]
    public class ClientServiceTests
    {
        private Mock<IClientRepository> _mockClientRepository;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private ClientService _clientService;

        [SetUp]
        public void Setup()
        {
            _mockClientRepository = new Mock<IClientRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _clientService = new ClientService(_mockClientRepository.Object, _mockUnitOfWork.Object);
        }

        [Test]
        public async Task AddIndividualClientAsync_Success()
        {
            // Arrange
            var dto = new AddIndividualClientDTO
            {
                Address = "Address",
                Email = "email@example.com",
                PhoneNumber = "123456789",
                FirstName = "FirstName",
                LastName = "LastName",
                PESEL = "12345678901"
            };

            _mockClientRepository.Setup(r => r.AddClientAsync(It.IsAny<Client>())).Returns(Task.CompletedTask);
            _mockClientRepository.Setup(r => r.AddIndividualClientAsync(It.IsAny<IndividualClient>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CommitTransactionAsync()).Returns(Task.CompletedTask);

            // Act
            await _clientService.AddIndividualClientAsync(dto);

            // Assert
            _mockClientRepository.Verify(r => r.AddClientAsync(It.IsAny<Client>()), Times.Once);
            _mockClientRepository.Verify(r => r.AddIndividualClientAsync(It.IsAny<IndividualClient>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.BeginTransactionAsync(), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitTransactionAsync(), Times.Once);
        }

        [Test]
        public void AddIndividualClientAsync_RollsBackTransaction_OnException()
        {
            // Arrange
            var dto = new AddIndividualClientDTO
            {
                Address = "Address",
                Email = "email@example.com",
                PhoneNumber = "123456789",
                FirstName = "FirstName",
                LastName = "LastName",
                PESEL = "12345678901"
            };

            _mockClientRepository.Setup(r => r.AddClientAsync(It.IsAny<Client>())).Throws(new Exception());
            _mockUnitOfWork.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.RollbackTransactionAsync()).Returns(Task.CompletedTask);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _clientService.AddIndividualClientAsync(dto));
            _mockUnitOfWork.Verify(u => u.RollbackTransactionAsync(), Times.Once);
        }

        [Test]
        public async Task AddCompanyClientAsync_Success()
        {
            // Arrange
            var dto = new AddCompanyClientDTO
            {
                Address = "Address",
                Email = "email@example.com",
                PhoneNumber = "123456789",
                CompanyName = "CompanyName",
                KRS = "KRS123456"
            };

            _mockClientRepository.Setup(r => r.AddClientAsync(It.IsAny<Client>())).Returns(Task.CompletedTask);
            _mockClientRepository.Setup(r => r.AddCompanyClientAsync(It.IsAny<CompanyClient>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CommitTransactionAsync()).Returns(Task.CompletedTask);

            // Act
            await _clientService.AddCompanyClientAsync(dto);

            // Assert
            _mockClientRepository.Verify(r => r.AddClientAsync(It.IsAny<Client>()), Times.Once);
            _mockClientRepository.Verify(r => r.AddCompanyClientAsync(It.IsAny<CompanyClient>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.BeginTransactionAsync(), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitTransactionAsync(), Times.Once);
        }

        [Test]
        public void AddCompanyClientAsync_RollsBackTransaction_OnException()
        {
            // Arrange
            var dto = new AddCompanyClientDTO
            {
                Address = "Address",
                Email = "email@example.com",
                PhoneNumber = "123456789",
                CompanyName = "CompanyName",
                KRS = "KRS123456"
            };

            _mockClientRepository.Setup(r => r.AddClientAsync(It.IsAny<Client>())).Throws(new Exception());
            _mockUnitOfWork.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.RollbackTransactionAsync()).Returns(Task.CompletedTask);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _clientService.AddCompanyClientAsync(dto));
            _mockUnitOfWork.Verify(u => u.RollbackTransactionAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateIndividualClientAsync_Success()
        {
            // Arrange
            var dto = new UpdateIndividualClientDTO { FirstName = "NewFirstName" };
            var clientId = 1;
            var client = new Client
            {
                IdClient = clientId,
                IndividualClient = new IndividualClient { IdClient = clientId }
            };

            _mockClientRepository.Setup(r => r.GetClientWithSoftDeletedAsync(clientId)).ReturnsAsync(client);
            _mockClientRepository.Setup(r => r.GetClientWithSoftDeletedAllInfoAsync(clientId)).ReturnsAsync(client);
            _mockClientRepository.Setup(r => r.UpdateIndividualCLientAsync(dto, clientId)).Returns(Task.CompletedTask);

            // Act
            await _clientService.UpdateIndividualCLientAsync(dto, clientId);

            // Assert
            _mockClientRepository.Verify(r => r.UpdateIndividualCLientAsync(dto, clientId), Times.Once);
        }

        [Test]
        public void UpdateIndividualClientAsync_ThrowsNotFoundException_WhenClientNotFound()
        {
            // Arrange
            var dto = new UpdateIndividualClientDTO { FirstName = "NewFirstName" };
            var clientId = 1;

            _mockClientRepository.Setup(r => r.GetClientWithSoftDeletedAsync(clientId)).ReturnsAsync((Client)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundExeption>(async () => await _clientService.UpdateIndividualCLientAsync(dto, clientId));
        }

        [Test]
        public void UpdateIndividualClientAsync_ThrowsNotFoundException_WhenClientIsSoftDeleted()
        {
            // Arrange
            var dto = new UpdateIndividualClientDTO { FirstName = "NewFirstName" };
            var clientId = 1;
            var client = new Client { IdClient = clientId, IsDeleted = true };

            _mockClientRepository.Setup(r => r.GetClientWithSoftDeletedAsync(clientId)).ReturnsAsync(client);
            _mockClientRepository.Setup(r => r.GetClientWithSoftDeletedAllInfoAsync(clientId)).ReturnsAsync(client);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundExeption>(async () => await _clientService.UpdateIndividualCLientAsync(dto, clientId));
        }

        [Test]
        public void UpdateIndividualClientAsync_ThrowsBadRequestException_WhenClientIsCompany()
        {
            // Arrange
            var dto = new UpdateIndividualClientDTO { FirstName = "NewFirstName" };
            var clientId = 1;
            var client = new Client
            {
                IdClient = clientId,
                CompanyClient = new CompanyClient { IdClient = clientId }
            };

            _mockClientRepository.Setup(r => r.GetClientWithSoftDeletedAsync(clientId)).ReturnsAsync(client);
            _mockClientRepository.Setup(r => r.GetClientWithSoftDeletedAllInfoAsync(clientId)).ReturnsAsync(client);

            // Act & Assert
            Assert.ThrowsAsync<BadRequestExeption>(async () => await _clientService.UpdateIndividualCLientAsync(dto, clientId));
        }

        [Test]
        public async Task UpdateCompanyClientAsync_Success()
        {
            // Arrange
            var dto = new UpdateCompanyClientDto { CompanyName = "NewCompanyName" };
            var clientId = 1;
            var client = new Client
            {
                IdClient = clientId,
                CompanyClient = new CompanyClient { IdClient = clientId }
            };

            _mockClientRepository.Setup(r => r.GetClientWithSoftDeletedAsync(clientId)).ReturnsAsync(client);
            _mockClientRepository.Setup(r => r.GetClientWithSoftDeletedAllInfoAsync(clientId)).ReturnsAsync(client);
            _mockClientRepository.Setup(r => r.UpdateCompanyClientAsync(dto, clientId)).Returns(Task.CompletedTask);

            // Act
            await _clientService.UpdateCompanyClientAsync(dto, clientId);

            // Assert
            _mockClientRepository.Verify(r => r.UpdateCompanyClientAsync(dto, clientId), Times.Once);
        }

        [Test]
        public void UpdateCompanyClientAsync_ThrowsNotFoundException_WhenClientNotFound()
        {
            // Arrange
            var dto = new UpdateCompanyClientDto { CompanyName = "NewCompanyName" };
            var clientId = 1;

            _mockClientRepository.Setup(r => r.GetClientWithSoftDeletedAsync(clientId)).ReturnsAsync((Client)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundExeption>(async () => await _clientService.UpdateCompanyClientAsync(dto, clientId));
        }

        [Test]
        public void UpdateCompanyClientAsync_ThrowsNotFoundException_WhenClientIsSoftDeleted()
        {
            // Arrange
            var dto = new UpdateCompanyClientDto { CompanyName = "NewCompanyName" };
            var clientId = 1;
            var client = new Client { IdClient = clientId, IsDeleted = true };

            _mockClientRepository.Setup(r => r.GetClientWithSoftDeletedAsync(clientId)).ReturnsAsync(client);
            _mockClientRepository.Setup(r => r.GetClientWithSoftDeletedAllInfoAsync(clientId)).ReturnsAsync(client);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundExeption>(async () => await _clientService.UpdateCompanyClientAsync(dto, clientId));
        }

        [Test]
        public void UpdateCompanyClientAsync_ThrowsBadRequestException_WhenClientIsIndividual()
        {
            // Arrange
            var dto = new UpdateCompanyClientDto { CompanyName = "NewCompanyName" };
            var clientId = 1;
            var client = new Client
            {
                IdClient = clientId,
                IndividualClient = new IndividualClient { IdClient = clientId }
            };

            _mockClientRepository.Setup(r => r.GetClientWithSoftDeletedAsync(clientId)).ReturnsAsync(client);
            _mockClientRepository.Setup(r => r.GetClientWithSoftDeletedAllInfoAsync(clientId)).ReturnsAsync(client);

            // Act & Assert
            Assert.ThrowsAsync<BadRequestExeption>(async () => await _clientService.UpdateCompanyClientAsync(dto, clientId));
        }

        [Test]
        public async Task SoftDeleteIndividualClientAsync_Success()
        {
            // Arrange
            var clientId = 1;
            var client = new Client
            {
                IdClient = clientId,
                IndividualClient = new IndividualClient { IdClient = clientId }
            };

            _mockClientRepository.Setup(r => r.GetClientWithoutSoftDeletedAsync(clientId)).ReturnsAsync(client);
            _mockClientRepository.Setup(r => r.SoftDeleteIndividualCLientAsync(clientId)).Returns(Task.CompletedTask);

            // Act
            await _clientService.SoftDeleteIndividualCLientAsync(clientId);

            // Assert
            _mockClientRepository.Verify(r => r.SoftDeleteIndividualCLientAsync(clientId), Times.Once);
        }

        [Test]
        public void SoftDeleteIndividualClientAsync_ThrowsNotFoundException_WhenClientNotFound()
        {
            // Arrange
            var clientId = 1;

            _mockClientRepository.Setup(r => r.GetClientWithoutSoftDeletedAsync(clientId)).ReturnsAsync((Client)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundExeption>(async () => await _clientService.SoftDeleteIndividualCLientAsync(clientId));
        }
    }
}
