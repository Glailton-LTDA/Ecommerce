using Catalog.Core.Entities;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.IO;
using System.Reflection;
using System.Net;
using System.Text.Json;

namespace Catalog.Infrastructure.Data
{
    public class DatabaseSeeder
    {
        public static async Task SeedAsync(IMongoClient client, IOptions<DatabaseSettings> options, string contentRootPath, ILogger logger)
        {
            var settings = options.Value;
            var db = client.GetDatabase(settings.DatabaseName);

            var products = db.GetCollection<Product>(settings.ProductCollectionName);
            var brands = db.GetCollection<ProductBrand>(settings.BrandCollectionName);
            var types = db.GetCollection<ProductType>(settings.TypeCollectionName);

            // dois locais possíveis para os arquivos de seed:
            var apiSeedPath = Path.Combine(contentRootPath, "Data", "SeedData");

            var infraAssemblyLocation = Path.GetDirectoryName(typeof(DatabaseSeeder).Assembly.Location) ?? string.Empty;
            var infraSeedPath = Path.Combine(infraAssemblyLocation, "Data", "SeedData");

            string seedBasePath;
            if (Directory.Exists(apiSeedPath))
            {
                seedBasePath = apiSeedPath;
                logger.LogInformation("Seed directory found in API content root: {SeedBasePath}", seedBasePath);
            }
            else if (Directory.Exists(infraSeedPath))
            {
                seedBasePath = infraSeedPath;
                logger.LogInformation("Seed directory found in Infrastructure assembly output: {SeedBasePath}", seedBasePath);
            }
            else
            {
                logger.LogWarning("Seed directory not found. Checked API: {ApiSeedPath} and Infrastructure: {InfraSeedPath}. Skipping seeding.",
                    apiSeedPath, infraSeedPath);
                return;
            }

            // aguarda resolução DNS do host do Mongo antes de tentar conectar
            var mongoUrl = new MongoUrl(settings.ConnectionString);
            var mongoHost = mongoUrl?.Server?.Host ?? "localhost";

            const int dnsMaxAttempts = 30;
            var dnsAttempt = 0;
            while (true)
            {
                try
                {
                    var addresses = Dns.GetHostAddresses(mongoHost);
                    if (addresses != null && addresses.Length > 0)
                    {
                        logger.LogInformation("Resolved Mongo host {Host} to {Addresses}", mongoHost, string.Join(',', addresses));
                        break;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogDebug(ex, "DNS resolution for {Host} failed on attempt {Attempt}", mongoHost, dnsAttempt + 1);
                }

                dnsAttempt++;
                if (dnsAttempt >= dnsMaxAttempts)
                {
                    logger.LogWarning("DNS did not resolve {Host} after {Attempts} attempts. Proceeding to connection retries.", mongoHost, dnsMaxAttempts);
                    break;
                }
                await Task.Delay(1000);
            }

            // wait for DB to become available (retry with backoff)
            const int maxAttempts = 10;
            var attempt = 0;
            while (true)
            {
                try
                {
                    var ping = new BsonDocument("ping", 1);
                    await db.RunCommandAsync<BsonDocument>(ping);
                    break;
                }
                catch (Exception ex)
                {
                    attempt++;
                    logger.LogWarning(ex, "MongoDB ping/connection failed (attempt {Attempt}/{Max}).", attempt, maxAttempts);
                    if (attempt >= maxAttempts)
                    {
                        logger.LogError(ex, "MongoDB not available after {MaxAttempts} attempts. Aborting seeding.", maxAttempts);
                        throw;
                    }
                    // exponential-ish backoff
                    await Task.Delay(1000 * attempt);
                }
            }

            static async Task<List<T>> ReadListFromFileAsync<T>(string filePath, ILogger logger)
            {
                if (!File.Exists(filePath))
                {
                    logger.LogWarning("Seed file not found: {FilePath}", filePath);
                    return new List<T>();
                }

                var json = await File.ReadAllTextAsync(filePath);
                try
                {
                    return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to deserialize seed file {FilePath}", filePath);
                    return new List<T>();
                }
            }

            //Seed Brands
            var brandFile = Path.Combine(seedBasePath, "brands.json");
            var hasAnyBrand = await brands.Find(_ => true).Limit(1).AnyAsync();
            if (!hasAnyBrand)
            {
                var brandList = await ReadListFromFileAsync<ProductBrand>(brandFile, logger);
                if (brandList.Count > 0)
                    await brands.InsertManyAsync(brandList);
            }

            //Seed Types
            var typeFile = Path.Combine(seedBasePath, "types.json");
            var hasAnyType = await types.Find(_ => true).Limit(1).AnyAsync();
            if (!hasAnyType)
            {
                var typeList = await ReadListFromFileAsync<ProductType>(typeFile, logger);
                if (typeList.Count > 0)
                    await types.InsertManyAsync(typeList);
            }

            //Seed Products
            var productFile = Path.Combine(seedBasePath, "products.json");
            var hasAnyProduct = await products.Find(_ => true).Limit(1).AnyAsync();
            if (!hasAnyProduct)
            {
                var productList = await ReadListFromFileAsync<Product>(productFile, logger);

                foreach (var product in productList)
                {
                    product.Id = null;
                    if (product.CreatedDate == default)
                        product.CreatedDate = DateTimeOffset.UtcNow;
                }
                if (productList.Count > 0)
                    await products.InsertManyAsync(productList);
            }
        }
    }
}
