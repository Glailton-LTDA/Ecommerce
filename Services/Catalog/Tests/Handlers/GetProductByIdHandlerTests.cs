using Catalog.Aplication.Handlers;
using Catalog.Aplication.Queries;
using Catalog.Core.Repositories;
using Moq;
using Tests.Builders;

namespace Tests.Handlers
{
    public class GetProductByIdHandlerTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly GetProductByIdHandler _handler;

        public GetProductByIdHandlerTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _handler = new GetProductByIdHandler(_mockProductRepository.Object);
        }

        [Fact]
        public async Task Handle_WithValidId_ShouldReturnProduct()
        {
            // Arrange
            var product = ProductBuilder.ADefaultProduct().Build();
            var query = new GetProductByIdQuery(product.Id);

            _mockProductRepository.Setup(x => x.GetProduct(product.Id))
                .ReturnsAsync(product);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Id, result.Id);
            Assert.Equal(product.Name, result.Name);
            _mockProductRepository.Verify(x => x.GetProduct(product.Id), Times.Once);
        }

        [Fact]
        public async Task Handle_WithInvalidId_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var invalidId = "invalid_id";
            var query = new GetProductByIdQuery(invalidId);

            _mockProductRepository.Setup(x => x.GetProduct(invalidId))
                .ReturnsAsync((Catalog.Core.Entities.Product)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
            Assert.Contains($"Product with ID {invalidId} not found", exception.Message);
            _mockProductRepository.Verify(x => x.GetProduct(invalidId), Times.Once);
        }
    }
}
