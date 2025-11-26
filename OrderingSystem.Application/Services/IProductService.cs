using OrderingSystem.Application.DTOs;
using OrderingSystem.Application.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Application.Services
{
     
        public interface IProductService
        {
            Task<(int TotalCount, List<ProductDto> Items)> GetPagedAsync(int pageNumber, int pageSize, string? search, string? name, string? sku);
            Task<ProductDto?> GetByIdAsync(int id);
            Task<ProductDto> CreateAsync(CreateProductDto dto);
            Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto);
            Task<bool> DeleteAsync(int id);
        }
    }
 
