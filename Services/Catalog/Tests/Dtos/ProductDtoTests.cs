using Catalog.Aplication.Dtos;
using Tests.Builders;

namespace Tests.Dtos
{
    public class ProductDtoTests
    {
        [Fact]
        public void ProductDto_ShouldCreateWithAllProperties()
        {
            // Arrange & Act
            var productDto = ProductDtoBuilder.ADefaultProductDto()
                .WithName("Test Product")
                .WithPrice(99.99m)
                .Build();

            // Assert
            Assert.NotNull(productDto);
            Assert.Equal("Test Product", productDto.Name);
            Assert.Equal(99.99m, productDto.Price);
        }

        [Fact]
        public void ProductDto_ShouldHaveValidBrandAndType()
        {
            // Arrange & Act
            var brand = BrandDtoBuilder.ABrandDtoNamed("Test Brand").Build();
            var type = TypeDtoBuilder.ATypeDtoNamed("Test Type").Build();
            var productDto = ProductDtoBuilder.ADefaultProductDto()
                .WithBrand(brand)
                .WithType(type)
                .Build();

            // Assert
            Assert.NotNull(productDto.Brand);
            Assert.NotNull(productDto.Type);
            Assert.Equal("Test Brand", productDto.Brand.Name);
            Assert.Equal("Test Type", productDto.Type.Name);
        }

        [Fact]
        public void ProductDto_ShouldBeImmutable()
        {
            // Arrange
            var productDto = ProductDtoBuilder.ADefaultProductDto().Build();

            // Act & Assert - records are immutable
            Assert.NotNull(productDto);
            // Trying to modify would create a new instance with 'with' expression
            var newProductDto = productDto with { Name = "New Name" };
            Assert.NotEqual(productDto.Name, newProductDto.Name);
        }
    }
}
