using Catalog.Aplication.Dtos;
using MongoDB.Bson;

namespace Tests.Builders
{
    public class TypeDtoBuilder
    {
        private string _id = ObjectId.GenerateNewId().ToString();
        private string _name = "Test Type";

        public TypeDtoBuilder WithId(string id)
        {
            _id = id;
            return this;
        }

        public TypeDtoBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public TypeDto Build()
        {
            return new TypeDto(_id, _name);
        }

        public static TypeDtoBuilder ADefaultTypeDto()
        {
            return new TypeDtoBuilder().WithName("Default Type");
        }

        public static TypeDtoBuilder ATypeDtoNamed(string name)
        {
            return new TypeDtoBuilder().WithName(name);
        }
    }
}
