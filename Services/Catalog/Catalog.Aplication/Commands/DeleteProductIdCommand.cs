using MediatR;

namespace Catalog.Aplication.Commands
{
    public record DeleteProductIdCommand(string Id) : IRequest<bool>;
}
