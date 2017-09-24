using System;
using System.Data.Entity;
using System.Linq;
using Banking.Core.Aggregates;
using Banking.Core.ReadModel.Implementation;
using Banking.Core.Exceptions;
using System.Data.Entity.Infrastructure;

namespace Banking.Core.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private bool _disposed;
        private readonly DbContext _context;
        private readonly DbSet<Account> _dbSet;

        public AccountRepository(BankContext context)
        {
            _context = context;
            _dbSet = context.Accounts;
        }

        public Account GetById(Guid id)
        {
            return _dbSet.Find(id);
        }

        public void Save(Account account)
        {
            if (_dbSet.Any(x => x.Id == account.Id))
            {
                _dbSet.Attach(account);
                _context.Entry(account).State = EntityState.Modified;
            }
            else
            {
                _dbSet.Add(account);
            }
            
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
