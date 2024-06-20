using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RevenueRecodnition.Api.Exeptions;
using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Repositories.Interfaces;
using RevenueRecodnition.Api.Services;
using RevenueRecodnition.Api.Services.Interfaces;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecognitionDatabaseUnitTests
{
    [TestFixture]
    public class SubscriptionServiceTests
    {
        private Mock<IExchangeRateService> _mockExchangeRateService;
        private Mock<IClientRepository> _mockClientRepository;
        private Mock<IContracrRepository> _mockContractRepository;
        private Mock<IProductRepository> _mockProductRepository;
        private Mock<IDicountRepository> _mockDiscountRepository;
        private Mock<ISubscriptionRepository> _mockSubscriptionRepository;
        private Mock<IPayementRepository> _mockPaymentRepository;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private SubscriptionService _subscriptionService;

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
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            _subscriptionService = new SubscriptionService(
                _mockExchangeRateService.Object,
                _mockClientRepository.Object,
                _mockContractRepository.Object,
                _mockProductRepository.Object,
                _mockDiscountRepository.Object,
                _mockSubscriptionRepository.Object,
                _mockPaymentRepository.Object,
                _mockUnitOfWork.Object);
        }

        [Test]
        public async Task AddSubscriptionAsync_Success()
        {
            // Arrange
            var dto = new AddSubscriptionDTO
            {
                IdClient = 1,
                IdProduct = 1,
                Name = "Subscription",
                RenewalPeriodInMonths = 12
            };

            var client = new Client { IdClient = dto.IdClient };
            var product = new Product { IdProduct = dto.IdProduct, BasePrice = 1200 };
            var discount = new Discount { Percentage = 5 };

            _mockClientRepository.Setup(r => r.GetClientWithoutSoftDeletedAsync(dto.IdClient)).ReturnsAsync(client);
            _mockProductRepository.Setup(r => r.GetProductAsync(dto.IdProduct)).ReturnsAsync(product);
            _mockDiscountRepository.Setup(r => r.GetCurrentHighestDiscountAsync()).ReturnsAsync(discount);

            // Act
            var result = await _subscriptionService.AddSubscriptionAsync(dto);

            // Assert
            _mockUnitOfWork.Verify(u => u.BeginTransactionAsync(), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitTransactionAsync(), Times.Once);
            _mockSubscriptionRepository.Verify(r => r.AddSubscriptionAsync(It.Is<Subscription>(s =>
                s.IdClient == dto.IdClient &&
                s.IdProduct == dto.IdProduct &&
                s.Price == (product.BasePrice / 12) * dto.RenewalPeriodInMonths * 0.95M)), Times.Once);
            _mockPaymentRepository.Verify(r => r.AddPaymentAsync(It.Is<Payment>(p =>
                p.IdSubscription == result &&
                p.Amount == (product.BasePrice / 12) * dto.RenewalPeriodInMonths * ((100 - discount.Percentage) * 0.01M))), Times.Once);
        }

        [Test]
        public void AddSubscriptionAsync_ThrowsNotFoundExeption_WhenClientNotFound()
        {
            // Arrange
            var dto = new AddSubscriptionDTO
            {
                IdClient = 1,
                IdProduct = 1,
                Name = "Subscription",
                RenewalPeriodInMonths = 12
            };

            _mockClientRepository.Setup(r => r.GetClientWithoutSoftDeletedAsync(dto.IdClient)).ReturnsAsync((Client)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundExeption>(async () => await _subscriptionService.AddSubscriptionAsync(dto));
            Assert.AreEqual("Client not found.", ex.Message);
        }

        [Test]
        public void AddSubscriptionAsync_ThrowsNotFoundExeption_WhenProductNotFound()
        {
            // Arrange
            var dto = new AddSubscriptionDTO
            {
                IdClient = 1,
                IdProduct = 1,
                Name = "Subscription",
                RenewalPeriodInMonths = 12
            };

            var client = new Client { IdClient = dto.IdClient };

            _mockClientRepository.Setup(r => r.GetClientWithoutSoftDeletedAsync(dto.IdClient)).ReturnsAsync(client);
            _mockProductRepository.Setup(r => r.GetProductAsync(dto.IdProduct)).ReturnsAsync((Product)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundExeption>(async () => await _subscriptionService.AddSubscriptionAsync(dto));
            Assert.AreEqual("Product not found.", ex.Message);
        }

        [Test]
        public void AddSubscriptionAsync_ThrowsBadRequestExeption_WhenRenewalPeriodIsInvalid()
        {
            // Arrange
            var invalidPeriods = new[] { 0, 25 };
            foreach (var invalidPeriod in invalidPeriods)
            {
                var dto = new AddSubscriptionDTO
                {
                    IdClient = 1,
                    IdProduct = 1,
                    Name = "Subscription",
                    RenewalPeriodInMonths = invalidPeriod // Invalid periods
                };

                var client = new Client { IdClient = dto.IdClient };
                var product = new Product { IdProduct = dto.IdProduct };

                _mockClientRepository.Setup(r => r.GetClientWithoutSoftDeletedAsync(dto.IdClient)).ReturnsAsync(client);
                _mockProductRepository.Setup(r => r.GetProductAsync(dto.IdProduct)).ReturnsAsync(product);

                // Act & Assert
                var ex = Assert.ThrowsAsync<BadRequestExeption>(async () => await _subscriptionService.AddSubscriptionAsync(dto));
                Assert.AreEqual("Renewal period can only be from 1 - 24 months", ex.Message);
            }
        }

        [Test]
        public async Task PayForSubscriptionAsync_Success()
        {
            // Arrange
            var dto = new PayementForSubscription
            {
                subscriptionID = 1,
                Amount = 1140 // Assuming discount applied
            };

            var subscription = new Subscription
            {
                IdSubscription = dto.subscriptionID,
                Price = 1140,
                StartDateRenewalPayement = DateTime.Now.AddDays(-1),
                EndDateRenewalPayement = DateTime.Now.AddMonths(1),
                Canceled = false
            };

            _mockSubscriptionRepository.Setup(r => r.GetSubscriptionAsync(dto.subscriptionID)).ReturnsAsync(subscription);

            // Act
            await _subscriptionService.PayForSubscriptionAsync(dto);

            // Assert
            _mockUnitOfWork.Verify(u => u.BeginTransactionAsync(), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitTransactionAsync(), Times.Once);
            _mockPaymentRepository.Verify(r => r.AddPaymentAsync(It.Is<Payment>(p =>
                p.IdSubscription == dto.subscriptionID &&
                p.Amount == dto.Amount)), Times.Once);
            _mockSubscriptionRepository.Verify(r => r.UpdateSubscriptionDatesAsync(dto.subscriptionID), Times.Once);
        }

        [Test]
        public void PayForSubscriptionAsync_ThrowsNotFoundExeption_WhenSubscriptionNotFound()
        {
            // Arrange
            var dto = new PayementForSubscription
            {
                subscriptionID = 1,
                Amount = 1140
            };

            _mockSubscriptionRepository.Setup(r => r.GetSubscriptionAsync(dto.subscriptionID)).ReturnsAsync((Subscription)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundExeption>(async () => await _subscriptionService.PayForSubscriptionAsync(dto));
            Assert.AreEqual("Subscription not found.", ex.Message);
        }

        [Test]
        public void PayForSubscriptionAsync_ThrowsBadRequestExeption_WhenSubscriptionIsCancelled()
        {
            // Arrange
            var dto = new PayementForSubscription
            {
                subscriptionID = 1,
                Amount = 1140
            };

            var subscription = new Subscription
            {
                IdSubscription = dto.subscriptionID,
                Canceled = true
            };

            _mockSubscriptionRepository.Setup(r => r.GetSubscriptionAsync(dto.subscriptionID)).ReturnsAsync(subscription);

            // Act & Assert
            var ex = Assert.ThrowsAsync<BadRequestExeption>(async () => await _subscriptionService.PayForSubscriptionAsync(dto));
            Assert.AreEqual("Subscription is canceled.", ex.Message);
        }

        [Test]
        public void PayForSubscriptionAsync_ThrowsBadRequestExeption_WhenPaymentDateIsOutOfRange()
        {
            // Arrange
            var dto = new PayementForSubscription
            {
                subscriptionID = 1,
                Amount = 1140
            };

            var subscription = new Subscription
            {
                IdSubscription = dto.subscriptionID,
                StartDateRenewalPayement = DateTime.Now.AddMonths(1),
                EndDateRenewalPayement = DateTime.Now.AddMonths(2)
            };

            _mockSubscriptionRepository.Setup(r => r.GetSubscriptionAsync(dto.subscriptionID)).ReturnsAsync(subscription);

            // Act & Assert
            var ex = Assert.ThrowsAsync<BadRequestExeption>(async () => await _subscriptionService.PayForSubscriptionAsync(dto));
            Assert.AreEqual("Cannot pay for next period.", ex.Message);
        }

        [Test]
        public void PayForSubscriptionAsync_ThrowsBadRequestExeption_WhenPaymentAmountIsIncorrect()
        {
            // Arrange
            var dto = new PayementForSubscription
            {
                subscriptionID = 1,
                Amount = 1000 // Incorrect amount
            };

            var subscription = new Subscription
            {
                IdSubscription = dto.subscriptionID,
                Price = 1140,
                StartDateRenewalPayement = DateTime.Now.AddDays(-1),
                EndDateRenewalPayement = DateTime.Now.AddMonths(1)
            };

            _mockSubscriptionRepository.Setup(r => r.GetSubscriptionAsync(dto.subscriptionID)).ReturnsAsync(subscription);

            // Act & Assert
            var ex = Assert.ThrowsAsync<BadRequestExeption>(async () => await _subscriptionService.PayForSubscriptionAsync(dto));
            Assert.AreEqual("Incorrect Payement amount", ex.Message);
        }
    }
}
