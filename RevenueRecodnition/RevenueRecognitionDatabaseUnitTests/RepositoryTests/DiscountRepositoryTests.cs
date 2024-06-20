using System;
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
    public class DiscountRepositoryTests
    {
        private RRConext _context;
        private DiscountRepository _discountRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<RRConext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new RRConext(options);
            _discountRepository = new DiscountRepository(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetCurrentHighestDiscountAsync_ReturnsHighestDiscount()
        {
            // Arrange
            var discount1 = new Discount { Name = "Discount1", Percentage = 10, StartDate = DateTime.Now.AddDays(-10), EndDate = DateTime.Now.AddDays(10) };
            var discount2 = new Discount { Name = "Discount2", Percentage = 20, StartDate = DateTime.Now.AddDays(-10), EndDate = DateTime.Now.AddDays(10) };
            var discount3 = new Discount { Name = "Discount3", Percentage = 15, StartDate = DateTime.Now.AddDays(-10), EndDate = DateTime.Now.AddDays(10) };

            await _context.Discounts.AddRangeAsync(discount1, discount2, discount3);
            await _context.SaveChangesAsync();

            // Act
            var result = await _discountRepository.GetCurrentHighestDiscountAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(20, result.Percentage);
        }

        [Test]
        public async Task GetCurrentHighestDiscountAsync_ReturnsNull_WhenNoActiveDiscounts()
        {
            // Arrange
            var discount1 = new Discount { Name = "Discount1", Percentage = 10, StartDate = DateTime.Now.AddDays(-20), EndDate = DateTime.Now.AddDays(-10) };
            var discount2 = new Discount { Name = "Discount2", Percentage = 20, StartDate = DateTime.Now.AddDays(-30), EndDate = DateTime.Now.AddDays(-15) };

            await _context.Discounts.AddRangeAsync(discount1, discount2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _discountRepository.GetCurrentHighestDiscountAsync();

            // Assert
            Assert.IsNull(result);
        }
    }
}
