using Catalog.Aplication.Handlers;
using Catalog.Aplication.Queries;
using Catalog.Core.Repositories;
using Catalog.Core.Specifications;
using Moq;
using Tests.Builders;

namespace Tests.Handlers
{
    public class GetAllProductsHandlerTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly GetAllProductsHandler _handler;

        public GetAllProductsHandlerTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _handler = new GetAllProductsHandler(_mockProductRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPaginatedProducts()
        {
            // Arrange
            var products = new List<Catalog.Core.Entities.Product>
            {
                ProductBuilder.ADefaultProduct().Build(),
                ProductBuilder.AProductNamed("Product 2").Build(),
                ProductBuilder.AProductNamed("Product 3").Build()
            };

            var pagination = new Pagination<Catalog.Core.Entities.Product>
            {
                Data = products.ToList(),
                Count = 3,
                PageIndex = 1,
                PageSize = 10
            };

            var query = new GetAllProductsQuery(new CatalogSpecParams { PageIndex = 1, PageSize = 10 });

            _mockProductRepository.Setup(x => x.GetProducts(It.IsAny<CatalogSpecParams>()))
                .ReturnsAsync(pagination);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(3, result.Data.Count());
            _mockProductRepository.Verify(x => x.GetProducts(It.IsAny<CatalogSpecParams>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithEmptyResult_ShouldReturnEmptyPagination()
        {
            // Arrange
            var pagination = new Pagination<Catalog.Core.Entities.Product>
            {
                Data = new List<Catalog.Core.Entities.Product>().AsReadOnly(),
                Count = 0,
                PageIndex = 1,
                PageSize = 10
            };

            var query = new GetAllProductsQuery(new CatalogSpecParams { PageIndex = 1, PageSize = 10 });

            _mockProductRepository.Setup(x => x.GetProducts(It.IsAny<CatalogSpecParams>()))
                .ReturnsAsync(pagination);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task Handle_WithPaginationParams_ShouldRespectParams()
        {
            // Arrange
            var product = ProductBuilder.ADefaultProduct().Build();
            var pagination = new Pagination<Catalog.Core.Entities.Product>
            {
                Data = new[] { product }.ToList(),
                Count = 100,
                PageIndex = 2,
                PageSize = 10
            };

            var catalogSpecParams = new CatalogSpecParams { PageIndex = 2, PageSize = 10 };
            var query = new GetAllProductsQuery(catalogSpecParams);

            _mockProductRepository.Setup(x => x.GetProducts(It.IsAny<CatalogSpecParams>()))
                .ReturnsAsync(pagination);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.PageIndex);
            Assert.Equal(10, result.PageSize);
            _mockProductRepository.Verify(x => x.GetProducts(It.IsAny<CatalogSpecParams>()), Times.Once);
        }
    }
}
