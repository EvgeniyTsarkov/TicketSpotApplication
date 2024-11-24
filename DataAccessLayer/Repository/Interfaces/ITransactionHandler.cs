namespace DataAccessLayer.Repository.Interfaces
{
    public interface ITransactionHandler
    {
        Task BeginTransactionAsync();
        Task CommitAsync();
        void Dispose();
        Task RollbackAsync();
    }
}