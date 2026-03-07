using Catalog.Aplication.Responses;
using Catalog.Core.Specifications;
using MediatR;

namespace Catalog.Aplication.Queries
{
    public record GetAllProductsQuery(CatalogSpecParams CatalogSpecParams) : IRequest<Pagination<ProductResponse>>
    {
    }
}
