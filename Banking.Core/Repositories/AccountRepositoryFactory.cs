using Banking.Core.ReadModel.Implementation;

namespace Banking.Core.Repositories
{
    public class AccountRepositoryFactory : IAccountRepositoryFactory
    {
        public IAccountRepository Create()
        {
            return new AccountRepository(new BankContext());
        }
    }
}
