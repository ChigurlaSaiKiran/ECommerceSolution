using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;

namespace ECommerce.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomerService(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            return customers.Select(c => new CustomerDto
            {
                CustomerId = c.Id,            
                FullName = c.FullName,
                Email = c.Email,
               
            });
        }

        public async Task<CustomerDto?> GetByIdAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null) return null;

            return new CustomerDto
            {
                CustomerId = customer.Id,
                FullName = customer.FullName,               
                Email = customer.Email,
               
            };
        }

        public async Task AddAsync(CustomerDto dto)
        {
            var customer = new Customer
            {
                FullName = dto.FullName,                
                Email = dto.Email,
                
            };
            await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(CustomerDto dto)
        {
            var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);
            if (customer == null) return;

            customer.FullName = dto.FullName;           
            customer.Email = dto.Email;
          

            _customerRepository.Update(customer);
            await _customerRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer != null)
            {
                _customerRepository.Delete(customer);
                await _customerRepository.SaveChangesAsync();
            }
        }
    }
}
