using Catalog.Aplication.Handlers;
using Catalog.Aplication.Queries;
using Catalog.Core.Repositories;
using Moq;
using Tests.Builders;

namespace Tests.Handlers
{
    public class GetAllBrandsHandlerTests
    {
        private readonly Mock<IBrandRepository> _mockBrandRepository;
        private readonly GetAllBrandsHandler _handler;

        public GetAllBrandsHandlerTests()
        {
            _mockBrandRepository = new Mock<IBrandRepository>();
            _handler = new GetAllBrandsHandler(_mockBrandRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllBrands()
        {
            // Arrange
            var brands = BrandBuilder.MultipleBrands(3);
            var query = new GetAllBrandsQuery();

            _mockBrandRepository.Setup(x => x.GetAllBrands())
                .ReturnsAsync(brands);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            _mockBrandRepository.Verify(x => x.GetAllBrands(), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNoBrands_ShouldReturnEmpty()
        {
            // Arrange
            var query = new GetAllBrandsQuery();

            _mockBrandRepository.Setup(x => x.GetAllBrands())
                .ReturnsAsync(Enumerable.Empty<Catalog.Core.Entities.ProductBrand>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }
    }
}
