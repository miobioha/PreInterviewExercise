using Banking.SharedKernel.Interface;
using System;

namespace Banking.Core.Events
{
    public class CashCardCreated : IEvent
    {
        public CashCardCreated(Guid id, Guid accountId, string pinHash)
        {
            Id = id;
            AccountId = accountId;
            PinHash = pinHash;
        }

        public Guid Id { get; set; }

        public Guid AccountId { get; private set; }

        public string PinHash { get; private set; }
    }
}
