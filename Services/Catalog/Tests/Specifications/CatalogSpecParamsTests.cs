using Catalog.Core.Specifications;

namespace Tests.Specifications
{
    public class CatalogSpecParamsTests
    {
        [Fact]
        public void DefaultValues_ShouldBeSet()
        {
            // Arrange & Act
            var specParams = new CatalogSpecParams();

            // Assert
            Assert.Equal(1, specParams.PageIndex);
            Assert.Equal(10, specParams.PageSize);
            Assert.Null(specParams.Sort);
            Assert.Null(specParams.Search);
            Assert.Null(specParams.BrandId);
            Assert.Null(specParams.TypeId);
        }

        [Fact]
        public void PageSize_ExceedingMax_ShouldCapAtMaxPageSize()
        {
            // Arrange
            var specParams = new CatalogSpecParams();

            // Act
            specParams.PageSize = 100;

            // Assert
            Assert.Equal(70, specParams.PageSize); // MaxPageSize is 70
        }

        [Fact]
        public void PageSize_WithValidValue_ShouldSetCorrectly()
        {
            // Arrange
            var specParams = new CatalogSpecParams();

            // Act
            specParams.PageSize = 25;

            // Assert
            Assert.Equal(25, specParams.PageSize);
        }

        [Fact]
        public void PageIndex_ShouldBeSettable()
        {
            // Arrange & Act
            var specParams = new CatalogSpecParams { PageIndex = 5 };

            // Assert
            Assert.Equal(5, specParams.PageIndex);
        }

        [Fact]
        public void Search_ShouldBeNullByDefault()
        {
            // Arrange & Act
            var specParams = new CatalogSpecParams();

            // Assert
            Assert.Null(specParams.Search);
        }

        [Fact]
        public void Search_ShouldBeSettable()
        {
            // Arrange & Act
            var specParams = new CatalogSpecParams { Search = "Product" };

            // Assert
            Assert.Equal("Product", specParams.Search);
        }

        [Fact]
        public void BrandId_ShouldBeSettable()
        {
            // Arrange & Act
            var specParams = new CatalogSpecParams { BrandId = "brand_id_123" };

            // Assert
            Assert.Equal("brand_id_123", specParams.BrandId);
        }

        [Fact]
        public void TypeId_ShouldBeSettable()
        {
            // Arrange & Act
            var specParams = new CatalogSpecParams { TypeId = "type_id_123" };

            // Assert
            Assert.Equal("type_id_123", specParams.TypeId);
        }

        [Fact]
        public void Sort_ShouldBeSettable()
        {
            // Arrange & Act
            var specParams = new CatalogSpecParams { Sort = "price" };

            // Assert
            Assert.Equal("price", specParams.Sort);
        }
    }
}
