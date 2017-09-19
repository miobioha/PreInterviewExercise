using Banking.Core.Aggregates;
using Banking.Core.Commands;
using Banking.Core.Repositories;

namespace Banking.Core.Handlers
{
    public class AccountCommandHandlers
    {
        private readonly IAccountRepositoryFactory _factory;

        public AccountCommandHandlers(IAccountRepositoryFactory  factory)
        {
            _factory = factory;
        }

        public void Handle(CreateAccount createAccount)
        {
            using (var accountRepository = _factory.Create())
            {      
                var account = new Account(createAccount.AccountId, createAccount.BankId, createAccount.InitialAmount);

                accountRepository.Save(account);
            }
        }

        public void Handle(WithdrawFromAccount withdrawMoney)
        {
            using (var accountRepository = _factory.Create())
            {
                var account = accountRepository.GetById(withdrawMoney.Id);
                account.Withdraw(withdrawMoney.Amount);

                accountRepository.Save(account);
            }
        }
    }
}
