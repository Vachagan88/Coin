using System.Data;

namespace Core.Contracts;

public interface IUnitOfWork : IDisposable
{
    Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    void DisposeTransaction();
    void Commit();
    Task<int> SaveAsync();
    void RollBack();
}
