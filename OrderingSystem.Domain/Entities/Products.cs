using OrderingSystem.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace OrderingSystem.Domain.DbModels
{
    public class Products
    {
        private Products() { } // EF Core

        public Products(string name, string sku, decimal price, int stockQuantity)
        {
            Name = name;
            SKU = sku;
            Price = price;
            StockQuantity = stockQuantity;
            CreatedAt = DateTime.UtcNow;
            IsDeleted = false;
            IsActive = true;
        }

        [Key]
        public int Id { get; private set; }

        [Required]
        public string Name { get; private set; } = string.Empty;

        [Required]
        public string SKU { get; private set; } = string.Empty;

        [Required]
        public decimal Price { get; private set; }

        [Required]
        public int StockQuantity { get; private set; }

        [Required]
        public DateTime CreatedAt { get; private set; }

        public bool IsActive { get; private set; }
        public bool IsDeleted { get; private set; }


 

        public void Update(string name, string sku, decimal price, int stockQuantity)
        {
            Name = name;
            SKU = sku;
            Price = price;
            StockQuantity = stockQuantity;
        }

        public void MarkDeleted()
        {
            IsDeleted = true;
            IsActive = false;
        }

        public void DecreaseStock(int qty)
        {
            if (qty <= 0)
                throw new DomainException("Quantity must be greater than 0.");

            if (StockQuantity < qty)
                throw new DomainException($"Insufficient stock for product SKU {SKU}.");

            StockQuantity -= qty;
        }

        public void IncreaseStock(int qty)
        {
            if (qty <= 0)
                throw new DomainException("Quantity must be greater than 0.");

            StockQuantity += qty;
        }
 

        public static Products Create(string name, string sku, decimal price, int stockQuantity)
        {
            return new Products(name, sku, price, stockQuantity);
        }

        public static Products CreateFromDb(
            int id,
            string name,
            string sku,
            decimal price,
            int stockQuantity,
            DateTime createdAt,
            bool isDeleted,
            bool isActive)
        {
            return new Products
            {
                Id = id,
                Name = name,
                SKU = sku,
                Price = price,
                StockQuantity = stockQuantity,
                CreatedAt = createdAt,
                IsDeleted = isDeleted,
                IsActive = isActive
            };
        }
    }
}
