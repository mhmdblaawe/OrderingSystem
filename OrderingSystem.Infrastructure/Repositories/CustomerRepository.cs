using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrderingSystem.Application.Interfaces;
using OrderingSystem.Domain.DbModels;
using OrderingSystem.Infrastructure.Extensions.OrderingSystem.Infrastructure.Extensions;
using OrderingSystem.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly OrderingDbContext _context;
        private readonly IConfiguration _config;

        public CustomerRepository(OrderingDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task AddAsync(Customers entity)
        {
            await _context.Customers.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<Customers> entities)
        {
            await _context.Customers.AddRangeAsync(entities);
        }

        public async Task<int> CountAsync(string? nameFilter, string? emailFilter)
        {
            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nameFilter))
                query = query.Where(c => c.Name.Contains(nameFilter));

            if (!string.IsNullOrWhiteSpace(emailFilter))
                query = query.Where(c => c.Email.Contains(emailFilter));

            return await query.CountAsync();
        }

        public void Delete(Customers entity)
        {
            _context.Customers.Remove(entity);
        }


        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Customers.AnyAsync(c => c.Email == email && !c.IsDeleted);
        }


        public async Task<List<Customers>> FindAsync(Expression<Func<Customers, bool>> predicate)
        {
            return await _context.Customers.Where(predicate).ToListAsync();
        }

        public async Task<Customers?> GetActiveByIdAsync(int id)
        {
            return await _context.Customers
                .Where(c => !c.IsDeleted && c.Id == id)
                .FirstOrDefaultAsync();
        }


        public async Task<List<Customers>> GetAllAsync()
        {
            return await _context.Customers
                .Where(c => !c.IsDeleted)
                .ToListAsync();
        }
        public async Task<Customers?> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<(List<Customers> Items, int TotalCount)> GetPagedAsync(int pageNumber,int pageSize,string? nameFilter,string? emailFilter)
        {
            var list = new List<Customers>();
            int totalCount = 0;

            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync();

            using var cmd = new SqlCommand("GetCustomersPaged", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@CustomerName", (object?)nameFilter ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CustomerEmail", (object?)emailFilter ?? DBNull.Value);

            using var reader = await cmd.ExecuteReaderAsync();
 
            while (await reader.ReadAsync())
            {
                list.Add(Customers.CreateFromDb(
                    reader.GetIntSafe("Id"),
                    reader.GetStringSafe("CustomerName") ?? string.Empty,
                    reader.GetStringSafe("Email") ?? string.Empty,
                    reader.GetStringSafe("Phone") ?? string.Empty,
                    reader.GetDateTimeSafe("CreatedAt"),
                    reader.GetBoolean("IsDeleted"),
                    reader.GetBoolSafe("IsActive")
                ));
            }
 
            if (await reader.NextResultAsync())
            {
                if (await reader.ReadAsync())
                {
                    totalCount = reader.GetInt32(reader.GetOrdinal("TotalCount"));
                }
            }

            return (list, totalCount);
        }


        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }


        public void Update(Customers entity)
        {
            _context.Customers.Update(entity);
        }
    }
}
