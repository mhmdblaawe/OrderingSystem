using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Application.DTOs.Products
{
    public class CreateProductDto
    {
        public string Name { get; set; } = "";
        public string SKU { get; set; } = "";
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
