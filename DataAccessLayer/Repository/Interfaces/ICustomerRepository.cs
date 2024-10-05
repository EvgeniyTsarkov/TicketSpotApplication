using Common.Models;

namespace DataAccessLayer.Repository.Interfaces;

public interface ICustomerRepository : IBaseRepository<Customer>
{
    Task<Customer> CreateAsync(Customer customer);
    Task<Customer> UpdateAsync(Customer customer);
    Task DeleteAsync(int id);
}