using Catalog.Aplication.Responses;
using MediatR;

namespace Catalog.Aplication.Queries
{
    public record GetAllTypesQuery : IRequest<IEnumerable<TypeResponse>>
    {
    }
}
