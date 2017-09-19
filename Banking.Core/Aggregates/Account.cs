using Banking.Core.Events;
using Banking.Core.Exceptions;
using Banking.SharedKernel;
using System;

namespace Banking.Core.Aggregates
{
    public class Account : AggregateRoot
    {
        public Account(Guid id, int bankId, decimal initialBalance = 0.0m)
        {
            ApplyChange(new AccountCreated(id, bankId, initialBalance));
        }

        public Account()
        {
        }

        public int BankId  { get; private set; }

        public decimal Balance { get; private set; }

        public void Deposit(decimal amount)
        {
            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount > Balance)
            {
                throw new InsufficientFundsException();
            }

            ApplyChange(new MoneyWithdrew(Id, amount));
        }

        public void Apply(AccountCreated @event)
        {
            Id = @event.Id;
            BankId = @event.BankId;
            Balance = @event.Balance;
        }

        public void Apply(MoneyWithdrew @event)
        {
            Balance -= @event.Amount;
        }
    }
}
