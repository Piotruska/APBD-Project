using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RevenueRecodnition.Api.Repositories;
using RevenueRecodnition.DataBase.Context;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecognitionDatabaseUnitTests
{
    [TestFixture]
    public class ProductRepositoryTests
    {
        private RRConext _context;
        private ProductRepository _productRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<RRConext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new RRConext(options);
            _productRepository = new ProductRepository(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetProductAsync_ReturnsProduct()
        {
            // Arrange
            var product = new Product 
            { 
                IdProduct = 1, 
                Name = "Test Product", 
                BasePrice = 100, 
                Category = "Software", 
                CurrentVersion = "1.0", 
                Description = "Test Description"
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _productRepository.GetProductAsync(product.IdProduct);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(product.IdProduct, result.IdProduct);
            Assert.AreEqual(product.Name, result.Name);
            Assert.AreEqual(product.BasePrice, result.BasePrice);
            Assert.AreEqual(product.Category, result.Category);
            Assert.AreEqual(product.CurrentVersion, result.CurrentVersion);
            Assert.AreEqual(product.Description, result.Description);
        }

        [Test]
        public async Task GetProductAsync_ReturnsNull_WhenProductDoesNotExist()
        {
            // Arrange
            var nonExistentProductId = 99;

            // Act
            var result = await _productRepository.GetProductAsync(nonExistentProductId);

            // Assert
            Assert.IsNull(result);
        }
    }
}
