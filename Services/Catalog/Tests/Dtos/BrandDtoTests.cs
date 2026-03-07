using Catalog.Aplication.Dtos;
using Tests.Builders;

namespace Tests.Dtos
{
    public class BrandDtoTests
    {
        [Fact]
        public void BrandDto_ShouldCreateWithValidData()
        {
            // Arrange & Act
            var brandDto = BrandDtoBuilder.ABrandDtoNamed("Test Brand").Build();

            // Assert
            Assert.NotNull(brandDto);
            Assert.Equal("Test Brand", brandDto.Name);
            Assert.NotNull(brandDto.Id);
        }

        [Fact]
        public void BrandDto_ShouldBeImmutable()
        {
            // Arrange
            var brandDto = BrandDtoBuilder.ADefaultBrandDto().Build();

            // Act & Assert
            var newBrandDto = brandDto with { Name = "New Brand" };
            Assert.NotEqual(brandDto.Name, newBrandDto.Name);
        }
    }
}
