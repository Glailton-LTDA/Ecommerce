using Catalog.Aplication.Mappers;
using Tests.Builders;

namespace Tests.Mappers
{
    public class BrandMapperTests
    {
        [Fact]
        public void ToResponse_WithValidBrand_ShouldMapCorrectly()
        {
            // Arrange
            var brand = BrandBuilder.ADefaultBrand()
                .WithName("Test Brand")
                .Build();

            // Act
            var response = brand.ToResponse();

            // Assert
            Assert.NotNull(response);
            Assert.Equal(brand.Id, response.Id);
            Assert.Equal(brand.Name, response.Name);
        }

        [Fact]
        public void ToResponse_WithNullBrand_ShouldReturnNull()
        {
            // Arrange
            Catalog.Core.Entities.ProductBrand brand = null;

            // Act
            var response = brand.ToResponse();

            // Assert
            Assert.Null(response);
        }

        [Fact]
        public void ToResponseList_WithValidBrands_ShouldMapAll()
        {
            // Arrange
            var brands = BrandBuilder.MultipleBrands(3);

            // Act
            var responses = brands.ToResponseList();

            // Assert
            Assert.NotNull(responses);
            Assert.Equal(3, responses.Count());
            Assert.All(responses, r => Assert.NotNull(r));
        }

        [Fact]
        public void ToResponseList_WithNullList_ShouldReturnEmpty()
        {
            // Arrange
            IEnumerable<Catalog.Core.Entities.ProductBrand> brands = null;

            // Act
            var responses = brands.ToResponseList();

            // Assert
            Assert.Empty(responses);
        }

        [Fact]
        public void ToResponseList_WithEmptyList_ShouldReturnEmpty()
        {
            // Arrange
            var brands = new List<Catalog.Core.Entities.ProductBrand>();

            // Act
            var responses = brands.ToResponseList();

            // Assert
            Assert.Empty(responses);
        }
    }
}
