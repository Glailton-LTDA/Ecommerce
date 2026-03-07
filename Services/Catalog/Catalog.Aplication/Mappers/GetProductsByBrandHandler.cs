using Catalog.Aplication.Queries;
using Catalog.Aplication.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Aplication.Mappers
{
    public class GetProductsByBrandHandler : IRequestHandler<GetProductsByBrandQuery, IEnumerable<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsByBrandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductResponse>> Handle(GetProductsByBrandQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetProductsByBrand(request.BrandName);
            return products.ToResponseList();
        }
    }
}
