using Catalog.Aplication.Commands;
using MongoDB.Bson;

namespace Tests.Builders
{
    public class CreateProductCommandBuilder
    {
        private string _name = "Test Product";
        private string _summary = "Test Summary";
        private string _description = "Test Description";
        private string _imageFile = "test-image.jpg";
        private string _brandId = ObjectId.GenerateNewId().ToString();
        private string _typeId = ObjectId.GenerateNewId().ToString();
        private decimal _price = 99.99m;

        public CreateProductCommandBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public CreateProductCommandBuilder WithSummary(string summary)
        {
            _summary = summary;
            return this;
        }

        public CreateProductCommandBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public CreateProductCommandBuilder WithImageFile(string imageFile)
        {
            _imageFile = imageFile;
            return this;
        }

        public CreateProductCommandBuilder WithBrandId(string brandId)
        {
            _brandId = brandId;
            return this;
        }

        public CreateProductCommandBuilder WithTypeId(string typeId)
        {
            _typeId = typeId;
            return this;
        }

        public CreateProductCommandBuilder WithPrice(decimal price)
        {
            _price = price;
            return this;
        }

        public CreateProductCommand Build()
        {
            return new CreateProductCommand
            {
                Name = _name,
                Summary = _summary,
                Description = _description,
                ImageFile = _imageFile,
                BrandId = _brandId,
                TypeId = _typeId,
                Price = _price
            };
        }

        public static CreateProductCommandBuilder ADefaultCreateProductCommand()
        {
            return new CreateProductCommandBuilder();
        }

        public static CreateProductCommandBuilder ACreateProductCommandWithName(string name)
        {
            return new CreateProductCommandBuilder().WithName(name);
        }
    }
}
