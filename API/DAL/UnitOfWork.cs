using Core.Contracts;
using DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace DAL;

public class UnitOfWork : IUnitOfWork
{
    private readonly ChatDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(ChatDbContext context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        if (_transaction != null)
            throw new InvalidOperationException("transaction has already opened");

        _transaction = await _context.Database.BeginTransactionAsync(isolationLevel);
    }

    public void Commit()
    {
        if (_transaction == null)
            throw new InvalidOperationException("there is no opened transaction to commit");
        _transaction.Commit();
        DisposeTransaction();
    }

    public void Dispose()
    {
        DisposeTransaction();
        GC.SuppressFinalize(this);
    }

    public void DisposeTransaction()
    {
        _transaction?.Dispose();
        _transaction = null;
    }

    public void RollBack()
    {
        _transaction?.Rollback();
        DisposeTransaction();
    }

    public async Task<int> SaveAsync()
    {
        var result = await _context.SaveChangesAsync(new CancellationToken());

        foreach (var entry in _context.ChangeTracker.Entries())
        {
            entry.State = EntityState.Detached;
        }
        return result;
    }
}
