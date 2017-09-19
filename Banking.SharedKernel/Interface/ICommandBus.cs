namespace Banking.SharedKernel.Interface
{
    public interface ICommandBus
    {
        void Send<T>(T command) where T : ICommand;
    }
}
