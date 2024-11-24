using Microsoft.EntityFrameworkCore.Storage;
using DataAccessLayer.Repository.Interfaces;

namespace DataAccessLayer.Repository.Implementations;

public class TransactionHandler(TicketSpotDbContext context) : ITransactionHandler
{
    private readonly TicketSpotDbContext _context = context;
    private IDbContextTransaction _transaction;

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        await _transaction.CommitAsync();
    }

    public async Task RollbackAsync()
    {
        await _transaction.RollbackAsync();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
    }
}
