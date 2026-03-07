using Catalog.Aplication.Responses;
using MediatR;

namespace Catalog.Aplication.Queries
{
    public record GetAllBrandsQuery : IRequest<IEnumerable<BrandResponse>>
    {
    }
}
