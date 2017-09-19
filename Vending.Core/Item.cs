namespace Vending.Core
{
    public class Item
    {
        public static readonly Item None = new Item(0, "None");
        public static readonly Item CanBeverage = new Item(1, "CanBeverage");

        public virtual string Name { get; private set; }

        protected Item()
        {
        }

        private Item(long id, string name)
            : this()
        {
            //Id = id;
            Name = name;
        }
    }
}
