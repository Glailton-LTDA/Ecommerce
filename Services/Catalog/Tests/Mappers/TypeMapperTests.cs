using Catalog.Aplication.Mappers;
using Tests.Builders;

namespace Tests.Mappers
{
    public class TypeMapperTests
    {
        [Fact]
        public void ToResponse_WithValidType_ShouldMapCorrectly()
        {
            // Arrange
            var type = TypeBuilder.ADefaultType()
                .WithName("Test Type")
                .Build();

            // Act
            var response = type.ToResponse();

            // Assert
            Assert.NotNull(response);
            Assert.Equal(type.Id, response.Id);
            Assert.Equal(type.Name, response.Name);
        }

        [Fact]
        public void ToResponse_WithNullType_ShouldReturnNull()
        {
            // Arrange
            Catalog.Core.Entities.ProductType type = null;

            // Act
            var response = type.ToResponse();

            // Assert
            Assert.Null(response);
        }

        [Fact]
        public void ToResponseList_WithValidTypes_ShouldMapAll()
        {
            // Arrange
            var types = TypeBuilder.MultipleTypes(3);

            // Act
            var responses = types.ToResponseList();

            // Assert
            Assert.NotNull(responses);
            Assert.Equal(3, responses.Count());
            Assert.All(responses, r => Assert.NotNull(r));
        }

        [Fact]
        public void ToResponseList_WithNullList_ShouldReturnEmpty()
        {
            // Arrange
            IEnumerable<Catalog.Core.Entities.ProductType> types = null;

            // Act
            var responses = types.ToResponseList();

            // Assert
            Assert.Empty(responses);
        }

        [Fact]
        public void ToResponseList_WithEmptyList_ShouldReturnEmpty()
        {
            // Arrange
            var types = new List<Catalog.Core.Entities.ProductType>();

            // Act
            var responses = types.ToResponseList();

            // Assert
            Assert.Empty(responses);
        }
    }
}
