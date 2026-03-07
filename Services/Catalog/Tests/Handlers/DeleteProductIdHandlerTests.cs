using Catalog.Aplication.Handlers;
using Catalog.Aplication.Queries;
using Catalog.Core.Repositories;
using Moq;
using Tests.Builders;

namespace Tests.Handlers
{
    public class DeleteProductIdHandlerTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly DeleteProductIdHandler _handler;

        public DeleteProductIdHandlerTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _handler = new DeleteProductIdHandler(_mockProductRepository.Object);
        }

        [Fact]
        public async Task Handle_WithValidId_ShouldDeleteProduct()
        {
            // Arrange
            var product = ProductBuilder.ADefaultProduct().Build();
            var command = new Catalog.Aplication.Commands.DeleteProductIdCommand(product.Id);

            _mockProductRepository.Setup(x => x.GetProduct(product.Id))
                .ReturnsAsync(product);
            _mockProductRepository.Setup(x => x.DeleteProduct(product.Id))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mockProductRepository.Verify(x => x.GetProduct(product.Id), Times.Once);
            _mockProductRepository.Verify(x => x.DeleteProduct(product.Id), Times.Once);
        }

        [Fact]
        public async Task Handle_WithInvalidId_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var invalidId = "invalid_id";
            var command = new Catalog.Aplication.Commands.DeleteProductIdCommand(invalidId);

            _mockProductRepository.Setup(x => x.GetProduct(invalidId))
                .ReturnsAsync((Catalog.Core.Entities.Product)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
            
            Assert.Contains($"Product with ID {invalidId} not found", exception.Message);
            _mockProductRepository.Verify(x => x.GetProduct(invalidId), Times.Once);
            _mockProductRepository.Verify(x => x.DeleteProduct(It.IsAny<string>()), Times.Never);
        }
    }
}
