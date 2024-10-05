using Common.Models;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository.Implementations
{
    public class CustomerRepository(TicketSpotDbContext northwindContext)
        : BaseRepository<Customer>(northwindContext), ICustomerRepository
    {
        public async Task<Customer> CreateAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task DeleteAsync(int id)
        {
            var itemToDelete = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);

            if (itemToDelete == null)
            {
                throw new RecordNotFoundException(string.Format("Customer with id: {0} is not found in the database", id));
            }

            _context.Customers.Remove(itemToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
