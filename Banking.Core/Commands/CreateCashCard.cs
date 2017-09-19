using System;
using Banking.SharedKernel.Interface;

namespace Banking.Core.Commands
{
    public class CreateCashCard : ICommand
    {
        public CreateCashCard()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public Guid AccountId { get; set; }

        public string Pin  { get; set; }
    }
}
