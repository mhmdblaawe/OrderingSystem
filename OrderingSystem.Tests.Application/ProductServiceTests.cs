using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OrderingSystem.Application.DTOs.Products;
using OrderingSystem.Application.Interfaces;
using OrderingSystem.Application.Services;
using OrderingSystem.Domain.DbModels;
using Xunit;

namespace OrderingSystem.Tests.Application
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_When_Deleted()
        {
            var p = Products.Create("Mouse", "SKU-1", 10m, 10);
            p.MarkDeleted();

            var repo = new Mock<IProductRepository>();
            repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(p);

            var svc = new ProductService(repo.Object);

            var result = await svc.GetByIdAsync(1);

            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_Should_Return_Dto()
        {
            var repo = new Mock<IProductRepository>();
            repo.Setup(r => r.SkuExistsAsync("SKU-1", null)).ReturnsAsync(false);

            var svc = new ProductService(repo.Object);

            var dto = new CreateProductDto
            {
                Name = "Mouse",
                SKU = "SKU-1",
                Price = 10m,
                StockQuantity = 5
            };

            var result = await svc.CreateAsync(dto);

            result.Name.Should().Be("Mouse");
            repo.Verify(r => r.AddAsync(It.IsAny<Products>()), Times.Once);
        }
    }
}
