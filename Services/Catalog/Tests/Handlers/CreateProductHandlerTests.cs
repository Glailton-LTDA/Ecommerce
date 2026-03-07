using Catalog.Aplication.Commands;
using Catalog.Aplication.Handlers;
using Catalog.Core.Repositories;
using Moq;
using Tests.Builders;

namespace Tests.Handlers
{
    public class CreateProductHandlerTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly CreateProductHandler _handler;

        public CreateProductHandlerTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _handler = new CreateProductHandler(_mockProductRepository.Object);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldCreateProduct()
        {
            // Arrange
            var brand = BrandBuilder.ADefaultBrand().Build();
            var type = TypeBuilder.ADefaultType().Build();
            var command = CreateProductCommandBuilder.ADefaultCreateProductCommand()
                .WithBrandId(brand.Id)
                .WithTypeId(type.Id)
                .Build();

            var product = ProductBuilder.ADefaultProduct()
                .WithBrand(brand)
                .WithType(type)
                .Build();

            _mockProductRepository.Setup(x => x.GetBrandByIdAsync(brand.Id))
                .ReturnsAsync(brand);
            _mockProductRepository.Setup(x => x.GetTypeByIdAsync(type.Id))
                .ReturnsAsync(type);
            _mockProductRepository.Setup(x => x.CreateProduct(It.IsAny<Catalog.Core.Entities.Product>()))
                .ReturnsAsync(product);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Price, result.Price);
            _mockProductRepository.Verify(x => x.GetBrandByIdAsync(brand.Id), Times.Once);
            _mockProductRepository.Verify(x => x.GetTypeByIdAsync(type.Id), Times.Once);
            _mockProductRepository.Verify(x => x.CreateProduct(It.IsAny<Catalog.Core.Entities.Product>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithInvalidBrandId_ShouldThrowApplicationException()
        {
            // Arrange
            var command = CreateProductCommandBuilder.ADefaultCreateProductCommand().Build();

            _mockProductRepository.Setup(x => x.GetBrandByIdAsync(command.BrandId))
                .ReturnsAsync((Catalog.Core.Entities.ProductBrand)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(
                () => _handler.Handle(command, CancellationToken.None));
            Assert.Contains("Brand", exception.Message);
        }

        [Fact]
        public async Task Handle_WithInvalidTypeId_ShouldThrowApplicationException()
        {
            // Arrange
            var brand = BrandBuilder.ADefaultBrand().Build();
            var command = CreateProductCommandBuilder.ADefaultCreateProductCommand()
                .WithBrandId(brand.Id)
                .Build();

            _mockProductRepository.Setup(x => x.GetBrandByIdAsync(brand.Id))
                .ReturnsAsync(brand);
            _mockProductRepository.Setup(x => x.GetTypeByIdAsync(command.TypeId))
                .ReturnsAsync((Catalog.Core.Entities.ProductType)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(
                () => _handler.Handle(command, CancellationToken.None));
            Assert.Contains("Type", exception.Message);
        }
    }
}
