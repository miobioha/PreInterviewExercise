using Banking.Core.Aggregates;
using NUnit.Framework;
using System;

namespace Banking.Core.Test
{
    [TestFixture]
    public class CashCardTestFixture
    {
        private CashCard _card;

        [SetUp]
        public void GivenANewCashCard()
        {
            _card = new CashCard(Guid.NewGuid(), Guid.NewGuid(), "HASH1");
        }

        [Test]
        public void WhenPinIsChanged_ThenCardPinIsSetToNewPin()
        {
            _card.ChangePin("HASH2");

            Assert.AreEqual("HASH2", _card.PinHash);
        }

        [Test]
        public void PinHashIsSetToInitialPin()
        {
            Assert.AreEqual("HASH1", _card.PinHash);
        }
    }
}                                                                                              