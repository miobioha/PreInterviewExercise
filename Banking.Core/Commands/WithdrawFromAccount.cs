using Banking.SharedKernel.Interface;
using System;

namespace Banking.Core.Commands
{
    public class WithdrawFromAccount : ICommand
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
    }   
}
