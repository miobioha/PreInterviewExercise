using Banking.SharedKernel;
using System;
using Vending.Core.Exceptions;

namespace Vending.Core
{
    public class VendingMachine : AggregateRoot
    {   
        private readonly IAccountServices _accountServices;

        public VendingMachine(IAccountServices accountServices)
        {
            _accountServices = accountServices;

            Balance = 0.00m;
            ItemPile = ItemPile.Empty;   
        }

        public decimal Balance { get; private set; }

        public ItemPile ItemPile { get; private set; }
        
        public void VendItem(Guid cardId, string pin)
        {
            if (ItemPile.Quantity == 0)
            {
                throw new EmptyItemPileException();
            }

            if (!_accountServices.Withdraw(cardId, pin, ItemPile.Price))
            {
                throw new TransactionFailedException();
            }

            ItemPile = ItemPile.SubtractOne();
            Balance += ItemPile.Price;
        }

        public void LoadNewItemPile(ItemPile newItemPile)
        {
            if (newItemPile.Quantity > 25)
            {
                throw new InvalidOperationException("Cannot load more than 25 items.");
            }

            ItemPile = newItemPile;
        }
    }
}
