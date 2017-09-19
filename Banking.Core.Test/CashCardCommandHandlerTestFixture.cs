using System;
using Banking.Core.Aggregates;
using Banking.Core.Commands;
using Banking.Core.Handlers;
using Banking.Core.Repositories;
using Banking.Core.Services;
using Moq;
using NUnit.Framework;

namespace Banking.Core.Test
{
    [TestFixture]
    public class CashCardCommandHandlerTestFixture
    {
        private CashCardCommandHandlers _commandHandlers;
        private Mock<ICashCardRepositoryFactory> _cashCardRepositoryFactory;
        private Mock<ICashCardRepository> _cashCardRepository;
        private readonly IHashingService _hashingService = new Sha256HashingService();
        private static readonly Guid AccountId = Guid.NewGuid();
        private static readonly Guid CardId = Guid.NewGuid();

        [SetUp]
        public void GivenAnAccountCommandHandlers()
        {
            _cashCardRepositoryFactory = new Mock<ICashCardRepositoryFactory>();
            _cashCardRepository = new Mock<ICashCardRepository>();
            _cashCardRepositoryFactory.Setup(x => x.Create()).Returns(_cashCardRepository.Object);

            _commandHandlers = new CashCardCommandHandlers(_cashCardRepositoryFactory.Object, _hashingService);
        }

        [Test]
        public void WhenCreateCashCardIsHandled_NewCashCardIsCreated()
        {
            CashCard cashCard = null;
            _cashCardRepository.Setup(x => x.Save(It.IsAny<CashCard>())).Callback<CashCard>(card => cashCard = card);

            _commandHandlers.Handle(new CreateCashCard { AccountId = AccountId, Pin = "1234" });

            Assert.IsNotNull(cashCard);
            Assert.AreNotEqual(cashCard.Id, Guid.Empty);
            Assert.AreEqual(cashCard.AccountId, AccountId);
            Assert.AreEqual(cashCard.PinHash, _hashingService.Hash("1234"));
        }

        [Test]
        public void WhenChangeCardIsHandled_ThenCashCardPinIsSetToNewPin()
        {
            var pinHash = _hashingService.Hash("1234");
            var newPinHash = _hashingService.Hash("0000");
            var cashCard = new CashCard(CardId, AccountId, pinHash);
            _cashCardRepository.Setup(x => x.GetById(CardId)).Returns(cashCard);

            _commandHandlers.Handle(new ChangePin {Id = CardId, NewPinHash = newPinHash});

            Assert.AreEqual(cashCard.PinHash, newPinHash);
        }
    }
}
