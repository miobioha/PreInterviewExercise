using System;

namespace Banking.Core.Services
{
    public interface ITransactionServices
    {
        void Withdraw(Guid cardId, string pin, decimal amount);
    }
}
