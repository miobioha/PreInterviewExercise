using Banking.SharedKernel.Interface;
using System;

namespace Banking.Core.Events
{
    public class AccountCreated : IEvent
    {
        public AccountCreated(Guid id, int bankId, decimal initialBalance)
        {
            Id = id;
            BankId = bankId;
            Balance = initialBalance;
        }

        public Guid Id { get; private set; }

        public int BankId { get; private set; }

        public decimal Balance { get; private set; }
    }
}
