namespace Banking.Core.Repositories
{
    public interface IAccountRepositoryFactory
    {
        IAccountRepository Create();
    }
}
