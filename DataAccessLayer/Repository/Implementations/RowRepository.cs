using Common.Models;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository.Implementations;

public class RowRepository(TicketSpotDbContext context) : IRepository<Row>
{
    public new async Task<List<Row>> GetAllAsync() =>
        await context.Rows
        .AsNoTracking()
        .AsQueryable()
        .Include(r => r.Section)
        .ToListAsync();

    public async Task<Row> GetAsync(int id) =>
        await context.Rows
        .AsNoTracking()
        .AsQueryable()
        .Where(r => r.Id == id)
        .Include(r => r.Section)
        .SingleOrDefaultAsync();

    public async Task<Row> CreateAsync(Row rowToCreate)
    {
        await context.Rows.AddAsync(rowToCreate);
        await context.SaveChangesAsync();
        return rowToCreate;
    }

    public async Task<Row> UpdateAsync(Row updatedRow)
    {
        var itemToUpdate = await GetAsync(updatedRow.Id)
            ?? throw new RecordNotFoundException("The row to be updated is not found in the database");

        context.Rows.Update(updatedRow);
        await context.SaveChangesAsync();
        return updatedRow;
    }

    public async Task DeleteAsync(int id)
    {
        var itemToDelete = new Row { Id = id };

        context.Attach(itemToDelete);
        context.Rows.Remove(itemToDelete);
        await context.SaveChangesAsync();
    }
}
