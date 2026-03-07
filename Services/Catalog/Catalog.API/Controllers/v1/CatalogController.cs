using Catalog.Aplication.Commands;
using Catalog.Aplication.Dtos;
using Catalog.Aplication.Mappers;
using Catalog.Aplication.Queries;
using Catalog.Core.Specifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CatalogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts([FromQuery] CatalogSpecParams catalogSpecParams)
        {
            var query = new GetAllProductsQuery(catalogSpecParams);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct([FromQuery] string id)
        {
            var query = new GetProductByIdQuery(id);
            var result = await _mediator.Send(query);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("productName/{productName}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductByProductName([FromQuery] string productName)
        {
            var query = new GetProductByNameQuery(productName);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("productBrand/{productBrand}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductByProductBrand([FromQuery] string productBrand)
        {
            var query = new GetProductsByBrandQuery(productBrand);
            var result = await _mediator.Send(query);
            if (result == null || !result.Any())
                return NotFound();
            var dtoList = result.Select(p => p.ToDto()).ToList();
            return Ok(dtoList);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("id")]
        public async Task<ActionResult> DeleteProduct([FromQuery] string id)
        {
            var command = new DeleteProductIdCommand(id);
            var result = await _mediator.Send(command);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpPut("id")]
        public async Task<ActionResult> UpdateProduct(string id, UpdateProductDto updateProductDto)
        {
            var command = updateProductDto.ToCommand(id);
            var result = await _mediator.Send(command);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("GetAllBrands")]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetBrands()
        {
            var query = new GetAllBrandsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetAllTypes")]
        public async Task<ActionResult<IEnumerable<TypeDto>>> GetTypes()
        {
            var query = new GetAllTypesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("/brand/{brand}", Name = "GetProductsByBrandName")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByBrand(string brand)
        {
            //First get the products
            var query = new GetProductsByBrandQuery(brand);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
