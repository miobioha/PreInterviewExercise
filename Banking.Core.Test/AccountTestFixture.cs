using Banking.Core.Aggregates;
using Banking.Core.Exceptions;
using NUnit.Framework;
using System;

namespace Banking.Core.Test
{
    [TestFixture]
    public class AccountTestFixture
    {
        private Account _account;
        private const int BankId = 1;

        [SetUp]
        public void GivenABankAccountWithAnInitialBalanceOfZero()
        {
            // Arrange
            _account = new Account(Guid.NewGuid(), BankId);
        }

        [TestCase(100.00)]
        [TestCase(1000.00)]
        [TestCase(0.01)]
        public void WhenIDepositAnAmountTheBalanceIsIncrementedByThatAmount(decimal amount)
        {
            // Arrange
            var balance = _account.Balance;

            // Act
            _account.Deposit(amount);

            // Assert
            Assert.That(_account.Balance == balance + amount);
        }

        [TestCase(100.00)]
        [TestCase(999.99)]
        [TestCase(56.01)]
        public void WhenIWithdrawAnAmountLessThanTheAccountBalance_ThenThatAmountIsDeductedFromTheBalance(decimal amount)
        {
            // Arrange
            _account.Deposit(1000.00m);
            var balance = _account.Balance;

            // Act
            _account.Withdraw(amount);

            // Assert
            Assert.That(_account.Balance == balance - amount);
        }

        [Test]
        public void WhenIWithdrawAnAmountGreaterThanTheAmountBalance_ThrowsInsufficientFundsException()
        {
            // Arrange
            _account.Deposit(100.0m);

            // Act
            // Assert
            var ex = Assert.Throws<InsufficientFundsException>(() => _account.Withdraw(101.0m));
        }
    }
}
