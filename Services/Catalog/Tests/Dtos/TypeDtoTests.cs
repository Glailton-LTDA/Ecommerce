using Catalog.Aplication.Dtos;
using Tests.Builders;

namespace Tests.Dtos
{
    public class TypeDtoTests
    {
        [Fact]
        public void TypeDto_ShouldCreateWithValidData()
        {
            // Arrange & Act
            var typeDto = TypeDtoBuilder.ATypeDtoNamed("Test Type").Build();

            // Assert
            Assert.NotNull(typeDto);
            Assert.Equal("Test Type", typeDto.Name);
            Assert.NotNull(typeDto.Id);
        }

        [Fact]
        public void TypeDto_ShouldBeImmutable()
        {
            // Arrange
            var typeDto = TypeDtoBuilder.ADefaultTypeDto().Build();

            // Act & Assert
            var newTypeDto = typeDto with { Name = "New Type" };
            Assert.NotEqual(typeDto.Name, newTypeDto.Name);
        }
    }
}
