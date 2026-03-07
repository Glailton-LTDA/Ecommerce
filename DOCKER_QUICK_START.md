# Docker Quick Start Guide

This guide shows how to quickly get the Ecommerce platform running with Docker.

## Prerequisites

- **Docker Desktop** installed from https://www.docker.com/products/docker-desktop
- **Git** for cloning the repository

## Quick Start (3 Steps)

### Step 1: Copy Docker Compose File
```bash
# From project root
cp docker-compose.yml.example docker-compose.yml
```

### Step 2: Start Services
```bash
# From project root
docker-compose up -d
```

### Step 3: Test the API
```bash
# Get all products
curl http://localhost:5001/api/v1/catalog/GetAllProducts

# View Swagger UI
# Open browser to: http://localhost:5001/swagger
```

---

## Common Docker Commands

### View Services Status
```bash
docker-compose ps
```

### View Logs
```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f catalog-service
docker-compose logs -f mongodb
```

### Stop Services
```bash
# Keep data
docker-compose stop

# Remove services but keep volumes
docker-compose down

# Remove everything (including data)
docker-compose down -v
```

### Restart Services
```bash
# Restart all
docker-compose restart

# Restart specific service
docker-compose restart catalog-service
```

### Rebuild Images
```bash
# Rebuild Catalog service
docker-compose build --no-cache catalog-service

# Start updated service
docker-compose up -d catalog-service
```

---

## Accessing Services

After `docker-compose up -d`:

| Service | URL | Credentials |
|---------|-----|---|
| Catalog API | `http://localhost:5001/api/v1/catalog` | None |
| Swagger UI | `http://localhost:5001/swagger` | None |
| MongoDB | `mongodb://localhost:27017` | `admin:password` |
| MongoDB Compass | Connect to above | Use Compass GUI |

---

## Development Workflow

### Local Development (No Docker)
```bash
# Start MongoDB in Docker only
docker run -d -p 27017:27017 \
  -e MONGO_INITDB_ROOT_USERNAME=admin \
  -e MONGO_INITDB_ROOT_PASSWORD=password \
  mongo:7.0

# Run API locally
cd Services/Catalog/Catalog.API
dotnet run
```

### Full Docker Development
```bash
# Start all services with code mounting
docker-compose up -d

# Make code changes
# Services auto-reload (if configured)

# View logs
docker-compose logs -f catalog-service
```

### Run Tests in Docker
```bash
# Build test image
docker build -f Services/Catalog/Tests/Dockerfile.tests -t ecommerce-tests:latest .

# Run tests with coverage
docker run --rm ecommerce-tests:latest
```

---

## Troubleshooting

### Services Won't Start

**Check logs:**
```bash
docker-compose logs
```

**Common issues:**
- Port already in use: Change port in `docker-compose.yml`
- MongoDB connection: Wait 10+ seconds for MongoDB to be ready
- Image build failed: Run `docker-compose build --no-cache`

### Database Connection Failed

```bash
# Check if MongoDB is running
docker-compose ps

# Verify connection string
docker-compose exec catalog-service env | grep Database

# Connect to MongoDB directly
docker-compose exec mongodb mongosh -u admin -p password
```

### Port Already in Use

```bash
# Find service using port
netstat -ano | findstr :5001  # Windows
lsof -i :5001                  # Mac/Linux

# Change port in docker-compose.yml
ports:
  - "5002:8080"  # Use different port
```

### Clear Everything and Start Fresh

```bash
# Stop and remove everything
docker-compose down -v

# Remove unused images
docker system prune -a

# Start fresh
docker-compose up -d
```

---

## Monitoring

### View Container Resource Usage
```bash
docker stats

# Specific container
docker stats ecommerce-catalog
```

### Check Container Details
```bash
docker inspect ecommerce-catalog
```

### View Network
```bash
docker network ls
docker network inspect ecommerce-network
```

---

## Adding New Services

When you develop a new service (e.g., Order Service):

1. **Create Dockerfile** in `Services/Order/Order.API/Dockerfile`

2. **Add to docker-compose.yml:**
```yaml
order-service:
  build:
    context: .
    dockerfile: Services/Order/Order.API/Dockerfile
  container_name: ecommerce-order
  ports:
    - "5002:8080"
  environment:
    - DatabaseSettings__ConnectionString=mongodb://admin:password@mongodb:27017/OrderDb?authSource=admin
  depends_on:
    - mongodb
  networks:
    - ecommerce-network
```

3. **Start the new service:**
```bash
docker-compose up -d --build order-service
```

---

## Production Considerations

### Security
- Change MongoDB credentials in `docker-compose.yml`
- Use strong passwords
- Restrict network access
- Use secrets management for sensitive data

### Performance
- Adjust resource limits
- Enable logging drivers
- Use Docker Compose override files for different environments

### Monitoring
- Set up logging (ELK, Splunk, etc.)
- Monitor container health
- Set up alerts

Example resource limits in `docker-compose.yml`:
```yaml
services:
  catalog-service:
    deploy:
      resources:
        limits:
          cpus: '1.0'
          memory: 512M
        reservations:
          cpus: '0.5'
          memory: 256M
```

---

## Useful Resources

- [Docker Documentation](https://docs.docker.com/)
- [Docker Compose Reference](https://docs.docker.com/compose/compose-file/)
- [MongoDB Docker Hub](https://hub.docker.com/_/mongo)
- [.NET Docker Hub](https://hub.docker.com/_/microsoft-dotnet)

---

**Last Updated:** January 2024
