using OrderingSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Domain.DbModels
{
    public class Orders
    {
        private readonly List<OrderItems> _items = new();
        private Orders() { }

        public Orders(int customerId, DateTime orderDate)
        {
            CustomerId = customerId;
            OrderDate = orderDate;
            StatusId = 1; 
        }

        [Key]
        public int Id { get; private set; }
        public int CustomerId { get; private set; }
        public DateTime OrderDate { get; private set; }
        public int StatusId { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsDeleted { get; private set; }
        public decimal TotalAmount { get; private set; }
        public IReadOnlyCollection<OrderItems> Items => _items;
        public void AddItem(OrderItems item)
        {
            _items.Add(item);
            RecalculateTotal();
        }

        public void SetItems(IEnumerable<OrderItems> items)
        {
            _items.Clear();
            _items.AddRange(items);
            RecalculateTotal();
        }

        private void RecalculateTotal()
        {
            TotalAmount = _items.Sum(x => x.LineTotal);

        }
        public void UpdateStatus(int newStatus)
        {
            if (StatusId == 4)
                throw new DomainException("Cannot update cancelled order.");

            if (StatusId == 3)
                throw new DomainException("Cannot update shipped order.");

            StatusId = newStatus;
        }
        public void MarkDeleted()
        {
            IsDeleted = true;
            IsActive = false;
        }


    }
}
