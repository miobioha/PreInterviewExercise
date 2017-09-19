using System;
using Banking.SharedKernel.Interface;

namespace Banking.Core.Commands
{
    public class ChangePin : ICommand
    {
        public Guid Id { get; set; }
        public string NewPinHash { get; set; }
    }
}
