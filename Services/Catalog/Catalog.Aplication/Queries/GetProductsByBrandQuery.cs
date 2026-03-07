using Catalog.Aplication.Responses;
using MediatR;

namespace Catalog.Aplication.Queries
{
    public record GetProductsByBrandQuery(string BrandName) : IRequest<IEnumerable<ProductResponse>>
    {
    }
}
