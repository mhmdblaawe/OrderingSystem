using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrderingSystem.Application.DTOs.Orders;
using OrderingSystem.Application.Interfaces;
using OrderingSystem.Application.Shared;
using OrderingSystem.Domain.DbModels;
using OrderingSystem.Domain.ProcedureEntities;
using OrderingSystem.Infrastructure.Extensions;
using OrderingSystem.Infrastructure.Extensions.OrderingSystem.Infrastructure.Extensions;
using OrderingSystem.Infrastructure.Persistence;
using System.Data;
using System.Linq.Expressions;

namespace OrderingSystem.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderingDbContext _context;
        private readonly IConfiguration _config;

        public OrderRepository(OrderingDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        private string ConnStr => _config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        public Task AddAsync(Orders entity)
        {
            return _context.Orders.AddAsync(entity).AsTask();
        }

        public Task AddRangeAsync(IEnumerable<Orders> entities)
        {
            return _context.Orders.AddRangeAsync(entities);
        }

        public async Task<int> CountAsync(int? customerId, int? status, DateTime? startDate, DateTime? endDate)
        {
            using var conn = new SqlConnection(ConnStr);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("GetOrdersCount", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@CustomerId", (object?)customerId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@StatusId", (object?)status ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@StartDate", (object?)startDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EndDate", (object?)endDate ?? DBNull.Value);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<CreateOrderResult> CreateOrderAsync(int customerId, List<CreateOrderItemDto> items)
        {
            using var conn = new SqlConnection(ConnStr);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("CreateOrder", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            var tvp = new DataTable();
            tvp.Columns.Add("ProductId", typeof(int));
            tvp.Columns.Add("Quantity", typeof(int));

            foreach (var i in items)
                tvp.Rows.Add(i.ProductId, i.Quantity);

            cmd.Parameters.AddWithValue("@CustomerId", customerId);

            cmd.Parameters.Add(new SqlParameter("@Items", SqlDbType.Structured)
            {
                TypeName = "dbo.OrderItemsTVP",
                Value = tvp
            });

            cmd.Parameters.Add("@NewOrderId", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@IsSuccess", SqlDbType.Bit).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Message", SqlDbType.NVarChar, 4000).Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();

            return new CreateOrderResult
            {
                OrderId = Convert.ToInt32(cmd.Parameters["@NewOrderId"].Value),
                IsSuccess = Convert.ToBoolean(cmd.Parameters["@IsSuccess"].Value),
                Message = cmd.Parameters["@Message"].Value?.ToString() ?? ""
            };
        }

        public void Delete(Orders entity)
        {
            _context.Orders.Remove(entity);
        }

  

        public Task<List<Orders>> FindAsync(Expression<Func<Orders, bool>> predicate)
        {
            return _context.Orders.Where(predicate).ToListAsync();
        }

        public Task<List<Orders>> GetAllAsync()
        {
            return _context.Orders.ToListAsync();
        }

        public Task<Orders?> GetByIdAsync(int id)
        {
            return _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<OrderDetailsResult?> GetFullOrderAsync(int id)
        {
            using var conn = new SqlConnection(ConnStr);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("GetOrderById", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@OrderId", id);

            using var reader = await cmd.ExecuteReaderAsync();

            OrderDetailsResult? header = null;

            if (await reader.ReadAsync())
            {
                header = new OrderDetailsResult
                {
                    OrderId = reader.GetIntSafe("OrderId"),
                    CustomerId = reader.GetIntSafe("CustomerId"),
                    CustomerName = reader.GetStringSafe("CustomerName")!,
                    CustomerPhone = reader.GetStringSafe("CustomerPhone")!,
                    CustomerEmail = reader.GetStringSafe("CustomerEmail")!,
                    OrderDate = reader.GetDateTimeSafe("OrderDate"),
                    StatusId = reader.GetIntSafe("StatusId"),
                    TotalAmount = reader.GetDecimalSafe("TotalAmount")
                };
            }

            if (header == null)
                return null;

            await reader.NextResultAsync();

            while (await reader.ReadAsync())
            {
                header.Items.Add(new OrderItemsResult
                {
                    OrderItemId = reader.GetIntSafe("OrderItemId"),
                    ProductId = reader.GetIntSafe("ProductId"),
                    ProductName = reader.GetStringSafe("ProductName")!,
                    Quantity = reader.GetIntSafe("Quantity"),
                    UnitPrice = reader.GetDecimalSafe("UnitPrice"),
                    LineTotal = reader.GetDecimalSafe("LineTotal")
                });
            }

            return header;
        }

        public async Task<(int TotalCount, List<OrderListResult> Items)> GetPagedOrdersAsync(
        int pageNumber,
        int pageSize,
        int? customerId,
        int? status,
        DateTime? startDate,
        DateTime? endDate)
            {
                var items = new List<OrderListResult>();
                int totalCount = 0;

                using var conn = new SqlConnection(ConnStr);
                await conn.OpenAsync();

                using var cmd = new SqlCommand("GetOrdersPaged", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
                cmd.Parameters.AddWithValue("@CustomerId", (object?)customerId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@StatusId", (object?)status ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@StartDate", (object?)startDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EndDate", (object?)endDate ?? DBNull.Value);

                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    items.Add(new OrderListResult
                    {
                        OrderId = reader.GetIntSafe("OrderId"),
                        OrderDate = reader.GetDateTimeSafe("OrderDate"),
                        StatusDesc = reader.GetStringSafe("StatusDesc"),
                        StatusId = reader.GetIntSafe("StatusId"),
                        TotalAmount = reader.GetDecimalSafe("TotalAmount"),
                        CustomerName = reader.GetStringSafe("CustomerName")!,
                        CustomerPhone = reader.GetStringSafe("CustomerPhone")!
                    });
                }

                await reader.NextResultAsync();

                if (await reader.ReadAsync())
                    totalCount = reader.GetInt32(reader.GetOrdinal("TotalCount"));

                return (totalCount, items);
            }


        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Update(Orders entity)
        {
            _context.Orders.Update(entity);
        }

        
        public async Task<Result> DeleteAsync(int id)
        {
            using var conn = new SqlConnection(ConnStr);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("DeleteOrder", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@OrderId", id);

            try
            {
                await cmd.ExecuteNonQueryAsync();
                return new Result { Success = true, Message = "Order deleted successfully" };
            }
            catch (SqlException ex)
            {
                return new Result { Success = false, Message = ex.Message };
            }
            catch (Exception ex)
            {
                return new Result { Success = false, Message = "Unexpected error: " + ex.Message };

            }
        }
    }
}
