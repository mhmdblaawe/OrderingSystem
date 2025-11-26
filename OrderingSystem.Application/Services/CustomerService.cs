using OrderingSystem.Application.DTOs.Customers;
using OrderingSystem.Application.Interfaces;
using OrderingSystem.Domain.DbModels;

namespace OrderingSystem.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;

        public CustomerService(ICustomerRepository repo)
        {
            _repo = repo;
        }

        public async Task<CustomerDto?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetActiveByIdAsync(id);
            if (entity == null) return null;

            return new CustomerDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                Phone = entity.Phone,
                
            };
        }

        public async Task<(int TotalCount, List<CustomerDto> Items)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? name,
            string? email)
        {
            var items = await _repo.GetPagedAsync(pageNumber, pageSize, name, email);
       

            return (items.TotalCount, items.Items.Select(c => new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,
             }).ToList());
        }

        public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
        {
            if (await _repo.EmailExistsAsync(dto.Email))
                throw new InvalidOperationException("Email already exists.");

            var entity = Customers.Create(dto.Name, dto.Email, dto.Phone);

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();

            return new CustomerDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                Phone = entity.Phone,
              
            };
        }

        public async Task<CustomerDto?> UpdateAsync(int id, UpdateCustomerDto dto)
        {
            var entity = await _repo.GetActiveByIdAsync(id);
            if (entity == null) return null;

            // If email changed, check uniqueness
            if (dto.Email != entity.Email)
            {
                if (await _repo.EmailExistsAsync(dto.Email))
                    throw new InvalidOperationException("Email already exists.");
            }

            entity.Update(dto.Name, dto.Email, dto.Phone);
            _repo.Update(entity);
            await _repo.SaveChangesAsync();

            return new CustomerDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                Phone = entity.Phone,
                 
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetActiveByIdAsync(id);
            if (entity == null) return false;

            entity.MarkDeleted();
            _repo.Update(entity);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
