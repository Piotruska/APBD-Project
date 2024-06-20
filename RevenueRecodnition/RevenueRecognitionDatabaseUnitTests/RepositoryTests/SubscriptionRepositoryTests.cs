using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RevenueRecodnition.Api.Repositories;
using RevenueRecodnition.DataBase.Context;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecognitionDatabaseUnitTests
{
    [TestFixture]
    public class SubscriptionRepositoryTests
    {
        private RRConext _context;
        private SubscriptionRepository _subscriptionRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<RRConext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new RRConext(options);
            _subscriptionRepository = new SubscriptionRepository(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetSubscriptionAsync_ReturnsSubscription()
        {
            // Arrange
            var subscription = new Subscription 
            { 
                IdSubscription = 1, 
                IdClient = 1, 
                IdProduct = 1, 
                Canceled = false,
                StartDateRenewalPayement = DateTime.Now,
                EndDateRenewalPayement = DateTime.Now.AddMonths(1),
                RenewalPeriod = 1,
                Name = "Test Subscription",
                Price = 100
            };
            await _context.Subscriptions.AddAsync(subscription);
            await _context.SaveChangesAsync();

            // Act
            var result = await _subscriptionRepository.GetSubscriptionAsync(subscription.IdSubscription);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(subscription.IdSubscription, result.IdSubscription);
            Assert.AreEqual(subscription.Name, result.Name);
        }

        [Test]
        public async Task GetSubscriptionAsync_ReturnsNull_WhenSubscriptionDoesNotExist()
        {
            // Arrange
            var nonExistentSubscriptionId = 99;

            // Act
            var result = await _subscriptionRepository.GetSubscriptionAsync(nonExistentSubscriptionId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetActiveSubscriptionsForProductAsync_ReturnsActiveSubscription()
        {
            // Arrange
            var subscription = new Subscription 
            { 
                IdSubscription = 1, 
                IdClient = 1, 
                IdProduct = 1, 
                Canceled = false,
                StartDateRenewalPayement = DateTime.Now,
                EndDateRenewalPayement = DateTime.Now.AddMonths(1),
                RenewalPeriod = 1,
                Name = "Test Subscription",
                Price = 100
            };
            await _context.Subscriptions.AddAsync(subscription);
            await _context.SaveChangesAsync();

            // Act
            var result = await _subscriptionRepository.GetActiveSubscriptionsForProductAsync(subscription.IdProduct, subscription.IdClient);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(subscription.IdSubscription, result.IdSubscription);
        }

        [Test]
        public async Task GetActiveSubscriptionsForProductAsync_ReturnsNull_WhenSubscriptionIsCanceled()
        {
            // Arrange
            var subscription = new Subscription 
            { 
                IdSubscription = 1, 
                IdClient = 1, 
                IdProduct = 1, 
                Canceled = true,
                StartDateRenewalPayement = DateTime.Now,
                EndDateRenewalPayement = DateTime.Now.AddMonths(1),
                RenewalPeriod = 1,
                Name = "Test Subscription",
                Price = 100
            };
            await _context.Subscriptions.AddAsync(subscription);
            await _context.SaveChangesAsync();

            // Act
            var result = await _subscriptionRepository.GetActiveSubscriptionsForProductAsync(subscription.IdProduct, subscription.IdClient);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetListOfSubscriptionsWithPayementsAsync_ReturnsSubscriptionsWithPayments()
        {
            // Arrange
            var subscription = new Subscription 
            { 
                IdSubscription = 1, 
                IdClient = 1, 
                IdProduct = 1, 
                Canceled = false,
                StartDateRenewalPayement = DateTime.Now,
                EndDateRenewalPayement = DateTime.Now.AddMonths(1),
                RenewalPeriod = 1,
                Name = "Test Subscription",
                Price = 100
            };
            var payment = new Payment 
            { 
                IdPayement = 1, 
                IdSubscription = subscription.IdSubscription, 
                Amount = 100, 
                DatePayed = DateTime.Now 
            };
            await _context.Subscriptions.AddAsync(subscription);
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            // Act
            var result = await _subscriptionRepository.GetListOfSubscriptionsWithPayementsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result.First().Payments.Count);
            Assert.AreEqual(payment.IdPayement, result.First().Payments.First().IdPayement);
        }

        [Test]
        public async Task GetListOfSubscriptionsWithPayementsForProductAsync_ReturnsSubscriptionsWithPaymentsForProduct()
        {
            // Arrange
            var subscription = new Subscription 
            { 
                IdSubscription = 1, 
                IdClient = 1, 
                IdProduct = 1, 
                Canceled = false,
                StartDateRenewalPayement = DateTime.Now,
                EndDateRenewalPayement = DateTime.Now.AddMonths(1),
                RenewalPeriod = 1,
                Name = "Test Subscription",
                Price = 100
            };
            var payment = new Payment 
            { 
                IdPayement = 1, 
                IdSubscription = subscription.IdSubscription, 
                Amount = 100, 
                DatePayed = DateTime.Now 
            };
            await _context.Subscriptions.AddAsync(subscription);
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            // Act
            var result = await _subscriptionRepository.GetListOfSubscriptionsWithPayementsForProductAsync(subscription.IdProduct);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result.First().Payments.Count);
            Assert.AreEqual(payment.IdPayement, result.First().Payments.First().IdPayement);
        }

        [Test]
        public async Task GetListOfSubscriptionsNotCanceledAsync_ReturnsNotCanceledSubscriptions()
        {
            // Arrange
            var activeSubscription = new Subscription 
            { 
                IdSubscription = 1, 
                IdClient = 1, 
                IdProduct = 1, 
                Canceled = false,
                StartDateRenewalPayement = DateTime.Now,
                EndDateRenewalPayement = DateTime.Now.AddMonths(1),
                RenewalPeriod = 1,
                Name = "Active Subscription",
                Price = 100
            };
            var canceledSubscription = new Subscription 
            { 
                IdSubscription = 2, 
                IdClient = 2, 
                IdProduct = 2, 
                Canceled = true,
                StartDateRenewalPayement = DateTime.Now,
                EndDateRenewalPayement = DateTime.Now.AddMonths(1),
                RenewalPeriod = 1,
                Name = "Canceled Subscription",
                Price = 200
            };
            await _context.Subscriptions.AddRangeAsync(activeSubscription, canceledSubscription);
            await _context.SaveChangesAsync();

            // Act
            var result = await _subscriptionRepository.GetListOfSubscriptionsNotCanceledAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(activeSubscription.IdSubscription, result.First().IdSubscription);
        }

        [Test]
        public async Task GetListOfSubscriptionsForProductNotCanceledAsync_ReturnsNotCanceledSubscriptionsForProduct()
        {
            // Arrange
            var activeSubscription = new Subscription 
            { 
                IdSubscription = 1, 
                IdClient = 1, 
                IdProduct = 1, 
                Canceled = false,
                StartDateRenewalPayement = DateTime.Now,
                EndDateRenewalPayement = DateTime.Now.AddMonths(1),
                RenewalPeriod = 1,
                Name = "Active Subscription",
                Price = 100
            };
            var canceledSubscription = new Subscription 
            { 
                IdSubscription = 2, 
                IdClient = 1, 
                IdProduct = 1, 
                Canceled = true,
                StartDateRenewalPayement = DateTime.Now,
                EndDateRenewalPayement = DateTime.Now.AddMonths(1),
                RenewalPeriod = 1,
                Name = "Canceled Subscription",
                Price = 200
            };
            var otherProductSubscription = new Subscription 
            { 
                IdSubscription = 3, 
                IdClient = 2, 
                IdProduct = 2, 
                Canceled = false,
                StartDateRenewalPayement = DateTime.Now,
                EndDateRenewalPayement = DateTime.Now.AddMonths(1),
                RenewalPeriod = 1,
                Name = "Other Product Subscription",
                Price = 300
            };
            await _context.Subscriptions.AddRangeAsync(activeSubscription, canceledSubscription, otherProductSubscription);
            await _context.SaveChangesAsync();

            // Act
            var result = await _subscriptionRepository.GetListOfSubscriptionsForProductNotCanceledAsync(activeSubscription.IdProduct);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(activeSubscription.IdSubscription, result.First().IdSubscription);
        }

        [Test]
        public async Task AddSubscriptionAsync_Success()
        {
            // Arrange
            var subscription = new Subscription 
            { 
                IdSubscription = 1, 
                IdClient = 1, 
                IdProduct = 1, 
                Canceled = false,
                StartDateRenewalPayement = DateTime.Now,
                EndDateRenewalPayement = DateTime.Now.AddMonths(1),
                RenewalPeriod = 1,
                Name = "Test Subscription",
                Price = 100
            };

            // Act
            await _subscriptionRepository.AddSubscriptionAsync(subscription);
            var result = await _context.Subscriptions.FindAsync(subscription.IdSubscription);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(subscription.IdSubscription, result.IdSubscription);
            Assert.AreEqual(subscription.Name, result.Name);
        }

        [Test]
        public async Task UpdateSubscriptionDatesAsync_Success()
        {
            var timeNow = DateTime.Now;
            // Arrange
            var subscription = new Subscription 
            { 
                IdSubscription = 1, 
                IdClient = 1, 
                IdProduct = 1, 
                Canceled = false,
                StartDateRenewalPayement = timeNow,
                EndDateRenewalPayement = timeNow.AddMonths(1),
                RenewalPeriod = 1,
                Name = "Test Subscription",
                Price = 100
            };
            await _context.Subscriptions.AddAsync(subscription);
            await _context.SaveChangesAsync();

            // Act
            await _subscriptionRepository.UpdateSubscriptionDatesAsync(subscription.IdSubscription);
            var result = await _context.Subscriptions.FindAsync(subscription.IdSubscription);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(timeNow.AddMonths(1), result.StartDateRenewalPayement);
            Assert.AreEqual(timeNow.AddMonths(2), result.EndDateRenewalPayement);
        }
    }
}
