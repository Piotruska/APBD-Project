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
    public class ContractRepositoryTests
    {
        private RRConext _context;
        private ContractRepository _contractRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<RRConext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new RRConext(options);
            _contractRepository = new ContractRepository(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task AddContractAsync_Success()
        {
            // Arrange
            var contract = new Contract { IdClient = 1, IdProduct = 1, IsSigned = false, StartDatePayement = DateTime.Now, EndDatePayement = DateTime.Now.AddYears(1) };

            // Act
            var contractId = await _contractRepository.AddContractAsync(contract);
            var result = await _context.Contracts.FindAsync(contractId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(contract.IdClient, result.IdClient);
            Assert.AreEqual(contract.IdProduct, result.IdProduct);
            Assert.AreEqual(contract.IsSigned, result.IsSigned);
            Assert.AreEqual(contract.StartDatePayement, result.StartDatePayement);
            Assert.AreEqual(contract.EndDatePayement, result.EndDatePayement);
        }

        [Test]
        public async Task GetActiveContractForProductAsync_ReturnsActiveContract()
        {
            // Arrange
            var contract = new Contract { IdClient = 1, IdProduct = 1, IsSigned = true, StartDatePayement = DateTime.Now.AddMonths(-1), EndDatePayement = DateTime.Now.AddMonths(1) };
            await _context.Contracts.AddAsync(contract);
            await _context.SaveChangesAsync();

            // Act
            var result = await _contractRepository.GetActiveContractForProductAsync(1, 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(contract.IdContract, result.IdContract);
        }

        [Test]
        public async Task GetActiveContractForProductAsync_ReturnsNull_WhenNoActiveContract()
        {
            // Arrange
            var contract = new Contract { IdClient = 1, IdProduct = 1, IsSigned = false, StartDatePayement = DateTime.Now.AddMonths(-2), EndDatePayement = DateTime.Now.AddMonths(-1) };
            await _context.Contracts.AddAsync(contract);
            await _context.SaveChangesAsync();

            // Act
            var result = await _contractRepository.GetActiveContractForProductAsync(1, 1);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetListOfSignedContractsAsync_ReturnsSignedContracts()
        {
            // Arrange
            var signedContract = new Contract { IdClient = 1, IdProduct = 1, IsSigned = true };
            var unsignedContract = new Contract { IdClient = 1, IdProduct = 2, IsSigned = false };
            await _context.Contracts.AddRangeAsync(signedContract, unsignedContract);
            await _context.SaveChangesAsync();

            // Act
            var result = await _contractRepository.GetListOfSignedContractsAsync();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(signedContract.IdContract, result.First().IdContract);
        }

        [Test]
        public async Task GetListOfSignedContractsForProductAsync_ReturnsSignedContractsForProduct()
        {
            // Arrange
            var signedContract = new Contract { IdClient = 1, IdProduct = 1, IsSigned = true };
            var unsignedContract = new Contract { IdClient = 1, IdProduct = 1, IsSigned = false };
            var otherProductContract = new Contract { IdClient = 1, IdProduct = 2, IsSigned = true };
            await _context.Contracts.AddRangeAsync(signedContract, unsignedContract, otherProductContract);
            await _context.SaveChangesAsync();

            // Act
            var result = await _contractRepository.GetListOfSignedContractsForProductAsync(1);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(signedContract.IdContract, result.First().IdContract);
        }

        [Test]
        public async Task GetListOfAllContractsNotPastDatesAsync_ReturnsValidContracts()
        {
            // Arrange
            var currentContract = new Contract { IdClient = 1, IdProduct = 1, IsSigned = false, StartDatePayement = DateTime.Now.AddMonths(-1), EndDatePayement = DateTime.Now.AddMonths(1) };
            var pastContract = new Contract { IdClient = 1, IdProduct = 2, IsSigned = false, StartDatePayement = DateTime.Now.AddMonths(-2), EndDatePayement = DateTime.Now.AddMonths(-1) };
            var signedContract = new Contract { IdClient = 1, IdProduct = 3, IsSigned = true, StartDatePayement = DateTime.Now.AddMonths(-2), EndDatePayement = DateTime.Now.AddMonths(-1) };
            await _context.Contracts.AddRangeAsync(currentContract, pastContract, signedContract);
            await _context.SaveChangesAsync();

            // Act
            var result = await _contractRepository.GetListOfAllContractsNotPastDatesAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.Contains(currentContract, result);
            Assert.Contains(signedContract, result);
        }

        [Test]
        public async Task GetListOfAllContractsNotPastDatesForProductAsync_ReturnsValidContractsForProduct()
        {
            // Arrange
            var currentContract = new Contract { IdClient = 1, IdProduct = 1, IsSigned = false, StartDatePayement = DateTime.Now.AddMonths(-1), EndDatePayement = DateTime.Now.AddMonths(1) };
            var pastContract = new Contract { IdClient = 1, IdProduct = 1, IsSigned = false, StartDatePayement = DateTime.Now.AddMonths(-2), EndDatePayement = DateTime.Now.AddMonths(-1) };
            var signedContract = new Contract { IdClient = 1, IdProduct = 2, IsSigned = true, StartDatePayement = DateTime.Now.AddMonths(-2), EndDatePayement = DateTime.Now.AddMonths(-1) };
            await _context.Contracts.AddRangeAsync(currentContract, pastContract, signedContract);
            await _context.SaveChangesAsync();

            // Act
            var result = await _contractRepository.GetListOfAllContractsNotPastDatesForProductAsync(1);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.Contains(currentContract, result);
        }

        [Test]
        public async Task SignContractAsync_Success()
        {
            // Arrange
            var contract = new Contract { IdClient = 1, IdProduct = 1, IsSigned = false };
            await _context.Contracts.AddAsync(contract);
            await _context.SaveChangesAsync();

            // Act
            await _contractRepository.SignContractAsync(contract.IdContract);
            var result = await _context.Contracts.FindAsync(contract.IdContract);

            // Assert
            Assert.IsTrue(result.IsSigned);
        }
    }
}
