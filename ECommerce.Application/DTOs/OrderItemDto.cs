using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs
{
    public class OrderItemDto
    {
        public int OrderItemId { get; set; } // optional
        public int ProductId { get; set; }
        public string? ProductName { get; set; }   // ✅ ADD THIS

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;
    }
}
