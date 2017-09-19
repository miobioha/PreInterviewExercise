using System;
using System.Data.Entity;
using System.Linq;
using Banking.Core.Aggregates;
using Banking.Core.ReadModel.Implementation;
using System.Data;
using Banking.Core.Exceptions;

namespace Banking.Core.Repositories
{
    public class CashCardRepository : ICashCardRepository
    {
        private bool _disposed = false;
        private readonly DbContext _context;
        private readonly DbSet<CashCard> _dbSet;

        public CashCardRepository(BankContext context)
        {
            _context = context;
            _dbSet = context.CashCards;
        }

        public CashCard GetById(Guid id)
        {
            return _dbSet.Find(id);
        }

        public void Save(CashCard cashCard)
        {
            if (_dbSet.Any(x => x.Id == cashCard.Id))
            {
                _dbSet.Attach(cashCard);
                _context.Entry(cashCard).State = EntityState.Modified;
            }
            else
            {
                _dbSet.Add(cashCard);
            }

            try
            {
                _context.SaveChanges();
            }
            catch (DBConcurrencyException)
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
