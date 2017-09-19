using System;
using Banking.Core.Aggregates;

namespace Banking.Core.Repositories
{
    public interface ICashCardRepository : IDisposable
    {
        CashCard GetById(Guid id);

        void Save(CashCard entity);
    }
}
