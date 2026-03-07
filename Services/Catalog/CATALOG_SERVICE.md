# Catalog Service Documentation

This document contains detailed information about the Catalog Service, including API endpoints, domain models, commands, queries, and usage examples.

## Quick Links

- [API Endpoints](#api-endpoints)
- [Domain Model](#domain-model)
- [Commands & Queries](#commands--queries)
- [DTOs & Responses](#dtos--responses)
- [Error Handling](#error-handling)
- [Usage Examples](#usage-examples)

---

## Overview

The **Catalog Service** is a microservice responsible for managing product information in the e-commerce platform. It handles product CRUD operations, searching, filtering, and managing product metadata like brands and types.

### Architecture

**Design Patterns:**
- **CQRS (Command Query Responsibility Segregation)** - Separate read and write operations
- **Clean Architecture** - Clear separation of concerns
- **Repository Pattern** - Data access abstraction

### Technology Stack

- **.NET 10** - Framework
- **MongoDB** - Database
- **MediatR** - CQRS mediator
- **Docker** - Containerization

---

## Domain Model

### Product Entity

```csharp
public class Product : BaseEntity
{
    public string Name { get; set; }              // Product name
    public string Summary { get; set; }           // Short description
    public string Description { get; set; }       // Full description
    public string ImageFile { get; set; }         // Image URL/path
    public ProductBrand Brand { get; set; }       // Associated brand
    public ProductType Type { get; set; }         // Product type/category
    public decimal Price { get; set; }            // Product price
    public DateTimeOffset CreatedDate { get; set; } // Creation timestamp
}
```

### ProductBrand Entity

```csharp
public class ProductBrand : BaseEntity
{
    public string Name { get; set; }              // Brand name
}
```

### ProductType Entity

```csharp
public class ProductType : BaseEntity
{
    public string Name { get; set; }              // Type/Category name
}
```

---

## API Endpoints

### Base URL
```
/api/v1/catalog
```

### Product Endpoints

#### 1. Get All Products (Paginated & Filtered)
```
GET /api/v1/catalog/GetAllProducts?pageIndex=0&pageSize=10&sort=name
```

**Query Parameters:**
- `pageIndex` (int): Page number (0-based)
- `pageSize` (int): Items per page
- `sort` (string): Sort field (e.g., "name", "price")
- `brand` (string): Filter by brand name
- `type` (string): Filter by type name

**Response:** `200 OK`
```json
{
  "data": [
    {
      "id": "507f1f77bcf86cd799439011",
      "name": "Nike Air Max 90",
      "summary": "Comfortable sneaker",
      "description": "Premium athletic shoe with Air cushioning",
      "imageFile": "nike-air-max-90.jpg",
      "brand": { "id": "...", "name": "Nike" },
      "type": { "id": "...", "name": "Shoes" },
      "price": 129.99,
      "createdDate": "2024-01-15T10:30:00Z"
    }
  ],
  "count": 1,
  "pageIndex": 0,
  "pageSize": 10
}
```

---

#### 2. Get Product by ID
```
GET /api/v1/catalog/{id}
```

**Parameters:**
- `id` (string): Product ID (MongoDB ObjectId)

**Response:** `200 OK` or `404 Not Found`

**Example:**
```bash
curl "https://api.example.com/api/v1/catalog/507f1f77bcf86cd799439011"
```

---

#### 3. Get Products by Name
```
GET /api/v1/catalog/productName/{productName}
```

**Parameters:**
- `productName` (string): Product name to search

**Response:** `200 OK` with array of products

---

#### 4. Get Products by Brand
```
GET /api/v1/catalog/productBrand/{productBrand}
```

**Parameters:**
- `productBrand` (string): Brand name to filter by

**Response:** `200 OK` or `404 Not Found`

---

#### 5. Create Product
```
POST /api/v1/catalog
Content-Type: application/json
```

**Request Body:**
```json
{
  "name": "Nike Air Max 90",
  "summary": "Comfortable sneaker",
  "description": "Premium athletic shoe with Air cushioning",
  "imageFile": "nike-air-max-90.jpg",
  "brandId": "507f1f77bcf86cd799439001",
  "typeId": "507f1f77bcf86cd799439002",
  "price": 129.99
}
```

**Response:** `200 OK` with ProductResponse

**Error Handling:**
- `400 Bad Request` - Brand or Type not found
- `500 Internal Server Error` - Server error

---

#### 6. Update Product
```
PUT /api/v1/catalog/id?id={productId}
Content-Type: application/json
```

**Request Body:**
```json
{
  "name": "Updated Nike Air Max 90",
  "summary": "Updated description",
  "description": "Updated full description",
  "imageFile": "updated-image.jpg",
  "brandId": "507f1f77bcf86cd799439001",
  "typeId": "507f1f77bcf86cd799439002",
  "price": 139.99
}
```

**Response:** `204 No Content` or `404 Not Found`

---

#### 7. Delete Product
```
DELETE /api/v1/catalog/id?id={productId}
```

**Response:** `204 No Content` or `404 Not Found`

---

### Brand Endpoints

#### Get All Brands
```
GET /api/v1/catalog/GetAllBrands
```

**Response:** `200 OK` with array of brands

---

### Type Endpoints

#### Get All Types
```
GET /api/v1/catalog/GetAllTypes
```

**Response:** `200 OK` with array of types

---

## Commands & Queries

### Commands (Write Operations)

| Command | Purpose | Handler |
|---------|---------|---------|
| `CreateProductCommand` | Create new product | `CreateProductHandler` |
| `UpdateProductCommand` | Update existing product | `UpdateProductHandler` |
| `DeleteProductIdCommand` | Delete product | `DeleteProductIdHandler` |

### Queries (Read Operations)

| Query | Purpose | Handler |
|-------|---------|---------|
| `GetProductByIdQuery` | Get single product | `GetProductByIdHandler` |
| `GetAllProductsQuery` | Get all products (paginated) | `GetAllProductsHandler` |
| `GetProductByNameQuery` | Search products by name | `GetProductByNameHandler` |
| `GetProductsByBrandQuery` | Get products by brand | `GetProductsByBrandHandler` |
| `GetAllBrandsQuery` | Get all brands | `GetAllBrandsHandler` |
| `GetAllTypesQuery` | Get all types | `GetAllTypesHandler` |

---

## DTOs & Responses

### ProductDto
```json
{
  "id": "507f1f77bcf86cd799439011",
  "name": "Nike Air Max 90",
  "summary": "Comfortable sneaker",
  "description": "Premium athletic shoe",
  "imageFile": "nike-air-max-90.jpg",
  "brand": { "id": "...", "name": "Nike" },
  "type": { "id": "...", "name": "Shoes" },
  "price": 129.99,
  "createdDate": "2024-01-15T10:30:00Z"
}
```

### BrandDto & TypeDto
```json
{
  "id": "507f1f77bcf86cd799439001",
  "name": "Nike"
}
```

---

## Error Handling

| Exception | Scenario | HTTP Status |
|-----------|----------|------------|
| `KeyNotFoundException` | Product not found | 404 Not Found |
| `ApplicationException` | Brand not found | 400 Bad Request |
| `ApplicationException` | Type not found | 400 Bad Request |

---

## Usage Examples

### Create a Product
```bash
curl -X POST "https://api.example.com/api/v1/catalog" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Nike Air Max 90",
    "summary": "Premium athletic shoe",
    "description": "Features Air cushioning technology",
    "imageFile": "nike-air-max-90.jpg",
    "brandId": "507f1f77bcf86cd799439001",
    "typeId": "507f1f77bcf86cd799439002",
    "price": 129.99
  }'
```

### Get All Products
```bash
curl "https://api.example.com/api/v1/catalog/GetAllProducts?pageIndex=0&pageSize=10"
```

### Get Product by ID
```bash
curl "https://api.example.com/api/v1/catalog/507f1f77bcf86cd799439011"
```

### Search by Brand
```bash
curl "https://api.example.com/api/v1/catalog/productBrand/Nike"
```

### Update Product
```bash
curl -X PUT "https://api.example.com/api/v1/catalog/id?id=507f1f77bcf86cd799439011" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Updated Nike Air Max 90",
    "price": 139.99,
    "brandId": "507f1f77bcf86cd799439001",
    "typeId": "507f1f77bcf86cd799439002"
  }'
```

### Delete Product
```bash
curl -X DELETE "https://api.example.com/api/v1/catalog/id?id=507f1f77bcf86cd799439011"
```

---

## Project Structure

```
Services/Catalog/
├── Catalog.Core/              # Domain layer
│   ├── Entities/              # Domain models (Product, Brand, Type)
│   ├── Repositories/          # Repository interfaces
│   └── Specifications/        # Query specifications & pagination
├── Catalog.Aplication/        # Application layer (CQRS)
│   ├── Commands/              # Write operations
│   ├── Queries/               # Read operations
│   ├── Handlers/              # CQRS handlers
│   ├── Dtos/                  # Data Transfer Objects
│   ├── Responses/             # Response models
│   └── Mappers/               # Entity-to-DTO mappings
├── Catalog.Infrastructure/    # Infrastructure layer
│   └── Repositories/          # MongoDB implementations
├── Catalog.API/               # Presentation layer
│   └── Controllers/           # REST endpoints
└── Tests/                     # Unit & Integration tests
```

---

**Last Updated:** January 2024
**Version:** 1.0.0
