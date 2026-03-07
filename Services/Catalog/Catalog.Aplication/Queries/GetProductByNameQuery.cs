using Catalog.Aplication.Responses;
using MediatR;

namespace Catalog.Aplication.Queries
{
    public record GetProductByNameQuery(string Name) : IRequest<IEnumerable<ProductResponse>>
    {
    }
}
