using Banking.SharedKernel.Interface;
using System;

namespace Banking.Core.Events
{
    public class MoneyWithdrew : IEvent
    {
        public MoneyWithdrew(Guid id, decimal amount)
        {
            Id = id;
            Amount = amount;
        }

        public Guid Id { get; private set; }
        public decimal Amount { get; set; } 
    }
}
