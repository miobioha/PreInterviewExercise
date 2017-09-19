using Banking.Core.ReadModel.Implementation;

namespace Banking.Core.Repositories
{
    public class CashCardRepositoryFactory : ICashCardRepositoryFactory
    {
        public ICashCardRepository Create()
        {
            return new CashCardRepository(new BankContext());
        }
    }
}
