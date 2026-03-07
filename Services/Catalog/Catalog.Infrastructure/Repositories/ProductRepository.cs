using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specifications;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _products;
        private readonly IMongoCollection<ProductBrand> _brands;
        private readonly IMongoCollection<ProductType> _types;

        public ProductRepository(IOptions<DatabaseSettings> options)
        {
            var settings = options.Value;
            var client = new MongoClient(settings.ConnectionString);
            var db = client.GetDatabase(settings.DatabaseName);

            _products = db.GetCollection<Product>(settings.ProductCollectionName);
            _brands = db.GetCollection<ProductBrand>(settings.BrandCollectionName);
            _types = db.GetCollection<ProductType>(settings.TypeCollectionName);
        }

        public async Task<Product> CreateProduct(Product product)
        {
            await _products.InsertOneAsync(product);
            return product;
        }

        public async Task<bool> DeleteProduct(string productId)
        {
            var deletedProduct = await _products.DeleteOneAsync(x => x.Id == productId);
            return deletedProduct.IsAcknowledged && deletedProduct.DeletedCount > 0;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _products.Find(_ => true).ToListAsync();
        }

        public async Task<ProductBrand> GetBrandByIdAsync(string brandId)
        {
            return await _brands.Find(x => x.Id == brandId).FirstOrDefaultAsync();
        }

        public async Task<Product> GetProduct(string productId)
        {
            return await _products.Find(x => x.Id == productId).FirstOrDefaultAsync();
        }

        public async Task<Pagination<Product>> GetProducts(CatalogSpecParams catalogSpecParams)
        {
            var builder = Builders<Product>.Filter;
            var filter = builder.Empty;

            if (!string.IsNullOrEmpty(catalogSpecParams.Search))
            {
                filter &= builder
                    .Where(x => x.Name.ToLower().Contains(catalogSpecParams.Search.ToLower()));
            }

            if (!string.IsNullOrEmpty(catalogSpecParams.BrandId))
            {
                filter &= builder.Eq(x => x.Brand.Id, catalogSpecParams.BrandId);
            }

            if (!string.IsNullOrEmpty(catalogSpecParams.TypeId))
            {
                filter &= builder.Eq(x => x.Type.Id, catalogSpecParams.TypeId);
            }

            var totalItems = await _products.CountDocumentsAsync(filter);
            var data = await ApplyDataFilters(catalogSpecParams, filter);

            return new Pagination<Product>(
                catalogSpecParams.PageIndex, catalogSpecParams.PageSize, (int)totalItems, data
            );
        }

        public async Task<IEnumerable<Product>> GetProductsByBrand(string name)
        {
            return await _products
                .Find(x => x.Brand.Name.ToLower() == name.ToLower())
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByName(string name)
        {
            var filter = Builders<Product>.Filter
                .Regex(x => x.Name, new BsonRegularExpression($".*{name}.*", "i"));
            
            return await _products
                .Find(filter)
                .ToListAsync();
        }

        public async Task<ProductType> GetTypeByIdAsync(string typeId)
        {
            return await _types.Find(x => x.Id == typeId).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updatedProduct = await _products.ReplaceOneAsync(x => x.Id == product.Id, product);

            return updatedProduct.IsAcknowledged && updatedProduct.ModifiedCount > 0;
        }

        private async Task<IReadOnlyCollection<Product>> ApplyDataFilters(CatalogSpecParams catalogSpecParams, FilterDefinition<Product> filter)
        {
            var sortDefinition = Builders<Product>.Sort.Ascending("Name");

            if (!string.IsNullOrEmpty(catalogSpecParams.Sort))
            {
                sortDefinition = catalogSpecParams.Sort.ToLower() switch
                {
                    "priceAsc" => Builders<Product>.Sort.Ascending(x => x.Price),
                    "priceDesc" => Builders<Product>.Sort.Descending(x => x.Price),
                    _ => Builders<Product>.Sort.Ascending(x => x.Name)
                };
            }

            return await _products
                .Find(filter)
                .Sort(sortDefinition)
                .Skip((catalogSpecParams.PageIndex - 1) * catalogSpecParams.PageSize)
                .Limit(catalogSpecParams.PageSize)
                .ToListAsync();
        }
    }
}
