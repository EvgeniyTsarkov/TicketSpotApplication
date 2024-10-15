using Common.Models;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository.Implementations;

public class CustomerRepository(TicketSpotDbContext context) : IRepository<Customer>
{
    public async Task<List<Customer>> GetAllAsync() =>
    await context.Customers.AsNoTracking().ToListAsync();

    public async Task<Customer> GetAsync(int id) =>
        await context.Customers.AsNoTracking()
        .AsQueryable()
        .Where(c => c.Id == id)
        .SingleOrDefaultAsync();

    public async Task<Customer> CreateAsync(Customer customer)
    {
        await context.Customers.AddAsync(customer);
        await context.SaveChangesAsync();
        return customer;
    }

    public async Task<Customer> UpdateAsync(Customer customer)
    {
        var itemToUpdate = await GetAsync(customer.Id)
            ?? throw new RecordNotFoundException("The customer to be updated is not found in the database");

        context.Customers.Update(customer);
        await context.SaveChangesAsync();
        return customer;
    }

    public async Task DeleteAsync(int id)
    {
        var itemToDelete = new Customer { Id = id };

        context.Customers.Attach(itemToDelete);
        context.Customers.Remove(itemToDelete);
        await context.SaveChangesAsync();
    }
}
