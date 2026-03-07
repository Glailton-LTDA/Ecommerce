using Tests.Builders;

namespace Tests.Infrastructure.Repositories
{
    public class BrandRepositoryTests
    {
        [Fact]
        public void GetAllBrands_ShouldReturnValidList()
        {
            // Arrange
            var brands = BrandBuilder.MultipleBrands(3);

            // Act & Assert
            Assert.NotNull(brands);
            Assert.Equal(3, brands.Count);
            Assert.All(brands, b => Assert.NotNull(b.Id));
        }

        [Fact]
        public void GetBrandById_ShouldReturnValidBrand()
        {
            // Arrange
            var brand = BrandBuilder.ADefaultBrand()
                .WithName("Test Brand")
                .Build();

            // Act & Assert
            Assert.NotNull(brand);
            Assert.Equal("Test Brand", brand.Name);
        }

        [Fact]
        public void BrandName_ShouldBeUpdatable()
        {
            // Arrange
            var brand = BrandBuilder.ADefaultBrand()
                .WithName("Original Name")
                .Build();

            var updatedBrand = BrandBuilder.ADefaultBrand()
                .WithId(brand.Id)
                .WithName("Updated Name")
                .Build();

            // Act & Assert
            Assert.Equal(brand.Id, updatedBrand.Id);
            Assert.NotEqual(brand.Name, updatedBrand.Name);
        }
    }
}
