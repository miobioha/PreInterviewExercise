using Banking.Core.Commands;
using Banking.Core.Handlers;
using Banking.Core.Repositories;
using Banking.Core.Services;
using Banking.SharedKernel;
using System;

namespace PreInterviewExercise.IntegrationTests
{
    public static class TestHelper
    {
        public static Guid CreateCashCard(Guid accountId, string pin)
        {
            var handlers = new CashCardCommandHandlers(new CashCardRepositoryFactory(), new Sha256HashingService());
            var bus = new CommandBus();
            bus.RegisterHandler(new Action<CreateCashCard>(handlers.Handle));
            var command = new CreateCashCard { AccountId = accountId, Pin = pin };
            bus.Send(command);

            return command.Id;
        }

        public static Guid CreateAccount(int bankId, decimal intitialAmount)
        {
            var handlers = new AccountCommandHandlers(new AccountRepositoryFactory());
            var bus = new CommandBus();
            bus.RegisterHandler<CreateAccount>(handlers.Handle);
            var command = new CreateAccount { BankId = bankId, InitialAmount = intitialAmount };
            bus.Send(command);

            return command.AccountId;
        }
    }
}
