using Catalog.Aplication.Mappers;
using Tests.Builders;

namespace Tests.Mappers
{
    public class ProductMapperTests
    {
        [Fact]
        public void ToResponse_WithValidProduct_ShouldMapCorrectly()
        {
            // Arrange
            var product = ProductBuilder.ADefaultProduct()
                .WithName("Test Product")
                .WithPrice(99.99m)
                .Build();

            // Act
            var response = product.ToResponse();

            // Assert
            Assert.NotNull(response);
            Assert.Equal(product.Id, response.Id);
            Assert.Equal(product.Name, response.Name);
            Assert.Equal(product.Price, response.Price);
            Assert.Equal(product.Description, response.Description);
            Assert.Equal(product.Summary, response.Summary);
        }

        [Fact]
        public void ToResponse_WithNullProduct_ShouldReturnNull()
        {
            // Arrange
            Catalog.Core.Entities.Product product = null;

            // Act
            var response = product.ToResponse();

            // Assert
            Assert.Null(response);
        }

        [Fact]
        public void ToResponse_ShouldMapBrandAndType()
        {
            // Arrange
            var brand = BrandBuilder.ADefaultBrand().WithName("Test Brand").Build();
            var type = TypeBuilder.ADefaultType().WithName("Test Type").Build();
            var product = ProductBuilder.ADefaultProduct()
                .WithBrand(brand)
                .WithType(type)
                .Build();

            // Act
            var response = product.ToResponse();

            // Assert
            Assert.NotNull(response.Brand);
            Assert.NotNull(response.Type);
            Assert.Equal(brand.Id, response.Brand.Id);
            Assert.Equal(type.Id, response.Type.Id);
        }

        [Fact]
        public void ToDto_WithValidResponse_ShouldMapCorrectly()
        {
            // Arrange
            var product = ProductBuilder.ADefaultProduct()
                .WithName("Test Product")
                .WithPrice(99.99m)
                .Build();
            var response = product.ToResponse();

            // Act
            var dto = response.ToDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(response.Id, dto.Id);
            Assert.Equal(response.Name, dto.Name);
            Assert.Equal(response.Price, dto.Price);
        }

        [Fact]
        public void ToDto_WithNullResponse_ShouldReturnNull()
        {
            // Arrange
            Catalog.Aplication.Responses.ProductResponse response = null;

            // Act
            var dto = response.ToDto();

            // Assert
            Assert.Null(dto);
        }
    }
}
