using Catalog.Aplication.Mappers;
using Catalog.Aplication.Queries;
using Catalog.Aplication.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Aplication.Handlers
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductResponse>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProduct(request.Id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {request.Id} not found.");
            }
            return product.ToResponse();
        }
    }
}
