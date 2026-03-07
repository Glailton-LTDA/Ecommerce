using Catalog.Aplication.Dtos;
using MongoDB.Bson;

namespace Tests.Builders
{
    public class BrandDtoBuilder
    {
        private string _id = ObjectId.GenerateNewId().ToString();
        private string _name = "Test Brand";

        public BrandDtoBuilder WithId(string id)
        {
            _id = id;
            return this;
        }

        public BrandDtoBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public BrandDto Build()
        {
            return new BrandDto(_id, _name);
        }

        public static BrandDtoBuilder ADefaultBrandDto()
        {
            return new BrandDtoBuilder().WithName("Default Brand");
        }

        public static BrandDtoBuilder ABrandDtoNamed(string name)
        {
            return new BrandDtoBuilder().WithName(name);
        }
    }
}
