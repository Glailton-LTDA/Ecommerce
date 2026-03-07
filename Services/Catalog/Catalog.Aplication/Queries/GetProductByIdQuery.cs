using Catalog.Aplication.Responses;
using MediatR;

namespace Catalog.Aplication.Queries
{
    public record GetProductByIdQuery(string Id) : IRequest<ProductResponse>
    {
    }
}
