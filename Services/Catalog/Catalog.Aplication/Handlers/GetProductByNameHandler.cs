using Catalog.Aplication.Mappers;
using Catalog.Aplication.Queries;
using Catalog.Aplication.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Aplication.Handlers
{
    public class GetProductByNameHandler : IRequestHandler<GetProductByNameQuery, IEnumerable<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByNameHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductResponse>> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductsByName(request.Name);
            return product.ToResponseList();
        }
    }
}
