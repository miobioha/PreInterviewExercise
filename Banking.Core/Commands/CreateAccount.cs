using Banking.SharedKernel.Interface;
using System;

namespace Banking.Core.Commands
{
    public class CreateAccount : ICommand
    {
        public CreateAccount()
        {
            AccountId = Guid.NewGuid();
        }

        public Guid AccountId { get; private set; }

        public int BankId { get; set; }

        public decimal InitialAmount { get; set; }
    }
}
