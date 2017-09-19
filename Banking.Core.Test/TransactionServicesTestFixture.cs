using System;
using Banking.Core.Aggregates;
using Banking.Core.Commands;
using Banking.Core.Exceptions;
using Banking.Core.Repositories;
using Banking.Core.Services;
using Banking.SharedKernel.Interface;
using Moq;
using NUnit.Framework;

namespace Banking.Core.Test
{
    [TestFixture]
    public class TransactionServicesTestFixture
    {
        private TransactionServices _transactionService;
        private Mock<ICommandBus> _commandBusMock;
        private Mock<ICashCardRepository> _cashCardRepositoryMock;
        private readonly IHashingService _hashingService = new Sha256HashingService();     
        private readonly Guid _cashCardId = Guid.NewGuid();
        private readonly Guid _accountId = Guid.NewGuid();
        private const string Pin = "1234";
        private const string BadPin = "0000";
        private CashCard _cashCard;
        

        [SetUp]
        public void GivenATransactionService()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _cashCardRepositoryMock = new Mock<ICashCardRepository>();

            _cashCard = new CashCard(_cashCardId, _accountId, _hashingService.Hash(Pin));
            _cashCardRepositoryMock.Setup(x => x.GetById(_cashCardId)).Returns(_cashCard);

            _transactionService = new TransactionServices(
                _cashCardRepositoryMock.Object,        
                _hashingService,
                _commandBusMock.Object);
        }

        [Test]
        public void WhenPinIsInvalidForWithdrawTransaction_ThrowsInvalidPinException()
        {
            Assert.Throws<InvalidPinException>(() => _transactionService.Withdraw(_cashCardId, BadPin, 0.50m));
        }

        [Test]
        public void WhenPinIsValidForWithdrawTransaction_ThenAmountIsWithdrawnFromAccount()
        {
            WithdrawFromAccount withdrawFromAccount = null;
            _commandBusMock.Setup(x => x.Send(It.IsAny<WithdrawFromAccount>()))
                .Callback(new Action<WithdrawFromAccount>(command => withdrawFromAccount = command));

            _transactionService.Withdraw(_cashCardId, Pin, 0.50m);

            Assert.IsNotNull(withdrawFromAccount);
            Assert.AreEqual(withdrawFromAccount.Id, _accountId);
            Assert.AreEqual(withdrawFromAccount.Amount, 0.50m);
        }
    }
}
