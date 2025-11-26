using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrderingSystem.Application.Interfaces;
using OrderingSystem.Domain.DbModels;
using OrderingSystem.Infrastructure.Extensions;
using OrderingSystem.Infrastructure.Extensions.OrderingSystem.Infrastructure.Extensions;
using OrderingSystem.Infrastructure.Persistence;
using System.Data;
using System.Linq.Expressions;

namespace OrderingSystem.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly OrderingDbContext _context;
        private readonly IConfiguration _config;

        public ProductRepository(OrderingDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        private string ConnStr => _config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


        public async Task<(int TotalCount, List<Products> Items)> GetPagedAsync(int pageNumber, int pageSize, string? search, string? Name, string? SKU)
        {
            var items = new List<Products>();
            int totalCount = 0;

            using var conn = new SqlConnection(ConnStr);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("GetProductsPaged", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@Search", (object?)search ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SKU", (object?)SKU ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Name", (object?)Name ?? DBNull.Value);

            using var reader = await cmd.ExecuteReaderAsync();


            if (await reader.ReadAsync())
                totalCount = reader.GetInt32(0);

            await reader.NextResultAsync();


            while (await reader.ReadAsync())
            {
                items.Add(Products.CreateFromDb(
                    reader.GetIntSafe("Id"),
                    reader.GetStringSafe("Name")!,
                    reader.GetStringSafe("SKU")!,
                    reader.GetDecimalSafe("Price"),
                    reader.GetIntSafe("StockQuantity"),
                    reader.GetDateTimeSafe("CreatedAt"),
                    reader.GetBoolSafe("IsDeleted"),
                    reader.GetBoolSafe("IsActive")
                ));
            }

            return (totalCount, items);
        }


        public Task<int> CountAsync(string? search)
        {
            return Task.FromResult(_context.Products.Count(a => a.IsActive == true));

        }

        public async Task<bool> SkuExistsAsync(string sku)
        {
            using var conn = new SqlConnection(ConnStr);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_CheckSkuExists", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@SKU", sku);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result) > 0;
        }

        public async Task<Products?> GetByIdAsync(int id)
        {
            using var conn = new SqlConnection(ConnStr);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("GetProductById", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return Products.CreateFromDb(
                    reader.GetIntSafe("Id"),
                    reader.GetStringSafe("Name")!,
                    reader.GetStringSafe("SKU")!,
                    reader.GetDecimalSafe("Price"),
                    reader.GetIntSafe("StockQuantity"),
                    reader.GetDateTimeSafe("CreatedAt"),
                    reader.GetBoolSafe("IsDeleted"),
                    reader.GetBoolSafe("IsActive")
                );
            }

            return null;
        }

        public async Task<Products?> GetActiveByIdAsync(int id)
        {
            var product = await GetByIdAsync(id);
            return (product != null && !product.IsDeleted) ? product : null;
        }


        public async Task AddAsync(Products entity)
        {
            await _context.Products.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<Products> entities)
        {
            await _context.Products.AddRangeAsync(entities);
        }

        public void Update(Products entity)
        {
            _context.Products.Update(entity);
        }

        public void Delete(Products entity)
        {
            entity.MarkDeleted();
            _context.Products.Update(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }


        public Task<List<Products>> GetAllAsync()
        {
            return _context.Products
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }

        public Task<List<Products>> FindAsync(Expression<Func<Products, bool>> predicate)
        {
            return _context.Products.Where(predicate).ToListAsync();
        }

        public Task<Products?> GetBySkuAsync(string sku)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SkuExistsAsync(string sku, int? excludeId = null)
        {
            return await _context.Products
                .AnyAsync(p => p.SKU == sku && (excludeId == null || p.Id != excludeId));
        }

    }
}
