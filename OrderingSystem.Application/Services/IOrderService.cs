using OrderingSystem.Application.DTOs.Orders;
using OrderingSystem.Application.Shared;
using OrderingSystem.Domain.ProcedureEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Application.Services
{

    public interface IOrderService
    {
        Task<CreateOrderResult> CreateAsync(CreateOrderDto dto);
        Task<OrderDetailsResult?> GetFullAsync(int id);
        Task<(int TotalCount, List<OrderListResult> Items)> GetPagedAsync(int pageNumber, int pageSize, int? customerId, int? statusId, DateTime? startDate, DateTime? endDate);
        Task<bool> UpdateStatusAsync(UpdateOrderStatusDto dto);
        Task<Result> DeleteAsync(int id);
    }
}

