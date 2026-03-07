using Catalog.Aplication.Handlers;
using Catalog.Aplication.Mappers;
using Catalog.Aplication.Queries;
using Catalog.Core.Repositories;
using Moq;
using Tests.Builders;

namespace Tests.Handlers
{
    public class GetProductsByBrandHandlerTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly GetProductsByBrandHandler _handler;

        public GetProductsByBrandHandlerTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _handler = new GetProductsByBrandHandler(_mockProductRepository.Object);
        }

        [Fact]
        public async Task Handle_WithValidBrand_ShouldReturnProductsByBrand()
        {
            // Arrange
            var brand = BrandBuilder.ADefaultBrand().Build();
            var products = new List<Catalog.Core.Entities.Product>
            {
                ProductBuilder.ADefaultProduct().WithBrand(brand).Build(),
                ProductBuilder.AProductNamed("Product 2").WithBrand(brand).Build()
            };

            var query = new GetProductsByBrandQuery(brand.Name);

            _mockProductRepository.Setup(x => x.GetProductsByBrand(brand.Name))
                .ReturnsAsync(products);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, p => Assert.Equal(brand.Id, p.Brand.Id));
        }

        [Fact]
        public async Task Handle_WithNonExistentBrand_ShouldReturnEmpty()
        {
            // Arrange
            var brandName = "Non Existent Brand";
            var query = new GetProductsByBrandQuery(brandName);

            _mockProductRepository.Setup(x => x.GetProductsByBrand(brandName))
                .ReturnsAsync(Enumerable.Empty<Catalog.Core.Entities.Product>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }
    }
}
