using Catalog.API.Controllers.v1;
using Catalog.Aplication.Commands;
using Catalog.Aplication.Dtos;
using Catalog.Aplication.Mappers;
using Catalog.Aplication.Queries;
using Catalog.Core.Specifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tests.Builders;

namespace Tests.Controllers
{
    public class CatalogControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly CatalogController _controller;

        public CatalogControllerTests()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new CatalogController(_mockMediator.Object);
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnOkWithProducts()
        {
            // Arrange
            var catalogSpecParams = new CatalogSpecParams { PageIndex = 1, PageSize = 10 };
            var product1 = ProductBuilder.ADefaultProduct().Build();
            var product2 = ProductBuilder.AProductNamed("Product 2").Build();

            var productResponses = new List<Catalog.Aplication.Responses.ProductResponse>
            {
                product1.ToResponse(),
                product2.ToResponse()
            };

            var pagination = new Pagination<Catalog.Aplication.Responses.ProductResponse>
            {
                Data = productResponses.AsReadOnly(),
                Count = 2,
                PageIndex = 1,
                PageSize = 10
            };

            _mockMediator.Setup(x => x.Send(It.IsAny<GetAllProductsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagination);

            // Act
            var result = await _controller.GetAllProducts(catalogSpecParams);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            _mockMediator.Verify(x => x.Send(It.IsAny<GetAllProductsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetProduct_WithValidId_ShouldReturnOk()
        {
            // Arrange
            var product = ProductBuilder.ADefaultProduct().Build();
            var productResponse = product.ToResponse();

            _mockMediator.Setup(x => x.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(productResponse);

            // Act
            var result = await _controller.GetProduct(product.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetProduct_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var invalidId = "invalid_id";

            _mockMediator.Setup(x => x.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Catalog.Aplication.Responses.ProductResponse)null);

            // Act
            var result = await _controller.GetProduct(invalidId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetProductByProductName_ShouldReturnOk()
        {
            // Arrange
            var productName = "Test Product";
            var productResponses = new List<Catalog.Aplication.Responses.ProductResponse>
            {
                ProductBuilder.AProductNamed(productName).Build().ToResponse(),
                ProductBuilder.AProductNamed(productName).Build().ToResponse()
            };

            _mockMediator.Setup(x => x.Send(It.IsAny<GetProductByNameQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(productResponses.AsEnumerable());

            // Act
            var result = await _controller.GetProductByProductName(productName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task CreateProduct_WithValidCommand_ShouldReturnOk()
        {
            // Arrange
            var command = CreateProductCommandBuilder.ADefaultCreateProductCommand().Build();
            var response = ProductBuilder.ADefaultProduct().Build().ToResponse();

            _mockMediator.Setup(x => x.Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.CreateProduct(command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            _mockMediator.Verify(x => x.Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteProduct_WithValidId_ShouldReturnNoContent()
        {
            // Arrange
            var productId = "valid_id";

            _mockMediator.Setup(x => x.Send(It.IsAny<Catalog.Aplication.Commands.DeleteProductIdCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteProduct(productId);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task DeleteProduct_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var invalidId = "invalid_id";

            _mockMediator.Setup(x => x.Send(It.IsAny<Catalog.Aplication.Commands.DeleteProductIdCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteProduct(invalidId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_WithValidData_ShouldReturnNoContent()
        {
            // Arrange
            var productId = "valid_id";
            var updateDto = new UpdateProductDto
            {
                Name = "Updated Name",
                Summary = "Updated Summary",
                Description = "Updated Description",
                ImageFile = "updated.jpg",
                BrandId = "brand_id",
                TypeId = "type_id",
                Price = 150.00m
            };

            _mockMediator.Setup(x => x.Send(It.IsAny<Catalog.Aplication.Commands.UpdateProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateProduct(productId, updateDto);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task GetBrands_ShouldReturnOk()
        {
            // Arrange
            var brandResponses = new List<Catalog.Aplication.Responses.BrandResponse>
            {
                BrandBuilder.ADefaultBrand().Build().ToResponse(),
                BrandBuilder.ABrandNamed("Brand 2").Build().ToResponse()
            };

            _mockMediator.Setup(x => x.Send(It.IsAny<GetAllBrandsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(brandResponses.AsEnumerable());

            // Act
            var result = await _controller.GetBrands();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetTypes_ShouldReturnOk()
        {
            // Arrange
            var typeResponses = new List<Catalog.Aplication.Responses.TypeResponse>
            {
                TypeBuilder.ADefaultType().Build().ToResponse(),
                TypeBuilder.ATypeNamed("Type 2").Build().ToResponse()
            };

            _mockMediator.Setup(x => x.Send(It.IsAny<GetAllTypesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(typeResponses.AsEnumerable());

            // Act
            var result = await _controller.GetTypes();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetProductByProductBrand_WithValidBrand_ShouldReturnOk()
        {
            // Arrange
            var brand = BrandBuilder.ADefaultBrand().Build();
            var productResponses = new List<Catalog.Aplication.Responses.ProductResponse>
            {
                ProductBuilder.ADefaultProduct().WithBrand(brand).Build().ToResponse(),
                ProductBuilder.AProductNamed("Product 2").WithBrand(brand).Build().ToResponse()
            };

            _mockMediator.Setup(x => x.Send(It.IsAny<GetProductsByBrandQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(productResponses.AsEnumerable());

            // Act
            var result = await _controller.GetProductByProductBrand(brand.Name);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetProductByProductBrand_WithInvalidBrand_ShouldReturnNotFound()
        {
            // Arrange
            var brandName = "Non Existent Brand";

            _mockMediator.Setup(x => x.Send(It.IsAny<GetProductsByBrandQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<Catalog.Aplication.Responses.ProductResponse>());

            // Act
            var result = await _controller.GetProductByProductBrand(brandName);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
    }
}
