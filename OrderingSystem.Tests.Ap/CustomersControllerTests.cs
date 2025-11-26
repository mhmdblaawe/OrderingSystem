using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderingSystem.Api.Controllers;
using OrderingSystem.Api.Controllers.OrderingSystem.Api.Controllers;
using OrderingSystem.Application.DTOs.Customers;
using OrderingSystem.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace OrderingSystem.Tests.Api
{
    public class CustomersControllerTests
    {
        [Fact]
        public async Task GetCustomerById_Should_Return_404_When_NotFound()
        {
            var svc = new Mock<ICustomerService>();
            svc.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((CustomerDto?)null);

            var ctrl = new CustomersController(svc.Object);

            var result = await ctrl.GetCustomerById(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task CreateCustomer_Should_Return_Created()
        {
            var svc = new Mock<ICustomerService>();
            svc.Setup(s => s.CreateAsync(It.IsAny<CreateCustomerDto>()))
                .ReturnsAsync(new CustomerDto { Id = 10, Name = "Malik" });

            var ctrl = new CustomersController(svc.Object);

            var dto = new CreateCustomerDto
            {
                Name = "Malik",
                Email = "m@example.com",
                Phone = "0790000000"
            };

            var result = await ctrl.CreateCustomer(dto);

            var created = result as CreatedAtActionResult;
            created.Should().NotBeNull();
            created!.RouteValues!["id"].Should().Be(10);
        }
    }
}
