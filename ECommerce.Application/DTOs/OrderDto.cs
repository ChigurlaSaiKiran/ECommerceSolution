using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
       // public string ProductName { get; set; }   // ✅ ADD THIS

        public DateTime OrderDate { get; set; }
        //  public decimal TotalAmount { get; set; }
        public decimal TotalAmount => OrderItems.Sum(oi => oi.TotalPrice);
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }
}
