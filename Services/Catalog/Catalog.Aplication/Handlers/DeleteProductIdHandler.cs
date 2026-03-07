using Catalog.Aplication.Commands;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Aplication.Handlers
{
    public class DeleteProductIdHandler : IRequestHandler<DeleteProductIdCommand, bool>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductIdHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(DeleteProductIdCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProduct(request.Id);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {request.Id} not found.");
            }

            return await _productRepository.DeleteProduct(request.Id);
        }
    }
}
