using Banking.Core.Commands;
using Banking.Core.Exceptions;
using Banking.Core.Handlers;
using Banking.Core.ReadModel.Implementation;
using Banking.Core.Repositories;
using Banking.Core.Services;
using Banking.SharedKernel;
using System;
using System.Threading.Tasks;
using Vending.Core;

namespace BankApiConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        public void ParallelWithdraw()
        {
            Parallel.Invoke(WithdrawFromAccount, WithdrawFromAccount);
        }
       
        public void ParallelWithdrawWithRetryPattern()
        {
            Parallel.Invoke(
            ActionHelper.ExecuteWithRetryAsync(WithdrawFromAccount, 3, TimeSpan.Zero, IsTransient).Wait,
            ActionHelper.ExecuteWithRetryAsync(WithdrawFromAccount, 3, TimeSpan.Zero, IsTransient).Wait
            );
        }

        public static void CreateCashCard()
        {
            var handlers = new CashCardCommandHandlers(new CashCardRepositoryFactory(), new Sha256HashingService());
            var bus = new CommandBus();
            bus.RegisterHandler(new Action<CreateCashCard>(handlers.Handle));
            var command = new CreateCashCard { AccountId = new Guid("04469380-6C5D-4152-8101-A380D20DDF6B"), Pin = "1234"};
            bus.Send(command);
        }

        public static void CreateAccount()
        {
            var handlers = new AccountCommandHandlers(new AccountRepositoryFactory());
            var bus = new CommandBus();
            bus.RegisterHandler<CreateAccount>(handlers.Handle);
            var command = new CreateAccount { BankId = 1, InitialAmount = 1000.00m };
            bus.Send(command);
        }

        public static void WithdrawFromAccount()
        {
            var handlers = new AccountCommandHandlers(new AccountRepositoryFactory());
            var bus = new CommandBus();
            bus.RegisterHandler(new Action<WithdrawFromAccount>(handlers.Handle));
            var command = new WithdrawFromAccount { Id = new Guid("0ECF3677-8F49-43CA-9D8E-4B0E8C56562D"), Amount = 0.50m };
            bus.Send(command);
        }

        public static void PerformTransaction()
        {
            var handlers = new AccountCommandHandlers(new AccountRepositoryFactory());
            var commandBus = new CommandBus();
            commandBus.RegisterHandler<WithdrawFromAccount>(handlers.Handle);
            new TransactionServices(new CashCardRepository(new BankContext()), new Sha256HashingService(),
                commandBus).Withdraw(new Guid("0417996D-ECF4-44DC-84A2-B66E72D5F46A"), "9999", 0.50m);
        }

        public static void PerformTransactionWebApi()
        {
            var svc = new AccountServices("http://localhost:46902/api/accounts/withdraw");
            svc.Withdraw(new Guid("0417996D-ECF4-44DC-84A2-B66E72D5F46A"), "1234", 0.50m);
        }


        public static bool IsTransient(Exception exception)
        {
            return exception is ConcurrencyException;
        }
    }
}
