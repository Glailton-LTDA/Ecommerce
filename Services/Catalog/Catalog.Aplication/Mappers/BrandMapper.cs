using System.Collections.Generic;
using System.Linq;
using Catalog.Core.Entities;
using Catalog.Aplication.Responses;

namespace Catalog.Aplication.Mappers
{
    public static class BrandMapper
    {
        public static BrandResponse ToResponse(this ProductBrand brand)
        {
            if (brand == null) return null;
            return new BrandResponse
            {
                Id = brand.Id,
                Name = brand.Name
            };
        }

        public static IEnumerable<BrandResponse> ToResponseList(this IEnumerable<ProductBrand> brands)
        {
            if (brands == null) return Enumerable.Empty<BrandResponse>();
            return brands.Select(b => b.ToResponse());
        }
    }
}
