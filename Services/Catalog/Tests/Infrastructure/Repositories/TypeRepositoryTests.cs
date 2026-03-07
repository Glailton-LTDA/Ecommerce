using Tests.Builders;

namespace Tests.Infrastructure.Repositories
{
    public class TypeRepositoryTests
    {
        [Fact]
        public void GetAllTypes_ShouldReturnValidList()
        {
            // Arrange
            var types = TypeBuilder.MultipleTypes(3);

            // Act & Assert
            Assert.NotNull(types);
            Assert.Equal(3, types.Count);
            Assert.All(types, t => Assert.NotNull(t.Id));
        }

        [Fact]
        public void GetTypeById_ShouldReturnValidType()
        {
            // Arrange
            var type = TypeBuilder.ADefaultType()
                .WithName("Test Type")
                .Build();

            // Act & Assert
            Assert.NotNull(type);
            Assert.Equal("Test Type", type.Name);
        }

        [Fact]
        public void TypeName_ShouldBeUpdatable()
        {
            // Arrange
            var type = TypeBuilder.ADefaultType()
                .WithName("Original Name")
                .Build();

            var updatedType = TypeBuilder.ADefaultType()
                .WithId(type.Id)
                .WithName("Updated Name")
                .Build();

            // Act & Assert
            Assert.Equal(type.Id, updatedType.Id);
            Assert.NotEqual(type.Name, updatedType.Name);
        }
    }
}
