using OrderingSystem.Application.Repositories;
using OrderingSystem.Domain.DbModels;
 
namespace OrderingSystem.Application.Interfaces;

public interface ICustomerRepository : IRepository<Customers>
{
    Task<Customers?> GetActiveByIdAsync(int id);
    Task<bool> EmailExistsAsync(string email);

    Task<(List<Customers> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? nameFilter, string? emailFilter);

   
}
