# Setup Checklist ✅

Complete guide to get the Ecommerce platform running.

## Prerequisites ✓

- [ ] Git installed
- [ ] Docker Desktop installed (https://www.docker.com/products/docker-desktop)
- [ ] .NET 10 SDK installed (for local development)
- [ ] MongoDB Atlas or local MongoDB (optional, Docker Compose provides it)

---

## Documentation Review

Start with these documents in order:

1. [ ] **[README.md](README.md)** - Main project documentation
2. [ ] **[DOCKER_QUICK_START.md](DOCKER_QUICK_START.md)** - Docker guide
3. [ ] **[Services/Catalog/CATALOG_SERVICE.md](Services/Catalog/CATALOG_SERVICE.md)** - API reference
4. [ ] **[DOCUMENTATION.md](DOCUMENTATION.md)** - Documentation index

---

## Installation (Choose One)

### Option A: Local Development

- [ ] Clone repository: `git clone https://github.com/Glailton-LTDA/Ecommerce.git`
- [ ] Navigate to project: `cd Ecommerce`
- [ ] Restore dependencies: `dotnet restore Ecommerce.slnx`
- [ ] Update appsettings.json with MongoDB connection
- [ ] Build solution: `dotnet build Ecommerce.slnx`
- [ ] Run Catalog API: `cd Services/Catalog/Catalog.API && dotnet run`
- [ ] Test API: `curl http://localhost:5001/api/v1/catalog/GetAllProducts`
- [ ] View Swagger: Open `http://localhost:5001/swagger` in browser

### Option B: Docker Setup (Recommended)

- [ ] Clone repository: `git clone https://github.com/Glailton-LTDA/Ecommerce.git`
- [ ] Navigate to project: `cd Ecommerce`
- [ ] Copy docker-compose: `cp docker-compose.yml.example docker-compose.yml`
- [ ] Start services: `docker-compose up -d`
- [ ] Wait for services to start (30 seconds)
- [ ] Test API: `curl http://localhost:5001/api/v1/catalog/GetAllProducts`
- [ ] View Swagger: Open `http://localhost:5001/swagger` in browser

---

## Verification

After setup, verify everything works:

### Local Development
- [ ] `dotnet build Ecommerce.slnx` - Build successful
- [ ] `dotnet test Ecommerce.slnx` - Tests pass
- [ ] API responds to requests
- [ ] Swagger UI loads

### Docker Setup
- [ ] `docker-compose ps` - All services running
- [ ] API responds to requests
- [ ] Swagger UI loads
- [ ] MongoDB accessible: `docker-compose exec mongodb mongosh`

---

## First Steps

Once everything is running:

1. [ ] **Explore API**
   - [ ] Get all products: `GET /api/v1/catalog/GetAllProducts`
   - [ ] Get all brands: `GET /api/v1/catalog/GetAllBrands`
   - [ ] Get all types: `GET /api/v1/catalog/GetAllTypes`

2. [ ] **Create a product**
   ```bash
   curl -X POST "http://localhost:5001/api/v1/catalog" \
     -H "Content-Type: application/json" \
     -d '{
       "name": "Test Product",
       "summary": "Test",
       "description": "Test description",
       "imageFile": "test.jpg",
       "brandId": "607f1f77bcf86cd799439001",
       "typeId": "607f1f77bcf86cd799439002",
       "price": 99.99
     }'
   ```

3. [ ] **Run tests**
   - Local: `dotnet test Ecommerce.slnx`
   - Docker: `docker-compose exec catalog-service dotnet test`

4. [ ] **View logs**
   - Docker: `docker-compose logs -f`

---

## Configuration

### MongoDB
- **Container name:** `ecommerce-mongo`
- **Port:** 27017
- **Username:** admin
- **Password:** password
- **Connection string:** `mongodb://admin:password@localhost:27017`

### Catalog API
- **Container name:** `ecommerce-catalog`
- **Port:** 5001
- **Base URL:** `http://localhost:5001`
- **API:** `http://localhost:5001/api/v1/catalog`
- **Swagger:** `http://localhost:5001/swagger`

---

## Common Operations

### View Service Logs
```bash
docker-compose logs -f                    # All services
docker-compose logs -f catalog-service    # Specific service
docker-compose logs -f mongodb             # MongoDB
```

### Stop Services
```bash
docker-compose stop       # Pause services (keep data)
docker-compose down       # Stop services (keep data)
docker-compose down -v    # Stop services (remove data)
```

### Restart Services
```bash
docker-compose restart catalog-service    # Restart specific service
docker-compose up -d --build              # Rebuild and restart
```

### Connect to MongoDB
```bash
docker-compose exec mongodb mongosh -u admin -p password
```

---

## Testing

### Run All Tests
```bash
# Local
dotnet test Ecommerce.slnx

# Docker
docker-compose exec catalog-service dotnet test
```

### Run Tests with Coverage
```bash
dotnet test Ecommerce.slnx /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### Run Specific Tests
```bash
dotnet test --filter "ClassName=DeleteProductIdHandlerTests"
```

---

## Troubleshooting

### Port 5001 Already in Use
Edit `docker-compose.yml`:
```yaml
ports:
  - "5002:8080"  # Change to different port
```

### MongoDB Connection Failed
- Wait 30 seconds for MongoDB to fully start
- Check logs: `docker-compose logs mongodb`
- Verify connection string in environment

### Services Won't Start
```bash
docker-compose logs              # View error logs
docker-compose build --no-cache # Rebuild images
docker-compose up -d             # Start again
```

### Clear Everything and Start Fresh
```bash
docker-compose down -v           # Remove all volumes/data
docker system prune -a           # Remove unused images
docker-compose up -d             # Start fresh
```

---

## Next Steps

1. **Read Documentation**
   - [ ] API Documentation: [CATALOG_SERVICE.md](Services/Catalog/CATALOG_SERVICE.md)
   - [ ] Docker Guide: [DOCKER_QUICK_START.md](DOCKER_QUICK_START.md)
   - [ ] CI/CD Info: See [README.md - CI/CD Pipeline](README.md#cicd-pipeline)

2. **Development**
   - [ ] Make code changes
   - [ ] Run tests locally
   - [ ] Push to GitHub (triggers CI/CD)

3. **Add New Service** (when ready)
   - [ ] Create `Services/YourService/` structure
   - [ ] Create Dockerfile
   - [ ] Add to docker-compose.yml
   - [ ] Start: `docker-compose up -d --build`

4. **Contributing**
   - [ ] Read [Contributing Guidelines](README.md#contributing)
   - [ ] Create feature branch
   - [ ] Make changes
   - [ ] Run tests
   - [ ] Submit Pull Request

---

## Support Resources

| Need | Resource |
|------|----------|
| Getting started | [README.md](README.md) |
| Docker issues | [DOCKER_QUICK_START.md](DOCKER_QUICK_START.md) |
| API documentation | [CATALOG_SERVICE.md](Services/Catalog/CATALOG_SERVICE.md) |
| Testing | [README.md - Testing](README.md#testing) |
| CI/CD | [README.md - CI/CD Pipeline](README.md#cicd-pipeline) |
| Code coverage | [README.md - Code Coverage](README.md#code-coverage) |
| All docs | [DOCUMENTATION.md](DOCUMENTATION.md) |

---

## Success Criteria ✅

You've successfully set up the project when:

- [ ] Repository cloned locally
- [ ] Documentation reviewed
- [ ] Services running (Docker or local)
- [ ] API responds to requests
- [ ] Swagger UI loads
- [ ] Tests pass
- [ ] Can create/read/update/delete products
- [ ] Ready to develop!

---

**Version:** 1.0.0
**Last Updated:** January 2024
**Repository:** https://github.com/Glailton-LTDA/Ecommerce

🎉 **Happy coding!**
