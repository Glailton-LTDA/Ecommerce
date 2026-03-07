using Catalog.Core.Specifications;

namespace Tests.Specifications
{
    public class PaginationTests
    {
        [Fact]
        public void Pagination_ShouldInitializeWithData()
        {
            // Arrange
            var data = new List<string> { "item1", "item2", "item3" };

            // Act
            var pagination = new Pagination<string>
            {
                Data = data.AsReadOnly(),
                Count = 3,
                PageIndex = 1,
                PageSize = 10
            };

            // Assert
            Assert.NotNull(pagination);
            Assert.Equal(3, pagination.Count);
            Assert.Equal(1, pagination.PageIndex);
            Assert.Equal(10, pagination.PageSize);
            Assert.Equal(3, pagination.Data.Count());
        }

        [Fact]
        public void Pagination_WithEmptyData_ShouldBeValid()
        {
            // Arrange & Act
            var pagination = new Pagination<string>
            {
                Data = new List<string>().AsReadOnly(),
                Count = 0,
                PageIndex = 1,
                PageSize = 10
            };

            // Assert
            Assert.NotNull(pagination);
            Assert.Equal(0, pagination.Count);
            Assert.Empty(pagination.Data);
        }

        [Fact]
        public void Pagination_ShouldTrackPageInfo()
        {
            // Arrange & Act
            var data = Enumerable.Range(1, 5).Select(x => x.ToString()).ToList();
            var pagination = new Pagination<string>
            {
                Data = data.AsReadOnly(),
                Count = 50,
                PageIndex = 2,
                PageSize = 5
            };

            // Assert
            Assert.Equal(2, pagination.PageIndex);
            Assert.Equal(5, pagination.PageSize);
            Assert.Equal(50, pagination.Count);
        }
    }
}
