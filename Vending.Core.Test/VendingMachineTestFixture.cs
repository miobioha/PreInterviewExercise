using System;
using Moq;
using NUnit.Framework;
using Vending.Core.Exceptions;

namespace Vending.Core.Test
{
    [TestFixture]
    public class VendingMachineTestFixture
    {
        private VendingMachine _vendingMachine;
        private Mock<IAccountServices> _accountServices;
        private readonly Guid _cardId = Guid.NewGuid();
        private const string Pin = "1234";
        private const decimal Price = 0.50m;

        [SetUp]
        public void GivenAVendingMachine()
        {
            _accountServices = new Mock<IAccountServices>();
            WhenPaymentIs(true);

            _vendingMachine = new VendingMachine(_accountServices.Object);
        }

        [Test]
        public void WhenItemPileWithQuantityGreaterThanTwentyFiveIsLoaded_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => _vendingMachine.LoadNewItemPile(new ItemPile(Item.CanBeverage, 30, Price)));
        }

        [Test]
        public void WhenItemPileIsEmptiedAndUserAttemptsToDispenseEmptyPile_ThrowsEmptyItemPileException()
        {
            _vendingMachine.LoadNewItemPile(new ItemPile(Item.CanBeverage, 1, 0));

            _vendingMachine.VendItem(_cardId, Pin);

            Assert.Throws<EmptyItemPileException>(() =>  _vendingMachine.VendItem(_cardId, Pin));
        }

        [Test]
        public void WhenUserVendsItem_ItemPileIsReducedByOne()
        {
            _vendingMachine.LoadNewItemPile(new ItemPile(Item.CanBeverage, 18, 0));

            _vendingMachine.VendItem(_cardId, Pin);

            Assert.That(_vendingMachine.ItemPile.Quantity == 17);
        }

        [Test]
        public void WhenPaymentFails_ItemPileIsNotReducedByOne()
        {
            _vendingMachine.LoadNewItemPile(new ItemPile(Item.CanBeverage, 18, 0));

            WhenPaymentIs(false);
            Assert.Throws<TransactionFailedException>(() => _vendingMachine.VendItem(_cardId, Pin));

            Assert.AreEqual(18, _vendingMachine.ItemPile.Quantity);
        }

        private void WhenPaymentIs(bool success)
        {
            _accountServices.Setup(x => x.Withdraw(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<decimal>())).Returns(success);
        }
    }
}
