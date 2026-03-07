# Testes do Serviço Catalog

Este projeto contém uma suíte abrangente de testes para o serviço Catalog, incluindo testes unitários, de mapeadores, controladores e testes de integração.

## Estrutura dos Testes

### 1. **Builders** (`/Builders`)
Classes builder para criar dados mockados facilmente nos testes.

#### Arquivos:
- `ProductBuilder.cs` - Cria instâncias de Product para testes
- `BrandBuilder.cs` - Cria instâncias de ProductBrand para testes
- `TypeBuilder.cs` - Cria instâncias de ProductType para testes
- `CreateProductCommandBuilder.cs` - Cria comandos de criação de produto
- `ProductDtoBuilder.cs` - Cria DTOs de produto para testes
- `BrandDtoBuilder.cs` - Cria DTOs de marca para testes
- `TypeDtoBuilder.cs` - Cria DTOs de tipo para testes

#### Exemplo de Uso:
```csharp
// Criar um produto padrão
var product = ProductBuilder.ADefaultProduct().Build();

// Criar um produto com nome específico
var productNamed = ProductBuilder.AProductNamed("Test Product").Build();

// Criar um produto com múltiplos atributos
var customProduct = ProductBuilder.ADefaultProduct()
    .WithName("Custom Product")
    .WithPrice(99.99m)
    .WithBrand(someBrand)
    .Build();

// Criar múltiplas marcas
var brands = BrandBuilder.MultipleBrands(5);
```

### 2. **Handlers Tests** (`/Handlers`)
Testes para os handlers de comandos (Commands) e queries.

#### Arquivos:
- `CreateProductHandlerTests.cs` - Testa a criação de produtos
- `UpdateProductHandlerTests.cs` - Testa a atualização de produtos
- `DeleteProductIdHandlerTests.cs` - Testa a exclusão de produtos
- `GetAllProductsHandlerTests.cs` - Testa a recuperação paginada de produtos
- `GetProductByIdHandlerTests.cs` - Testa a busca de produto por ID
- `GetProductByNameHandlerTests.cs` - Testa a busca de produto por nome
- `GetProductsByBrandHandlerTests.cs` - Testa a busca de produtos por marca
- `GetAllBrandsHandlerTests.cs` - Testa a recuperação de todas as marcas
- `GetAllTypesHandlerTests.cs` - Testa a recuperação de todos os tipos

#### Cobertura de Testes:
- ✅ Sucesso com dados válidos
- ✅ Falha com dados inválidos
- ✅ Exceções appropriadas lançadas
- ✅ Métodos do repositório chamados corretamente

### 3. **Mappers Tests** (`/Mappers`)
Testes para as classes de mapeamento (extensões).

#### Arquivos:
- `ProductMapperTests.cs` - Testa mapeamento de Product → ProductResponse e ProductResponse → ProductDto
- `BrandMapperTests.cs` - Testa mapeamento de ProductBrand → BrandResponse
- `TypeMapperTests.cs` - Testa mapeamento de ProductType → TypeResponse

#### Cobertura:
- Mapeamento de entidades simples
- Mapeamento de coleções
- Tratamento de valores nulos
- Mapeamento de objetos aninhados

### 4. **Controller Tests** (`/Controllers`)
Testes do controlador API.

#### Arquivo:
- `CatalogControllerTests.cs` - Testa todos os endpoints do CatalogController

#### Endpoints Testados:
- ✅ GET `/GetAllProducts` - Retorna produtos paginados
- ✅ GET `/{id}` - Retorna um produto específico
- ✅ GET `/productName/{productName}` - Busca por nome
- ✅ GET `/productBrand/{productBrand}` - Busca por marca
- ✅ POST `/` - Cria novo produto
- ✅ PUT `/{id}` - Atualiza produto
- ✅ DELETE `/{id}` - Deleta produto
- ✅ GET `/GetAllBrands` - Lista todas as marcas
- ✅ GET `/GetAllTypes` - Lista todos os tipos

### 5. **Infrastructure Tests** (`/Infrastructure/Repositories`)
Testes da camada de infraestrutura.

