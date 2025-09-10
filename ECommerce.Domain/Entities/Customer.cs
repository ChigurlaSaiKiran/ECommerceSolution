namespace ECommerce.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        // Store hashed password instead of plain text
        public string PasswordHash { get; set; }

        // Default role will be Customer
        public string Role { get; set; } = "Customer";

        public ICollection<Order> Orders { get; set; }
    }
}

//namespace ECommerce.Domain.Entities
//{
//    public class Customer
//    {
//        public int Id { get; set; }
//        public string FullName { get; set; }
//        public string Email { get; set; }

//        public ICollection<Order> Orders { get; set; }
//    }
//}
