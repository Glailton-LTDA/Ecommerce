using Catalog.Aplication.Responses;
using Catalog.Core.Entities;

namespace Catalog.Aplication.Mappers
{
    public static class TypeMapper
    {
        public static TypeResponse ToResponse(this ProductType type)
        {
            if (type == null) return null;
            return new TypeResponse
            {
                Id = type.Id,
                Name = type.Name
            };
        }

        public static IEnumerable<TypeResponse> ToResponseList(this IEnumerable<ProductType> types)
        {
            if (types == null) return Enumerable.Empty<TypeResponse>();
            return types.Select(b => b.ToResponse());
        }
    }
}
