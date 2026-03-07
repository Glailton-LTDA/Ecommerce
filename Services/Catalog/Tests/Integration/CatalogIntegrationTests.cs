using Catalog.Aplication.Commands;
using Catalog.Aplication.Handlers;
using Catalog.Aplication.Queries;
using Catalog.Core.Repositories;
using Moq;
using Tests.Builders;

namespace Tests.Integration
{
    public class CatalogIntegrationTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;

        public CatalogIntegrationTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
        }

        [Fact]
        public async Task CreateProduct_Then_GetProduct_ShouldReturnSameProduct()
        {
            // Arrange
            var brand = BrandBuilder.ADefaultBrand().Build();
            var type = TypeBuilder.ADefaultType().Build();
            var createCommand = CreateProductCommandBuilder.ADefaultCreateProductCommand()
                .WithBrandId(brand.Id)
                .WithTypeId(type.Id)
                .WithName("Test Product")
                .Build();

            var product = ProductBuilder.ADefaultProduct()
                .WithName("Test Product")
                .WithBrand(brand)
                .WithType(type)
                .Build();

            var createHandler = new CreateProductHandler(_mockProductRepository.Object);
            var getHandler = new GetProductByIdHandler(_mockProductRepository.Object);

            _mockProductRepository.Setup(x => x.GetBrandByIdAsync(brand.Id))
                .ReturnsAsync(brand);
            _mockProductRepository.Setup(x => x.GetTypeByIdAsync(type.Id))
                .ReturnsAsync(type);
            _mockProductRepository.Setup(x => x.CreateProduct(It.IsAny<Catalog.Core.Entities.Product>()))
                .ReturnsAsync(product);
            _mockProductRepository.Setup(x => x.GetProduct(product.Id))
                .ReturnsAsync(product);

            // Act
            var createResult = await createHandler.Handle(createCommand, CancellationToken.None);
            var getQuery = new GetProductByIdQuery(product.Id);
            var getResult = await getHandler.Handle(getQuery, CancellationToken.None);

            // Assert
            Assert.NotNull(createResult);
            Assert.NotNull(getResult);
            Assert.Equal(createResult.Name, getResult.Name);
            Assert.Equal(createResult.Price, getResult.Price);
        }

        [Fact]
        public async Task CreateMultipleProducts_Then_GetAll_ShouldReturnAll()
        {
            // Arrange
            var products = new List<Catalog.Core.Entities.Product>
            {
                ProductBuilder.AProductNamed("Product 1").Build(),
                ProductBuilder.AProductNamed("Product 2").Build(),
                ProductBuilder.AProductNamed("Product 3").Build()
            };

            var pagination = new Catalog.Core.Specifications.Pagination<Catalog.Core.Entities.Product>
            {
                Data = products.ToList().AsReadOnly(),
                Count = 3,
                PageIndex = 1,
                PageSize = 10
            };

            var getAllHandler = new GetAllProductsHandler(_mockProductRepository.Object);
            var query = new GetAllProductsQuery(new Catalog.Core.Specifications.CatalogSpecParams());

            _mockProductRepository.Setup(x => x.GetProducts(It.IsAny<Catalog.Core.Specifications.CatalogSpecParams>()))
                .ReturnsAsync(pagination);

            // Act
            var result = await getAllHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(3, result.Data.Count());
        }

        [Fact]
        public async Task DeleteProduct_Then_GetProduct_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var product = ProductBuilder.ADefaultProduct().Build();
            var deleteHandler = new DeleteProductIdHandler(_mockProductRepository.Object);
            var getHandler = new GetProductByIdHandler(_mockProductRepository.Object);

            _mockProductRepository.Setup(x => x.GetProduct(product.Id))
                .ReturnsAsync(product);
            _mockProductRepository.Setup(x => x.DeleteProduct(product.Id))
                .ReturnsAsync(true);

            // Act
            var deleteCommand = new Catalog.Aplication.Commands.DeleteProductIdCommand(product.Id);
            var deleteResult = await deleteHandler.Handle(deleteCommand, CancellationToken.None);

            // After delete, setup GetProduct to return null
            _mockProductRepository.Setup(x => x.GetProduct(product.Id))
                .ReturnsAsync((Catalog.Core.Entities.Product)null);

            var getQuery = new GetProductByIdQuery(product.Id);
            var getException = await Assert.ThrowsAsync<KeyNotFoundException>(() => getHandler.Handle(getQuery, CancellationToken.None));

            // Assert
            Assert.True(deleteResult);
            Assert.Contains($"Product with ID {product.Id} not found", getException.Message);
        }

        [Fact]
        public async Task UpdateProduct_ShouldReflectChanges()
        {
            // Arrange
            var brand = BrandBuilder.ADefaultBrand().Build();
            var type = TypeBuilder.ADefaultType().Build();
            var product = ProductBuilder.ADefaultProduct()
                .WithName("Original Name")
                .WithBrand(brand)
                .WithType(type)
                .Build();

            var updateCommand = new UpdateProductCommand
            {
                Id = product.Id,
                Name = "Updated Name",
                Summary = "Updated Summary",
                Description = "Updated Description",
                ImageFile = "updated.jpg",
                BrandId = brand.Id,
                TypeId = type.Id,
                Price = 150.00m
            };

            var updateHandler = new UpdateProductHandler(_mockProductRepository.Object);

            _mockProductRepository.Setup(x => x.GetProduct(product.Id))
                .ReturnsAsync(product);
            _mockProductRepository.Setup(x => x.GetBrandByIdAsync(brand.Id))
                .ReturnsAsync(brand);
            _mockProductRepository.Setup(x => x.GetTypeByIdAsync(type.Id))
                .ReturnsAsync(type);
            _mockProductRepository.Setup(x => x.UpdateProduct(It.IsAny<Catalog.Core.Entities.Product>()))
                .ReturnsAsync(true);

            // Act
            var result = await updateHandler.Handle(updateCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mockProductRepository.Verify(x => x.GetProduct(product.Id), Times.Once);
            _mockProductRepository.Verify(x => x.UpdateProduct(It.IsAny<Catalog.Core.Entities.Product>()), Times.Once);
        }

        [Fact]
        public async Task SearchProducts_ByName_ShouldReturnMatchingProducts()
        {
            // Arrange
            var productName = "Test Product";
            var products = new List<Catalog.Core.Entities.Product>
            {
                ProductBuilder.AProductNamed(productName).Build(),
                ProductBuilder.AProductNamed(productName).Build()
            };

            var handler = new GetProductByNameHandler(_mockProductRepository.Object);
            var query = new GetProductByNameQuery(productName);

            _mockProductRepository.Setup(x => x.GetProductsByName(productName))
                .ReturnsAsync(products);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, p => Assert.Equal(productName, p.Name));
        }

        [Fact]
        public async Task GetProductsByBrand_ShouldReturnProductsForBrand()
        {
            // Arrange
            var brand = BrandBuilder.ABrandNamed("Nike").Build();
            var products = new List<Catalog.Core.Entities.Product>
            {
                ProductBuilder.ADefaultProduct().WithBrand(brand).Build(),
                ProductBuilder.AProductNamed("Product 2").WithBrand(brand).Build()
            };

            var handler = new Catalog.Aplication.Mappers.GetProductsByBrandHandler(_mockProductRepository.Object);
            var query = new GetProductsByBrandQuery(brand.Name);

            _mockProductRepository.Setup(x => x.GetProductsByBrand(brand.Name))
                .ReturnsAsync(products);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, p => Assert.Equal(brand.Name, p.Brand.Name));
        }
    }
}
