using OrderingSystem.Application.DTOs.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Application.Services
{
    public interface ICustomerService
    {
        Task<(int TotalCount, List<CustomerDto> Items)> GetPagedAsync(
    int pageNumber,
    int pageSize,
    string? name,
    string? email);
        //Task<int> CountAsync(string? name, string? email);
        Task<CustomerDto?> GetByIdAsync(int id);
        Task<CustomerDto> CreateAsync(CreateCustomerDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
