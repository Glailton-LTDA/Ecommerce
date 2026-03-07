# Quick Reference

One-page reference for common tasks.

## Essential Commands

### Docker

```bash
# Start services
docker-compose up -d

# View logs
docker-compose logs -f

# Stop services
docker-compose down

# Restart services
docker-compose restart

# Remove everything
docker-compose down -v
```

### Testing

```bash
# Run all tests
dotnet test Ecommerce.slnx

# Run with coverage
dotnet test Ecommerce.slnx /p:CollectCoverage=true

# Run specific test
dotnet test --filter "ClassName=DeleteProductIdHandlerTests"
```

### Build & Run

```bash
# Restore
dotnet restore Ecommerce.slnx

# Build
dotnet build Ecommerce.slnx

# Run API (local)
cd Services/Catalog/Catalog.API
dotnet run
```

---

## Service URLs

| Service | URL | Port |
|---------|-----|------|
| Catalog API | `http://localhost:5001/api/v1/catalog` | 5001 |
| Swagger UI | `http://localhost:5001/swagger` | 5001 |
| MongoDB | `mongodb://localhost:27017` | 27017 |

---

## API Endpoints

### Products

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/GetAllProducts` | Get all products (paginated) |
| GET | `/{id}` | Get single product |
| GET | `/productName/{name}` | Search by name |
| GET | `/productBrand/{brand}` | Filter by brand |
| POST | `/` | Create product |
| PUT | `/id?id={id}` | Update product |
| DELETE | `/id?id={id}` | Delete product |

### Brands & Types

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/GetAllBrands` | Get all brands |
| GET | `/GetAllTypes` | Get all types |

---

## Environment Variables

```
DatabaseSettings__ConnectionString = mongodb://admin:password@mongodb:27017/CatalogDb?authSource=admin
DatabaseSettings__DatabaseName = CatalogDb
ASPNETCORE_ENVIRONMENT = Development
ASPNETCORE_URLS = http://+:8080
```

---

## Project Structure

```
Ecommerce/
├── Services/Catalog/
│   ├── Catalog.Core/           Domain models
│   ├── Catalog.Aplication/     CQRS handlers
│   ├── Catalog.Infrastructure/ Data access
│   ├── Catalog.API/            REST endpoints
│   └── Tests/                  Unit & Integration tests
├── .github/workflows/          CI/CD
└── Documentation files
```

---

## MongoDB Credentials

```
Host: mongodb
Port: 27017
Username: admin
Password: password
```

---

## Troubleshooting

### Port in use?
```bash
# Change port in docker-compose.yml
ports:
  - "5002:8080"
```

### Connection failed?
```bash
# Wait 30 seconds for services to start
docker-compose logs
```

### Start fresh?
```bash
docker-compose down -v
docker-compose up -d
```

---

## Documentation Files

| File | Purpose |
|------|---------|
| `README.md` | Main documentation |
| `INDEX.md` | Documentation map |
| `SETUP_CHECKLIST.md` | Setup guide |
| `DOCKER_QUICK_START.md` | Docker reference |
| `DOCUMENTATION.md` | Documentation index |
| `Services/Catalog/CATALOG_SERVICE.md` | API reference |

---

## Test API Examples

```bash
# Create product
curl -X POST "http://localhost:5001/api/v1/catalog" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test",
    "summary": "Test",
    "description": "Test",
    "imageFile": "test.jpg",
    "brandId": "607f1f77bcf86cd799439001",
    "typeId": "607f1f77bcf86cd799439002",
    "price": 99.99
  }'

# Get all products
curl "http://localhost:5001/api/v1/catalog/GetAllProducts"

# Get product by ID
curl "http://localhost:5001/api/v1/catalog/607f1f77bcf86cd799439011"

# Delete product
curl -X DELETE "http://localhost:5001/api/v1/catalog/id?id=607f1f77bcf86cd799439011"
```

---

## Key Files

```
docker-compose.yml.example  ← Copy to docker-compose.yml
Services/Catalog/Catalog.API/Dockerfile  ← Service image
.github/workflows/dotnet-tests.yml  ← CI/CD
Ecommerce.slnx  ← Solution file
```

---

**Need more info?** See [README.md](README.md) or [INDEX.md](INDEX.md)

**Version:** 1.0.0
