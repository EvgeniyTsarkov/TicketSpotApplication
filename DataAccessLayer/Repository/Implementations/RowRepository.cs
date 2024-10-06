using Common.Models;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository.Implementations;

public class RowRepository(TicketSpotDbContext ticketSpotContext)
    : BaseRepository<Row>(ticketSpotContext), IRowRepository
{
    public new async Task<List<Row>> GetAllAsync() =>
        await _context.Rows
        .AsNoTracking()
        .AsQueryable()
        .Include(r => r.Section)
        .ToListAsync();

    public async Task<Row> GetAsync(int id) =>
        await _context.Rows
        .AsNoTracking()
        .AsQueryable()
        .Where(r => r.Id == id)
        .Include(r => r.Section)
        .SingleOrDefaultAsync();

    public async Task<Row> CreateAsync(Row rowToCreate)
    {
        await _context.Rows.AddAsync(rowToCreate);
        await _context.SaveChangesAsync();
        return rowToCreate;
    }

    public async Task<Row> UpdateAsync(Row updatedRow)
    {
        var itemToUpdate = await Get(x => x.Id == updatedRow.Id)
            ?? throw new RecordNotFoundException("The row to be updated is not found in the database");

        _context.Rows.Update(updatedRow);
        await _context.SaveChangesAsync();
        return updatedRow;
    }

    public async Task DeleteAsync(int id)
    {
        var itemToDelete = await GetAsync(id)
            ?? throw new RecordNotFoundException(string.Format("Row with id: {0} is not found in the database", id));

        _context.Rows.Remove(itemToDelete);
        await _context.SaveChangesAsync();
    }
}
