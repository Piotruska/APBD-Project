using System;
using Moq;
using RevenueRecodnition.Api.Exeptions;
using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Repositories.Interfaces;
using RevenueRecodnition.Api.Services;
using RevenueRecodnition.Api.Services.Interfaces;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecognitionDatabaseUnitTests
{
    [TestFixture]
    public class ContractServiceTests
    {
        private Mock<IExchangeRateService> _mockExchangeRateService;
        private Mock<IClientRepository> _mockClientRepository;
        private Mock<IContracrRepository> _mockContractRepository;
        private Mock<IProductRepository> _mockProductRepository;
        private Mock<IDicountRepository> _mockDiscountRepository;
        private Mock<ISubscriptionRepository> _mockSubscriptionRepository;
        private Mock<IPayementRepository> _mockPaymentRepository;
        private ContractService _contractService;

        [SetUp]
        public void Setup()
        {
            _mockExchangeRateService = new Mock<IExchangeRateService>();
            _mockClientRepository = new Mock<IClientRepository>();
            _mockContractRepository = new Mock<IContracrRepository>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockDiscountRepository = new Mock<IDicountRepository>();
            _mockSubscriptionRepository = new Mock<ISubscriptionRepository>();
            _mockPaymentRepository = new Mock<IPayementRepository>();
            _contractService = new ContractService(
                _mockExchangeRateService.Object,
                _mockClientRepository.Object,
                _mockContractRepository.Object,
                _mockProductRepository.Object,
                _mockDiscountRepository.Object,
                _mockSubscriptionRepository.Object,
                _mockPaymentRepository.Object);
        }

        [Test]
        public async Task CreateContractAsync_Success()
        {
            // Arrange
            var dto = new CreateContractDTO
            {
                ClientId = 1,
                ProductId = 1,
                TimePeriodForPayement = 10,
                ContractLengthInYears = 2,
                AdditionalSupportTimeInYears = 1
            };

            var client = new Client
            {
                IdClient = dto.ClientId,
                Contracts = new System.Collections.Generic.List<Contract>(),
                Subscriptions = new System.Collections.Generic.List<Subscription>()
            };

            var product = new Product
            {
                IdProduct = dto.ProductId,
                BasePrice = 500
            };

            var discount = new Discount
            {
                Percentage = 10
            };

            _mockClientRepository.Setup(r => r.GetClientWithoutSoftDeletedAsync(dto.ClientId)).ReturnsAsync(client);
            _mockClientRepository.Setup(r => r.GetClientWithoutSoftDeletedAllInfoAsync(dto.ClientId)).ReturnsAsync(client);
            _mockProductRepository.Setup(r => r.GetProductAsync(dto.ProductId)).ReturnsAsync(product);
            _mockDiscountRepository.Setup(r => r.GetCurrentHighestDiscountAsync()).ReturnsAsync(discount);
            _mockSubscriptionRepository.Setup(r => r.GetActiveSubscriptionsForProductAsync(dto.ProductId, dto.ClientId)).ReturnsAsync((Subscription)null);
            _mockContractRepository.Setup(r => r.GetActiveContractForProductAsync(dto.ProductId, dto.ClientId)).ReturnsAsync((Contract)null);
            _mockContractRepository.Setup(r => r.AddContractAsync(It.IsAny<Contract>())).ReturnsAsync(1);

            // Act
            var result = await _contractService.CreateContractAsync(dto);

            // Assert
            Assert.AreEqual(1, result);
        }

        [Test]
        public void CreateContractAsync_ThrowsNotFoundException_WhenClientNotFound()
        {
            // Arrange
            var dto = new CreateContractDTO { ClientId = 1 };

            _mockClientRepository.Setup(r => r.GetClientWithoutSoftDeletedAsync(dto.ClientId)).ReturnsAsync((Client)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundExeption>(async () => await _contractService.CreateContractAsync(dto));
        }

        [Test]
        public void CreateContractAsync_ThrowsNotFoundException_WhenProductNotFound()
        {
            // Arrange
            var dto = new CreateContractDTO { ClientId = 1, ProductId = 1 };

            var client = new Client { IdClient = dto.ClientId };

            _mockClientRepository.Setup(r => r.GetClientWithoutSoftDeletedAsync(dto.ClientId)).ReturnsAsync(client);
            _mockClientRepository.Setup(r => r.GetClientWithoutSoftDeletedAllInfoAsync(dto.ClientId)).ReturnsAsync(client);
            _mockProductRepository.Setup(r => r.GetProductAsync(dto.ProductId)).ReturnsAsync((Product)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundExeption>(async () => await _contractService.CreateContractAsync(dto));
        }

        [Test]
        public void CreateContractAsync_ThrowsBadRequestException_WhenClientHasActiveSubscriptionOrContract()
        {
            // Arrange
            var dto = new CreateContractDTO { ClientId = 1, ProductId = 1 };

            var client = new Client { IdClient = dto.ClientId };
            var product = new Product { IdProduct = dto.ProductId };

            var subscription = new Subscription();
            var contract = new Contract();

            _mockClientRepository.Setup(r => r.GetClientWithoutSoftDeletedAsync(dto.ClientId)).ReturnsAsync(client);
            _mockClientRepository.Setup(r => r.GetClientWithoutSoftDeletedAllInfoAsync(dto.ClientId)).ReturnsAsync(client);
            _mockProductRepository.Setup(r => r.GetProductAsync(dto.ProductId)).ReturnsAsync(product);
            _mockSubscriptionRepository.Setup(r => r.GetActiveSubscriptionsForProductAsync(dto.ProductId, dto.ClientId)).ReturnsAsync(subscription);
            _mockContractRepository.Setup(r => r.GetActiveContractForProductAsync(dto.ProductId, dto.ClientId)).ReturnsAsync(contract);

            // Act & Assert
            Assert.ThrowsAsync<BadRequestExeption>(async () => await _contractService.CreateContractAsync(dto));
        }

 [Test]
        public void CreateContractAsync_ThrowsBadRequestException_WhenAdditionalSupportTimeOutOfRange()
        {
            // Arrange
            var dto = new CreateContractDTO
            {
                ClientId = 1,
                ProductId = 1,
                ContractLengthInYears = 5,
                TimePeriodForPayement = 10,
                AdditionalSupportTimeInYears = 4  // Invalid value (greater than 3)
            };

            var client = new Client { IdClient = dto.ClientId, Contracts = new System.Collections.Generic.List<Contract>(), Subscriptions = new System.Collections.Generic.List<Subscription>() };
            var product = new Product { IdProduct = dto.ProductId, BasePrice = 100 };

            _mockClientRepository.Setup(r => r.GetClientWithoutSoftDeletedAsync(dto.ClientId)).ReturnsAsync(client);
            _mockClientRepository.Setup(r => r.GetClientWithoutSoftDeletedAllInfoAsync(dto.ClientId)).ReturnsAsync(client);
            _mockProductRepository.Setup(r => r.GetProductAsync(dto.ProductId)).ReturnsAsync(product);
            _mockSubscriptionRepository.Setup(r => r.GetActiveSubscriptionsForProductAsync(dto.ProductId, dto.ClientId)).ReturnsAsync((Subscription)null);
            _mockContractRepository.Setup(r => r.GetActiveContractForProductAsync(dto.ProductId, dto.ClientId)).ReturnsAsync((Contract)null);
            _mockDiscountRepository.Setup(r => r.GetCurrentHighestDiscountAsync()).ReturnsAsync(new Discount { Percentage = 10 });

            // Act & Assert
            var ex = Assert.ThrowsAsync<BadRequestExeption>(async () => await _contractService.CreateContractAsync(dto));
            Assert.AreEqual("Support can only be [0,1,2,3] years", ex.Message);

            // Arrange for value less than 0
            dto.AdditionalSupportTimeInYears = -1;  // Invalid value (less than 0)

            // Act & Assert
            ex = Assert.ThrowsAsync<BadRequestExeption>(async () => await _contractService.CreateContractAsync(dto));
            Assert.AreEqual("Support can only be [0,1,2,3] years", ex.Message);
        }

        [Test]
        public void CreateContractAsync_ThrowsBadRequestException_WhenTimePeriodForPayementOutOfRange()
        {
            // Arrange
            var dto = new CreateContractDTO
            {
                ClientId = 1,
                ProductId = 1,
                ContractLengthInYears = 1,
                TimePeriodForPayement = 2  // Invalid value (less than 3)
            };

            var client = new Client { IdClient = dto.ClientId, Contracts = new System.Collections.Generic.List<Contract>(), Subscriptions = new System.Collections.Generic.List<Subscription>() };
            var product = new Product { IdProduct = dto.ProductId, BasePrice = 100 };

            _mockClientRepository.Setup(r => r.GetClientWithoutSoftDeletedAsync(dto.ClientId)).ReturnsAsync(client);
            _mockClientRepository.Setup(r => r.GetClientWithoutSoftDeletedAllInfoAsync(dto.ClientId)).ReturnsAsync(client);
            _mockProductRepository.Setup(r => r.GetProductAsync(dto.ProductId)).ReturnsAsync(product);
            _mockSubscriptionRepository.Setup(r => r.GetActiveSubscriptionsForProductAsync(dto.ProductId, dto.ClientId)).ReturnsAsync((Subscription)null);
            _mockContractRepository.Setup(r => r.GetActiveContractForProductAsync(dto.ProductId, dto.ClientId)).ReturnsAsync((Contract)null);
            _mockDiscountRepository.Setup(r => r.GetCurrentHighestDiscountAsync()).ReturnsAsync(new Discount { Percentage = 10 });

            // Act & Assert
            var ex = Assert.ThrowsAsync<BadRequestExeption>(async () => await _contractService.CreateContractAsync(dto));
            Assert.AreEqual("TimePeriod cannot be less then 3 or larger then 30", ex.Message);

            // Arrange for value greater than 30
            dto.TimePeriodForPayement = 31;  // Invalid value (greater than 30)

            // Act & Assert
            ex = Assert.ThrowsAsync<BadRequestExeption>(async () => await _contractService.CreateContractAsync(dto));
            Assert.AreEqual("TimePeriod cannot be less then 3 or larger then 30", ex.Message);
        }
        [Test]
        public async Task IssuePaymentForContractAsync_Success()
        {
            // Arrange
            var dto = new PaymentForContractDTO
            {
                contractID = 1,
                Amount = 500
            };

            var contract = new Contract
            {
                IdContract = dto.contractID,
                Price = 1000,
                IsSigned = false,
                EndDatePayement = DateTime.Now.AddDays(10)
            };

            var payments = new System.Collections.Generic.List<Payment>
            {
                new Payment { Amount = 500 }
            };

            _mockContractRepository.Setup(r => r.GetContractAsync(dto.contractID)).ReturnsAsync(contract);
            _mockPaymentRepository.Setup(r => r.GetPayementsForContractAsync(dto.contractID)).ReturnsAsync(payments);
            _mockPaymentRepository.Setup(r => r.AddPaymentAsync(It.IsAny<Payment>())).Returns(Task.CompletedTask);
            _mockContractRepository.Setup(r => r.SignContractAsync(dto.contractID)).Returns(Task.CompletedTask);

            // Act
            await _contractService.IssuePayementForContractAsync(dto);

            // Assert
            _mockPaymentRepository.Verify(r => r.AddPaymentAsync(It.IsAny<Payment>()), Times.Once);
            _mockContractRepository.Verify(r => r.SignContractAsync(dto.contractID), Times.Once);
        }

        [Test]
        public void IssuePaymentForContractAsync_ThrowsNotFoundException_WhenContractNotFound()
        {
            // Arrange
            var dto = new PaymentForContractDTO { contractID = 1 };

            _mockContractRepository.Setup(r => r.GetContractAsync(dto.contractID)).ReturnsAsync((Contract)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundExeption>(async () => await _contractService.IssuePayementForContractAsync(dto));
        }

        [Test]
        public void IssuePaymentForContractAsync_ThrowsBadRequestException_WhenContractIsSigned()
        {
            // Arrange
            var dto = new PaymentForContractDTO { contractID = 1 };

            var contract = new Contract
            {
                IdContract = dto.contractID,
                IsSigned = true
            };

            _mockContractRepository.Setup(r => r.GetContractAsync(dto.contractID)).ReturnsAsync(contract);

            // Act & Assert
            Assert.ThrowsAsync<BadRequestExeption>(async () => await _contractService.IssuePayementForContractAsync(dto));
        }

        [Test]
        public void IssuePaymentForContractAsync_ThrowsBadRequestException_WhenPaymentDateExpired()
        {
            // Arrange
            var dto = new PaymentForContractDTO { contractID = 1 };

            var contract = new Contract
            {
                IdContract = dto.contractID,
                EndDatePayement = DateTime.Now.AddDays(-1)
            };

            _mockContractRepository.Setup(r => r.GetContractAsync(dto.contractID)).ReturnsAsync(contract);

            // Act & Assert
            Assert.ThrowsAsync<BadRequestExeption>(async () => await _contractService.IssuePayementForContractAsync(dto));
        }

        [Test]
        public void IssuePaymentForContractAsync_ThrowsBadRequestException_WhenPaymentAmountTooLarge()
        {
            // Arrange
            var dto = new PaymentForContractDTO
            {
                contractID = 1,
                Amount = 600
            };

            var contract = new Contract
            {
                IdContract = dto.contractID,
                Price = 1000,
                EndDatePayement = DateTime.Now.AddDays(10)
            };

            var payments = new System.Collections.Generic.List<Payment>
            {
                new Payment { Amount = 500 }
            };

            _mockContractRepository.Setup(r => r.GetContractAsync(dto.contractID)).ReturnsAsync(contract);
            _mockPaymentRepository.Setup(r => r.GetPayementsForContractAsync(dto.contractID)).ReturnsAsync(payments);

            // Act & Assert
            Assert.ThrowsAsync<BadRequestExeption>(async () => await _contractService.IssuePayementForContractAsync(dto));
        }
    }
}
