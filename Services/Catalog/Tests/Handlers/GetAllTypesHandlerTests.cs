using Catalog.Aplication.Handlers;
using Catalog.Aplication.Queries;
using Catalog.Core.Repositories;
using Moq;
using Tests.Builders;

namespace Tests.Handlers
{
    public class GetAllTypesHandlerTests
    {
        private readonly Mock<ITypeRepository> _mockTypeRepository;
        private readonly GetAllTypesHandler _handler;

        public GetAllTypesHandlerTests()
        {
            _mockTypeRepository = new Mock<ITypeRepository>();
            _handler = new GetAllTypesHandler(_mockTypeRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllTypes()
        {
            // Arrange
            var types = TypeBuilder.MultipleTypes(3);
            var query = new GetAllTypesQuery();

            _mockTypeRepository.Setup(x => x.GetAllTypes())
                .ReturnsAsync(types);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            _mockTypeRepository.Verify(x => x.GetAllTypes(), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNoTypes_ShouldReturnEmpty()
        {
            // Arrange
            var query = new GetAllTypesQuery();

            _mockTypeRepository.Setup(x => x.GetAllTypes())
                .ReturnsAsync(Enumerable.Empty<Catalog.Core.Entities.ProductType>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }
    }
}
