using Banking.SharedKernel;
using System;

namespace Vending.Core
{
    public class ItemPile : ValueObject<ItemPile>
    {
        public static readonly ItemPile Empty = new ItemPile(Item.None, 0, 0m);

        public Item Item { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }

        private ItemPile()
        {
        }

        public ItemPile(Item item, int quantity, decimal price) : this()
        {
            if (quantity < 0)
                throw new InvalidOperationException();
            if (price < 0)
                throw new InvalidOperationException();
            if (price % 0.01m > 0)
                throw new InvalidOperationException();

            Item = item;
            Quantity = quantity;
            Price = price;
        }

        public ItemPile SubtractOne()
        {
            return new ItemPile(Item, Quantity - 1, Price);
        }
    }
}
