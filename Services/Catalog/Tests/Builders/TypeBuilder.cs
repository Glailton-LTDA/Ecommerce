using Catalog.Core.Entities;
using MongoDB.Bson;

namespace Tests.Builders
{
    public class TypeBuilder
    {
        private string _id = ObjectId.GenerateNewId().ToString();
        private string _name = "Test Type";

        public TypeBuilder WithId(string id)
        {
            _id = id;
            return this;
        }

        public TypeBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ProductType Build()
        {
            return new ProductType
            {
                Id = _id,
                Name = _name
            };
        }

        public static TypeBuilder ADefaultType()
        {
            return new TypeBuilder().WithName("Default Type");
        }

        public static TypeBuilder ATypeNamed(string name)
        {
            return new TypeBuilder().WithName(name);
        }

        public static List<ProductType> MultipleTypes(int count)
        {
            var types = new List<ProductType>();
            for (int i = 0; i < count; i++)
            {
                types.Add(new TypeBuilder().WithName($"Type {i + 1}").Build());
            }
            return types;
        }
    }
}
