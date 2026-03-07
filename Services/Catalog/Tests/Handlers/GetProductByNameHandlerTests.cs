using Catalog.Aplication.Handlers;
using Catalog.Aplication.Queries;
using Catalog.Core.Repositories;
using Moq;
using Tests.Builders;

namespace Tests.Handlers
{
    public class GetProductByNameHandlerTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly GetProductByNameHandler _handler;

        public GetProductByNameHandlerTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _handler = new GetProductByNameHandler(_mockProductRepository.Object);
        }

        [Fact]
        public async Task Handle_WithValidName_ShouldReturnProducts()
        {
            // Arrange
            var productName = "Test Product";
            var products = new List<Catalog.Core.Entities.Product>
            {
                ProductBuilder.AProductNamed(productName).Build(),
                ProductBuilder.AProductNamed(productName).Build()
            };

            var query = new GetProductByNameQuery(productName);

            _mockProductRepository.Setup(x => x.GetProductsByName(productName))
                .ReturnsAsync(products);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, p => Assert.Equal(productName, p.Name));
            _mockProductRepository.Verify(x => x.GetProductsByName(productName), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNonExistentName_ShouldReturnEmpty()
        {
            // Arrange
            var productName = "Non Existent";
            var query = new GetProductByNameQuery(productName);

            _mockProductRepository.Setup(x => x.GetProductsByName(productName))
                .ReturnsAsync(Enumerable.Empty<Catalog.Core.Entities.Product>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
            _mockProductRepository.Verify(x => x.GetProductsByName(productName), Times.Once);
        }
    }
}
