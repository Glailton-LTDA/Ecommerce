using Catalog.Aplication.Queries;
using Catalog.Aplication.Responses;
using Catalog.Core.Repositories;
using MediatR;
using Catalog.Aplication.Mappers;

namespace Catalog.Aplication.Handlers
{
    public class GetAllBrandsHandler : IRequestHandler<GetAllBrandsQuery, IEnumerable<BrandResponse>>
    {
        private readonly IBrandRepository _brandRepository;

        public GetAllBrandsHandler(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<IEnumerable<BrandResponse>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands = await _brandRepository.GetAllBrands();
            return brands.ToResponseList();
        }
    }
}
