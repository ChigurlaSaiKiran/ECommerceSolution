namespace ECommerce.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        // 👇 Computed TotalAmount — no DB column, just C#
        public decimal TotalAmount => OrderItems.Sum(x => x.UnitPrice * x.Quantity);
    }
}
