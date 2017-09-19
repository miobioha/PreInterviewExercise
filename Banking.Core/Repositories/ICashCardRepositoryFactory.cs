namespace Banking.Core.Repositories
{
    public interface ICashCardRepositoryFactory
    {
        ICashCardRepository Create();
    }
}
