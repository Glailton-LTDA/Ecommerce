using Catalog.Aplication.Queries;
using Catalog.Aplication.Mappers;
using Catalog.Aplication.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Aplication.Handlers
{
    public class GetAllTypesHandler : IRequestHandler<GetAllTypesQuery, IEnumerable<TypeResponse>>
    {
        private readonly ITypeRepository _typeRepository;

        public GetAllTypesHandler(ITypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }

        public async Task<IEnumerable<TypeResponse>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            var types = await _typeRepository.GetAllTypes();
            return types.ToResponseList();
        }
    }
}
