using OrderingSystem.Application.DTOs.Orders;
using OrderingSystem.Application.Repositories;
using OrderingSystem.Application.Shared;
using OrderingSystem.Domain.DbModels;
using OrderingSystem.Domain.ProcedureEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Application.Interfaces
{
    public interface IOrderRepository  :IRepository<Orders>
    {
        Task<CreateOrderResult> CreateOrderAsync(int customerId, List<CreateOrderItemDto> items);

        Task<OrderDetailsResult?> GetFullOrderAsync(int id);
        Task<(int TotalCount, List<OrderListResult> Items)> GetPagedOrdersAsync(int pageNumber,int pageSize,int? customerId,int? status,DateTime? startDate,DateTime? endDate);
        Task<Result> DeleteAsync(int id);
        Task<int> CountAsync(
            int? customerId,
            int? status,
            DateTime? startDate,
            DateTime? endDate);
     }
}
