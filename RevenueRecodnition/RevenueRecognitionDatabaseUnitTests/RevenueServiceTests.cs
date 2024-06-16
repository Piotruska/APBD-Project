using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RevenueRecodnition.Api.Exeptions;
using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Repositories;
using RevenueRecodnition.Api.Services;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecognitionDatabaseUnitTests
{
    [TestFixture]
    public class RevenueServiceTests
    {
        private Mock<IExchangeRateService> _mockExchangeRateService;
        private Mock<IClientRepository> _mockClientRepository;
        private Mock<IContracrRepository> _mockContractRepository;
        private Mock<IProductRepository> _mockProductRepository;
        private Mock<IDicountRepository> _mockDiscountRepository;
        private Mock<ISubscriptionRepository> _mockSubscriptionRepository;
        private Mock<IPayementRepository> _mockPaymentRepository;
        private RevenueService _revenueService;

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
            _revenueService = new RevenueService(
                _mockExchangeRateService.Object,
                _mockClientRepository.Object,
                _mockContractRepository.Object,
                _mockProductRepository.Object,
                _mockDiscountRepository.Object,
                _mockSubscriptionRepository.Object,
                _mockPaymentRepository.Object);
        }

        [Test]
        public async Task CalculateCurrentRevenueAsync_ForCompanyInPLN_ReturnsTotalRevenue()
        {
            // Arrange
            var requestDto = new RevenueCalculationRequestDTO { For = "company", CurrencyCode = "PLN" };
            var contracts = new List<Contract>
            {
                new Contract { Price = 1000 },
                new Contract { Price = 2000 }
            };
            var subscriptions = new List<Subscription>
            {
                new Subscription { Payments = new List<Payment> { new Payment { Amount = 500 } } },
                new Subscription { Payments = new List<Payment> { new Payment { Amount = 700 } } }
            };

            _mockContractRepository.Setup(r => r.GetListOfSignedContracts()).ReturnsAsync(contracts);
            _mockSubscriptionRepository.Setup(r => r.GetListOfSubscriptionsWithPayementsAsync()).ReturnsAsync(subscriptions);

            // Act
            var result = await _revenueService.CalculateCurrentRevenueAsync(requestDto);

            // Assert
            Assert.AreEqual(4200, result);
        }

        [Test]
        public async Task CalculateCurrentRevenueAsync_ForProductInPLN_ReturnsTotalRevenue()
        {
            // Arrange
            var requestDto = new RevenueCalculationRequestDTO { For = "product", ProductId = 1, CurrencyCode = "PLN" };
            var contracts = new List<Contract>
            {
                new Contract { Price = 1000 },
                new Contract { Price = 2000 }
            };
            var subscriptions = new List<Subscription>
            {
                new Subscription { Payments = new List<Payment> { new Payment { Amount = 500 } } },
                new Subscription { Payments = new List<Payment> { new Payment { Amount = 700 } } }
            };

            _mockContractRepository.Setup(r => r.GetListOfSignedContractsForProduct(requestDto.ProductId)).ReturnsAsync(contracts);
            _mockSubscriptionRepository.Setup(r => r.GetListOfSubscriptionsWithPayementsForProductAsync(requestDto.ProductId)).ReturnsAsync(subscriptions);

            // Act
            var result = await _revenueService.CalculateCurrentRevenueAsync(requestDto);

            // Assert
            Assert.AreEqual(4200, result);
        }

        [Test]
        public async Task CalculateCurrentRevenueAsync_ForCompanyInUSD_ReturnsTotalRevenueInUSD()
        {
            // Arrange
            var requestDto = new RevenueCalculationRequestDTO { For = "company", CurrencyCode = "USD" };
            var contracts = new List<Contract>
            {
                new Contract { Price = 1000 },
                new Contract { Price = 2000 }
            };
            var subscriptions = new List<Subscription>
            {
                new Subscription { Payments = new List<Payment> { new Payment { Amount = 500 } } },
                new Subscription { Payments = new List<Payment> { new Payment { Amount = 700 } } }
            };

            _mockContractRepository.Setup(r => r.GetListOfSignedContracts()).ReturnsAsync(contracts);
            _mockSubscriptionRepository.Setup(r => r.GetListOfSubscriptionsWithPayementsAsync()).ReturnsAsync(subscriptions);
            _mockExchangeRateService.Setup(s => s.GetExchangeRateAsync("USD")).ReturnsAsync(0.25m);

            // Act
            var result = await _revenueService.CalculateCurrentRevenueAsync(requestDto);

            // Assert
            Assert.AreEqual(1050, result); // 4200 * 0.25
        }

        [Test]
        public async Task CalculateCurrentRevenueAsync_ForProductInUSD_ReturnsTotalRevenueInUSD()
        {
            // Arrange
            var requestDto = new RevenueCalculationRequestDTO { For = "product", ProductId = 1, CurrencyCode = "USD" };
            var contracts = new List<Contract>
            {
                new Contract { Price = 1000 },
                new Contract { Price = 2000 }
            };
            var subscriptions = new List<Subscription>
            {
                new Subscription { Payments = new List<Payment> { new Payment { Amount = 500 } } },
                new Subscription { Payments = new List<Payment> { new Payment { Amount = 700 } } }
            };

            _mockContractRepository.Setup(r => r.GetListOfSignedContractsForProduct(requestDto.ProductId)).ReturnsAsync(contracts);
            _mockSubscriptionRepository.Setup(r => r.GetListOfSubscriptionsWithPayementsForProductAsync(requestDto.ProductId)).ReturnsAsync(subscriptions);
            _mockExchangeRateService.Setup(s => s.GetExchangeRateAsync("USD")).ReturnsAsync(0.25m);

            // Act
            var result = await _revenueService.CalculateCurrentRevenueAsync(requestDto);

            // Assert
            Assert.AreEqual(1050, result); // 4200 * 0.25
        }

        [Test]
        public void CalculateCurrentRevenueAsync_ThrowsBadRequestExeption_WhenRequestTypeInvalid()
        {
            // Arrange
            var requestDto = new RevenueCalculationRequestDTO { For = "invalid", CurrencyCode = "PLN" };

            // Act & Assert
            Assert.ThrowsAsync<BadRequestExeption>(async () => await _revenueService.CalculateCurrentRevenueAsync(requestDto));
        }

        [Test]
        public async Task CalculatePredictedRevenueAsync_ForCompanyInPLN_ReturnsTotalRevenue()
        {
            // Arrange
            var requestDto = new RevenueCalculationRequestDTO { For = "company", CurrencyCode = "PLN" };
            var contracts = new List<Contract>
            {
                new Contract { Price = 1000 },
                new Contract { Price = 2000 }
            };
            var subscriptions = new List<Subscription>
            {
                new Subscription { Payments = new List<Payment> { new Payment { Amount = 500 } } },
                new Subscription { Payments = new List<Payment> { new Payment { Amount = 700 } } }
            };
            var notCanceledSubscriptions = new List<Subscription>
            {
                new Subscription { Price = 300 },
                new Subscription { Price = 400 }
            };

            _mockContractRepository.Setup(r => r.GetListOfAllContractsNotPastDates()).ReturnsAsync(contracts);
            _mockSubscriptionRepository.Setup(r => r.GetListOfSubscriptionsWithPayementsAsync()).ReturnsAsync(subscriptions);
            _mockSubscriptionRepository.Setup(r => r.GetListOfSubscriptionsNotCanceledAsync()).ReturnsAsync(notCanceledSubscriptions);

            // Act
            var result = await _revenueService.CalculatePredictedRevenueAsync(requestDto);

            // Assert
            Assert.AreEqual(4900, result); // 3000 (contracts) + 1200 (payments) + 700 (not canceled)
        }

        [Test]
        public async Task CalculatePredictedRevenueAsync_ForProductInPLN_ReturnsTotalRevenue()
        {
            // Arrange
            var requestDto = new RevenueCalculationRequestDTO { For = "product", ProductId = 1, CurrencyCode = "PLN" };
            var contracts = new List<Contract>
            {
                new Contract { Price = 1000,IdProduct = 1},
                new Contract { Price = 2000,IdProduct = 1 }
            };
            var subscriptions = new List<Subscription>
            {
                new Subscription { Payments = new List<Payment> { new Payment { Amount = 500 } },IdProduct = 1,Price =500 },
                new Subscription { Payments = new List<Payment> { new Payment { Amount = 700 } },IdProduct = 1,Price =700  }
            };
            var notCanceledSubscriptions = new List<Subscription>
            {
                new Subscription { Price = 300,IdProduct = 1 },
                new Subscription { Price = 400,IdProduct = 1 }
            };

            _mockContractRepository.Setup(r => r.GetListOfAllContractsNotPastDatesForProduct(requestDto.ProductId)).ReturnsAsync(contracts);
            _mockSubscriptionRepository.Setup(r => r.GetListOfSubscriptionsWithPayementsForProductAsync(requestDto.ProductId)).ReturnsAsync(subscriptions);
            _mockSubscriptionRepository.Setup(r => r.GetListOfSubscriptionsForProductNotCanceledAsync(requestDto.ProductId)).ReturnsAsync(notCanceledSubscriptions);

            // Act
            var result = await _revenueService.CalculatePredictedRevenueAsync(requestDto);

            // Assert
            Assert.AreEqual(4900, result); // 3000 (contracts) + 1200 (payments) + 700 (not canceled)
        }

        [Test]
        public async Task CalculatePredictedRevenueAsync_ForCompanyInUSD_ReturnsTotalRevenueInUSD()
        {
            // Arrange
            var requestDto = new RevenueCalculationRequestDTO { For = "company", CurrencyCode = "USD" };
            var contracts = new List<Contract>
            {
                new Contract { Price = 1000 },
                new Contract { Price = 2000 }
            };
            var subscriptions = new List<Subscription>
            {
                new Subscription { Payments = new List<Payment> { new Payment { Amount = 500 } } },
                new Subscription { Payments = new List<Payment> { new Payment { Amount = 700 } } }
            };
            var notCanceledSubscriptions = new List<Subscription>
            {
                new Subscription { Price = 300 },
                new Subscription { Price = 400 }
            };

            _mockContractRepository.Setup(r => r.GetListOfAllContractsNotPastDates()).ReturnsAsync(contracts);
            _mockSubscriptionRepository.Setup(r => r.GetListOfSubscriptionsWithPayementsAsync()).ReturnsAsync(subscriptions);
            _mockSubscriptionRepository.Setup(r => r.GetListOfSubscriptionsNotCanceledAsync()).ReturnsAsync(notCanceledSubscriptions);
            _mockExchangeRateService.Setup(s => s.GetExchangeRateAsync("USD")).ReturnsAsync(0.25m);

            // Act
            var result = await _revenueService.CalculatePredictedRevenueAsync(requestDto);

            // Assert
            Assert.AreEqual(1225, result); // 4900 * 0.25
        }

        [Test]
        public async Task CalculatePredictedRevenueAsync_ForProductInUSD_ReturnsTotalRevenueInUSD()
        {
            // Arrange
            var requestDto = new RevenueCalculationRequestDTO { For = "product", ProductId = 1, CurrencyCode = "USD" };
            var contracts = new List<Contract>
            {
                new Contract { Price = 1000 },
                new Contract { Price = 2000 }
            };
            var subscriptions = new List<Subscription>
            {
                new Subscription { Payments = new List<Payment> { new Payment { Amount = 500 } } },
                new Subscription { Payments = new List<Payment> { new Payment { Amount = 700 } } }
            };
            var notCanceledSubscriptions = new List<Subscription>
            {
                new Subscription { Price = 300 },
                new Subscription { Price = 400 }
            };

            _mockContractRepository.Setup(r => r.GetListOfAllContractsNotPastDatesForProduct(requestDto.ProductId)).ReturnsAsync(contracts);
            _mockSubscriptionRepository.Setup(r => r.GetListOfSubscriptionsWithPayementsForProductAsync(requestDto.ProductId)).ReturnsAsync(subscriptions);
            _mockSubscriptionRepository.Setup(r => r.GetListOfSubscriptionsForProductNotCanceledAsync(requestDto.ProductId)).ReturnsAsync(notCanceledSubscriptions);
            _mockExchangeRateService.Setup(s => s.GetExchangeRateAsync("USD")).ReturnsAsync(0.25m);

            // Act
            var result = await _revenueService.CalculatePredictedRevenueAsync(requestDto);

            // Assert
            Assert.AreEqual(1225, result); // 5900 * 0.25
        }

        [Test]
        public void CalculatePredictedRevenueAsync_ThrowsBadRequestExeption_WhenRequestTypeInvalid()
        {
            // Arrange
            var requestDto = new RevenueCalculationRequestDTO { For = "invalid", CurrencyCode = "PLN" };

            // Act & Assert
            Assert.ThrowsAsync<BadRequestExeption>(async () => await _revenueService.CalculatePredictedRevenueAsync(requestDto));
        }
    }
}
