using Banking.Core.Events;
using Banking.SharedKernel;
using System;

namespace Banking.Core.Aggregates
{
    public class CashCard : AggregateRoot
    {
        public CashCard()
        {         
        }

        public CashCard(Guid id, Guid accountId, string pinHash)
        {
            Guard.ForNullOrEmpty(pinHash, "pinHash");

            ApplyChange(new CashCardCreated(id, accountId, pinHash));
        }

        public Guid AccountId { get; private set; }

        public string PinHash { get; private set; }

        public void ChangePin(string newPinHash)
        {
            PinHash = newPinHash;
        }

        public void Apply(CashCardCreated @event)
        {
            Id = @event.Id;
            AccountId = @event.AccountId;
            PinHash = @event.PinHash;
        }
    }   
}