#### Arquivos:
- `ProductRepositoryTests.cs` - Testa operações CRUD de produtos
- `BrandRepositoryTests.cs` - Testa operações com marcas
- `TypeRepositoryTests.cs` - Testa operações com tipos

### 6. **DTOs Tests** (`/Dtos`)
Testes das classes DTO.

#### Arquivos:
- `ProductDtoTests.cs` - Testa ProductDto e suas propriedades
- `BrandDtoTests.cs` - Testa BrandDto e suas propriedades
- `TypeDtoTests.cs` - Testa TypeDto e suas propriedades

#### Cobertura:
- Imutabilidade de records
- Inicialização de propriedades
- Modificação usando `with` expressions

### 7. **Specifications Tests** (`/Specifications`)
Testes das classes de especificação.

#### Arquivos:
- `CatalogSpecParamsTests.cs` - Testa parâmetros de paginação e filtros
  - Valores padrão
  - Limite de tamanho de página
  - Filtros por search, brand, type
  
- `PaginationTests.cs` - Testa classe de paginação
  - Inicialização com dados
  - Dados vazios
  - Rastreamento de página

### 8. **Integration Tests** (`/Integration`)
Testes de integração entre múltiplos componentes.

#### Arquivo:
- `CatalogIntegrationTests.cs` - Testa fluxos completos de negócio

#### Cenários Testados:
- ✅ Criar e recuperar produto
- ✅ Criar múltiplos produtos e listar todos
- ✅ Deletar e verificar inexistência
- ✅ Atualizar produto
- ✅ Buscar produtos por nome
- ✅ Buscar produtos por marca

## Recursos de Teste

### Mock Framework
- **Moq** - Para criar mocks de dependências
- Injeção de dependências mockadas

### Bibliotecas de Teste
- **xUnit** - Framework de teste
- **Moq** - Mocking library

## Padrões de Teste

### Padrão AAA (Arrange-Act-Assert)
```csharp
[Fact]
public async Task Handle_WithValidId_ShouldReturnProduct()
{
    // Arrange - Setup inicial
    var product = ProductBuilder.ADefaultProduct().Build();
    var query = new GetProductByIdQuery(product.Id);
    
    _mockRepository.Setup(x => x.GetProduct(product.Id))
        .ReturnsAsync(product);

    // Act - Executar o código sendo testado
    var result = await _handler.Handle(query, CancellationToken.None);

    // Assert - Verificar resultados
    Assert.NotNull(result);
    Assert.Equal(product.Id, result.Id);
}
```

### Builder Pattern
```csharp
ProductBuilder.ADefaultProduct()
    .WithName("Custom Name")
    .WithPrice(150.00m)
    .Build();
```

## Executando os Testes

### Executar todos os testes:
```bash
dotnet test Services\Catalog\Tests\Tests.csproj
```

### Executar categoria específica:
```bash
dotnet test Services\Catalog\Tests\Tests.csproj --filter "ClassName=CreateProductHandlerTests"
```

### Com relatório de cobertura:
```bash
dotnet test Services\Catalog\Tests\Tests.csproj /p:CollectCoverage=true
```

## Boas Práticas

1. **Isolamento** - Cada teste é independente
2. **Nomes Descritivos** - Nomes de testes explicam o comportamento esperado
3. **Dados Realistas** - Builders criam dados realistas e válidos
4. **Mocks Apropriados** - Apenas dependências externas são mockadas
5. **Cobertura Completa** - Casos de sucesso, erro e edge cases
6. **Sem Acoplamento** - Testes não dependem um do outro

## Futuras Melhorias

- [ ] Testes de performance
- [ ] Testes de carga com MongoDB
- [ ] Testes E2E com cliente HTTP
- [ ] Testes de autenticação e autorização
- [ ] Testes de validação de entrada
- [ ] Cobertura de 100% de código crítico

## Contribuindo

Ao adicionar novos testes:
1. Use builders para criar dados
2. Siga o padrão AAA
3. Nome o teste descritivamente
4. Inclua casos positivos e negativos
5. Mantenha testes independentes
