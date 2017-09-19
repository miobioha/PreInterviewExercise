using System;
using Banking.Core.Aggregates;

namespace Banking.Core.Repositories
{
    public interface IAccountRepository : IDisposable
    {
        Account GetById(Guid id);

        void Save(Account entity);
    }
}
