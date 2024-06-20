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
    public class PayementRepositoryTests
    {
        private RRConext _context;
        private PayementRepository _payementRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<RRConext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new RRConext(options);
            _payementRepository = new PayementRepository(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetPayementsForContractAsync_ReturnsPayments()
        {
            // Arrange
            var contractId = 1;
            var payment1 = new Payment { IdContract = contractId, Amount = 100, DatePayed = DateTime.Now };
            var payment2 = new Payment { IdContract = contractId, Amount = 200, DatePayed = DateTime.Now.AddDays(-1) };
            var payment3 = new Payment { IdContract = 2, Amount = 300, DatePayed = DateTime.Now.AddDays(-2) }; // Different contract

            await _context.Payments.AddRangeAsync(payment1, payment2, payment3);
            await _context.SaveChangesAsync();

            // Act
            var result = await _payementRepository.GetPayementsForContractAsync(contractId);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.Contains(payment1, result);
            Assert.Contains(payment2, result);
            Assert.IsFalse(result.Contains(payment3));
        }

        [Test]
        public async Task AddPaymentAsync_Success()
        {
            // Arrange
            var payment = new Payment { IdContract = 1, Amount = 100, DatePayed = DateTime.Now };

            // Act
            await _payementRepository.AddPaymentAsync(payment);
            var result = await _context.Payments.FindAsync(payment.IdPayement);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(payment.IdContract, result.IdContract);
            Assert.AreEqual(payment.Amount, result.Amount);
            Assert.AreEqual(payment.DatePayed, result.DatePayed);
        }
    }
}
