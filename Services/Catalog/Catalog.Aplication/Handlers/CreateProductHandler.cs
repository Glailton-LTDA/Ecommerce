using Catalog.Aplication.Commands;
using Catalog.Aplication.Mappers;
using Catalog.Aplication.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Aplication.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductResponse>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var brand = await _productRepository.GetBrandByIdAsync(request.BrandId);
            var type = await _productRepository.GetTypeByIdAsync(request.TypeId);

            if (brand == null)
            {
                throw new ApplicationException($"Brand with ID {request.BrandId} not found.");
            }

            if (type == null)
            {
                throw new ApplicationException($"Type with ID {request.TypeId} not found.");
            }

            var productEntity = request.ToEntity(brand, type);
            var newProduct = await _productRepository.CreateProduct(productEntity);

            return newProduct.ToResponse();
        }
    }
}
