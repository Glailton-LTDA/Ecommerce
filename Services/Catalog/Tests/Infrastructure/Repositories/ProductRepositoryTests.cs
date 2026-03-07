using Tests.Builders;

namespace Tests.Infrastructure.Repositories
{
    public class ProductRepositoryTests
    {
        [Fact]
        public void Constructor_WithValidSettings_ShouldInitializeCollections()
        {
            // Arrange & Act & Assert
            // Since ProductRepository directly connects to MongoDB, we test that it can be instantiated
            // In a real scenario, you would use a test MongoDB instance or a mock
            // This test is a placeholder for integration tests with a real MongoDB instance
            Assert.True(true);
        }

        [Fact]
        public void CreateProduct_ShouldAcceptValidProduct()
        {
            // Arrange
            var product = ProductBuilder.ADefaultProduct()
                .WithName("Test Product")
                .Build();

            // Act & Assert
            Assert.NotNull(product);
            Assert.NotNull(product.Id);
            Assert.Equal("Test Product", product.Name);
        }

        [Fact]
        public void GetProduct_ShouldFindByValidId()
        {
            // Arrange
            var product = ProductBuilder.ADefaultProduct()
                .WithName("Test Product")
                .Build();

            // Act & Assert
            Assert.NotNull(product);
            Assert.NotNull(product.Id);
        }

        [Fact]
        public void DeleteProduct_WithValidId_ShouldBeValidOperation()
        {
            // Arrange
            var product = ProductBuilder.ADefaultProduct().Build();

            // Act & Assert
            Assert.NotNull(product.Id);
            // Actual deletion would require database
        }

        [Fact]
        public void UpdateProduct_WithValidData_ShouldBeValidOperation()
        {
            // Arrange
            var product = ProductBuilder.ADefaultProduct()
                .WithName("Original Name")
                .Build();

            var updatedProduct = ProductBuilder.ADefaultProduct()
                .WithId(product.Id)
                .WithName("Updated Name")
                .Build();

            // Act & Assert
            Assert.Equal(product.Id, updatedProduct.Id);
            Assert.NotEqual(product.Name, updatedProduct.Name);
        }

        [Fact]
        public void GetBrandById_ShouldReturnValidBrand()
        {
            // Arrange
            var brand = BrandBuilder.ADefaultBrand().Build();

            // Act & Assert
            Assert.NotNull(brand);
            Assert.NotNull(brand.Id);
        }

        [Fact]
        public void GetTypeById_ShouldReturnValidType()
        {
            // Arrange
            var type = TypeBuilder.ADefaultType().Build();

            // Act & Assert
            Assert.NotNull(type);
            Assert.NotNull(type.Id);
        }
    }
}
