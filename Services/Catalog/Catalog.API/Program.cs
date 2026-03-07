using Catalog.Aplication.Commands;
using Catalog.Aplication.Queries;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System.Reflection;

public partial class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //Register custom Serializers for MongoDB
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        //Add Swagger services
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //Register MediatR services
        var assemblies = new Assembly[]
        {
    Assembly.GetExecutingAssembly(),
    typeof(CreateProductCommand).Assembly,
    typeof(GetAllProductsQuery).Assembly
        };
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

        //Custom services
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IBrandRepository, BrandRepository>();
        builder.Services.AddScoped<ITypeRepository, TypeRepository>();

        //Bind strongly-typed settings
        builder.Services.Configure<DatabaseSettings>(
            builder.Configuration.GetSection("DatabaseSettings"));

        //Register MongoDB client as singleton
        builder.Services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            return new MongoClient(settings.ConnectionString);
        });

        var app = builder.Build();

        //Seed Mongo db on startup using the registered IMongoClient and app content root
        using (var scope = app.Services.CreateScope())
        {
            var options = scope.ServiceProvider.GetRequiredService<IOptions<DatabaseSettings>>();
            var client = scope.ServiceProvider.GetRequiredService<IMongoClient>();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<DatabaseSeeder>>();
            await DatabaseSeeder.SeedAsync(client, options, env.ContentRootPath, logger);
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        //Enable swagger middleware
        app.UseSwagger();
        app.UseSwaggerUI();

        if (app.Environment.IsProduction())
        {
            app.UseHttpsRedirection();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}