using Catalog.Core.Entities;
using MongoDB.Bson;

namespace Tests.Builders
{
    public class ProductBuilder
    {
        private string _id = ObjectId.GenerateNewId().ToString();
        private string _name = "Test Product";
        private string _summary = "Test Summary";
        private string _description = "Test Description";
        private string _imageFile = "test-image.jpg";
        private ProductBrand _brand;
        private ProductType _type;
        private decimal _price = 99.99m;
        private DateTimeOffset _createdDate = DateTimeOffset.UtcNow;

        public ProductBuilder()
        {
            _brand = BrandBuilder.ADefaultBrand().Build();
            _type = TypeBuilder.ADefaultType().Build();
        }

        public ProductBuilder WithId(string id)
        {
            _id = id;
            return this;
        }

        public ProductBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ProductBuilder WithSummary(string summary)
        {
            _summary = summary;
            return this;
        }

        public ProductBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public ProductBuilder WithImageFile(string imageFile)
        {
            _imageFile = imageFile;
            return this;
        }

        public ProductBuilder WithBrand(ProductBrand brand)
        {
            _brand = brand;
            return this;
        }

        public ProductBuilder WithType(ProductType type)
        {
            _type = type;
            return this;
        }

        public ProductBuilder WithPrice(decimal price)
        {
            _price = price;
            return this;
        }

        public ProductBuilder WithCreatedDate(DateTimeOffset createdDate)
        {
            _createdDate = createdDate;
            return this;
        }

        public Product Build()
        {
            return new Product
            {
                Id = _id,
                Name = _name,
                Summary = _summary,
                Description = _description,
                ImageFile = _imageFile,
                Brand = _brand,
                Type = _type,
                Price = _price,
                CreatedDate = _createdDate
            };
        }

        public static ProductBuilder ADefaultProduct()
        {
            return new ProductBuilder();
        }

        public static ProductBuilder AProductNamed(string name)
        {
            return new ProductBuilder().WithName(name);
        }

        public static ProductBuilder AProductWithPrice(decimal price)
        {
            return new ProductBuilder().WithPrice(price);
        }
    }
}
