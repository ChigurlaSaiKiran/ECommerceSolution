namespace ECommerce.Application.DTOs
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; } = string.Empty;
        
    }
}
