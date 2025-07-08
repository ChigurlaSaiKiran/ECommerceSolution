namespace ECommerce.Application.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }  // for edit/view
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
    }
}
