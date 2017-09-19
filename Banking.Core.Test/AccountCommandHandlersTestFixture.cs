using System;
using Banking.Core.Aggregates;
using Banking.Core.Commands;
using Banking.Core.Handlers;
using Banking.Core.Repositories;
using Moq;
using NUnit.Framework;

namespace Banking.Core.Test
{
    [TestFixture]
    public class AccountCommandHandlersTestFixture
    {
        private AccountCommandHandlers _commandHandlers;
        private Mock<IAccountRepositoryFactory> _accountRepositoryFactory;
        private Mock<IAccountRepository> _accountRepository;
        private static readonly Guid AccountId = new Guid();
        private const int BankId = 1;

        [SetUp]
        public void GivenAnAccountCommandHandlers()
        {
            _accountRepositoryFactory = new Mock<IAccountRepositoryFactory>();
            _accountRepository = new Mock<IAccountRepository>();
            _accountRepositoryFactory.Setup(x => x.Create()).Returns(_accountRepository.Object);

            _commandHandlers = new AccountCommandHandlers(_accountRepositoryFactory.Object);
        }

        [Test]
        public void WhenCreateAccountIsHandled_NewAccountIsCreated()
        {
            Account account = null;
            _accountRepository.Setup(x => x.Save(It.IsAny<Account>())).Callback<Account>(acc => account = acc);

            _commandHandlers.Handle(new CreateAccount {BankId = 1, InitialAmount = 70.00m});

            Assert.IsNotNull(account);
            Assert.AreNotEqual(account.Id, Guid.Empty);
            Assert.AreEqual(account.BankId, 1);
            Assert.AreEqual(account.Balance, 70.00m);
        }

        [Test]
        public void WhenWithdrawFromAccountIsHandled_AmountIsWithdrawnFromAccount()
        {
            var account = new Account(AccountId, BankId, 10.00m);

            _accountRepository.Setup(x => x.GetById(AccountId)).Returns(account);

            _commandHandlers.Handle(new WithdrawFromAccount { Id = AccountId, Amount = 0.50m });

            Assert.AreEqual(account.Balance, 9.50m);
        }

        [Test]
        public void WhenWithdrawFromAccountIsHandled_AccountIsPersisted()
        {
            var account = new Account(AccountId, BankId, 10.00m);

            _accountRepository.Setup(x => x.GetById(AccountId)).Returns(account);
            _accountRepository.Setup(x => x.Save(account)).Verifiable();

            _commandHandlers.Handle(new WithdrawFromAccount { Id = AccountId, Amount = 0.50m });

            _accountRepository.Verify();
        }
    }
}
