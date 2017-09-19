using System;

namespace Vending.Core
{
    public interface IAccountServices
    {
        bool Withdraw(Guid cardId, string pin, decimal amount);
    }
}
