using Catalog.Aplication.Dtos;
using MongoDB.Bson;

namespace Tests.Builders
{
    public class ProductDtoBuilder
    {
        private string _id = ObjectId.GenerateNewId().ToString();
        private string _name = "Test Product";
        private string _summary = "Test Summary";
        private string _description = "Test Description";
        private string _imageFile = "test-image.jpg";
        private BrandDto _brand;
        private TypeDto _type;
        private decimal _price = 99.99m;
        private DateTimeOffset _createdDate = DateTimeOffset.UtcNow;

        public ProductDtoBuilder()
        {
            _brand = BrandDtoBuilder.ADefaultBrandDto().Build();
            _type = TypeDtoBuilder.ADefaultTypeDto().Build();
        }

        public ProductDtoBuilder WithId(string id)
        {
            _id = id;
            return this;
        }

        public ProductDtoBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ProductDtoBuilder WithPrice(decimal price)
        {
            _price = price;
            return this;
        }

        public ProductDtoBuilder WithBrand(BrandDto brand)
        {
            _brand = brand;
            return this;
        }

        public ProductDtoBuilder WithType(TypeDto type)
        {
            _type = type;
            return this;
        }

        public ProductDto Build()
        {
            return new ProductDto(
                _id,
                _name,
                _summary,
                _description,
                _imageFile,
                _brand,
                _type,
                _price,
                _createdDate
            );
        }

        public static ProductDtoBuilder ADefaultProductDto()
        {
            return new ProductDtoBuilder();
        }

        public static ProductDtoBuilder AProductDtoNamed(string name)
        {
            return new ProductDtoBuilder().WithName(name);
        }
    }
}
