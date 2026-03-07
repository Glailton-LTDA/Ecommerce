using Catalog.Aplication.Handlers;
using Catalog.Aplication.Queries;
using Catalog.Core.Repositories;
using Moq;
using Tests.Builders;

namespace Tests.Handlers
{
    public class UpdateProductHandlerTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly UpdateProductHandler _handler;

        public UpdateProductHandlerTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _handler = new UpdateProductHandler(_mockProductRepository.Object);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldUpdateProduct()
        {
            // Arrange
            var product = ProductBuilder.ADefaultProduct().Build();
            var command = new Catalog.Aplication.Commands.UpdateProductCommand
            {
                Id = product.Id,
                Name = "Updated Name",
                Summary = "Updated Summary",
                Description = "Updated Description",
                ImageFile = "updated.jpg",
                BrandId = product.Brand.Id,
                TypeId = product.Type.Id,
                Price = 150.00m
            };

            _mockProductRepository.Setup(x => x.GetProduct(command.Id))
                .ReturnsAsync(product);
            _mockProductRepository.Setup(x => x.GetBrandByIdAsync(command.BrandId))
                .ReturnsAsync(product.Brand);
            _mockProductRepository.Setup(x => x.GetTypeByIdAsync(command.TypeId))
                .ReturnsAsync(product.Type);
            _mockProductRepository.Setup(x => x.UpdateProduct(It.IsAny<Catalog.Core.Entities.Product>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mockProductRepository.Verify(x => x.GetProduct(command.Id), Times.Once);
            _mockProductRepository.Verify(x => x.UpdateProduct(It.IsAny<Catalog.Core.Entities.Product>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithInvalidBrand_ShouldThrowException()
        {
            // Arrange
            var product = ProductBuilder.ADefaultProduct().Build();
            var command = new Catalog.Aplication.Commands.UpdateProductCommand
            {
                Id = product.Id,
                BrandId = "invalid_brand_id",
                TypeId = product.Type.Id
            };

            _mockProductRepository.Setup(x => x.GetProduct(command.Id))
                .ReturnsAsync(product);
            _mockProductRepository.Setup(x => x.GetBrandByIdAsync(command.BrandId))
                .ReturnsAsync((Catalog.Core.Entities.ProductBrand)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(
                () => _handler.Handle(command, CancellationToken.None));
            Assert.Contains("Brand", exception.Message);
        }

        [Fact]
        public async Task Handle_WithNonExistentProduct_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var brand = BrandBuilder.ADefaultBrand().Build();
            var type = TypeBuilder.ADefaultType().Build();
            var command = new Catalog.Aplication.Commands.UpdateProductCommand
            {
                Id = "non_existent_id",
                BrandId = brand.Id,
                TypeId = type.Id
            };

            _mockProductRepository.Setup(x => x.GetProduct(command.Id))
                .ReturnsAsync((Catalog.Core.Entities.Product)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            Assert.Contains($"Product with ID {command.Id} not found", exception.Message);
            _mockProductRepository.Verify(x => x.GetProduct(command.Id), Times.Once);
            _mockProductRepository.Verify(x => x.UpdateProduct(It.IsAny<Catalog.Core.Entities.Product>()), Times.Never);
        }
    }
}
