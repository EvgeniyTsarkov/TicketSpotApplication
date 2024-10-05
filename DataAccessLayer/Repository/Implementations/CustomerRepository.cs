using Common.Models;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;

namespace DataAccessLayer.Repository.Implementations;

public class CustomerRepository(TicketSpotDbContext ticketSpotContext)
    : BaseRepository<Customer>(ticketSpotContext), ICustomerRepository
{
    public async Task<Customer> CreateAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<Customer> UpdateAsync(Customer customer)
    {
        var itemToUpdate = await Get(x => x.Id == customer.Id)
            ?? throw new RecordNotFoundException("The customer to be updated is not found in the database");

        _context.ChangeTracker.Clear();

        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task DeleteAsync(int id)
    {
        var itemToDelete = await Get(x => x.Id == id)
            ?? throw new RecordNotFoundException(string.Format("Customer with id: {0} is not found in the database", id));

        _context.ChangeTracker.Clear();

        _context.Customers.Remove(itemToDelete);
        await _context.SaveChangesAsync();
    }
}
