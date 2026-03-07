using Catalog.Aplication.Commands;
using Catalog.Aplication.Mappers;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Aplication.Handlers
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var exitedProduct = await _productRepository.GetProduct(request.Id);
            if (exitedProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {request.Id} not found.");
            }

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

            var updatedProduct = request.ToUpdateEntity(exitedProduct, brand, type);
            return await _productRepository.UpdateProduct(updatedProduct);
        }
    }
}
