using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Domain.ProcedureEntities
{
    public class OrderDetailsResult
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = "";
        public string CustomerPhone { get; set; } = "";
        public string CustomerEmail{ get; set; } = "";
        public DateTime OrderDate { get; set; }
        public int StatusId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemsResult> Items { get; set; } = new();
    }
}
