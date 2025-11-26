using FluentAssertions;
using OrderingSystem.Domain.Common;
using OrderingSystem.Domain.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Tests.Domain
{
    public class ProductsTests
    {
        [Fact]
        public void Create_Should_Set_Initial_Values()
        {
            var p = Products.Create("Mouse", "SKU-1", 10m, 5);

            p.Name.Should().Be("Mouse");
            p.SKU.Should().Be("SKU-1");
            p.Price.Should().Be(10m);
            p.StockQuantity.Should().Be(5);
            p.IsDeleted.Should().BeFalse();
            p.IsActive.Should().BeTrue();
        }

        [Fact]
        public void DecreaseStock_Should_Reduce_Stock_When_Sufficient()
        {
            var p = Products.Create("Mouse", "SKU-1", 10m, 5);

            p.DecreaseStock(3);

            p.StockQuantity.Should().Be(2);
        }

        [Fact]
        public void DecreaseStock_Should_Throw_When_Insufficient()
        {
            var p = Products.Create("Mouse", "SKU-1", 10m, 2);

            Action act = () => p.DecreaseStock(5);

            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void MarkDeleted_Should_Set_Flags()
        {
            var p = Products.Create("Mouse", "SKU-1", 10m, 2);

            p.MarkDeleted();

            p.IsDeleted.Should().BeTrue();
            p.IsActive.Should().BeFalse();
        }
    }
}

