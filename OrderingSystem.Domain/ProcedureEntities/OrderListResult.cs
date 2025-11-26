using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Domain.ProcedureEntities
{
    public class OrderListResult
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string? StatusDesc { get; set; }
        public int? StatusId { get; set; }
        public decimal TotalAmount { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
    }
}
