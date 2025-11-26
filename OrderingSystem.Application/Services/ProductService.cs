using OrderingSystem.Application.DTOs;
using OrderingSystem.Application.DTOs.Products;
using OrderingSystem.Application.Interfaces;
using OrderingSystem.Domain.DbModels;

namespace OrderingSystem.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return null;

            return new ProductDto
            {
                Id = entity.Id,
                Name = entity.Name,
                SKU = entity.SKU,
                Price = entity.Price,
                StockQuantity = entity.StockQuantity
            };
        }

        public async Task<(int TotalCount, List<ProductDto> Items)> GetPagedAsync(int pageNumber,int pageSize,string? search,string? name,string? sku)
        {
            var (total, items) = await _repo.GetPagedAsync(pageNumber, pageSize, search, name, sku);

            return (
                total,
                items.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    SKU = p.SKU,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity
                }).ToList()
            );
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            if (await _repo.SkuExistsAsync(dto.SKU))
                throw new InvalidOperationException("SKU already exists.");

            var entity = Products.Create(dto.Name, dto.SKU, dto.Price, dto.StockQuantity);

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();

            return new ProductDto
            {
                Id = entity.Id,
                Name = entity.Name,
                SKU = entity.SKU,
                Price = entity.Price,
                StockQuantity = entity.StockQuantity
            };
        }

        public async Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return null;

            if (dto.SKU != entity.SKU && await _repo.SkuExistsAsync(dto.SKU, id))
                throw new InvalidOperationException("SKU already exists.");

            entity.Update(dto.Name, dto.SKU, dto.Price, dto.StockQuantity);

            _repo.Update(entity);
            await _repo.SaveChangesAsync();

            return new ProductDto
            {
                Id = entity.Id,
                Name = entity.Name,
                SKU = entity.SKU,
                Price = entity.Price,
                StockQuantity = entity.StockQuantity,
                IsActive = entity.IsActive
            };
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return false;

            entity.MarkDeleted();
            _repo.Update(entity);
            await _repo.SaveChangesAsync();

            return true;
        }
    }
}
