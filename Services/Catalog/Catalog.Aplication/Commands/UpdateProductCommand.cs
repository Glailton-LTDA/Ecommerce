using Catalog.Aplication.Responses;
using MediatR;

namespace Catalog.Aplication.Commands
{
    public record UpdateProductCommand : IRequest<bool>
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Summary { get; init; }
        public string Description { get; init; }
        public string ImageFile { get; init; }
        public string BrandId { get; init; }
        public string TypeId { get; init; }
        public decimal Price { get; init; }
    }
}
