using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Application.DTOs.Orders
{
    public class UpdateOrderStatusDto
    {
        public int OrderId { get; set; }
        public int StatusId { get; set; }
    }
}
