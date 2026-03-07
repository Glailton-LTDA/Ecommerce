using Catalog.Core.Entities;
using MongoDB.Bson;

namespace Tests.Builders
{
    public class BrandBuilder
    {
        private string _id = ObjectId.GenerateNewId().ToString();
        private string _name = "Test Brand";

        public BrandBuilder WithId(string id)
        {
            _id = id;
            return this;
        }

        public BrandBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ProductBrand Build()
        {
            return new ProductBrand
            {
                Id = _id,
                Name = _name
            };
        }

        public static BrandBuilder ADefaultBrand()
        {
            return new BrandBuilder().WithName("Default Brand");
        }

        public static BrandBuilder ABrandNamed(string name)
        {
            return new BrandBuilder().WithName(name);
        }

        public static List<ProductBrand> MultipleBrands(int count)
        {
            var brands = new List<ProductBrand>();
            for (int i = 0; i < count; i++)
            {
                brands.Add(new BrandBuilder().WithName($"Brand {i + 1}").Build());
            }
            return brands;
        }
    }
}
